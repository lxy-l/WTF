using IdentityServer7.EntityFramework.Storage.DbContexts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [Authorize]
    public class ApiResourceController : Controller
    {
        private readonly ConfigurationDbContext _context;
        public ApiResourceController(ConfigurationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _context.ApiResources.ToListAsync();
            return View(list);
        }
    }
}
