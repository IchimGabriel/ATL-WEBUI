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
    public class ShipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [Authorize(Roles = "Admin, Broker")]
        public async Task<IActionResult> Index()
        {
            var list = await _context.Shipments.ToListAsync();

            if (!User.Identity.IsAuthenticated && (!User.IsInRole("Admin") || !User.IsInRole("Broker")))
            {
                return RedirectToAction("PageUnauthorise", "Home");
            }

            return View(list);
        }

        [Authorize(Roles = "Admin, Broker")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (!User.Identity.IsAuthenticated && (!User.IsInRole("Admin") || !User.IsInRole("Broker")))
            {
                return RedirectToAction("PageUnauthorise", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(m => m.Shipment_Id == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        [Authorize(Roles = "Broker")]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated && (!User.IsInRole("Broker")))
            {
                return RedirectToAction("PageUnauthorise", "Home");
            }
            var users = _context.Users.ToList();
            var addresses = _context.Addresses.ToList();
            var routes = _context.Routes.ToList();

            ViewBag.Routes = routes;
            ViewBag.Addresses = addresses;
            ViewBag.Users = users;
            return View();
        }

        
        [HttpPost]
        [Authorize(Roles = "Broker")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Shipment_Id,Customer_Id,Employee_Id,Status,Address_From_Id,Address_To_Id,Created_Date,Departure_Date,Arrival_Date,Total_Price,Route_Id")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                shipment.Shipment_Id = Guid.NewGuid();
                _context.Add(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shipment);
        }
    }
}
