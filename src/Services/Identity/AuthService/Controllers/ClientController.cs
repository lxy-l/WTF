using IdentityServer7.EntityFramework.Storage.DbContexts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly ConfigurationDbContext _context;
        public ClientController(ConfigurationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.Clients.Include(x=>x.AllowedGrantTypes).ToListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _context.Clients.FindAsync(id);
            return View(model);
        }
    }
}
