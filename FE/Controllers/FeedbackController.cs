using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
