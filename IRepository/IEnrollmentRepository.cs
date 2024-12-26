// using System.Collections.Generic; 
// namespace Eduhub_MVC_Project.IRepository 
// { 
//     public interface IEnrollmentRepository 
//     { 
//         void EnrollStudent(Enrollment enrollment); 
//         IEnumerable<Enrollment> GetEnrollmentsByUserId(int userId); 
//         IEnumerable<Enrollment> GetAllEnrollments(); 
//         Enrollment GetEnrollmentById(int id); 
//     } 
// }


using System.Collections.Generic;

namespace Eduhub_MVC_Project.IRepository
{
    public interface IEnrollmentRepository
    {
       public void AddEnrollment(int courseid, int userid);
        IEnumerable<Enrollment> GetEnrollmentsByCourseId(int courseId);
        IEnumerable<Enrollment> GetEnrollmentsByUserId(int userId);
        public Enrollment GetEnrollmentById(int enrollmentId);
         public void UpdateEnrollment(Enrollment enrollment);
    }
}
