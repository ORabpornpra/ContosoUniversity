using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.Models;
using ContosoUniversity.DAL;

namespace ContosoUniversity.Controllers
{ 
    public class CourseController : Controller
    {
        //private SchoolContext db = new SchoolContext();
        private UnitOfWork unitOfWork = new UnitOfWork();//create UnitOfWork object

        //
        // GET: /Course/

        public ViewResult Index()
        {
            //see the .Include key word that mean it's using Eager Loading
            //var courses = db.Courses.Include(c => c.Department);
            var courses = unitOfWork.CourseRepository.Get(includeProperties: "Department");
            return View(courses.ToList());// create the query
        }

        //
        // GET: /Course/Details/5

        public ViewResult Details(int id)
        {
            //Course course = db.Courses.Find(id);
            Course course = unitOfWork.CourseRepository.GetByID(id);
            return View(course);
        }

        //
        // GET: /Course/Create

        public ActionResult Create()
        {
            //ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");

            //adding dropdown list
            PopulateDepartmentsDropDownList();
            return View();
        } 

        //
        // POST: /Course/Create

        [HttpPost]
        public ActionResult Create(Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //db.Courses.Add(course);
                    //db.SaveChanges();
                    unitOfWork.CourseRepository.Insert(course);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {

                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            //ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }
        
        //
        // GET: /Course/Edit/5
 
        public ActionResult Edit(int id)
        {
            //Course course = db.Courses.Find(id);
            Course course = unitOfWork.CourseRepository.GetByID(id);

            //ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        //
        // POST: /Course/Edit/5

        [HttpPost]
        public ActionResult Edit(Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(course).State = EntityState.Modified;
                    //db.SaveChanges();
                    unitOfWork.CourseRepository.Update(course);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            
            //ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        //Fuction for dropdown list
        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            //var departmentsQuery = from d in db.Departments
            //                       orderby d.Name
            //                       select d;
            var departmentsQuery = unitOfWork.DepartmentRepository.Get(
                orderBy: q => q.OrderBy(d => d.Name));
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
        }

        //
        // GET: /Course/Delete/5
 
        public ActionResult Delete(int id)
        {
            //Course course = db.Courses.Find(id);
            Course course = unitOfWork.CourseRepository.GetByID(id);
            return View(course);
        }

        //
        // POST: /Course/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            //Course course = db.Courses.Find(id);
            //db.Courses.Remove(course);
            //db.SaveChanges();
            Course course = unitOfWork.CourseRepository.GetByID(id);
            unitOfWork.CourseRepository.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}