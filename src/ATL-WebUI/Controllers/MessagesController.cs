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
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Return all messages for a Shipment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(Guid id)
        {
            if (!User.Identity.IsAuthenticated && (!User.IsInRole("Customer")))
            {
                return RedirectToAction("PageUnauthorise", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var messages = await _context.Messages.Where(m => m.Shipment_Id == id).ToListAsync();

            return View(messages);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Message_Id,Shipment_Id,Details")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.Message_Id = Guid.NewGuid();
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }
    }
}
