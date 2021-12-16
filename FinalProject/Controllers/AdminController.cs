using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Security.Cryptography;

namespace FinalProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment webHostEnvironment;

        public AdminController(AppDbContext context , IWebHostEnvironment _webHostEnvironment)
        {
            _context = context;
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
        public string encrypt(string Password)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(Password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    Password = Convert.ToBase64String(ms.ToArray());
                }
            }
            return Password;
        }
        // GET: Admin
        public IActionResult Index()
        {
            AdminViewModel adminViewModel = new AdminViewModel
            {
                courses = _context.Courses.OrderByDescending(c => c.CreateDate).Include(u => u.User).Include(c => c.Category).Take(5).OrderBy(c => c.CreateDate).ToList(),
                users = _context.Users.OrderByDescending(u => u.CreateDate).Take(5).OrderBy(u => u.CreateDate).ToList(),
                categories = _context.Categories.OrderByDescending(c => c.CreateDate).Take(5).OrderBy(c => c.CreateDate).ToList()
            };

            return View(adminViewModel);
        }

        #region Category

        public IActionResult GetAllCategories()
        {
            return View(_context.Categories.ToList());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> CategoryDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Create
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([Bind("CategoryId,CategoryName,IsActive,IsDeleted")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetAllCategories));
            }
            return View(category);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, [Bind("CategoryId,CategoryName,IsActive,IsDeleted")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
              
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                
                
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
        #endregion

        #region User
        // GET: Admin/Details/5
        public async Task<IActionResult> UserDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id,User model,string UserImg)
        {
            if (id != model.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                    model.FullName = model.FirstName + " " + model.LastName;
                    model.FullAddress = model.City + ", " + model.Country;
                    model.UserImg = UserImg;

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                
                
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.IsDeleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult RestUserPassword(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RestUserPassword(int id , string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            newPassword = encrypt(newPassword);
            user.Password = newPassword;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        #region Course
        public IActionResult GetCourses()
        {
            var courses = _context.Courses.Include(u => u.User).Include(c => c.Category);
            return View(courses);
        }

        public IActionResult CourseDetail(int id)
        {
            var course = _context.Courses.Include(u => u.User).Include(c => c.Category).Where(c => c.CourseId == id).SingleOrDefault();
            return View(course);
        }


        [HttpGet]
        public IActionResult CreateCourse()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
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

                _context.Courses.Add(course);
                _context.SaveChanges();



                return RedirectToAction("Index", "Admin");
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(model);
        }

        public IActionResult EditCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Courses.Include(u => u.User).Where(c => c.CourseId == id).SingleOrDefault();
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, Course course, string CourseImg, int UserId)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }
            course.UserId = UserId;
            course.CourseImg = CourseImg;
            if (ModelState.IsValid)
            {

                _context.Courses.Update(course);
                await _context.SaveChangesAsync();

                
                return RedirectToAction("Index", "Admin");
               
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(course);
        }

        public async Task<IActionResult> DeleteCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("DeleteCourse")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Admin");
            
        }
        #endregion
    }
}
