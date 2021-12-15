﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models.ViewModel
{
    public class CourseViewModel
    {
        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please Enter Course")]
        public string CourseName { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please Enter Description")]
        public string CourseDescription { get; set; }

        [Display(Name = "Course Period")]
        [Required(ErrorMessage = "Please Enter Course Period")]
        public int Period { get; set; }

        [Display(Name = "Course Price")]
        [Required(ErrorMessage = "Please Enter Course Price")]
        public int Price { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Course Start")]
        [Required(ErrorMessage = "Please Choose Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Image")]
        public IFormFile CourseImg { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
