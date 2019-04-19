using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Models
{
    public class ShortestPathRequestView
    {
        [Required]
        [Display(Name = "Departure City")]
        public string DepartureCity { get; set; }

        [Required]
        [Display(Name = "Arrival City")]
        public string ArrivalCity { get; set; }

        [Required]
        [Display(Name = "Media (TRUCK / TRAIN / SHIP / BARGE)")]
        public string Media { get; set; }          //  TRUCK / TRAIN / SHIP / BARGE 

        [Required]
        [Display(Name = "Number of Cities Transit (Int)")]
        public int NoNodes { get; set; }
    }

    public class Values
    {
        public List<string> Cities { get; set; }
        public int TravelDistance { get; set; }
    }

    public class ShortestPath
    {
        public Values Values { get; set; }
        public List<string> Keys { get; set; }
    }
}
