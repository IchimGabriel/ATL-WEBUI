using ATL_WebUI.Models.SQL;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ATL_WebUI.Services
{
    public class ShipmentApiClient : IShipmentApiClient
    {
        private readonly HttpClient _httpClient;

        public ShipmentApiClient(HttpClient client)
        {
            _httpClient = client;
        }



        public async Task<List<Container>> GetAllContainersAsync()
        {
            var response = await _httpClient.GetAsync("/api/containers");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsJsonAsync<List<Container>>();
        }

        public async Task<Container> EditContainer(Guid? id)
        {
            var response = await _httpClient.GetAsync("/api/containers/" + id);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsJsonAsync<Container>();
        }

        public async Task<Container> SaveEdit(Guid? id, [Body]Container container)
        {
            var response = await _httpClient.PutAsync("/api/containers/" + id, null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsJsonAsync<Container>();
        }
    }
}
