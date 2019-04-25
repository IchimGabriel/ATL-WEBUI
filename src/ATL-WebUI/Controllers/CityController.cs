using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATL_WebUI.Models;
using ATL_WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

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

        /// <summary>
        /// GET: Neighbours
        /// </summary>
        /// <returns>List of Neighbour city conected by truck</returns>
        [HttpGet]
        public async Task<IActionResult> GetNeighbours()
        {
            var cities = await _client.TruckConnectedCityNeighbours();
            return View(cities);
        }

        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// GET: City/Create
        /// </summary>
        /// <returns>Imput new City Parameters</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: City/Create
        /// </summary>
        /// <param name="city"></param>
        /// <returns>Create new City / Node using POST -> return to List City</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Latitude,Longitude,iso,Port,Turnaround")] City city)
        {
            if (ModelState.IsValid)
            {
                await _client.CreateNode(city.Name, city.iso, city.Latitude, city.Longitude, city.Port, city.Turnaround);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        /// <summary>
        /// GET: City/CreateLink
        /// </summary>
        /// <returns>Imput form for Cities to link</returns>
        public IActionResult CreateLink()
        {
            return View();
        }

        /// <summary>
        /// POST: City/CreateLink
        /// </summary>
        /// <param name="link"></param>
        /// <returns>Create new connection between two cities</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLink([Bind("FromCity,ToCity,Media,Distance,Price,Speed,Emission")] CityLink link)
        {
            if (ModelState.IsValid)
            {
                await _client.CreateEdge(link.FromCity, link.ToCity, link.Media, link.Distance, link.Price, link.Emission,link.Speed) ;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}