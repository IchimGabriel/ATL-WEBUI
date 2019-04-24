using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Models
{
    public class Container
    {
        [Key]
        public Guid Unit_Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
