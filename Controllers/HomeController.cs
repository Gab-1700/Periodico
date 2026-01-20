using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Periodico.Models;

namespace Periodico.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
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
        public IActionResult Noticia1()
        {
            return View();
        }
        public IActionResult Noticia2()
        {
            var url= Url.Action("Noticia1");
            return RedirectToAction("Noticia1");
        }

        public IActionResult Noticia3()
        {
            return View();
        }

        public IActionResult Noticia4()
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
