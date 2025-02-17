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
            var applicationDbContext = _context.Track.Include(t => t.Language).Include(t => t.Scale).Include(t => t.SecondGuest);
            return View(await applicationDbContext.ToListAsync());
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

            //ViewData["LanguageCode"] = new SelectList(_context.Language, "LanguageCode", "LanguageCode");
            //ViewData["ScaleID"] = new SelectList(_context.Scale, "ScaleId", "ScaleId");
            //ViewData["SecondGuestID"] = new SelectList(_context.Performer, "PerformerID", "PerformerID");
            return View();
        }

        // POST: Tracks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrackID,Length,IsSong,Title,GuestID,SecondGuestID,LanguageCode,ScaleId")] Track track)
        {
            if (ModelState.IsValid)
            {
                _context.Add(track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

           
            //ViewData["LanguageCode"] = new SelectList(_context.Language, "LanguageCode", "LanguageCode", track.LanguageCode);
            //ViewData["ScaleID"] = new SelectList(_context.Scale, "ScaleId", "ScaleId", track.ScaleID);
            //ViewData["SecondGuestID"] = new SelectList(_context.Performer, "PerformerID", "PerformerID", track.SecondGuestID);
            return View(track);
        }

        // GET: Tracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Track.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            //ViewData["LanguageCode"] = new SelectList(_context.Language, "LanguageCode", "LanguageCode", track.LanguageCode);
            //ViewData["ScaleID"] = new SelectList(_context.Scale, "ScaleId", "ScaleId", track.ScaleID);
            //ViewData["SecondGuestID"] = new SelectList(_context.Performer, "PerformerID", "PerformerID", track.SecondGuestID);
            return View(track);
        }

        // POST: Tracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrackID,Length,IsSong,Title,GuestID,SecondGuestID,LanguageCode,ScaleID")] Track track)
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


            //ViewData["LanguageCode"] = new SelectList(_context.Language, "LanguageCode", "LanguageCode", track.LanguageCode);
            //ViewData["ScaleID"] = new SelectList(_context.Scale, "ScaleId", "ScaleId", track.ScaleID);
            //ViewData["SecondGuestID"] = new SelectList(_context.Performer, "PerformerID", "PerformerID", track.SecondGuestID);
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
