using Microsoft.AspNetCore.Mvc;

namespace hotel.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
