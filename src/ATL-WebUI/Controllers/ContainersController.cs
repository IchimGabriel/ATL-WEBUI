using ATL_WebUI.Data;
using ATL_WebUI.Models.SQL;
using ATL_WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(await _client.GetAllContainersAsync());
            }

            return Unauthorized();
        }

        public IActionResult Create()
        {
            return View();
        }

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

        /// <summary>
        /// Edit using API -> api/containers/edit/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Edit using API -> api/containers/edit/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                    if (container.Unit_Id != null)
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
    }
}
