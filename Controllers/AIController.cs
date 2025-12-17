using FitnessCenter.Models;
using FitnessCenter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenter.Controllers
{
    [Authorize] // Ensure only logged-in users can use AI features
    public class AIController : Controller
    {
        private readonly IAIService _aiService;

        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Plan()
        {
            return View(new FitnessPlanRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Plan(FitnessPlanRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _aiService.GenerateFitnessPlanAsync(request);
            ViewBag.Result = result;
            return View(request);
        }

        [HttpGet]
        public IActionResult Visualization()
        {
            return View(new VisualizationRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Visualization(VisualizationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _aiService.GenerateVisualizationAsync(request);
            ViewBag.Result = result;
            return View(request);
        }
    }
}
