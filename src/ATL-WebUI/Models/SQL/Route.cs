using System;
using System.ComponentModel.DataAnnotations;

namespace ATL_WebUI.Models.SQL
{
    public class Route
    {
        [Key]
        public Guid Route_Id { get; set; }

        [Display(Name = "Route Name:")]
        public string RouteName { get; set; }

        [Display(Name = "Nodes / Cities:")]
        public string RouteNodes { get; set; }

        [Display(Name = "Total KM:")]
        public int Total_KM { get; set; }

        [Display(Name = "Total Emission:")]
        public float Total_CO2 { get; set; }

        [Display(Name = "Total Time:")]
        public int Total_Time { get; set; }  // hours  
    }
}
