using Eduhub_MVC_Project.IRepository;
using System.Collections.Generic;
namespace Eduhub_MVC_Project.Repository
{
    public class CourseRepository:ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }

        public void AddCourse(Course course)
        {
            _context.Courses.Add(course); 
            _context.SaveChanges(); 
        }

        // public void UpdateCourse(Course course)
        // {
        //     _context.Courses.Update(course); 
        //     _context.SaveChanges(); 
        // }
         public void UpdateCourse(Course course)
{
    Console.WriteLine("Entering UpdateCourse method.");
    Console.WriteLine($"Course ID: {course.CourseId}");
    Console.WriteLine($"Course Title: {course.Title}");
    Console.WriteLine($"Course Description: {course.Description}");
    Console.WriteLine($"User ID: {course.UserId}");

    _context.Courses.Update(course);
    
    Console.WriteLine("Course updated in context.");
    _context.SaveChanges();
    Console.WriteLine("Changes saved to the database.");
}


         public IEnumerable<Course> GetCoursesByUserId(int userId)
        {
            return _context.Courses.Where(c => c.UserId == userId).ToList();
        }
        public Course GetCourseById(int id)
        {
            return _context.Courses
                        .Include(c => c.User) // Including the related User entity
                        .FirstOrDefault(c => c.CourseId == id);
        }


    }
}