using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Models.SQL
{
    public class Shipment
    {
        public enum Statuses { VALID, DELIVERED, TRANSIT, FAILURE, RETURNED, CANCELED }

        [Key]
        public Guid Shipment_Id { get; set; }
        public Guid Customer_Id { get; set; }
        public Guid Employee_Id { get; set; }

        [Display(Name = "Status")]
        public Statuses Status { get; set; }

        [Display(Name = "From Address")]
        public Guid Address_From_Id { get; set; }

        [Display(Name = "To Address")]
        public Guid Address_To_Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Created_Date { get; set; }

        [Display(Name = "Departure")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Departure_Date { get; set; }

        [Display(Name = "Arrival")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Arrival_Date { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Cost €")]
        public decimal Total_Price { get; set; }

        [Display(Name = "Route")]
        public Guid Route_Id { get; set; }

        public ICollection<Message> Messages { get; set; }
        public ICollection<Detail> Details { get; set; }
    }
}
