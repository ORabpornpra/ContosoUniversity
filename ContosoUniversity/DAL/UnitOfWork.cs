using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
{
    public class UnitOfWork : IDisposable //inherit from the IDisposable interface in order to use dispose() to close the database connection
    {//The unit of work class serves one purpose: to make sure that when you use multiple repositories, they share a single database context.

        private SchoolContext context = new SchoolContext(); // create SchoolContext object

        // the reason to create the GenericRepository Class is to get rid of redundant code by using it for both Department and Course Class 
        private GenericRepository<Department> departmentRepository;
        private GenericRepository<Course> courseRepository;


        //Department contex
        public GenericRepository<Department> DepartmentRepository
        {
            get
            {
                if (this.departmentRepository == null)
                {
                    this.departmentRepository = new GenericRepository<Department>(context);
                }
                return departmentRepository;
            }
        }

        //Course contex
        public GenericRepository<Course> CourseRepository
        {
            get
            {//Each repository property checks whether the repository already exists. 
                //If not, it instantiates the repository, passing in the context instance. 
                //As a result, all repositories share the same context instance
                if (this.courseRepository == null)
                {
                    this.courseRepository = new GenericRepository<Course>(context);
                }
                return courseRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 
    }
}