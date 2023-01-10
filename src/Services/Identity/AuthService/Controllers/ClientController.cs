using IdentityServer7.EntityFramework.Storage.DbContexts;
using IdentityServer7.EntityFramework.Storage.Entities;

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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == default)
            {
                return NotFound();
            }

            var model = await _context.Clients.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, Client model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var client = await _context.Clients.FindAsync(id);
                if (client == null)
                {
                    return NotFound();
                }
                var result = _context.Update(client);
                if (_context.SaveChanges() == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _context.Clients.FindAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == default || _context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'User'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                var result = _context.Clients.Remove(client);
                if (_context.SaveChanges()==1)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return NotFound();
        }
    }
}
