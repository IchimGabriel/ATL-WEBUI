using ATL_WebUI.Models;
using ATL_WebUI.Models.SQL;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ATL_WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Guest");
            }

            if (User.IsInRole("Admin"))
            {
                return View("IndexA", "_LayoutAdmin");
            }

            if (User.IsInRole("Broker"))
            {
                return View("IndexB", "_LayoutAdmin");
            }

            if (User.IsInRole("Customer"))
            {
                return View("IndexC", "_LayoutUser");
            }

            return null;
        }


        public IActionResult Guest()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [Route("404")]
        public IActionResult Page404()
        {
            return View();
        }

        [Route("302")]
        public IActionResult PageUnauthorise()
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
