using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    [Route("Catalogue")]
    public class CatalogueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogueController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Catalogue
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var releasesQuery = _context.Release
                .Include(r => r.MainGenre)
                .Include(r => r.SecondGenre)
                .Include(r => r.ReleaseType)
                .OrderByDescending(r => r.ReleaseDate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                releasesQuery = releasesQuery.Where(r =>
                    r.Title.Contains(searchTerm) ||
                    (r.MainGenre != null && r.MainGenre.GenreName.Contains(searchTerm)) ||
                    (r.ReleaseType != null && r.ReleaseType.ReleaseTypeName.Contains(searchTerm)));
            }

            var viewModel = await releasesQuery.Select(r => new ReleaseCatalogueViewModel
            {
                ReleaseID = r.ReleaseID,
                Title = r.Title,
                ReleaseDate = r.ReleaseDate,
                MainGenre = r.MainGenre != null ? r.MainGenre.GenreName : null,
                ReleaseType = r.ReleaseType != null ? r.ReleaseType.ReleaseTypeName : null,
                CoverImageUrl = r.ReleaseCover != null ?
                    $"data:image/jpeg;base64,{Convert.ToBase64String(r.ReleaseCover)}" :
                    "/images/default-cover.jpg"
            }).ToListAsync();

            ViewBag.SearchTerm = searchTerm;
            return View(viewModel);
        }
    }
}