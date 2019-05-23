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
        public IActionResult Create(string RouteName, string RouteNodes)
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
            var speed = 0;
            var CO2 = 0.00f;
            #region Media Values 
            if (route.RouteName.Contains("TRUCK"))
            {
                speed = 70;     // KM/H
                CO2 = 8.89f;    // GR/KM
            }
            if (route.RouteName.Contains("TRAIN"))
            {
                speed = 50;     // KM/H
                CO2 = 1.89f;    // GR/KM
            }
            if (route.RouteName.Contains("SHIP"))
            {
                speed = 25;     // KM/H
                CO2 = 17.89f;   // GR/KM
            }
            if (route.RouteName.Contains("BARGE"))
            {
                speed = 20;     // KM/H
                CO2 = 17.89f;   // GR/KM
            }
            #endregion
           
            if (ModelState.IsValid)
            {
                route.Route_Id = Guid.NewGuid();

                route.Total_Time = TotalTime(route.Total_KM, speed);
                route.Total_CO2 = TotalEmission(route.Total_KM, CO2);

                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(route);
        }

        private int TotalTime(int km, int speed)
        {
            var time = 0;

            if (km !=0 && speed !=0)
            {
                return time = km / speed;
            }
            
            return (int)time;
        }

        private float TotalEmission(int km, float co2)
        {
            var emission = 0.00f;

            if (km !=0 && co2 !=0.00f)
            {
                return emission = km * co2;
            }
            
            return emission;
        }
    }
}
