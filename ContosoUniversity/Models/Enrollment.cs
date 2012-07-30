using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Enrollment
    {
        public int EnrollmentID { set; get; } //EnrollmentID is the primarykey
        public int CourseID { get; set; }//CourseID  is a foreign key,
        public int StudentID { get; set; }//StudentID is a foreign key

        [DisplayFormat(DataFormatString = "{0:#.#}", ApplyFormatInEditMode = true, NullDisplayText = "No grade")]
        public decimal? Grade { get; set; }// ? after decimal mean that Grade property is nullable

        // Start Navigation properties Section

        // one-to-one
        public virtual Course Course { get; set; }//Course is Navigation properties
        public virtual Student Student { get; set; }//Student is Navigation properties

        // End Navigation properties Section
    }
}