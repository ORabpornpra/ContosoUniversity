using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.Models;
using PagedList;

namespace ContosoUniversity.Controllers
{ 
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();//instanitate the SchoolContext class

        //
        // GET: /Student/

        //public ViewResult Index()
        //{
        //    //The Index action method gets a list of students 
        //    //from the Students property of the database context instance:
        //    return View(db.Students.ToList());
        //}

        //Adding sorting functionality to the index method
        public ViewResult Index(string sortOrder, string currentFilter, string SearchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";

            if (Request.HttpMethod =="GET")
            {
                SearchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = SearchString;

            var students = from s in db.Students
                           select s;

            //adding the search box
            if (!String.IsNullOrEmpty(SearchString))// check that the using searching for a student or not
            {
                //change the search and the name in the database in upper case and mathc up
                students = students.Where(s => s.LastName.ToUpper().Contains(SearchString.ToUpper())
                                               || s.FirstMidName.ToUpper().Contains(SearchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "Name desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            //return View(students.ToList());

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));

        }

        //
        // GET: /Student/Details/5

        public ViewResult Details(int id)
        {
            //The id value comes from a query string in the Details hyperlink on the Index page.
            Student student = db.Students.Find(id);
            return View(student);
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Student/Create

        [HttpPost]
        public ActionResult Create(Student student)
        {
            //Use try and catch to catch the data error before add it into the table
            try
            {
                if (ModelState.IsValid)
                {
                    //db is decalre at the top
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, " +
                                         "and if the problem persists see your system administrator.");
            }

            return View(student);
        }
        
        //
        // GET: /Student/Edit/5
 
        public ActionResult Edit(int id)
        {
            Student student = db.Students.Find(id);
            return View(student);
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                     
                    
                    db.Entry(student).State = EntityState.Modified;//Modified flag causes the Entity Framework to create
                                                                   //SQL statements to update the database row.
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            catch (DataException)
            {

                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
    
            }
            return View(student);
        }

        //
        // GET: /Student/Delete/5
 
        //public ActionResult Delete(int id)
        //{
        //    Student student = db.Students.Find(id);
        //    return View(student);
        //}
        public  ActionResult Delete(int id, bool? saveChangesError)
        {//bool? is null(false) when the HttpGet Delete is called, and it when be changed to true from the fuction 
            //DeleteConfirmed when HttpPost Delete is called with an error
            if(saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            return View(db.Students.Find(id));
        }

        //
        // POST: /Student/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            
            try
            {
                //Student student = db.Students.Find(id);
                //db.Students.Remove(student);

                //avoid an unnecessary SQL query to retrieve the row
                Student studentToDelete  = new Student(){PersonID = id};
                //studentToDelete point to the student in the Student Class base on the StudentID
                db.Entry(studentToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (DataException)
            {

                //Log the error (add a variable name after DataException)
                //if it catch the error, resend the value of id = id and saveChangesError = true, and so 
                //HttpGet Delete can catch the error and display on the view
                return RedirectToAction("Delete", new System.Web.Routing.RouteValueDictionary
                                                      {
                                                          {"id", id},
                                                          {"saveChangesError", true}
                                                      });
            }
            return RedirectToAction("Index");
        }

        //it overides the function in the IDisposable interface
        protected override void Dispose(bool disposing)//to make sure that the database connections 
                                                        //are not left open
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}