using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.Controllers
{
    public class TrackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Track/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new TrackViewModel
            {
                AvailablePerformers = await _context.Performer
                    .Select(p => new PerformerViewModel
                    {
                        PerformerID = p.PerformerID,
                        Name = p.Name
                    })
                    .ToListAsync(),
                AvailableReleases = await _context.Release
                    .Select(r => new ReleaseViewModel
                    {
                        ReleaseID = r.ReleaseID,
                        Title = r.Title
                    })
                    .ToListAsync()
            };

            ViewBag.Guests = new SelectList(_context.Performer.Select(p => new { p.PerformerID, p.Name }), "PerformerID", "Name");
            ViewBag.Languages = new SelectList(_context.Language.Select(l => new { l.LanguageCode, l.LanguageName }), "LanguageCode", "LanguageName");
            ViewBag.Scales = new SelectList(_context.Scale.Select(s => new { s.ScaleId, s.Name }), "ScaleId", "Name");

            return View(viewModel);
        }

        // POST: Track/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrackViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var track = new Track
                {
                    Title = viewModel.Title,
                    Length = viewModel.Length
                };

                _context.Add(track);
                await _context.SaveChangesAsync();

                if (viewModel.ReleaseID.HasValue)
                {
                    var release = await _context.Release
                        .Include(r => r.ReleaseTracks)
                        .FirstOrDefaultAsync(r => r.ReleaseID == viewModel.ReleaseID.Value);

                    if (release != null)
                    {
                        var maxTrackNumber = release.ReleaseTracks.Any() ? 
                            release.ReleaseTracks.Max(rt => rt.TrackNumber) : 0;

                        release.ReleaseTracks.Add(new ReleaseTrack
                        {
                            TrackID = track.TrackID,
                            TrackNumber = maxTrackNumber + 1
                        });

                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            viewModel.AvailablePerformers = await _context.Performer
                .Select(p => new PerformerViewModel
                {
                    PerformerID = p.PerformerID,
                    Name = p.Name
                })
                .ToListAsync();

            viewModel.AvailableReleases = await _context.Release
                .Select(r => new ReleaseViewModel
                {
                    ReleaseID = r.ReleaseID,
                    Title = r.Title
                })
                .ToListAsync();

            ViewBag.Guests = new SelectList(_context.Performer.Select(p => new { p.PerformerID, p.Name }), "PerformerID", "Name");
            ViewBag.Languages = new SelectList(_context.Language.Select(l => new { l.LanguageCode, l.LanguageName }), "LanguageCode", "LanguageName");
            ViewBag.Scales = new SelectList(_context.Scale.Select(s => new { s.ScaleId, s.Name }), "ScaleId", "Name");

            return View(viewModel);
        }

        // GET: Track/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Track
                .Include(t => t.ReleaseTracks)
                .FirstOrDefaultAsync(t => t.TrackID == id);

            if (track == null)
            {
                return NotFound();
            }

            var viewModel = new TrackViewModel
            {
                TrackID = track.TrackID,
                Title = track.Title,
                Length = track.Length,
                ReleaseID = track.ReleaseTracks.FirstOrDefault()?.ReleaseID,
                AvailablePerformers = await _context.Performer
                    .Select(p => new PerformerViewModel
                    {
                        PerformerID = p.PerformerID,
                        Name = p.Name
                    })
                    .ToListAsync(),
                AvailableReleases = await _context.Release
                    .Select(r => new ReleaseViewModel
                    {
                        ReleaseID = r.ReleaseID,
                        Title = r.Title
                    })
                    .ToListAsync()
            };

            ViewBag.Guests = new SelectList(_context.Performer.Select(p => new { p.PerformerID, p.Name }), "PerformerID", "Name");
            ViewBag.Languages = new SelectList(_context.Language.Select(l => new { l.LanguageCode, l.LanguageName }), "LanguageCode", "LanguageName");
            ViewBag.Scales = new SelectList(_context.Scale.Select(s => new { s.ScaleId, s.Name }), "ScaleId", "Name");

            return View(viewModel);
        }

        // POST: Track/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrackViewModel viewModel)
        {
            if (id != viewModel.TrackID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var track = await _context.Track
                        .Include(t => t.ReleaseTracks)
                        .FirstOrDefaultAsync(t => t.TrackID == id);

                    if (track == null)
                    {
                        return NotFound();
                    }

                    track.Title = viewModel.Title;
                    track.Length = viewModel.Length;

                    // Handle release assignment
                    var currentReleaseTrack = track.ReleaseTracks.FirstOrDefault();
                    if (viewModel.ReleaseID.HasValue)
                    {
                        if (currentReleaseTrack == null || currentReleaseTrack.ReleaseID != viewModel.ReleaseID.Value)
                        {
                            // Remove from current release if exists
                            if (currentReleaseTrack != null)
                            {
                                _context.ReleaseTrack.Remove(currentReleaseTrack);
                            }

                            // Add to new release
                            var release = await _context.Release
                                .Include(r => r.ReleaseTracks)
                                .FirstOrDefaultAsync(r => r.ReleaseID == viewModel.ReleaseID.Value);

                            if (release != null)
                            {
                                var maxTrackNumber = release.ReleaseTracks.Any() ?
                                    release.ReleaseTracks.Max(rt => rt.TrackNumber) : 0;

                                release.ReleaseTracks.Add(new ReleaseTrack
                                {
                                    TrackID = track.TrackID,
                                    TrackNumber = maxTrackNumber + 1
                                });
                            }
                        }
                    }
                    else if (currentReleaseTrack != null)
                    {
                        // Remove from current release if no release selected
                        _context.ReleaseTrack.Remove(currentReleaseTrack);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackExists(viewModel.TrackID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            viewModel.AvailablePerformers = await _context.Performer
                .Select(p => new PerformerViewModel
                {
                    PerformerID = p.PerformerID,
                    Name = p.Name
                })
                .ToListAsync();

            viewModel.AvailableReleases = await _context.Release
                .Select(r => new ReleaseViewModel
                {
                    ReleaseID = r.ReleaseID,
                    Title = r.Title
                })
                .ToListAsync();

            ViewBag.Guests = new SelectList(_context.Performer.Select(p => new { p.PerformerID, p.Name }), "PerformerID", "Name");
            ViewBag.Languages = new SelectList(_context.Language.Select(l => new { l.LanguageCode, l.LanguageName }), "LanguageCode", "LanguageName");
            ViewBag.Scales = new SelectList(_context.Scale.Select(s => new { s.ScaleId, s.Name }), "ScaleId", "Name");

            return View(viewModel);
        }

        private bool TrackExists(int id)
        {
            return _context.Track.Any(e => e.TrackID == id);
        }
    }
} 