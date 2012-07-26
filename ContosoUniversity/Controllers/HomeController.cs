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
    }
}
