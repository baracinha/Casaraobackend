using Microsoft.AspNetCore.Mvc;

namespace hotel.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
