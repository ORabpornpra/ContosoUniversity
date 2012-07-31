using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels; //in order to access in the InsructionIndexData in the ViewModels

namespace ContosoUniversity.Controllers
{ 
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Instructor/

        // the .Include key word mean that is Eager Loading
        //public ViewResult Index()
        //{
        //    var instructors = db.Instructors.Include(i => i.OfficeAssignment);
        //    return View(instructors.ToList());
        //}

        public ActionResult Index(Int32? id, Int32? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses.Select(c => c.Department))
                .OrderBy(i => i.LastName);

            if(id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors.Where(i => i.InstructorID == id.Value).Single().Courses;
            }
            //if (courseID != null)
            //{
            //    ViewBag.CourseID = courseID.Value;
            //    viewModel.Enrollments = viewModel.Courses.Where(x => x.CourseID == courseID).Single().Enrollments;
            //}

            //using Explicit Load
            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single();
                db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    db.Entry(enrollment).Reference(x => x.Student).Load();
                }

                viewModel.Enrollments = selectedCourse.Enrollments;
            }
                

            return View(viewModel);
        }

        //
        // GET: /Instructor/Details/5

        public ViewResult Details(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            return View(instructor);
        }

        //
        // GET: /Instructor/Create

        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.OfficeAssignments, "InstructorID", "Location");
            return View();
        } 

        //
        // POST: /Instructor/Create

        [HttpPost]
        public ActionResult Create(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.InstructorID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.InstructorID);
            return View(instructor);
        }
        
        //
        // GET: /Instructor/Edit/5
 
        public ActionResult Edit(int id)
        {
            //Instructor instructor = db.Instructors.Find(id);
            //ViewBag.InstructorID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.InstructorID);
            // .find doesn't allow in the Eager Load method, and so it need to use .Where and . Single()

            Instructor instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Where(i => i.InstructorID == id)
                .Single();

            PopulateAssignedCourseData(instructor);// pass the instructorToUpdate for the function PopulateAssignedCourseData in order to
            //create the data for the checkbox in the GET method in the Edit

            return View(instructor);
        }
        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = db.Courses; // db is an iobject that is pointing to SchoolContext Class, and then use db.Courses to access to the 
            // fuction Courses in the SchoolContext Class which mean to access to the Course table in the data base
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));//create the mark in the checkbox for the classes 
            //base on the InstructorID
            var viewModel = new List<AssignedCourseData>();// object viewModel is pointing to AssignedCourseData class, and viewModel is going to 
            //use for adding data to the CourseID, Title, and Assigned in the AssignedCourseData class
            foreach (var course in allCourses)// loop through all the classes in the Course table
            {
                viewModel.Add(new AssignedCourseData
                                  {
                                      CourseID = course.CourseID,
                                      Title = course.Title,
                                      Assigned = instructorCourses.Contains(course.CourseID)//get the CourseID base on the Instructor
                                      // Assigned is bool type
                                     
                                  });
            }
            ViewBag.Courses = viewModel;
        }

        //
        // POST: /Instructor/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection, string[] selectedCourses)
        {
            var instructorToUpdate = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Where(i => i.InstructorID == id)
                .Single();
            if (TryUpdateModel(instructorToUpdate, "", null, new string[] { "Courses" }))
                //Updates the retrieved Instructor entity with values 
                //from the model binder, excluding the Courses navigation property
            {
                try
                {
                    if(string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }

                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                    db.Entry(instructorToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {

                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    //return View();
                }
            }

            PopulateAssignedCourseData(instructorToUpdate);// pass the instructorToUpdate for the function PopulateAssignedCourseData in order to
            //create the data for the checkbox in the GET method in the Edit
            return View(instructorToUpdate);
        }

        // new fuction to get the classes that are selected from the checkbox to add those classes for the Instructor
        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                //If no check boxes were selected, the code in UpdateInstructorCourses initializes the Courses navigation property with an empty collection
                instructorToUpdate.Courses = new List<Course>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.Courses.Select(c => c.CourseID));

            foreach (var course in db.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))//if the courses are selected from the checkbox
                {
                    if (!instructorCourses.Contains(course.CourseID))//if the courses that are selected are not in the Instructor.Courses navigation property
                    {
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseID))// if the corses aren't selected, but they are in the Instructor.Courses navigation property,
                        //and then delete them
                    {
                        instructorToUpdate.Courses.Remove(course);
                    }
                }
            }
        }

        //
        // GET: /Instructor/Delete/5
 
        public ActionResult Delete(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            return View(instructor);
        }

        //
        // POST: /Instructor/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
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