using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class UserController : Controller
    {
        #region Settings

            private AppDbContext db;
            private IWebHostEnvironment webHostEnvironment;

            public UserController(AppDbContext _db, IWebHostEnvironment _webHostEnvironment)
            {
                db = _db;
                webHostEnvironment = _webHostEnvironment;
            }

            public string UplodeFile(IFormFile file)
            {
                string newFileName = null;

                if (file != null)
                {
                    string fullFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "images/courses");
                    newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string fullFilePath = Path.Combine(fullFolderPath, newFileName);

                    using (var fstrem = new FileStream(fullFilePath, FileMode.Create))
                    {
                        file.CopyTo(fstrem);
                    }

                }

                return newFileName;
            }

        #endregion

        #region Course
            [HttpGet]
            public IActionResult CreateCourse()
            {
                ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName");
                return View();
            }

            [HttpPost]
            public IActionResult CreateCourse(CourseViewModel model)
            {
                if (ModelState.IsValid)
                {
                    int UserId = (int)HttpContext.Session.GetInt32("UserId");
                    string newFileName = "~/images/courses/" + UplodeFile(model.CourseImg);
                    Course course = new Course
                    {
                        CourseName = model.CourseName,
                        CourseDescription = model.CourseDescription,
                        CategoryId = model.CategoryId,
                        Period = model.Period,
                        Price = model.Price,
                        StartDate = model.StartDate,
                        CourseImg = newFileName,
                        UserId = UserId
                    };

                    db.Courses.Add(course);
                    db.SaveChanges();

                   
                    
                return RedirectToAction("Index", "Home");
                }
                ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName");
                return View(model);
            }

            public IActionResult Enroll(int UserId , int CourseId)
            {
                UserCourse userCourse = new UserCourse { UserId = UserId, CourseId = CourseId };
                db.UserCourses.Add(userCourse);
                db.SaveChanges();

            return RedirectToAction("Index", "Home");
            }
        #endregion

        #region User
            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Login(string username, string password)
            {
                if (username != null && password != null)
                {
                    var user = db.Users.Where(u => u.UserName == username && u.Password == password).SingleOrDefault();

                    if (user != null)
                    {
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        return RedirectToAction("Index", "Home");
                    }
                }

                return View();
            }

            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }

            public IActionResult Register(UserViewModel model)
            {
                if (ModelState.IsValid)
                {
                    string NewFileName = "~/images/users/" + UplodeFile(model.UserImg);

                    User user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        FullName = model.FirstName + " " + model.LastName,
                        UserName = model.UserName,
                        Age = model.Age,
                        Email = model.Email,
                        Phone = model.Phone,
                        Major = model.Major,
                        Country = model.Country,
                        City = model.City,
                        FullAddress = model.City + ", " + model.Country,
                        Password = model.Password,
                        UserImg = NewFileName
                    };

                    db.Users.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("Login");
                }

                return View(model);
            }

            public IActionResult Logout()
            {
                HttpContext.Session.Remove("UserId");
                return RedirectToAction("Index", "Home");
            }
        #endregion

    }
}
