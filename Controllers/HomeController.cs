using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Algoromida_01.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Algoromida_01.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
	private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _logger = logger;
	    _emailSender = emailSender;
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

        public IActionResult Contact()
        {
            _logger.LogInformation("viewing contact");
            return View();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeliverContact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("sending contact");
                await _emailSender.SendEmailAsync("contact@algoromida.com", "A message from a web visitor: ", contact.FullName + " " + contact.Email + " " + contact.Message);
                return RedirectToAction("SuccessMessage");
            }
            return RedirectToAction("FailMessage");
        }

        public IActionResult SuccessMessage()
        {
            _logger.LogInformation("contact succeeded");
            return View();
        }

        public IActionResult FailMessage()
        {
            _logger.LogInformation("contact failed");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
