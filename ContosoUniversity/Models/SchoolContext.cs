using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ContosoUniversity.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ContosoUniversity.Models
{
    public class SchoolContext : DbContext //SchoolContext is instantiate from Class DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Person> People { get; set; }


        //customize some of the mapping using fluent API calls.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //one-to-zero-or-one relationship between the Instructor and OfficeAssignment entities
            modelBuilder.Entity<Instructor>()
                .HasOptional(p => p.OfficeAssignment).WithRequired(p => p.Instructor);

            //many-to-many relationship between the Instructor and Course
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors).WithMany(i => i.Courses)
                //set the collum name instead of InstructorPersonID from auto gernerate to be PersonID 
                .Map(t => t.MapLeftKey("CourseID")
                    .MapRightKey("PersonID")
                    .ToTable("CourseInstructor"));
            modelBuilder.Entity<Department>()
                .HasOptional(x => x.Administrator);

        }
    }
}