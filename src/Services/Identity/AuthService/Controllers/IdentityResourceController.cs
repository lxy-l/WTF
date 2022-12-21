using IdentityServer7.EntityFramework.Storage.DbContexts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [Authorize]
    public class IdentityResourceController : Controller
    {
        private readonly ConfigurationDbContext _context;
        public IdentityResourceController(ConfigurationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var list= await _context.IdentityResources.ToListAsync();
            return View(list);
        }
    }
}
