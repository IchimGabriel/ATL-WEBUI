using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ATL_WebUI.Models.SQL;
using Refit;

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

        [Get("/containers")]
        Task<List<Container>> GetAllContainersAsync(); 
    }
}
