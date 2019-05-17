using ATL_WebUI.Models.SQL;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATL_WebUI.Services
{
    public interface IShipmentApiClient
    {

        /// <summary>
        /// fix problem https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.2
        /// </summary>
        /// <returns></returns>
        //[Get("/api/addresses")]
        //Task<List<Address>> GetAllAddressesAsync();

        [Get("/api/containers")]
        Task<List<Container>> GetAllContainersAsync();

        [Get("/api/containers/{id}")]
        Task<Container> EditContainer(Guid? id);

        [Put("/api/containers/{id}")]
        Task<Container> SaveEdit(Guid? id, [Body]Container container);
    }
}
