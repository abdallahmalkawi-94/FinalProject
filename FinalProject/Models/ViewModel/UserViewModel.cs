using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.ViewModel
{
    public class UserViewModel
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please Enter Username")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Please Enter Valid Email")]
        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Please Enter Valid Phone")]
        [Required(ErrorMessage = "Please Enter Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please Choose Gender")]
        public Gender Gender { get; set; }

        [Display(Name = "Image")]
        public IFormFile UserImg { get; set; }

        [Required(ErrorMessage = "Please Enter Age")]
        public int Age { get; set; }
        public string Major { get; set; } = "Not Choose";

        [Required(ErrorMessage = "Please Enter Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        public string City { get; set; }
        public string FullAddress { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; } = "User";
    }
}
