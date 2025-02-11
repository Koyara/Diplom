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

        // GET: Performers/Create
        public IActionResult Create()
        {
            var genres = _context.Genre
                .Select(gn => new GenreViewModel
                {
                    GenreID = gn.GenreID,
                    GenreName = gn.GenreName
                }).ToList();
            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");
            ViewBag.SecondaryGenre = new SelectList(genres, "GenreID", "GenreName");



            //ViewData["MainGenreID"] = new SelectList(_context.Genre, "GenreID", "GenreID");
            //ViewData["PerformerTypeID"] = new SelectList(_context.PerformerType, "PerformerTypeID", "PerformerTypeID");

            var performerTypes = _context.PerformerType
            .Select(pt => new PerformerTypeViewModel
            {
                PerformerTypeID = pt.PerformerTypeID,
                TypeName = pt.TypeName
            }).ToList();

            ViewBag.PerformerTypes = new SelectList(performerTypes, "PerformerTypeID", "TypeName");
            return View();

            ViewData["SecondaryGenreID"] = new SelectList(_context.Genre, "GenreID", "GenreID");
            return View();
        }

        // POST: Performers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PerformerID,PerformerTypeID,Name,Description,MainGenreID,SecondaryGenreID")] Performer performer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(performer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MainGenreID"] = new SelectList(_context.Genre, "GenreID", "GenreID", performer.MainGenreID);
            //ViewData["PerformerTypeID"] = new SelectList(_context.PerformerType, "PerformerTypeID", "PerformerTypeID", performer.PerformerTypeID);

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

            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");

            ViewBag.PerformerTypes = new SelectList(performerTypes, "PerformerTypeID", "TypeName");

            ViewData["SecondaryGenreID"] = new SelectList(_context.Genre, "GenreID", "GenreID", performer.SecondaryGenreID);
            return View(performer);
        }

        // GET: Performers/Edit/5
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

            var genres = _context.Genre
                .Select(gn => new GenreViewModel
                {
                    GenreID = gn.GenreID,
                    GenreName = gn.GenreName
                }).ToList();

            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName");



            ViewData["MainGenreID"] = new SelectList(_context.Genre, "GenreID", "GenreID", performer.MainGenreID);
            //ViewData["PerformerTypeID"] = new SelectList(_context.PerformerType, "PerformerTypeID", "PerformerTypeID", performer.PerformerTypeID);

            var performerTypes = _context.PerformerType
            .Select(pt => new PerformerTypeViewModel
            {
                PerformerTypeID = pt.PerformerTypeID,
                TypeName = pt.TypeName
            }).ToList();

            ViewBag.PerformerTypes = new SelectList(performerTypes, "PerformerTypeID", "TypeName");

            ViewData["SecondaryGenreID"] = new SelectList(_context.Genre, "GenreID", "GenreID", performer.SecondaryGenreID);
            return View(performer);
        }

        // POST: Performers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PerformerID,PerformerTypeID,Name,Description,MainGenreID,SecondaryGenreID")] Performer performer)
        {
            if (id != performer.PerformerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["MainGenreID"] = new SelectList(_context.Genre, "GenreID", "GenreID", performer.MainGenreID);
            ViewData["PerformerTypeID"] = new SelectList(_context.PerformerType, "PerformerTypeID", "PerformerTypeID", performer.PerformerTypeID);
            ViewData["SecondaryGenreID"] = new SelectList(_context.Genre, "GenreID", "GenreID", performer.SecondaryGenreID);
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

        private bool PerformerExists(int id)
        {
            return _context.Performer.Any(e => e.PerformerID == id);
        }
    }
}
