using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    public class IdentityResourceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
