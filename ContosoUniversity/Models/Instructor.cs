using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Instructor : Person // inherit from the Person class
    {
        //public Int32 PersonID { get; set; }

        ////create the property with some attritubes
        //[Required(ErrorMessage = "Last name is required.")]
        //[Display(Name = "Last Name")]
        //[MaxLength(50)]
        //public string LastName { get; set; }

        //[Required(ErrorMessage = "First name is required.")]
        //[Column("FirstName")]
        //[Display(Name = "First Name")]
        //[MaxLength(50)]
        //public string FirstMidName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Hire date is required.")]
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; } //? make it nullable

        //public string FullName //return full name by concatinate the firstname and lastname
        //{
        //    get { return LastName + ", " + FirstMidName; }
        //}

        //Start Navigation properties

        //use the ICollection<T> because one instructor can teach many classes
        // one-to-many
        public virtual ICollection<Course> Courses { get; set; }

        //the Navigation properties for the OfficeAssignment Class
        //one instructor can have only 
        //one-to-one
        public virtual OfficeAssignment OfficeAssignment { get; set; }

        //End Navigation properties

    }
}