using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Models
{
    public class Detail
    {
        [Key]
        public Guid Detail_Id { get; set; }
        public Guid Shipment_Id { get; set; }
        public Guid Container_Id { get; set; }
        public int Quantity { get; set; }
    }
}
