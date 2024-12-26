using System.Collections.Generic;
using System.Linq;
using Eduhub_MVC_Project.IRepository;
namespace Eduhub_MVC_Project.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AppDbContext _context;

        public FeedbackRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddFeedback(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
        }

        public IEnumerable<Feedback> GetFeedbacksByCourseId(int courseId)
        {
            return _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Course)
                .Where(f => f.CourseId == courseId)
                .ToList();
        }

        public IEnumerable<Feedback> GetFeedbacksByUserId(int userId)
        {
            return _context.Feedbacks
                .Include(f => f.Course)
                .Where(f => f.UserId == userId)
                .ToList();
        }
        public IEnumerable<Feedback> GetFeedbacksByCourseId(List<int> courseIds)
        {
            return _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Course)
                .Where(f => courseIds.Contains(f.CourseId))
                .ToList();
        }

    }
}
