using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
