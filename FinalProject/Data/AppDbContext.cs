using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCourse>()
                .HasKey(uc => new { uc.UserId, uc.CourseId });

            modelBuilder.Entity<UserCourse>()
                .HasOne(u => u.User)
                .WithMany(uc => uc.UserCourses)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<UserCourse>()
                .HasOne(c => c.Course)
                .WithMany(uc => uc.UserCourses)
                .HasForeignKey(c => c.CourseId);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
    }
}
