namespace Eduhub_MVC_Project.IRepository
{
    public interface ICourseRepository
    {
       public List<Course> GetAllCourses();
       Course GetCourseById(int id);
       void AddCourse(Course course);
       void UpdateCourse(Course course);
       IEnumerable<Course> GetCoursesByUserId(int userId);
       
    }
}