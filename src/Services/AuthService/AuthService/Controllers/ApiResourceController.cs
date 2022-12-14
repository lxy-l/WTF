using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    public class ApiResourceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
