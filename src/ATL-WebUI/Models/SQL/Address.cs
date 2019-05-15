using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Models.SQL
{
    public class Address
    {
        [Key]
        public Guid Address_Id { get; set; }
        public Guid User_Id { get; set; }

        [MaxLength(100)]
        public string Field_1 { get; set; }

        [MaxLength(100)]
        public string Field_2 { get; set; }

        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(20)]
        public string Zip { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }
    }
}
