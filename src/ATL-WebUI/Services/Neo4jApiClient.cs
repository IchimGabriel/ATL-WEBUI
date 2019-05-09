using ATL_WebUI.Models;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ATL_WebUI.Services
{
    public interface INeo4jApiClient
    {
        [Get("/cities")]
        Task<List<City>> GetAllCitiesAsync();

        [Get("/spath/{departure}/{arrival}/{media}/{nrnodes}")]
        Task<List<ShortestPath>> GetSPath(string departure, string arrival, string media, int nrnodes); 

        [Get("/neighbours")]
        Task<IEnumerable<Neighbours>> TruckConnectedCityNeighbours();

        [Post("/create/{city}/{iso}/{lat}/{lng}/{is_port}/{turnaround}")]
        Task<City> CreateNode(string city, string iso, float lat, float lng, bool is_port, int turnaround);

        [Post("/create/edge/{fromCity}/{toCity}/{media}/{distance}/{price}/{cotwo}/{speed}")]
        Task<CityLink> CreateEdge(string fromCity, string toCity, string media, int distance, decimal price, float cotwo, int speed);
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
            var response = await _httpClient.GetAsync("/cities");
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
            var response = await _httpClient.GetAsync("/spath/" + departure +"/"+ arrival +"/"+ media +"/"+ nrnodes);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsJsonAsync<List<ShortestPath>>();
        }

        /// <summary>
        /// GET: All cities connected by TRUCK
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Neighbours>> TruckConnectedCityNeighbours()
        {
            var response = await _httpClient.GetAsync("/neighbours");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsJsonAsync<IEnumerable<Neighbours>>();
        }

       
        /// <summary>
        /// POST: Create New Node / City
        /// </summary>
        /// <param name="city"></param>
        /// <param name="iso"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="is_port"></param>
        /// <param name="turnaround"></param>
        /// <returns></returns>
        public async Task<City> CreateNode(string city, string iso, float lat, float lng, bool is_port, int turnaround)
        {
            var response = await _httpClient.PostAsync("/create/" + city +"/"+ iso +"/"+ lat +"/"+ lng +"/"+ is_port +"/"+ turnaround, null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsJsonAsync<City>();
        }

        /// <summary>
        /// POST: Create Link between two nodes (connections between cities) 
        /// </summary>
        /// <param name="fromCity"></param>
        /// <param name="toCity"></param>
        /// <param name="media"></param>    TRUCK / TRAIN / SHIP / BARGE
        /// <param name="distance"></param>
        /// <param name="price"></param>
        /// <param name="cotwo"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public async Task<CityLink> CreateEdge(string fromCity, string toCity, string media, int distance, decimal price, float cotwo, int speed)
        {
            var response = await _httpClient.PostAsync("/create/edge/" + fromCity +"/"+ toCity +"/"+ media +"/"+ distance +"/"+ price +"/"+ cotwo +"/"+ speed, null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsJsonAsync<CityLink>();
        }
    }
}
