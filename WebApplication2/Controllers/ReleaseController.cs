using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;

namespace WebApplication2.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReleaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ManageTracks(int id, string searchTerm = "")
        {
            var release = await _context.Release
                .Include(r => r.ReleaseTracks)
                    .ThenInclude(rt => rt.Track)
                        .ThenInclude(t => t.TrackPerformers)
                            .ThenInclude(tp => tp.Performer)
                .FirstOrDefaultAsync(r => r.ReleaseID == id);

            if (release == null)
            {
                return NotFound();
            }

            var viewModel = new ReleaseTracksViewModel
            {
                ReleaseID = release.ReleaseID,
                ReleaseTitle = release.Title,
                SearchTerm = searchTerm,
                CurrentTracks = release.ReleaseTracks
                    .OrderBy(rt => rt.TrackNumber)
                    .Select(rt => new ReleaseTracksTrackViewModel
                    {
                        TrackID = rt.TrackID,
                        Title = rt.Track.Title,
                        PerformerName = rt.Track.TrackPerformers.Select(tp => tp.Performer.Name).FirstOrDefault() ?? "Unknown",
                        Length = rt.Track.Length
                    })
                    .ToList()
            };

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var tracks = _context.Track
                    .Include(t => t.TrackPerformers)
                        .ThenInclude(tp => tp.Performer)
                    .Where(t => string.IsNullOrEmpty(searchTerm) || 
                               t.Title.Contains(searchTerm))
                    .Union(_context.Track
                        .Include(t => t.TrackPerformers)
                            .ThenInclude(tp => tp.Performer)
                        .Where(t => !string.IsNullOrEmpty(searchTerm) &&
                                   t.TrackPerformers.Any(tp => tp.Performer != null) &&
                                   t.TrackPerformers.Any(tp => tp.Performer.Name.Contains(searchTerm))))
                    .Select(t => new ReleaseTracksTrackViewModel
                    {
                        TrackID = t.TrackID,
                        Title = t.Title,
                        PerformerName = t.TrackPerformers.Select(tp => tp.Performer.Name).FirstOrDefault() ?? "Unknown",
                        Length = t.Length
                    })
                    .ToList();

                viewModel.AvailableTracks = tracks;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTracks(int id, ReleaseTracksViewModel model)
        {
            var release = await _context.Release
                .Include(r => r.ReleaseTracks)
                .FirstOrDefaultAsync(r => r.ReleaseID == id);

            if (release == null)
            {
                return NotFound();
            }

            if (model.SelectedTrackIds != null && model.SelectedTrackIds.Any())
            {
                var maxTrackNumber = release.ReleaseTracks.Any() ? 
                    release.ReleaseTracks.Max(rt => rt.TrackNumber) : 0;

                foreach (var trackId in model.SelectedTrackIds)
                {
                    if (!release.ReleaseTracks.Any(rt => rt.TrackID == trackId))
                    {
                        release.ReleaseTracks.Add(new ReleaseTrack
                        {
                            TrackID = trackId,
                            TrackNumber = ++maxTrackNumber
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageTracks), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTrack(int id, int trackId)
        {
            var releaseTrack = await _context.ReleaseTrack
                .FirstOrDefaultAsync(rt => rt.ReleaseID == id && rt.TrackID == trackId);

            if (releaseTrack != null)
            {
                _context.ReleaseTrack.Remove(releaseTrack);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageTracks), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTrackOrder(int id, List<int> trackIds)
        {
            var releaseTracks = await _context.ReleaseTrack
                .Where(rt => rt.ReleaseID == id)
                .ToListAsync();

            for (int i = 0; i < trackIds.Count; i++)
            {
                var releaseTrack = releaseTracks.FirstOrDefault(rt => rt.TrackID == trackIds[i]);
                if (releaseTrack != null)
                {
                    releaseTrack.TrackNumber = i + 1;
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
} 