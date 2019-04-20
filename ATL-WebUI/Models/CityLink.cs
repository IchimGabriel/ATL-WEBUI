using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Models
{
    public class CityLink
    {
        [JsonProperty("fromCity")]
        public string FromCity { get; set; }

        [JsonProperty("toCity")]
        public string ToCity { get; set; }

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
