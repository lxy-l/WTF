using IdentityServer7.EntityFramework.Storage.DbContexts;

using Microsoft.AspNetCore.Authorization;
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
    }
}
