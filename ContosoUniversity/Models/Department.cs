using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ContosoUniversity.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }// Primary Key

        [Required(ErrorMessage = "Department name is required.")]
        [MaxLength(50)]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Required(ErrorMessage = "Budget is required.")]
        [Column(TypeName = "money")]//make the database to use money type
        public decimal? Budget { get; set; }// ? make it nullable

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]//{0:d} is short format of Date
        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Administrator")]
        //Foreign Key
        public int? InstructorID { get; set; }// ? mean InstructorID can be null or nullable


        // Start Navigation properties Section

        //one-to-one
        public virtual Instructor Administrator { get; set; }

        //many-to-many
        public virtual ICollection<Course> Courses { get; set; } 

        // End Navigation properties Section
    }
}