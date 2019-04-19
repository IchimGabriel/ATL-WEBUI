using ATL_WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ATL_WebUI.Services
{
    public interface INeo4jApiClient
    {
        Task<List<City>> GetAllCitiesAsync();
        Task<List<ShortestPath>> GetSPath(string departure, string arrival, string medium, int nrnodes);
        Task<IEnumerable<Neighbours>> TruckConnectedCityNeighbours();
    }
    public class Neo4jApiClient : INeo4jApiClient
    {
        private readonly HttpClient _httpClient;
        public Neo4jApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// GET: All Nodes
        /// </summary>
        /// <returns>RETURN ALL CITIES IN NEO4J DB</returns>
        public async Task<List<City>> GetAllCitiesAsync()
        {
            var response = await _httpClient.GetAsync("/api/city");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<List<City>>();
        }

        /// <summary>
        /// Get: Option Route 
        /// </summary>
        /// <param name="departure"></param>
        /// <param name="arrival"></param>
        /// <param name="media"></param>
        /// <param name="nrnodes"></param>
        /// <returns></returns>
        public async Task<List<ShortestPath>> GetSPath(string departure, string arrival, string media, int nrnodes)
        {
            var response = await _httpClient.GetAsync("/api/spath/" + departure + "/" + arrival + "/" + media + "/" + nrnodes);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<List<ShortestPath>>();
        }

        /// <summary>
        /// GET: All cities connected by TRUCK
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Neighbours>> TruckConnectedCityNeighbours()
        {
            var response = await _httpClient.GetAsync("/api/neighbours");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<IEnumerable<Neighbours>>();
        }
    }
}
