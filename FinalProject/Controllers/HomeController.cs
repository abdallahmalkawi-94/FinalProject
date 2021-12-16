using FinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {

        private AppDbContext db;

        public HomeController(AppDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var courses = db.Courses.Include(u => u.User).Include(c => c.Category).Where(u => u.User.IsDeleted == false).ToList();
            return View(courses);
        }

        public IActionResult GetAllCategories()
        {
            return View(db.Categories.ToList());
        }

        public IActionResult GetAllTrainer()
        {
            return View(db.Users.ToList());
        }

        public IActionResult GetAllCoursesInCategory(int Id)
        {
            var courses = db.Courses.Include(user => user.User).Include(category => category.Category).Where(category => category.CategoryId == Id).Where(u => u.User.IsDeleted == false).ToList();
            ViewBag.CategoryName = db.Categories.Find(Id).CategoryName;
            return View(courses);
        }

        public IActionResult CourseDetail(int id)
        {
            var course = db.Courses
                .Include(c => c.Category)
                .Include(uc => uc.UserCourses)
                .ThenInclude(u => u.User)
                .Where(c => c.CourseId == id).SingleOrDefault();

            return View(course);
        }
    }
}
