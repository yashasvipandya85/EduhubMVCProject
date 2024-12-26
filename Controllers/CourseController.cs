using System;
namespace Eduhub_MVC_Project.Controllers
{
    public class CourseController:Controller
    {
        private readonly CourseRepository _courseRepository;
       
        private readonly AppDbContext _context;
        public CourseController(CourseRepository CourseRepository, AppDbContext context)
        {
            _courseRepository = CourseRepository; 
            _context = context;  
        }
        public ActionResult Index()
        {
            var courses = _courseRepository.GetAllCourses();
            return View(courses);
        }
        public IActionResult StudentIndex() 
        { 
            var courses = _courseRepository.GetAllCourses(); 
            return View(courses); 
        }
        public Course GetCourseById(int id)
        {
            return _context.Courses.Find(id);
        }
        

        public IActionResult CoursesByUser (int userId)
        {
            int? loggedUserId = HttpContext.Session.GetInt32 ("UserId");
            if(loggedUserId.HasValue)
            {
                var courses = _courseRepository.GetCoursesByUserId(loggedUserId.Value);
                return View(courses);
            }
            else
            {
                return NotFound();
            }
            return View();
        }
        
        [HttpGet]
        public IActionResult Details(int id)
        {
            var course = _courseRepository.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _courseRepository.AddCourse(course); 
                return RedirectToAction("Index");
            }
            return View(course);
        }
       
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _courseRepository.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

         [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(Course course)
{
    Console.WriteLine("Entering Edit POST action.");

    if (ModelState.IsValid)
    {
        Console.WriteLine("Model state is valid.");
        Console.WriteLine($"Course ID: {course.CourseId}");
        Console.WriteLine($"Course Title: {course.Title}");
        Console.WriteLine($"Course Description: {course.Description}");

        // Fetch the existing course from the database
        var existingCourse = _courseRepository.GetCourseById(course.CourseId);
        if (existingCourse != null)
        {
            // Manually set the navigation properties to avoid validation issues
            course.User = existingCourse.User;
            course.Enrollments = existingCourse.Enrollments;

            // Update the course
            _courseRepository.UpdateCourse(course);
            Console.WriteLine("Course updated successfully.");

            return RedirectToAction("Index");
        }

        Console.WriteLine("Existing course not found.");
        ModelState.AddModelError("", "Course not found.");
    }

    Console.WriteLine("Model state is invalid.");
    foreach (var modelState in ModelState.Values)
    {
        foreach (var error in modelState.Errors)
        {
            Console.WriteLine($"Error: {error.ErrorMessage}");
        }
    }

    return View(course);
}




        
    }
}