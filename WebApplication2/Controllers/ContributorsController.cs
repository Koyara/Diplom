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
    public class ContributorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContributorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contributors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contributor.Include(c => c.Country);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Contributors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contributor = await _context.Contributor
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.ContributorID == id);
            if (contributor == null)
            {
                return NotFound();
            }

            return View(contributor);
        }

        // GET: Contributors/Create
        public IActionResult Create()
        {
            //ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode");
            var countries = _context.Country
                .Select(gn => new CountryViewModel
                {
                    CountryCode = gn.CountryCode,
                    CountryName = gn.CountryName
                }).ToList();

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");
            return View();
        }

        // POST: Contributors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContributorID,FullName,BirthDate,CountryCode,IsMale")] Contributor contributor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contributor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", contributor.CountryCode);
            return View(contributor);
        }

        // GET: Contributors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contributor = await _context.Contributor.FindAsync(id);
            if (contributor == null)
            {
                return NotFound();
            }
            //ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", contributor.CountryCode);
            var countries = _context.Country
                .Select(gn => new CountryViewModel
                {
                    CountryCode = gn.CountryCode,
                    CountryName = gn.CountryName
                }).ToList();

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");
            return View(contributor);
        }

        // POST: Contributors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContributorID,FullName,BirthDate,CountryCode,IsMale")] Contributor contributor)
        {
            if (id != contributor.ContributorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contributor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContributorExists(contributor.ContributorID))
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
            //ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", contributor.CountryCode);
            var countries = _context.Country
                .Select(gn => new CountryViewModel
                {
                    CountryCode = gn.CountryCode,
                    CountryName = gn.CountryName
                }).ToList();

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");
            return View(contributor);
        }

        // GET: Contributors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contributor = await _context.Contributor
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.ContributorID == id);
            if (contributor == null)
            {
                return NotFound();
            }

            return View(contributor);
        }

        // POST: Contributors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contributor = await _context.Contributor.FindAsync(id);
            if (contributor != null)
            {
                _context.Contributor.Remove(contributor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContributorExists(int id)
        {
            return _context.Contributor.Any(e => e.ContributorID == id);
        }
    }
}
