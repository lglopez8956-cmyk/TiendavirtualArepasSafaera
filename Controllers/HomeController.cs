using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // 🚨 Asegúrate de incluir este using para leer las sesiones
using System.Diagnostics;
using TiendavirtualArepasSafaera.Models;

namespace TiendavirtualArepasSafaera.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // PROTEGEMOS EL HOME (INDEX)
        public IActionResult Index() //
        {
            if (HttpContext.Session.GetString("Usuario") == null) 
            {
                return RedirectToAction("Index", "Login"); 
            }

            return View(); 
        }

        public IActionResult Privacy()
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