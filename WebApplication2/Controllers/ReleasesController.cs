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
            System.Diagnostics.Debug.WriteLine($"Description: {viewModel.Description}");
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
                    ReleaseTypeID = viewModel.ReleaseTypeID,
                    Description = viewModel.Description
                };

                System.Diagnostics.Debug.WriteLine($"Creating release with Description: '{release.Description}'");

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

            var viewModel = new ReleaseEditViewModel
            {
                ReleaseID = release.ReleaseID,
                Title = release.Title,
                Description = release.Description,
                ReleaseDate = release.ReleaseDate,
                MainGenreCode = release.MainGenreCode,
                SecondGenreCode = release.SecondGenreCode,
                ReleaseTypeID = release.ReleaseTypeID,
                ExistingCoverBase64 = release.ReleaseCover != null ? 
                    Convert.ToBase64String(release.ReleaseCover) : null,

                CurrentTracks = release.ReleaseTracks
                    .OrderBy(rt => rt.TrackNumber)
                    .Select(rt => new ReleaseCreateTrackViewModel
                    {
                        TrackID = rt.Track.TrackID,
                        Title = rt.Track.Title,
                        PerformerName = rt.Track.TrackPerformers.Select(tp => tp.Performer.Name).FirstOrDefault() ?? "Unknown",
                        Length = rt.Track.Length
                    }).ToList(),
                AvailableTracks = await _context.Track
                    .Include(t => t.TrackPerformers)
                        .ThenInclude(tp => tp.Performer)
                    .Where(t => !release.ReleaseTracks.Select(rt => rt.TrackID).Contains(t.TrackID))
                    .Select(t => new ReleaseCreateTrackViewModel
                    {
                        TrackID = t.TrackID,
                        Title = t.Title,
                        PerformerName = t.TrackPerformers.Select(tp => tp.Performer.Name).FirstOrDefault() ?? "Unknown",
                        Length = t.Length
                    })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Releases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReleaseEditViewModel viewModel)
        {
            if (id != viewModel.ReleaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRelease = await _context.Release
                        .Include(r => r.ReleaseTracks)
                        .FirstOrDefaultAsync(r => r.ReleaseID == id);

                    if (existingRelease == null)
                    {
                        return NotFound();
                    }

                    // Update basic properties
                    existingRelease.Title = viewModel.Title;
                    existingRelease.ReleaseDate = viewModel.ReleaseDate;
                    existingRelease.MainGenreCode = viewModel.MainGenreCode;
                    existingRelease.SecondGenreCode = viewModel.SecondGenreCode;
                    existingRelease.ReleaseTypeID = viewModel.ReleaseTypeID;
                    existingRelease.Description = viewModel.Description;

                    // Update tracks
                    var trackIds = !string.IsNullOrEmpty(viewModel.SelectedTrackIds) 
                        ? viewModel.SelectedTrackIds.Split(',')
                            .Where(id => !string.IsNullOrWhiteSpace(id))
                            .Select(id => int.Parse(id.Trim()))
                            .ToList()
                        : new List<int>();

                    // Get current track IDs
                    var currentTrackIds = existingRelease.ReleaseTracks.Select(rt => rt.TrackID).ToList();

                    // Find tracks to remove (in current but not in new selection)
                    var tracksToRemove = existingRelease.ReleaseTracks
                        .Where(rt => !trackIds.Contains(rt.TrackID))
                        .ToList();

                    // Find tracks to add (in new selection but not in current)
                    var tracksToAdd = trackIds
                        .Where(id => !currentTrackIds.Contains(id))
                        .ToList();

                    // Remove tracks that are no longer selected
                    if (tracksToRemove.Any())
                    {
                        _context.ReleaseTrack.RemoveRange(tracksToRemove);
                    }

                    // Add new tracks
                    if (tracksToAdd.Any())
                    {
                        var maxTrackNumber = existingRelease.ReleaseTracks.Any() ? 
                            existingRelease.ReleaseTracks.Max(rt => rt.TrackNumber) : 0;

                        foreach (var trackId in tracksToAdd)
                        {
                            _context.ReleaseTrack.Add(new ReleaseTrack
                            {
                                ReleaseID = existingRelease.ReleaseID,
                                TrackID = trackId,
                                TrackNumber = ++maxTrackNumber
                            });
                        }
                    }

                    // Update track numbers for all tracks
                    var allTracks = await _context.ReleaseTrack
                        .Where(rt => rt.ReleaseID == existingRelease.ReleaseID)
                        .OrderBy(rt => rt.TrackNumber)
                        .ToListAsync();

                    for (int i = 0; i < allTracks.Count; i++)
                    {
                        allTracks[i].TrackNumber = i + 1;
                    }

                    _context.Update(existingRelease);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReleaseExists(viewModel.ReleaseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
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

            return View(viewModel);
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

        // GET: Releases/UploadCover/5
        public async Task<IActionResult> UploadCover(int? id)
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

            var viewModel = new ReleaseCoverViewModel
            {
                ReleaseID = release.ReleaseID,
                ReleaseTitle = release.Title,
                ExistingCoverBase64 = release.ReleaseCover != null ? 
                    Convert.ToBase64String(release.ReleaseCover) : null
            };

            return View(viewModel);
        }

        // POST: Releases/UploadCover/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadCover(int id, ReleaseCoverViewModel viewModel, IFormFile ReleaseCover)
        {
            if (id != viewModel.ReleaseID)
            {
                return NotFound();
            }

            if (ReleaseCover == null || ReleaseCover.Length == 0)
            {
                ModelState.AddModelError("ReleaseCover", "Please select an image to upload.");
                return View(viewModel);
            }

            // Validate file type
            var fileExtension = Path.GetExtension(ReleaseCover.FileName).ToLower();
            if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
            {
                ModelState.AddModelError("ReleaseCover", "Invalid file type. Please upload a PNG or JPEG image.");
                return View(viewModel);
            }

            try
            {
                var release = await _context.Release.FindAsync(id);
                if (release == null)
                {
                    return NotFound();
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

                _context.Update(release);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = release.ReleaseID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while uploading the cover image. Please try again.");
                return View(viewModel);
            }
        }
    }
}
