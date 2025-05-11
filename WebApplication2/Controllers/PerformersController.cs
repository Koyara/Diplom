using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Controllers
{
    public class PerformersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerformersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Performers
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Performer.Include(p => p.MainGenre).Include(p => p.PerformerType).Include(p => p.SecondaryGenre);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Performers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load basic performer info
            var performer = await _context.Performer
                .Include(p => p.PerformerType)
                .Include(p => p.MainGenre)
                .Include(p => p.SecondaryGenre)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PerformerID == id);

            if (performer == null)
            {
                return NotFound();
            }

            // Load releases in a separate query
            performer.ReleasePerformers = await _context.ReleasePerformer
                .Include(rp => rp.Release)
                    .ThenInclude(r => r.ReleaseType)
                .Where(rp => rp.PerformerID == id)
                .AsNoTracking()
                .ToListAsync();

            // Load tracks where performer is the main performer
            performer.TrackPerformers = await _context.TrackPerformer
                .Include(tp => tp.Track)
                .Where(tp => tp.PerformerID == id)
                .AsNoTracking()
                .ToListAsync();

            // Load tracks where performer is a producer
            performer.ProducedTracks = await _context.TrackProducer
                .Include(tp => tp.Track)
                .Where(tp => tp.ProducerID == id)
                .AsNoTracking()
                .ToListAsync();

            return View(performer);
        }

        // GET: Performers/Create
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult Create()
        {
            var performerTypes = _context.PerformerType
                .Select(pt => new PerformerTypeViewModel
                {
                    PerformerTypeID = pt.PerformerTypeID,
                    TypeName = pt.TypeName
                }).ToList();

            var genres = _context.Genre
                .Select(gn => new GenreViewModel
                {
                    GenreID = gn.GenreID,
                    GenreName = gn.GenreName
                }).ToList();

            ViewBag.PerformerTypes = new SelectList(performerTypes, "PerformerTypeID", "TypeName");
            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");
            ViewBag.Countries = new SelectList(_context.Country.Select(c => new { c.CountryCode, c.CountryName }), "CountryCode", "CountryName");
            return View(new PerformerCreateViewModel());
        }

        // POST: Performers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Create(PerformerCreateViewModel viewModel, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                var performer = new Performer
                {
                    PerformerTypeID = viewModel.PerformerTypeID,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    MainGenreID = viewModel.MainGenreID,
                    SecondaryGenreID = viewModel.SecondaryGenreID,
                    CountryCode = viewModel.CountryCode
                };

                if (Photo != null && Photo.Length > 0)
                {
                    // Validate file type
                    var fileExtension = Path.GetExtension(Photo.FileName).ToLower();
                    if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
                    {
                        ModelState.AddModelError("Photo", "Invalid file type. Please upload a PNG or JPEG image.");
                        return View(viewModel);
                    }

                    // Check if the image has a 1:1 aspect ratio
                    using (var memoryStream = new MemoryStream())
                    {
                        await Photo.CopyToAsync(memoryStream);
                        performer.Photo = memoryStream.ToArray();

                        // Reset the memory stream position to the beginning
                        memoryStream.Position = 0;

                        // Use System.Drawing to check the image dimensions
                        using (var image = Image.FromStream(memoryStream))
                        {
                            if (image.Width != image.Height)
                            {
                                ModelState.AddModelError("Photo", "Picture has to be 1:1 ratio.");
                                return View(viewModel);
                            }
                        }
                    }
                }

                _context.Add(performer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var performerTypes = _context.PerformerType
                .Select(pt => new PerformerTypeViewModel
                {
                    PerformerTypeID = pt.PerformerTypeID,
                    TypeName = pt.TypeName
                }).ToList();

            var genres = _context.Genre
                .Select(gn => new GenreViewModel
                {
                    GenreID = gn.GenreID,
                    GenreName = gn.GenreName
                }).ToList();

            ViewBag.PerformerTypes = new SelectList(performerTypes, "PerformerTypeID", "TypeName");
            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");
            ViewBag.Countries = new SelectList(_context.Country.Select(c => new { c.CountryCode, c.CountryName }), "CountryCode", "CountryName");
            return View(viewModel);
        }

        // GET: Performers/Edit/5
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performer = await _context.Performer.FindAsync(id);
            if (performer == null)
            {
                return NotFound();
            }

            var performerTypes = _context.PerformerType
                .Select(pt => new PerformerTypeViewModel
                {
                    PerformerTypeID = pt.PerformerTypeID,
                    TypeName = pt.TypeName
                }).ToList();

            var genres = _context.Genre
                .Select(gn => new GenreViewModel
                {
                    GenreID = gn.GenreID,
                    GenreName = gn.GenreName
                }).ToList();

            ViewBag.PerformerTypes = new SelectList(performerTypes, "PerformerTypeID", "TypeName");
            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");
            ViewBag.Countries = new SelectList(_context.Country.Select(c => new { c.CountryCode, c.CountryName }), "CountryCode", "CountryName");
            return View(performer);
        }

        // POST: Performers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("PerformerID,PerformerTypeID,Name,Description,MainGenreID,SecondaryGenreID,CountryCode")] Performer performer)
        {
            if (id != performer.PerformerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing performer to preserve the photo
                    var existingPerformer = await _context.Performer.AsNoTracking().FirstOrDefaultAsync(p => p.PerformerID == id);
                    if (existingPerformer != null)
                    {
                        // Preserve the existing photo
                        performer.Photo = existingPerformer.Photo;
                    }

                    _context.Update(performer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformerExists(performer.PerformerID))
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

            var performerTypes = _context.PerformerType
                .Select(pt => new PerformerTypeViewModel
                {
                    PerformerTypeID = pt.PerformerTypeID,
                    TypeName = pt.TypeName
                }).ToList();

            var genres = _context.Genre
                .Select(gn => new GenreViewModel
                {
                    GenreID = gn.GenreID,
                    GenreName = gn.GenreName
                }).ToList();

            ViewBag.PerformerTypes = new SelectList(performerTypes, "PerformerTypeID", "TypeName");
            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");
            ViewBag.Countries = new SelectList(_context.Country.Select(c => new { c.CountryCode, c.CountryName }), "CountryCode", "CountryName");
            return View(performer);
        }

        // GET: Performers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performer = await _context.Performer
                .Include(p => p.MainGenre)
                .Include(p => p.PerformerType)
                .Include(p => p.SecondaryGenre)
                .FirstOrDefaultAsync(m => m.PerformerID == id);
            if (performer == null)
            {
                return NotFound();
            }

            return View(performer);
        }

        // POST: Performers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var performer = await _context.Performer.FindAsync(id);
            if (performer != null)
            {
                _context.Performer.Remove(performer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Performers/UploadPhoto/5
        public async Task<IActionResult> UploadPhoto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performer = await _context.Performer.FindAsync(id);
            if (performer == null)
            {
                return NotFound();
            }

            var viewModel = new PerformerPhotoViewModel
            {
                PerformerID = performer.PerformerID,
                PerformerName = performer.Name,
                ExistingPhotoBase64 = performer.Photo != null ? 
                    Convert.ToBase64String(performer.Photo) : null
            };

            return View(viewModel);
        }

        // POST: Performers/UploadPhoto/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhoto(int id, PerformerPhotoViewModel viewModel, IFormFile Photo)
        {
            if (id != viewModel.PerformerID)
            {
                return NotFound();
            }

            if (Photo == null || Photo.Length == 0)
            {
                ModelState.AddModelError("Photo", "Please select an image to upload.");
                return View(viewModel);
            }

            // Validate file type
            var fileExtension = Path.GetExtension(Photo.FileName).ToLower();
            if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
            {
                ModelState.AddModelError("Photo", "Invalid file type. Please upload a PNG or JPEG image.");
                return View(viewModel);
            }

            try
            {
                var performer = await _context.Performer.FindAsync(id);
                if (performer == null)
                {
                    return NotFound();
                }

                // Check if the image has a 1:1 aspect ratio
                using (var memoryStream = new MemoryStream())
                {
                    await Photo.CopyToAsync(memoryStream);
                    performer.Photo = memoryStream.ToArray();

                    // Reset the memory stream position to the beginning
                    memoryStream.Position = 0;

                    // Use System.Drawing to check the image dimensions
                    using (var image = Image.FromStream(memoryStream))
                    {
                        if (image.Width != image.Height)
                        {
                            ModelState.AddModelError("Photo", "Picture has to be 1:1 ratio.");
                            return View(viewModel);
                        }
                    }
                }

                _context.Update(performer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = performer.PerformerID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while uploading the photo. Please try again.");
                return View(viewModel);
            }
        }

        private bool PerformerExists(int id)
        {
            return _context.Performer.Any(e => e.PerformerID == id);
        }
    }
}
