using ATL_WebUI.Data;
using ATL_WebUI.Models.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Controllers
{
    [Authorize]
    public class RoutesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoutesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Broker")]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated && (!User.IsInRole("Admin") || !User.IsInRole("Broker")))
            {
                return RedirectToAction("PageUnauthorise", "Home");
            }

            return View(await _context.Routes.ToListAsync());
        }

        [Authorize(Roles = "Broker")]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated && (!User.IsInRole("Broker")))
            {
                return RedirectToAction("PageUnauthorise", "Home");
            }

            return View();
        }

        
        [HttpPost]
        [Authorize(Roles = "Broker")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Route_Id,RouteNodes,Total_KM,Total_CO2,Total_Time,RouteName")] Route route)
        {
            if (ModelState.IsValid)
            {
                route.Route_Id = Guid.NewGuid();
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(route);
        }
    }
}
