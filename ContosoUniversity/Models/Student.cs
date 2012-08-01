using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Student : Person //inherit from the Person Class
    {
        //create fileds for the database

        //public int PersonID { get; set; } //Primary Key => the name of the class and follow by ID 
        //                                   //is the key word collum of the table in this case 
        //                                    //is StudentID

        //[Required(ErrorMessage = "Last name is required.")]
        //[Display(Name = "Last Name")]
        //[MaxLength(50)]//adding the range limit of the string
        //public string LastName { get; set; }

        //[Required(ErrorMessage = "First name is required.")]
        //[Column("FirstName")]//set the collum name to be FirstName instead of FirstMidName
        //[Display(Name = "First Name")] 
        //[MaxLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        
        //public string FirstMidName { get; set; }

        [Required(ErrorMessage = "Enrollment date is required.")]
        [Display(Name = "Enrollment Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]//{0:d} is format for short date
        public DateTime? EnrollmentDate { get; set; }

        //public string FullName//cancatinate string and return full name
        //{
        //    get { return LastName + "," + FirstMidName; }
        //}

        // Start Navigation properties Section
        
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        /*
         * Enrollments property is a navigation property. Navigation properties hold other 
         entities that are related to this entity.
         * Navigation properties are typically defined 
         as virtual so that they can take advantage of an Entity Framework function called lazy loading.
         * ICollection is used when the Navigation property is many-to-many OR one-to-many
        */

        // End Navigation properties Section
    }
}