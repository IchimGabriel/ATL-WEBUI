using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ATL_WebUI.Models
{
    public class City
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lat")]
        public float Latitude { get; set; }

        [JsonProperty("lng")]
        public float Longitude { get; set; }

        [Required]
        [JsonProperty("iso")]
        public string iso { get; set; }     // iso3 - IRL . GBR . ROU .

        [JsonProperty("port_city")]
        public bool Port { get; set; }          // true or false

        [JsonProperty("turnaround")]
        public int Turnaround { get; set; }     // overhead time - hours
    }
}
