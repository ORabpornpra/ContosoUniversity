using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Student
    {
        //create fileds for the database
        public int StudentID { get; set; } //the name of the class and follow by ID 
                                           //is the key word collum of the table in this case 
                                            //is StudentID
        [MaxLength(50)]//adding the range limit of the string
        public string LastName { get; set; }

        [MaxLength(50, ErrorMessage = "First name cannot be longer than 50 characters."), Column("FirstName")]
        //set the collum name to be FirstName instead of FirstMidName
        public string FirstMidName { get; set; }
        
        //set the format for the DateTime display
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        /*
         * Enrollments property is a navigation property. Navigation properties hold other 
         entities that are related to this entity.
         * Navigation properties are typically defined 
         as virtual so that they can take advantage of an Entity Framework function called lazy loading.
         * ICollection is used when the Navigation property is many-to-many OR one-to-many
        */
    }
}