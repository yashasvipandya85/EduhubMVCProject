using System.Collections.Generic;

namespace Eduhub_MVC_Project.IRepository
{
    public interface IFeedbackRepository
    {
        void AddFeedback(Feedback feedback);
        IEnumerable<Feedback> GetFeedbacksByCourseId(int courseId);
        IEnumerable<Feedback> GetFeedbacksByUserId(int userId);
        public IEnumerable<Feedback> GetFeedbacksByCourseId(List<int> courseIds);
    }
}
