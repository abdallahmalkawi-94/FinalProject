using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class SharedProp
    {
        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Modified Date")]
        public DateTime ModifedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

    }
}
