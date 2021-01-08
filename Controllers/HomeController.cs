using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Algoromida_01.Models;
using Algoromida_01.Services;

namespace Algoromida_01.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IAlgoromidaService algoromidaService)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

	public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Manifesto()
        {
            return View();
        }

        public IActionResult Clusters()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
