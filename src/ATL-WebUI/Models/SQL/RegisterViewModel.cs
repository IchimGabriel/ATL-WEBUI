﻿using System.ComponentModel.DataAnnotations;

namespace ATL_WebUI.Models.SQL
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "User Name (Email)")]
        public string UserName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "User Roles")]
        public string UserRoles { get; set; }
    }
}
