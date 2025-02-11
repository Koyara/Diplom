using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class RondoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RondoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rondoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rondo.ToListAsync());
        }

        // GET: Rondoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rondo = await _context.Rondo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rondo == null)
            {
                return NotFound();
            }

            return View(rondo);
        }

        // GET: Rondoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rondoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Rondo rondo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rondo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rondo);
        }

        // GET: Rondoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rondo = await _context.Rondo.FindAsync(id);
            if (rondo == null)
            {
                return NotFound();
            }
            return View(rondo);
        }

        // POST: Rondoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Rondo rondo)
        {
            if (id != rondo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rondo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RondoExists(rondo.Id))
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
            return View(rondo);
        }

        // GET: Rondoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rondo = await _context.Rondo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rondo == null)
            {
                return NotFound();
            }

            return View(rondo);
        }

        // POST: Rondoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rondo = await _context.Rondo.FindAsync(id);
            if (rondo != null)
            {
                _context.Rondo.Remove(rondo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RondoExists(int id)
        {
            return _context.Rondo.Any(e => e.Id == id);
        }
    }
}
