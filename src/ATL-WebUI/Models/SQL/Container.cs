using System;
using System.ComponentModel.DataAnnotations;

namespace ATL_WebUI.Models.SQL
{
    public class Container
    {
        [Key]
        public Guid Unit_Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
