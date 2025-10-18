using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult History()
        {
            return View();
        }
    }
}
