namespace Eduhub_MVC_Project.Models
{
     public class Feedback
    {
        public int FeedbackId{ get; set; }
        public int UserId{ get; set; }
        public int CourseId { get; set; }
        public string StuFeedback { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; } 
        public Course Course { get; set; }
    }
}