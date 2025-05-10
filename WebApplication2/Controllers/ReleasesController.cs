using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;

namespace WebApplication2.Controllers
{
    public class ReleasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReleasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Releases
        public async Task<IActionResult> Index()
        {
            return View(await _context.Release.ToListAsync());
        }

        // GET: Releases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var release = await _context.Release
                .Include(r => r.MainGenre)
                .Include(r => r.SecondGenre)
                .Include(r => r.ReleaseType)
                .Include(r => r.ReleasePerformers)
                    .ThenInclude(rp => rp.Performer)
                .Include(r => r.ReleaseTracks)
                    .ThenInclude(rt => rt.Track)
                        .ThenInclude(t => t.TrackPerformers)
                            .ThenInclude(tp => tp.Performer)
                .FirstOrDefaultAsync(m => m.ReleaseID == id);

            if (release == null)
            {
                return NotFound();
            }

            return View(release);
        }

        // GET: Releases/Create
        public IActionResult Create()
        {
            var releasetypes = _context.ReleaseType
                .Select(gn => new ReleaseTypeViewModel
                {
                    ReleaseTypeID = gn.ReleaseTypeID,
                    ReleaseTypeName = gn.ReleaseTypeName
                }).ToList();

            ViewBag.ReleaseTypes = new SelectList(releasetypes, "ReleaseTypeID", "ReleaseTypeName");

            var genres = _context.Genre
                .Select(gn => new GenreViewModel
                {
                    GenreID = gn.GenreID,
                    GenreName = gn.GenreName
                }).ToList();

            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");

            var viewModel = new ReleaseCreateViewModel
            {
                AvailableTracks = _context.Track
                    .Include(t => t.TrackPerformers)
                        .ThenInclude(tp => tp.Performer)
                    .Select(t => new ReleaseCreateTrackViewModel
                    {
                        TrackID = t.TrackID,
                        Title = t.Title,
                        PerformerName = t.TrackPerformers.Select(tp => tp.Performer.Name).FirstOrDefault() ?? "Unknown",
                        Length = t.Length
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: Releases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReleaseCreateViewModel viewModel, IFormFile ReleaseCover)
        {
            // Log the incoming data
            System.Diagnostics.Debug.WriteLine($"Title: {viewModel.Title}");
            System.Diagnostics.Debug.WriteLine($"ReleaseDate: {viewModel.ReleaseDate}");
            System.Diagnostics.Debug.WriteLine($"MainGenreCode: {viewModel.MainGenreCode}");
            System.Diagnostics.Debug.WriteLine($"SecondGenreCode: {viewModel.SecondGenreCode}");
            System.Diagnostics.Debug.WriteLine($"ReleaseTypeID: {viewModel.ReleaseTypeID}");
            System.Diagnostics.Debug.WriteLine($"SelectedTrackIds: {viewModel.SelectedTrackIds}");

            // Log ModelState errors
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
            }

            var releasetypes = _context.ReleaseType
                .Select(gn => new ReleaseTypeViewModel
                {
                    ReleaseTypeID = gn.ReleaseTypeID,
                    ReleaseTypeName = gn.ReleaseTypeName
                }).ToList();

            ViewBag.ReleaseTypes = new SelectList(releasetypes, "ReleaseTypeID", "ReleaseTypeName");

            var genres = _context.Genre
                .Select(gn => new GenreViewModel
                {
                    GenreID = gn.GenreID,
                    GenreName = gn.GenreName
                }).ToList();

            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is valid, creating release...");
                
                var release = new Release
                {
                    Title = viewModel.Title,
                    ReleaseDate = viewModel.ReleaseDate,
                    MainGenreCode = viewModel.MainGenreCode,
                    SecondGenreCode = viewModel.SecondGenreCode,
                    ReleaseTypeID = viewModel.ReleaseTypeID
                };

                if (ReleaseCover != null && ReleaseCover.Length > 0)
                {
                    // Validate file type
                    var fileExtension = Path.GetExtension(ReleaseCover.FileName).ToLower();
                    if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
                    {
                        ModelState.AddModelError("ReleaseCover", "Invalid file type. Please upload a PNG or JPEG image.");
                        return View(viewModel);
                    }

                    // Check if the image has a 1:1 aspect ratio
                    using (var memoryStream = new MemoryStream())
                    {
                        await ReleaseCover.CopyToAsync(memoryStream);
                        release.ReleaseCover = memoryStream.ToArray();

                        // Reset the memory stream position to the beginning
                        memoryStream.Position = 0;

                        // Use System.Drawing to check the image dimensions
                        using (var image = Image.FromStream(memoryStream))
                        {
                            if (image.Width != image.Height)
                            {
                                ModelState.AddModelError("ReleaseCover", "Picture has to be 1:1 ratio.");
                                return View(viewModel);
                            }
                        }
                    }
                }

                try
                {
                    _context.Add(release);
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine($"Release created with ID: {release.ReleaseID}");

                    // Add selected tracks to the release
                    if (!string.IsNullOrEmpty(viewModel.SelectedTrackIds))
                    {
                        var trackIds = viewModel.SelectedTrackIds.Split(',')
                            .Select(id => int.Parse(id.Trim()))
                            .ToList();

                        System.Diagnostics.Debug.WriteLine($"Adding {trackIds.Count} tracks to release...");
                        var trackNumber = 1;
                        foreach (var trackId in trackIds)
                        {
                            _context.ReleaseTrack.Add(new ReleaseTrack
                            {
                                ReleaseID = release.ReleaseID,
                                TrackID = trackId,
                                TrackNumber = trackNumber++
                            });
                        }
                        await _context.SaveChangesAsync();
                        System.Diagnostics.Debug.WriteLine("Tracks added successfully");
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error saving release: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the release. Please try again.");
                }
            }

            // If we got here, something failed, reload available tracks
            viewModel.AvailableTracks = _context.Track
                .Include(t => t.TrackPerformers)
                    .ThenInclude(tp => tp.Performer)
                .Select(t => new ReleaseCreateTrackViewModel
                {
                    TrackID = t.TrackID,
                    Title = t.Title,
                    PerformerName = t.TrackPerformers.Select(tp => tp.Performer.Name).FirstOrDefault() ?? "Unknown",
                    Length = t.Length
                })
                .ToList();

            return View(viewModel);
        }

        // GET: Releases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var release = await _context.Release.FindAsync(id);
            if (release == null)
            {
                return NotFound();
            }

            var genres = _context.Genre
                .Select(gn => new GenreViewModel
                {
                    GenreID = gn.GenreID,
                    GenreName = gn.GenreName
                }).ToList();

            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");

            var releasetypes = _context.ReleaseType
                .Select(gn => new ReleaseTypeViewModel
                {
                    ReleaseTypeID = gn.ReleaseTypeID,
                    ReleaseTypeName = gn.ReleaseTypeName
                }).ToList();

            ViewBag.ReleaseTypes = new SelectList(releasetypes, "ReleaseTypeID", "ReleaseTypeName");

            return View(release);
        }

        // POST: Releases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Release release, IFormFile ReleaseCover)
        {
            if (id != release.ReleaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRelease = await _context.Release.FindAsync(id);
                    if (existingRelease == null)
                    {
                        return NotFound();
                    }

                    // Update basic properties
                    existingRelease.Title = release.Title;
                    existingRelease.ReleaseDate = release.ReleaseDate;
                    existingRelease.MainGenreCode = release.MainGenreCode;
                    existingRelease.SecondGenreCode = release.SecondGenreCode;
                    existingRelease.ReleaseTypeID = release.ReleaseTypeID;

                    // Handle cover image upload
                    if (ReleaseCover != null && ReleaseCover.Length > 0)
                    {
                        // Validate file type
                        var fileExtension = Path.GetExtension(ReleaseCover.FileName).ToLower();
                        if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
                        {
                            ModelState.AddModelError("ReleaseCover", "Invalid file type. Please upload a PNG or JPEG image.");
                            return View(release);
                        }

                        // Check if the image has a 1:1 aspect ratio
                        using (var memoryStream = new MemoryStream())
                        {
                            await ReleaseCover.CopyToAsync(memoryStream);
                            existingRelease.ReleaseCover = memoryStream.ToArray();

                            // Reset the memory stream position to the beginning
                            memoryStream.Position = 0;

                            // Use System.Drawing to check the image dimensions
                            using (var image = Image.FromStream(memoryStream))
                            {
                                if (image.Width != image.Height)
                                {
                                    ModelState.AddModelError("ReleaseCover", "Picture has to be 1:1 ratio.");
                                    return View(release);
                                }
                            }
                        }
                    }

                    _context.Update(existingRelease);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReleaseExists(release.ReleaseID))
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

            // If we got this far, something failed, redisplay form
            var genres = _context.Genre
                .Select(gn => new GenreViewModel
                {
                    GenreID = gn.GenreID,
                    GenreName = gn.GenreName
                }).ToList();

            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");

            var releasetypes = _context.ReleaseType
                .Select(gn => new ReleaseTypeViewModel
                {
                    ReleaseTypeID = gn.ReleaseTypeID,
                    ReleaseTypeName = gn.ReleaseTypeName
                }).ToList();

            ViewBag.ReleaseTypes = new SelectList(releasetypes, "ReleaseTypeID", "ReleaseTypeName");

            return View(release);
        }

        // GET: Releases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var release = await _context.Release
                .FirstOrDefaultAsync(m => m.ReleaseID == id);
            if (release == null)
            {
                return NotFound();
            }

            return View(release);
        }

        // POST: Releases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var release = await _context.Release.FindAsync(id);
            if (release != null)
            {
                _context.Release.Remove(release);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReleaseExists(int id)
        {
            return _context.Release.Any(e => e.ReleaseID == id);
        }


        [HttpGet]
        public IActionResult LoadGenres()
        {
            var scales = _context.Genre.Select(s => new { s.GenreID, s.GenreName}).ToList();
            var selectList = new SelectList(scales, "GenreID", "GenreName"); // Ensure property names match
            return PartialView("_GenreSelectList", selectList);
        }

        // GET: Releases/SearchTracks
        public IActionResult SearchTracks(string searchTerm)
        {
            var tracks = _context.Track
                .Include(t => t.TrackPerformers)
                    .ThenInclude(tp => tp.Performer)
                .Where(t => string.IsNullOrEmpty(searchTerm) || 
                           t.Title.Contains(searchTerm) ||
                           t.TrackPerformers.Any(tp => tp.Performer.Name.Contains(searchTerm)))
                .Select(t => new ReleaseCreateTrackViewModel
                {
                    TrackID = t.TrackID,
                    Title = t.Title,
                    PerformerName = t.TrackPerformers.Select(tp => tp.Performer.Name).FirstOrDefault() ?? "Unknown",
                    Length = t.Length
                })
                .ToList();

            return PartialView("_TrackList", tracks);
        }
    }
}
