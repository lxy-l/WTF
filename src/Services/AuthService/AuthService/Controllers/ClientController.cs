using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
