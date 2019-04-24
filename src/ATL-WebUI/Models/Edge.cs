using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Models
{
    public class Edge
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }    // TRUCK . TRAIN . SHIP . BARGE
        [JsonProperty("distance")]
        public int Distance { get; set; }   // Km
        [JsonProperty("speed")]
        public int Speed { get; set; }      // Km/h
        [JsonProperty("cotwo")]
        public float Emission { get; set; } // CO2 emission
    }
}
