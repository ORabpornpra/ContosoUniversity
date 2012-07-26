using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public class Course
    {
        public int CourseID { get; set; }// CourseID is the primary key
        public string Title { get; set; }
        public int Credits { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }//Enrollment is Navigation properties
    }
}