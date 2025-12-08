using Microsoft.AspNetCore.Mvc;

namespace FitnessCenter.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
