using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public abstract class Person // abstract class can inheritance but it can't instantiate
    {
        // This class is gonna be a base class for Student and Instructor Class

        [Key]// key for both student and Instructor
        public int PersonID { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [Column("FirstName")] // in the data base column will display "FirstName"
        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string FirstMidName { get; set; }

        public string FullName 
        {
            get { return LastName + ", " + FirstMidName; }
        }
    }
}