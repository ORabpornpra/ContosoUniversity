using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.Models;
using System.Data.Entity.Infrastructure; //for using DbUpdateConcurrencyException 

namespace ContosoUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Department/

        public ViewResult Index()
        {
            var departments = db.Departments.Include(d => d.Administrator);
            return View(departments.ToList());
        }

        //
        // GET: /Department/Details/5

        public ViewResult Details(int id)
        {
            Department department = db.Departments.Find(id);
            return View(department);
        }

        //
        // GET: /Department/Create

        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FullName");
            return View();
        }

        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FullName", department.InstructorID);
            return View(department);
        }

        //
        // GET: /Department/Edit/5

        public ActionResult Edit(int id)
        {
            Department department = db.Departments.Find(id);
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FullName", department.InstructorID);
            return View(department);
        }

        //
        // POST: /Department/Edit/5

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(department).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {

                var entry = ex.Entries.Single();
                var databaseValues = (Department)entry.GetDatabaseValues().ToObject();
                var clientValues = (Department)entry.Entity;

                // Checking the change for each textbox 
                if (databaseValues.Name != clientValues.Name)
                {
                    ModelState.AddModelError("Name", "Current value: " + databaseValues.Name);
                }
                if (databaseValues.Budget != clientValues.Budget)
                {
                    ModelState.AddModelError("Budget", "Current value: " + String.Format("{0:c}", databaseValues.Budget));
                }
                if (databaseValues.StartDate != clientValues.StartDate)
                {
                    ModelState.AddModelError("StartDate", "Current value: " + string.Format("{0:d}", databaseValues.StartDate));
                }
                if (databaseValues.InstructorID != clientValues.InstructorID)
                {
                    ModelState.AddModelError("InstructorID", "Current value: " + db.Instructors.Find(databaseValues.InstructorID).FullName);
                }
                ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, click "
                    + "the Save button again. Otherwise click the Back to List hyperlink.");

                department.Timestamp = databaseValues.Timestamp;// keep track of the timeStemp for the change
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FullName", department.InstructorID);
            return View(department);
        }

        //
        // GET: /Department/Delete/5

        public ActionResult Delete(int id)
        {
            Department department = db.Departments.Find(id);
            return View(department);
        }

        //
        // POST: /Department/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}