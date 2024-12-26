using System.Collections.Generic;
using System.Linq;
using Eduhub_MVC_Project.IRepository;
namespace Eduhub_MVC_Project.Repository
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;

        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddEnrollment(int courseid, int userid)
        {
            Console.WriteLine("+++++++++++++++ ==== ", courseid, userid);

            var data = new Enrollment{
                CourseId = courseid,
                UserId= userid,
                EnrollmentDate= DateTime.Now,
                Status = "Pending"
            };
            _context.Enrollments.Add(data);
            _context.SaveChanges();
        }

        public IEnumerable<Enrollment> GetEnrollmentsByCourseId(int courseId)
        {
            return _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .Where(e => e.CourseId == courseId)
                .ToList();
        }

        public IEnumerable<Enrollment> GetEnrollmentsByUserId(int userId)
        {
            return _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.UserId == userId)
                .ToList();
        }
        public Enrollment GetEnrollmentById(int enrollmentId)
        {
            return _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.User)
                .FirstOrDefault(e => e.EnrollmentId == enrollmentId);
        }

        public void UpdateEnrollment(Enrollment enrollment)
        {
            var existingEnrollment = _context.Enrollments.Find(enrollment.EnrollmentId);
            if (existingEnrollment != null)
            {
                existingEnrollment.Status = enrollment.Status;
                _context.SaveChanges();
            }
        }

    }
}
