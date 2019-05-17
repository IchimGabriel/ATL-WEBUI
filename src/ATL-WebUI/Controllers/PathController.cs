using ATL_WebUI.Models;
using ATL_WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATL_WebUI.Controllers
{
    public class PathController : Controller
    {
        private readonly INeo4jApiClient _client;
        private readonly ILogger<ShortestPathRequestView> _logger;

        public PathController(INeo4jApiClient client, ILogger<ShortestPathRequestView> logger)
        {
            _client = client;
            _logger = logger;
        }
        [BindProperty]
        public ShortestPathRequestView Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Imput Query params
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult OnGet()
        {
            //_logger.LogInformation("Input params...");

            IList<string> MediaList = new List<string> { "TRUCK", "TRAIN", "SHIP", "BARGE" };
            ViewBag.Media = new SelectList(MediaList);

            return View();
        }

        /// <summary>
        /// Display selection of routes
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _client.GetSPath(Input.DepartureCity, Input.ArrivalCity, Input.Media, Input.NoNodes);

            ViewBag.Media = Input.Media.ToLower();

            return View(result);
        }
    }
}