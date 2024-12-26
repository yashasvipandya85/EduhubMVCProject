namespace Eduhub_MVC_Project.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; }
        public User User { get; set; } 
        public Course Course { get; set; }
        
    }
}