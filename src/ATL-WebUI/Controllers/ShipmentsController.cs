﻿using ATL_WebUI.Data;
using ATL_WebUI.Models.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Controllers
{
    public class ShipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shipments
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

        // GET: Shipments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
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

        // GET: Shipments/Create
        public IActionResult Create()
        {
            var users = _context.Users.ToList();
            var addresses = _context.Addresses.ToList();
            var routes = _context.Routes.ToList();

            ViewBag.Routes = routes;
            ViewBag.Addresses = addresses;
            ViewBag.Users = users;
            return View();
        }

        // POST: Shipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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

        // GET: Shipments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }
            return View(shipment);
        }

        // POST: Shipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Shipment_Id,Customer_Id,Employee_Id,Status,Address_From_Id,Address_To_Id,Created_Date,Departure_Date,Arrival_Date,Total_Price,Route_Id")] Shipment shipment)
        {
            if (id != shipment.Shipment_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentExists(shipment.Shipment_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shipment);
        }

        // GET: Shipments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
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

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShipmentExists(Guid id)
        {
            return _context.Shipments.Any(e => e.Shipment_Id == id);
        }
    }
}
