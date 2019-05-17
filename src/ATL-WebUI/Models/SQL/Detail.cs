using System;
using System.ComponentModel.DataAnnotations;

namespace ATL_WebUI.Models.SQL
{
    public class Detail
    {
        [Key]
        public Guid Detail_Id { get; set; }
        [Required]
        public Guid Shipment_Id { get; set; }
        [Required]
        public Guid Container_Id { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
