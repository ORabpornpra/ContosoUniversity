using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;


namespace ContosoUniversity.Controllers
{

  public class HomeController : Controller
    {
      private  SchoolContext  db = new SchoolContext();// create the instantitate

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Contoso University";

            return View();
        }

        public ActionResult About()
        {
            //The LINQ statement groups the student entities by enrollment date, 
            //calculates the number of entities in each group, and stores the results 
            //in a collection of EnrollmentDateGroup view model objects.
            var data = from student in db.Students
                       group student by student.EnrollmentDate
                       into dateGroup
                       select new EnrollmentDateGroup()
                                  {
                                      EnrollmentDate = dateGroup.Key,
                                      StudentCount = dateGroup.Count()
                                  };

            return View(data);
        }

        protected override void Dispose(bool disposing)
        {//to close the connection
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
