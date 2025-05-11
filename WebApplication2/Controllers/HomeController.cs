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

        public async Task<IActionResult> Index(string searchTerm, string searchType, 
            string language, int? minBpm, int? maxBpm, int? scale, bool? isSong, 
            int? minLength, int? maxLength)
        {
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SearchType = searchType;

            // Set filter values for the view
            ViewBag.SelectedLanguage = language;
            ViewBag.MinBpm = minBpm;
            ViewBag.MaxBpm = maxBpm;
            ViewBag.SelectedScale = scale;
            ViewBag.IsSong = isSong;
            ViewBag.MinLength = minLength;
            ViewBag.MaxLength = maxLength;

            // Load filter options
            ViewBag.Languages = await _context.Language.ToListAsync();
            ViewBag.Scales = await _context.Scale.ToListAsync();

            var viewModel = new SearchResultsViewModel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                switch (searchType?.ToLower())
                {
                    case "tracks":
                        var tracksQuery = _context.Track
                            .Include(t => t.Language)
                            .Include(t => t.Scale)
                            .Include(t => t.TrackPerformers)
                                .ThenInclude(tp => tp.Performer)
                            .AsQueryable();

                        // Apply filters
                        if (!string.IsNullOrEmpty(language))
                            tracksQuery = tracksQuery.Where(t => t.LanguageCode == language);
                        
                        if (minBpm.HasValue)
                            tracksQuery = tracksQuery.Where(t => t.BPM.HasValue && t.BPM.Value >= minBpm);
                        
                        if (maxBpm.HasValue)
                            tracksQuery = tracksQuery.Where(t => t.BPM.HasValue && t.BPM.Value <= maxBpm);
                        
                        if (scale.HasValue)
                            tracksQuery = tracksQuery.Where(t => t.ScaleID == scale);
                        
                        if (isSong.HasValue)
                            tracksQuery = tracksQuery.Where(t => t.IsSong == isSong);
                        
                        if (minLength.HasValue)
                            tracksQuery = tracksQuery.Where(t => t.Length.HasValue && t.Length.Value.TotalSeconds >= minLength);
                        
                        if (maxLength.HasValue)
                            tracksQuery = tracksQuery.Where(t => t.Length.HasValue && t.Length.Value.TotalSeconds <= maxLength);

                        // Apply search term
                        tracksQuery = tracksQuery.Where(t => 
                            t.Title.Contains(searchTerm) || 
                            t.TrackPerformers.Any(tp => tp.Performer.Name.Contains(searchTerm)));

                        viewModel.Tracks = await tracksQuery.Select(t => new TrackSearchResult
                        {
                            TrackID = t.TrackID,
                            Title = t.Title,
                            ArtistName = t.TrackPerformers.FirstOrDefault() != null ? 
                                t.TrackPerformers.First().Performer.Name : "Unknown"
                        }).ToListAsync();
                        break;

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
