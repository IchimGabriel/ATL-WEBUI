using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATL_WebUI.Models;
using ATL_WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATL_WebUI.Controllers
{
    public class CityController : Controller
    {
        private readonly INeo4jApiClient _client;

        public CityController(INeo4jApiClient client)
        {
            _client = client;
        }
        /// <summary>
        /// GET: Cities
        /// </summary>
        /// <returns>List of all CITIES in Neo4j DB</returns>

        public async Task<IActionResult> Index()
        {
            var cities = await _client.GetAllCitiesAsync();
            return View(cities);
        }

        [HttpGet]
        public async Task<IActionResult> GetNeighbours()
        {
            var cities = await _client.TruckConnectedCityNeighbours();
            return View(cities);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Latitude,Longitude,iso,Port,Turnaround")] City city)
        {
            if (ModelState.IsValid)
            {
                city.Id = Guid.NewGuid();
                
                //_client.Add(city);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }
    }
}