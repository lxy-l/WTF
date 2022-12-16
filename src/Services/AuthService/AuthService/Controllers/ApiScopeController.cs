using IdentityServer7.EntityFramework.Storage.DbContexts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [Authorize]
    public class ApiScopeController : Controller
    {
        private readonly ConfigurationDbContext _context;
        public ApiScopeController(ConfigurationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var list = await _context.ApiScopes.ToListAsync();
            return View(list);
        }
    }
}
