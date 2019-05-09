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
        //private readonly IHttpClientFactory _clientFactory;

        public ShipmentApiClient(HttpClient client)
        {
            _httpClient = client;
            //_clientFactory = clientFactory;
        }
        public Task<List<Address>> GetAllAddressesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Container>> GetAllContainersAsync()
        {
            //var request = new HttpRequestMessage(HttpMethod.Get,"/api/containers");
            //var client = _clientFactory.CreateClient("sql");
            //var response = await client.SendAsync(request);

            var response = await _httpClient.GetAsync("/api/containers");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsJsonAsync<List<Container>>();
        }
    }
}
