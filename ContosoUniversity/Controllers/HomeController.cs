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
        private SchoolContext db = new SchoolContext();// create the instantitate

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

            // LINQ to SQL
            //var data = from student in db.Students
            //           group student by student.EnrollmentDate
            //               into dateGroup
            //               select new EnrollmentDateGroup()
            //               {
            //                   EnrollmentDate = dateGroup.Key,
            //                   StudentCount = dateGroup.Count()
            //               };

            var query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
                        + "FROM Person "
                        + "WHERE EnrollmentDate IS NOT NULL "
                        + "GROUP BY EnrollmentDate";
            var data = db.Database.SqlQuery<EnrollmentDateGroup>(query);//Use the DbDatabase.SqlQuery method 
            //for queries that return types that aren't entities. The returned data isn't tracked by the database context, 
            //even if you use this method to retrieve entity types

            return View(data);
        }

        protected override void Dispose(bool disposing)
        {//to close the connection
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}