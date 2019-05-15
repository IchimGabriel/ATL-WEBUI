using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ATL_WebUI.Data;
using ATL_WebUI.Models.SQL;
using ATL_WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ATL_WebUI.Controllers
{
    [Produces("application/json")]
    [Authorize(Roles = "Admin, Broker")]
    public class ContainersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IShipmentApiClient _client;

        public ContainersController(ApplicationDbContext context, IShipmentApiClient client)
        {
            _context = context;
            _client = client;
        }

        /// <summary>
        /// Implements the API - Route => api/containers
        /// </summary>
        /// <returns></returns>
        // GET: Containers
        
        
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated || User.IsInRole("Admin"))
            {
                return View(await _client.GetAllContainersAsync()); 
            }
            
            return Unauthorized();
        }

        //GET: Containers/Details/3af6eb9b-bf45-46e9-94c5-26e6ad69ecf2
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var container = await _context.Containers
                .FirstOrDefaultAsync(m => m.Unit_Id == id);
            if (container == null)
            {
                return NotFound();
            }

            return View(container);
        }

        // GET: Containers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Containers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Unit_Id,Name, Description")] Container container)
        {
            if (ModelState.IsValid)
            {
                container.Unit_Id = Guid.NewGuid();
                _context.Add(container);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(container);
        }
  
        // GET: Containers/Edit/3af6eb9b-bf45-46e9-94c5-26e6ad69ecf2
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var container = await _client.EditContainer(id);

            if (container == null)
            {
                return NotFound();
            }
            return View(container);
        }

        // POST: Containers/Edit/3af6eb9b-bf45-46e9-94c5-26e6ad69ecf2
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Unit_Id,Name, Description")] Container container)
        {
            if (id != container.Unit_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _client.SaveEdit(container.Unit_Id, container);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContainerExists(container.Unit_Id))
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
            return View(container);
        }

        // GET: Containers/Delete/3af6eb9b-bf45-46e9-94c5-26e6ad69ecf2
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var container = await _context.Containers
                .FirstOrDefaultAsync(m => m.Unit_Id == id);
            if (container == null)
            {
                return NotFound();
            }

            return View(container);
        }

        // POST: Containers/Delete/3af6eb9b-bf45-46e9-94c5-26e6ad69ecf2
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var container = await _context.Containers.FindAsync(id);
            _context.Containers.Remove(container);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContainerExists(Guid id)
        {
            return _context.Containers.Any(e => e.Unit_Id == id);
        }
    }
}
