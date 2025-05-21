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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Index()
        {
            var tracks = await _context.Track
                .Include(t => t.Language)
                .Include(t => t.Scale)
                .Include(t => t.MainGuest)
                .Include(t => t.SecondGuest)
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
                .Include(t => t.TrackProducers)
                    .ThenInclude(tp => tp.Producer)
                .Include(t => t.TrackPerformers)
                    .ThenInclude(tp => tp.Performer)
                .Include(t => t.ReleaseTracks)
                    .ThenInclude(rt => rt.Release)
                .FirstOrDefaultAsync(m => m.TrackID == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // GET: Tracks/Create
        [Authorize(Roles = "Editor,Admin")]
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
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Create([Bind("Title,Length,LanguageCode,ScaleID,GuestID,SecondGuestID,ProducerIDs,PerformerIDs")] Track track)
        {
            if (ModelState.IsValid)
            {
                _context.Add(track);
                await _context.SaveChangesAsync();

                // Add producer relationships
                if (track.ProducerIDs != null)
                {
                    foreach (var producerId in track.ProducerIDs)
                    {
                        var trackProducer = new TrackProducer
                        {
                            TrackID = track.TrackID,
                            ProducerID = producerId
                        };
                        _context.TrackProducer.Add(trackProducer);
                    }
                }

                // Add performer relationships
                if (track.PerformerIDs != null)
                {
                    foreach (var performerId in track.PerformerIDs)
                    {
                        var trackPerformer = new TrackPerformer
                        {
                            TrackID = track.TrackID,
                            PerformerID = performerId
                        };
                        _context.TrackPerformer.Add(trackPerformer);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GuestID"] = new SelectList(_context.Performer, "PerformerID", "Name", track.GuestID);
            ViewData["SecondGuestID"] = new SelectList(_context.Performer, "PerformerID", "Name", track.SecondGuestID);
            ViewData["LanguageCode"] = new SelectList(_context.Language, "LanguageCode", "Name", track.LanguageCode);
            ViewData["ScaleID"] = new SelectList(_context.Scale, "ScaleID", "Name", track.ScaleID);
            return View(track);
        }

        // GET: Tracks/Edit/5
        [Authorize(Roles = "Editor,Admin")]
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
                .Include(t => t.TrackProducers)
                .Include(t => t.TrackPerformers)
                .FirstOrDefaultAsync(m => m.TrackID == id);

            if (track == null)
            {
                return NotFound();
            }

            // Create SelectLists with selected values
            ViewBag.Guests = new SelectList(
                _context.Performer.Select(p => new { p.PerformerID, p.Name }), 
                "PerformerID", 
                "Name", 
                track.GuestID
            );
            
            ViewBag.SecondGuests = new SelectList(
                _context.Performer.Select(p => new { p.PerformerID, p.Name }), 
                "PerformerID", 
                "Name", 
                track.SecondGuestID
            );
            
            ViewBag.Languages = new SelectList(
                _context.Language.Select(l => new { l.LanguageCode, l.LanguageName }), 
                "LanguageCode", 
                "LanguageName", 
                track.LanguageCode
            );
            
            ViewBag.Scales = new SelectList(
                _context.Scale.Select(s => new { s.ScaleId, s.Name }), 
                "ScaleId", 
                "Name", 
                track.ScaleID
            );

            return View(track);
        }

        // POST: Tracks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("TrackID,Title,Length,BPM,IsSong,Lyrics,GuestID,SecondGuestID,LanguageCode,ScaleID")] Track track, int[] ProducerIDs, int[] PerformerIDs)
        {
            if (id != track.TrackID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update track
                    _context.Update(track);

                    // Remove existing producer relationships
                    var existingProducers = await _context.TrackProducer
                        .Where(tp => tp.TrackID == track.TrackID)
                        .ToListAsync();
                    _context.TrackProducer.RemoveRange(existingProducers);

                    // Add new producer relationships
                    if (ProducerIDs != null && ProducerIDs.Any())
                    {
                        foreach (var producerId in ProducerIDs)
                        {
                            var trackProducer = new TrackProducer
                            {
                                TrackID = track.TrackID,
                                ProducerID = producerId
                            };
                            _context.TrackProducer.Add(trackProducer);
                        }
                    }

                    // Remove existing performer relationships
                    var existingPerformers = await _context.TrackPerformer
                        .Where(tp => tp.TrackID == track.TrackID)
                        .ToListAsync();
                    _context.TrackPerformer.RemoveRange(existingPerformers);

                    // Add new performer relationships
                    if (PerformerIDs != null && PerformerIDs.Any())
                    {
                        foreach (var performerId in PerformerIDs)
                        {
                            var trackPerformer = new TrackPerformer
                            {
                                TrackID = track.TrackID,
                                PerformerID = performerId
                            };
                            _context.TrackPerformer.Add(trackPerformer);
                        }
                    }

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
        [Authorize(Roles = "Editor,Admin")]
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
        [Authorize(Roles = "Editor,Admin")]
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

        [HttpGet]
        public async Task<IActionResult> LoadTrackProducers(int id)
        {
            var producerIds = await _context.TrackProducer
                .Where(tp => tp.TrackID == id)
                .Select(tp => tp.ProducerID)
                .ToListAsync();
            
            return Json(producerIds);
        }
    }
}
