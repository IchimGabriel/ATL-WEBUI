using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Models.SQL
{
    public class Message
    {
        [Key]
        public Guid Message_Id { get; set; }
        public Guid Shipment_Id { get; set; }
        public string Details { get; set; }
    }
}
