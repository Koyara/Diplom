using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Editor")]
    public class ContributorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContributorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contributor
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contributor.Include(c => c.Country);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Contributor/Details/5
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

        // GET: Contributor/Create
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult Create()
        {
            var jwt = HttpContext.Request.Headers.ToString();

            Console.WriteLine($"🔑 JWT: {jwt}");

            Console.WriteLine("-----------------------");
            ViewBag.IsMaleOptions = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Not Specified" },
                new SelectListItem { Value = "true", Text = "Male" },
                new SelectListItem { Value = "false", Text = "Female" }
            }, "Value", "Text");

            var countries = _context.Country
                .Select(gn => new CountryViewModel
                {
                    CountryCode = gn.CountryCode,
                    CountryName = gn.CountryName
                }).ToList();

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");


            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode");
            return View();
        }

        // POST: Contributor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Create([Bind("ContributorID,FullName,BirthDate,CountryCode,IsMale")] Contributor contributor)
        {
            if (ModelState.IsValid)
            {
                _context.Contributor.Add(contributor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.IsMaleOptions = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Not Specified" },
                new SelectListItem { Value = "true", Text = "Male" },
                new SelectListItem { Value = "false", Text = "Female" }
            }, "Value", "Text");

            var countries = _context.Country
                .Select(gn => new CountryViewModel
                {
                    CountryCode = gn.CountryCode,
                    CountryName = gn.CountryName
                }).ToList();

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", contributor.CountryCode);
            return View(contributor);
        }

        // GET: Contributor/Edit/5
        [Authorize(Roles = "Editor,Admin")]
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

            ViewBag.IsMaleOptions = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Not Specified" },
                new SelectListItem { Value = "true", Text = "Male" },
                new SelectListItem { Value = "false", Text = "Female" }
            }, "Value", "Text");

            var countries = _context.Country
                .Select(gn => new CountryViewModel
                {
                    CountryCode = gn.CountryCode,
                    CountryName = gn.CountryName
                }).ToList();

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", contributor.CountryCode);
            return View(contributor);
        }

        // POST: Contributor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editor,Admin")]
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
                    _context.Contributor.Update(contributor);
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

            ViewBag.IsMaleOptions = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Not Specified" },
                new SelectListItem { Value = "true", Text = "Male" },
                new SelectListItem { Value = "false", Text = "Female" }
            }, "Value", "Text");

            var countries = _context.Country
                .Select(gn => new CountryViewModel
                {
                    CountryCode = gn.CountryCode,
                    CountryName = gn.CountryName
                }).ToList();

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", contributor.CountryCode);
            return View(contributor);
        }

        // GET: Contributor/Delete/5
        [Authorize(Roles = "Editor,Admin")]
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

        // POST: Contributor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editor,Admin")]
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
