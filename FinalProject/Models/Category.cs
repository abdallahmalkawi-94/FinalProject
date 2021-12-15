using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Category : SharedProp
    {
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please Enter Category")]
        public string CategoryName { get; set; }
    }
}
