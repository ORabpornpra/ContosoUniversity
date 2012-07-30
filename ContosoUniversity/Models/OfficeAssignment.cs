using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class OfficeAssignment
    {
        // InstructorID is used to be both Primary Key and Foreign  key for this class
        // in order to set it to be a Primary key, it has to add the [kry] attribute
        // othewise the primary key will be OfficeAssignmentID
        [Key]
        public int InstructorID { get; set; }

        [MaxLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        // Start Navigation properties

        //one-to-one
        public virtual Instructor Instructor { get; set; }

        // End Navigation properties
    }
}