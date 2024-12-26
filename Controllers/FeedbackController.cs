using System;
using System.Linq;
using System.Collections.Generic;

namespace Eduhub_MVC_Project.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly FeedbackRepository _feedbackRepository;
        private readonly CourseRepository _courseRepository;

        public FeedbackController(FeedbackRepository feedbackRepository, CourseRepository courseRepository)
        {
            _feedbackRepository = feedbackRepository;
            _courseRepository = courseRepository;
        }

        // For Students to view and post feedback for their courses
        public IActionResult MyFeedbacks()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("StudentLogin", "User");
            }

            var feedbacks = _feedbackRepository.GetFeedbacksByUserId(userId.Value);
            return View(feedbacks);
        }

        [HttpGet]
        public IActionResult SubmitFeedback(int courseId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("StudentLogin", "User");
            }

            var feedback = new Feedback
            {
                CourseId = courseId,
                UserId = userId.Value,
                Date = DateTime.Now
            };

            return View(feedback);
        }

           [HttpPost]
        public IActionResult SubmitFeedback(Feedback feedback)
        {
            Console.WriteLine("SubmitFeedback POST method called.");
            Console.WriteLine($"Received feedback: CourseId={feedback.CourseId}, UserId={feedback.UserId}, StuFeedback={feedback.StuFeedback}");

            // Exclude User and Course from ModelState validation
            ModelState.Remove("User");
            ModelState.Remove("Course");

            if (ModelState.IsValid)
            {
                feedback.Date = DateTime.Now;
                _feedbackRepository.AddFeedback(feedback);
                Console.WriteLine("Feedback submitted successfully.");
                TempData["SuccessMessage"] = "Feedback submitted successfully!";
                return RedirectToAction("MyFeedbacks");
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"ModelState error: {error.ErrorMessage}");
            }

            TempData["ErrorMessage"] = "Failed to submit feedback. Please try again.";
            return View(feedback);
        }


        // For Educators to view feedback for their courses
        public IActionResult CourseFeedbacks()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("EducatorLogin", "User");
            }

            // Fetch all courses taught by the educator
            var courses = _courseRepository.GetCoursesByUserId(userId.Value).ToList();
            if (!courses.Any())
            {
                return NotFound("No courses found for this educator.");
            }

            // Collect all feedbacks for the courses taught by the educator
            var courseIds = courses.Select(c => c.CourseId).ToList();
            var feedbacks = _feedbackRepository.GetFeedbacksByCourseId(courseIds);
            return View(feedbacks);
        }
    }
}
