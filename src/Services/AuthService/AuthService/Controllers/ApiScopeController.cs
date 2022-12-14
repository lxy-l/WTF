using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    public class ApiScopeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
