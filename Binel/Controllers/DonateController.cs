using Microsoft.AspNetCore.Mvc;

namespace Binel.Controllers
{
    public class DonateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
