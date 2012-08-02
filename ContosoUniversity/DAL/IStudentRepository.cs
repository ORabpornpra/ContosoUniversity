using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
{
    public interface IStudentRepository : IDisposable //inherit from the system Interface
    {
        //This Interface contain the set of  CRUD methods

        IEnumerable<Student> GetStudents(); // return all the students
        Student GetStudentByID(int studentId);// return the student base on the ID
        void InsertStudent(Student student);
        void DeleteStudent(int studentId);
        void UpdateStudent(Student student);
        void Save();

    }
}