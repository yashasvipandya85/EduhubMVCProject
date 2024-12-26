using Eduhub_MVC_Project.Models;
using Eduhub_MVC_Project.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Eduhub_MVC_Project.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly EnrollmentRepository _enrollmentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly UserRepository _userRepository;

        public EnrollmentController(EnrollmentRepository enrollmentRepository, CourseRepository courseRepository, UserRepository userRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Enroll(int courseId)
        {
           // Console.WriteLine("==========+=" + courseId);
            int? userId = HttpContext.Session.GetInt32("UserId");
            int id = (int)userId;
            Console.WriteLine("+++++" + id);
            _enrollmentRepository.AddEnrollment(courseId,id );
            
            return RedirectToAction("StudentIndex", "Course");
        }

        public IActionResult CourseEnrollments(int courseId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("EducatorLogin", "User");
            }

            var course = _courseRepository.GetCourseById(courseId);

            // Debugging output
            Console.WriteLine("CourseId: " + courseId);
            Console.WriteLine("Session UserId: " + userId);
            if (course != null)
            {
                Console.WriteLine("Course UserId: " + course.UserId);
            }
            else
            {
                Console.WriteLine("Course not found.");
            }

            if (course == null || course.UserId != userId.Value)
            {
                return NotFound("Course not found or you are not authorized to view enrollments for this course.");
            }

            var enrollments = _enrollmentRepository.GetEnrollmentsByCourseId(courseId);
            ViewBag.Course = course;
            return View(enrollments);
        }


        public IActionResult Enrollments()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("StudentLogin", "User");
            }

            var enrollments = _enrollmentRepository.GetEnrollmentsByUserId(userId.Value);
            return View(enrollments);
        }
        public IActionResult CoursesByUser(int courseId)
        {
            Console.WriteLine("CourseId received: " + courseId);

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("EducatorLogin", "User");
            }

            var course = _courseRepository.GetCourseById(courseId);

            // Debugging output
            Console.WriteLine("CourseId: " + courseId);
            Console.WriteLine("Session UserId: " + userId);
            if (course != null)
            {
                Console.WriteLine("Course UserId: " + course.UserId);
            }
            else
            {
                Console.WriteLine("Course not found.");
            }

            if (course == null || course.UserId != userId.Value)
            {
                return NotFound("Course not found or you are not authorized to view enrollments for this course.");
            }

            var enrollments = _enrollmentRepository.GetEnrollmentsByCourseId(courseId);
            return View(enrollments);
        }
        [HttpPost]
        public IActionResult UpdateStatus(int enrollmentId, string status)
        {
            Console.WriteLine($"Updating enrollment ID {enrollmentId} to status {status}");

            var enrollment = _enrollmentRepository.GetEnrollmentById(enrollmentId);
            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                TempData["ErrorMessage"] = "Enrollment not found.";
                return RedirectToAction("CourseEnrollments", new { courseId = enrollment.CourseId });
            }

            enrollment.Status = status;
            _enrollmentRepository.UpdateEnrollment(enrollment);

            Console.WriteLine("Enrollment status updated successfully.");
            TempData["SuccessMessage"] = "Enrollment status updated successfully!";
            return RedirectToAction("CourseEnrollments", new { courseId = enrollment.CourseId });
        }
        public IActionResult MyCourses()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("StudentLogin", "User");
            }

            var enrollments = _enrollmentRepository.GetEnrollmentsByUserId(userId.Value);
            var uniqueCourses = enrollments
                .GroupBy(e => e.CourseId)
                .Select(g => g.First().Course)
                .ToList();

            return View(uniqueCourses);
        }

    }
}
