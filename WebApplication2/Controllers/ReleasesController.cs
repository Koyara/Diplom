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
            return View();
        }

        // POST: Releases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Release release, IFormFile ReleaseCover)
        {
            var releasetypes = _context.ReleaseType
                .Select(gn => new ReleaseTypeViewModel
                {
                    ReleaseTypeID = gn.ReleaseTypeID,
                    ReleaseTypeName = gn.ReleaseTypeName
                }).ToList();

            ViewBag.ReleaseTypes = new SelectList(releasetypes, "ReleaseTypeID", "ReleaseTypeName");

            if (ModelState.IsValid)
            {
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
                        release.ReleaseCover = memoryStream.ToArray(); // Convert to byte array

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

                _context.Add(release);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(release);
        }
        //public async Task<IActionResult> Create([Bind("ReleaseID,Title,ReleaseDate,MainGenreCode,SecondGenreCode,ReleaseCover")] Release release, IFormFile ReleaseCover)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (ReleaseCover != null && ReleaseCover.Length > 0)
        //        {
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                await ReleaseCover.CopyToAsync(memoryStream);
        //                release.ReleaseCover = memoryStream.ToArray(); // Convert to byte array
        //            }
        //        }

        //        _context.Add(release);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(release);
        //}

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
            return View(release);
        }

        // POST: Releases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReleaseID,Title,ReleaseDate,MainGenreCode,SecondGenreCode,ReleaseCover")] Release release)
        {
            if (id != release.ReleaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(release);
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
    }
}
