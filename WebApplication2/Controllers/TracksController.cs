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
    public class TracksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TracksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tracks
        public async Task<IActionResult> Index()
        {
            var tracks = await _context.Track
         /*       .Include(t => t.Language)
                .Include(t => t.Scale)
                .Include(t => t.MainGuest)
                .Include(t => t.SecondGuest)*/
                .AsNoTracking()
                .ToListAsync();

            return View(tracks);
        }

        // GET: Tracks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Track
                .Include(t => t.Language)
                .Include(t => t.Scale)
                .Include(t => t.MainGuest)
                .Include(t => t.SecondGuest)
                .FirstOrDefaultAsync(m => m.TrackID == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // GET: Tracks/Create
        public IActionResult Create()
        {
            ViewBag.Guests = new SelectList(_context.Performer.Select(p => new { p.PerformerID, p.Name }), "PerformerID", "Name");
            ViewBag.Languages = new SelectList(_context.Language.Select(l => new { l.LanguageCode, l.LanguageName }), "LanguageCode", "LanguageName");
            ViewBag.Scales = new SelectList(_context.Scale.Select(s => new { s.ScaleId, s.Name }), "ScaleId", "Name");
            return View();
        }

        // POST: Tracks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrackID,Title,Length,IsSong,Lyrics,GuestID,SecondGuestID,LanguageCode,ScaleID")] Track track)
        {
            if (ModelState.IsValid)
            {
                _context.Add(track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Guests = new SelectList(_context.Performer.Select(p => new { p.PerformerID, p.Name }), "PerformerID", "Name");
            ViewBag.Languages = new SelectList(_context.Language.Select(l => new { l.LanguageCode, l.LanguageName }), "LanguageCode", "LanguageName");
            ViewBag.Scales = new SelectList(_context.Scale.Select(s => new { s.ScaleId, s.Name }), "ScaleId", "Name");
            return View(track);
        }

        // GET: Tracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Track
                .Include(t => t.Language)
                .Include(t => t.Scale)
                .Include(t => t.MainGuest)
                .Include(t => t.SecondGuest)
                .FirstOrDefaultAsync(m => m.TrackID == id);

            if (track == null)
            {
                return NotFound();
            }

            ViewBag.Guests = new SelectList(_context.Performer.Select(p => new { p.PerformerID, p.Name }), "PerformerID", "Name");
            ViewBag.Languages = new SelectList(_context.Language.Select(l => new { l.LanguageCode, l.LanguageName }), "LanguageCode", "LanguageName");
            ViewBag.Scales = new SelectList(_context.Scale.Select(s => new { s.ScaleId, s.Name }), "ScaleId", "Name");

            return View(track);
        }

        // POST: Tracks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrackID,Title,Length,IsSong,Lyrics,GuestID,SecondGuestID,LanguageCode,ScaleID")] Track track)
        {
            if (id != track.TrackID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(track);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackExists(track.TrackID))
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

            ViewBag.Guests = new SelectList(_context.Performer.Select(p => new { p.PerformerID, p.Name }), "PerformerID", "Name");
            ViewBag.Languages = new SelectList(_context.Language.Select(l => new { l.LanguageCode, l.LanguageName }), "LanguageCode", "LanguageName");
            ViewBag.Scales = new SelectList(_context.Scale.Select(s => new { s.ScaleId, s.Name }), "ScaleId", "Name");
            return View(track);
        }

        // GET: Tracks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Track
                .Include(t => t.Language)
                .Include(t => t.Scale)
                .Include(t => t.SecondGuest)
                .FirstOrDefaultAsync(m => m.TrackID == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var track = await _context.Track.FindAsync(id);
            if (track != null)
            {
                _context.Track.Remove(track);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackExists(int id)
        {
            return _context.Track.Any(e => e.TrackID == id);
        }



        [HttpGet]
        public IActionResult LoadGuests()
        {
            var guests = _context.Performer.Select(p => new { p.PerformerID, p.Name }).ToList();
            var selectList = new SelectList(guests, "PerformerID", "Name");
            return PartialView("_GuestSelectList", selectList);
        }

        [HttpGet]
        public IActionResult LoadLanguages()
        {
            var languages = _context.Language.Select(l => new { l.LanguageCode, l.LanguageName }).ToList();
            var selectList = new SelectList(languages, "LanguageCode", "LanguageName");
            return PartialView("_LanguageSelectList", selectList);
        }

        [HttpGet]
        public IActionResult LoadScales()
        {
            var scales = _context.Scale.Select(s => new { s.ScaleId, s.Name }).ToList();
            var selectList = new SelectList(scales, "ScaleId", "Name"); // Ensure property names match
            return PartialView("_ScaleSelectList", selectList);
        }
    }
}
