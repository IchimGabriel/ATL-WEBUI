using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ATL_WebUI.Data;
using ATL_WebUI.Models.SQL;

namespace ATL_WebUI.Controllers
{
    public class DetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        protected Guid Route_Id { get; set; }

        public DetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Details.ToListAsync());
        }

        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Shipment_Id = id;
            ViewBag.Containers = _context.Containers.ToList();
            return View();
        }

        /// <summary>
        /// Add cargo items to a Shipment order and update the value
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Detail_Id,Shipment_Id,Container_Id,Quantity")] Detail detail)
        {
            if (ModelState.IsValid)
            {
                detail.Detail_Id = Guid.NewGuid();
                _context.Add(detail);
                await _context.SaveChangesAsync();

                var shipment = await _context.Shipments.FirstOrDefaultAsync(m => m.Shipment_Id == detail.Shipment_Id);
                var route = await _context.Routes.FirstOrDefaultAsync(m => m.Route_Id == shipment.Route_Id);

                double value = 0; // value €/km

                if (route.RouteName.Contains("TRUCK"))
                {
                    value = 0.85;
                }
                if (route.RouteName.Contains("TRAIN"))
                {
                    value = 0.45;
                }
                if (route.RouteName.Contains("SHIP"))
                {
                    value = 0.045;
                }
                if (route.RouteName.Contains("BARGE"))
                {
                    value = 0.085;
                }

                var total = detail.Quantity * (route.Total_KM * value);

                await AddCargoValue(detail.Shipment_Id, (decimal)total);

                return RedirectToAction(nameof(Index), "Shipments");
            }
            return View(detail);
        }

        /// <summary>
        /// Update Total Value for a Shipment Order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        [HttpPut]
        private async Task<IActionResult> AddCargoValue(Guid id, decimal total)
        {
            if (id == null)
            {
                return NotFound();
            }
            var shipment = await _context.Shipments.FindAsync(id);

            shipment.Total_Price += total;

            _context.Update(shipment);
            await _context.SaveChangesAsync();

            return null;
        }


    }
}
