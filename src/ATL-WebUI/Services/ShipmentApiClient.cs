using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ATL_WebUI.Models.SQL;

namespace ATL_WebUI.Services
{
    public class ShipmentApiClient : IShipmentApiClient
    {
        private readonly HttpClient _httpClient;
        
        public ShipmentApiClient(HttpClient client)
        {
            _httpClient = client;
        }
        public Task<List<Address>> GetAllAddressesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Container>> GetAllContainersAsync()
        {
            var response = await _httpClient.GetAsync("/containers");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsJsonAsync<List<Container>>();
        }
    }
}
