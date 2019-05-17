using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ATL_WebUI.Models
{
    public class CityLink
    {
        [Required]
        [JsonProperty("fromCity")]
        public string FromCity { get; set; }

        [Required]
        [JsonProperty("toCity")]
        public string ToCity { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Media { get; set; }       //  TRUCK . TRAIN . SHIP . BARGE 

        [JsonProperty("distance")]
        public int Distance { get; set; }       //  Km

        [JsonProperty("price")]
        public decimal Price { get; set; }      //  €/Km

        [JsonProperty("speed")]
        public int Speed { get; set; }          //  Km/h

        [JsonProperty("cotwo")]
        public float Emission { get; set; }     //  CO2 emission
    }
}
