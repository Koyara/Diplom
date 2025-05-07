using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : EditorController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager) 
            : base(context)
        {
            _userManager = userManager;
        }

        public override IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisterEditor()
        {
            return RedirectToPage("/Account/Register", new { area = "Identity" });
        }
    }
} 