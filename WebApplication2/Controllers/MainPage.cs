using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    [Route("MainPage")]
    public class MainPage : Controller
    {
        private readonly ApplicationDbContext _context;

        public MainPage(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var releases = _context.Release
                .Include(r => r.MainGenre)
                .Include(r => r.ReleaseType)
                .ToList();
            return View(releases);
        }

    }
}