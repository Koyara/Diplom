using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm, string searchType = "releases")
        {
            var viewModel = new SearchResultsViewModel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                switch (searchType.ToLower())
                {
                    case "releases":
                        var releases = await _context.Release
                            .Include(r => r.ReleaseType)
                            .Where(r => r.Title.ToLower().Contains(searchTerm) ||
                                      r.ReleaseType.ReleaseTypeName.ToLower().Contains(searchTerm))
                            .ToListAsync();

                        viewModel.Releases = releases.Select(r => new ReleaseSearchResult
                        {
                            ReleaseID = r.ReleaseID,
                            Title = r.Title,
                            ReleaseType = r.ReleaseType?.ReleaseTypeName ?? "Unknown",
                            ReleaseDate = r.ReleaseDate,
                            CoverImageUrl = r.ReleaseCover != null ? 
                                $"data:image/jpeg;base64,{Convert.ToBase64String(r.ReleaseCover)}" : 
                                "/images/default-cover.jpg"
                        }).ToList();
                        break;

                    case "artists":
                        var performers = await _context.Performer
                            .Include(p => p.MainGenre)
                            .Where(p => p.Name.ToLower().Contains(searchTerm) ||
                                      p.MainGenre.GenreName.ToLower().Contains(searchTerm))
                            .ToListAsync();

                        viewModel.Artists = performers.Select(p => new PerformerSearchResult
                        {
                            PerformerID = p.PerformerID,
                            Name = p.Name,
                            MainGenre = p.MainGenre?.GenreName ?? "Unknown"
                        }).ToList();
                        break;

                    case "lyrics":
                        var tracks = await _context.Track
                            .Include(t => t.MainGuest)
                            .Where(t => t.Lyrics.ToLower().Contains(searchTerm))
                            .ToListAsync();

                        viewModel.Tracks = tracks.Select(t => new TrackSearchResult
                        {
                            TrackID = t.TrackID,
                            Title = t.Title,
                            ArtistName = t.MainGuest?.Name ?? "Unknown"
                        }).ToList();
                        break;
                }
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.SearchType = searchType;
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
