using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.ViewModel
{
    public class AdminViewModel
    {
        public List<Course> courses { get; set; }
        public List<User> users { get; set; }
        public List<Category> categories { get; set; }
    }
}
