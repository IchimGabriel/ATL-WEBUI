using ATL_WebUI.Data;
using ATL_WebUI.Models.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Controllers
{
    [Authorize]
    public class ShipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public ShipmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> UserShipments()
        {
            var userName = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            
            var list = await _context.Shipments.Where(s => s.Customer_Id.ToString() == user.Id.ToString()).OrderByDescending(o => o.Created_Date).ToListAsync();

            if (!User.Identity.IsAuthenticated && !User.IsInRole("Customer"))
            {
                return RedirectToAction("PageUnauthorise", "Home");
            }

            return View(list);
        }

        /// <summary>
        /// VALID Shipments List
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Broker")]
        public async Task<IActionResult> Index()
        {
            var list = await _context.Shipments.Where(s => s.Status == Statuses.VALID).OrderByDescending(o => o.Created_Date).ToListAsync();

            if (!User.Identity.IsAuthenticated && (!User.IsInRole("Admin") || !User.IsInRole("Broker")))
            {
                return RedirectToAction("PageUnauthorise", "Home");
            }

            return View(list);
        }

        /// <summary>
        /// TRANSIT Shipment List
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Broker")]
        public async Task<IActionResult> InTransit()
        {
            var list = await _context.Shipments.Where(s => s.Status == Statuses.TRANSIT).OrderByDescending(o => o.Created_Date).ToListAsync();
            return View(list);
        }

        /// <summary>
        /// DELIVERED Shipments List
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Broker")]
        public async Task<IActionResult> IsDelivered()
        {
            var list = await _context.Shipments.Where(s => s.Status == Statuses.DELIVERED).OrderByDescending(o => o.Created_Date).ToListAsync();
            return View(list);
        }


        /// <summary>
        /// Shipment Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(Roles = "Admin, Broker, Customer")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            //if (!User.Identity.IsAuthenticated && (!User.IsInRole("Admin") || !User.IsInRole("Broker")))
            //{
            //    return RedirectToAction("PageUnauthorise", "Home");
            //}

            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(m => m.Shipment_Id == id);

            var from = await _context.Addresses.FirstOrDefaultAsync(a => a.Address_Id == shipment.Address_From_Id);
            var to = await _context.Addresses.FirstOrDefaultAsync(a => a.Address_Id == shipment.Address_To_Id);
            var customerName = await _context.Users.FirstOrDefaultAsync(c => c.Id == shipment.Customer_Id.ToString());
            var detail = await _context.Details.Where(d => d.Shipment_Id == id).ToListAsync();
            var route = await _context.Routes.FirstOrDefaultAsync(r => r.Route_Id == shipment.Route_Id);
            var messages = await _context.Messages.Where(m => m.Shipment_Id == id).ToListAsync();

            var quantity = 0;
            foreach (var item in detail)
            {
                quantity += item.Quantity;
            }

            ViewBag.Detail = quantity;
            ViewBag.Route = route;
            ViewBag.Customer = customerName;
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Messages = messages;

            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        /// <summary>
        /// SET Shipment in TRANSIT state and add a message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Broker")]
        public async Task<IActionResult> Transit(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(m => m.Shipment_Id == id);

            shipment.Status = Statuses.TRANSIT;

            _context.Update(shipment);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == shipment.Customer_Id.ToString());
            var route = await _context.Routes.FirstOrDefaultAsync(r => r.Route_Id == shipment.Route_Id);

            //send message to customer
            var client = new SendGridClient("");
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("ichimgabriel75@gmail.com", "ATL-FF"));

            var recepients = new List<EmailAddress>
            {
                new EmailAddress("ichimgabriel75@gmail.com"),
                new EmailAddress(user.Email.ToString())
            };
            msg.AddTos(recepients);
            msg.SetSubject("Shipment in Transit");
            msg.AddContent(MimeType.Text, $"Shipment: {shipment.Shipment_Id} - is now in TRANSIT. Arrival Date: {shipment.Arrival_Date} has not been changed.");
            await client.SendEmailAsync(msg);

            //save message in db
            var message = $"2. Shipment: {shipment.Shipment_Id} is now in TRANSIT. Arrival Date: {shipment.Arrival_Date} has not been changed."
                + $" On Route: {route.RouteNodes}. User {user.Email} has been notify.";

            var addMessage = new Message()
            {
                Message_Id = Guid.NewGuid(),
                Shipment_Id = shipment.Shipment_Id,
                Details = message
            };
            _context.Add(addMessage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(InTransit));
        }

        /// <summary>
        /// SET Shipment in DELIVERED state and send final message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Broker")]
        public async Task<IActionResult> Delivered(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(m => m.Shipment_Id == id);

            shipment.Status = Statuses.DELIVERED;

            _context.Update(shipment);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == shipment.Customer_Id.ToString());
            var route = await _context.Routes.FirstOrDefaultAsync(r => r.Route_Id == shipment.Route_Id);

            //send message to customer
            var client = new SendGridClient(" ");
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("ichimgabriel75@gmail.com", "ATL-FF"));

            var recepients = new List<EmailAddress>
            {
                new EmailAddress("ichimgabriel75@gmail.com"),
                new EmailAddress(user.Email.ToString())
            };
            msg.AddTos(recepients);
            msg.SetSubject("Shipment Delivered");
            msg.AddContent(MimeType.Text, $"Shipment: {shipment.Shipment_Id} - Delivered.");
            await client.SendEmailAsync(msg);

            //save message in db
            var message = $"3. Shipment: {shipment.Shipment_Id} - DELIVERED. Arrival Date: {shipment.Arrival_Date} has not been changed."
                + $" User {user.Email} has been notify.";

            var addMessage = new Message()
            {
                Message_Id = Guid.NewGuid(),
                Shipment_Id = shipment.Shipment_Id,
                Details = message
            };
            _context.Add(addMessage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(IsDelivered));
        }

        /// <summary>
        /// CREATE Shipment
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> Create()
        {
            if (!User.Identity.IsAuthenticated && (!User.IsInRole("Broker")))
            {
                return RedirectToAction("PageUnauthorise", "Home");
            }

            var users = await _userManager.GetUsersInRoleAsync("Customer");
            var employee = await _userManager.GetUsersInRoleAsync("Broker");
            var addresses = await _context.Addresses.ToListAsync();
            var routes = await _context.Routes.ToListAsync();

            ViewBag.Routes = routes;
            ViewBag.Addresses = addresses;
            ViewBag.Users = users;
            ViewBag.Employee = employee;
            return View();
        }

        /// <summary>
        /// POST Shipment and attach message 
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
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

                var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == shipment.Customer_Id.ToString());
                var route = await _context.Routes.FirstOrDefaultAsync(r => r.Route_Id == shipment.Route_Id);

                //send message to customer
                var client = new SendGridClient("");
                var msg = new SendGridMessage();
                msg.SetFrom(new EmailAddress("ichimgabriel75@gmail.com", "ATL-FF"));

                var recepients = new List<EmailAddress>
            {
                new EmailAddress("ichimgabriel75@gmail.com"),
                new EmailAddress(user.Email.ToString())
            };
                msg.AddTos(recepients);
                msg.SetSubject("Shipment Created");
                msg.AddContent(MimeType.Text, $"Shipment: {shipment.Shipment_Id} - has been created." 
                + $"Departure Date: {shipment.Departure_Date} - Arrival Date: {shipment.Arrival_Date}."
                + $"Is set for Route: {route.RouteNodes}.");
                await client.SendEmailAsync(msg);

                //save message in db
                var message = $"1. Shipment created {shipment.Created_Date} is VALID. Departure Date: {shipment.Departure_Date} - Arrival Date: {shipment.Arrival_Date}." 
                    + $"Is set for Route: {route.RouteNodes}. User {user.Email} has been notify.";

                var addMessage = new Message()
                {
                    Message_Id = Guid.NewGuid(),
                    Shipment_Id = shipment.Shipment_Id,
                    Details = message
                };
                _context.Add(addMessage);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(shipment);
        }
    }
}
