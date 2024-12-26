using System.Diagnostics;
using Microsoft.AspNetCore.Http;
namespace Eduhub_MVC_Project.Controllers;

public class UserController : Controller
{
    private readonly UserRepository _userRepository;
     private readonly CourseRepository _courseRepository;
    public UserController(UserRepository userRepository, CourseRepository courseRepository)
    {
       _userRepository = userRepository;
       _courseRepository = courseRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Register(User user)
    {
        if(ModelState.IsValid)
        {
            _userRepository.Register(user);
            return RedirectToAction("Index");
        }
        return View(user);
    }
    public IActionResult EducatorLogin()
    {
        return View();
    }
    [HttpPost]
    public IActionResult EducatorLogin(User loginUser)
    {
         var user = _userRepository.Login(loginUser.Email, loginUser.Password);
            if (user != null && user.UserRole == "Educator")
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserRole", user.UserRole);
                return RedirectToAction("EducatorDashboard");
            }
            ModelState.AddModelError("", "Invalid credentials");
            return View();
            }
    public IActionResult StudentLogin()
    {
        return View();
    }
    [HttpPost]
    public IActionResult StudentLogin(User loginUser)
    {
        var user = _userRepository.Login(loginUser.Email, loginUser.Password);
            if (user != null && user.UserRole == "Student")
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserRole", user.UserRole);
                return RedirectToAction("StudentDashboard");
            }
            ModelState.AddModelError("", "Invalid credentials");
            return View();
    }
    public IActionResult EducatorDashboard()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        string userRole = HttpContext.Session.GetString("UserRole");

        if (userId == null || userRole != "Educator")
        {
            return Unauthorized(); 
        }

        var courses = _courseRepository.GetCoursesByUserId(userId.Value);
         if (courses == null || !courses.Any())
            {
                ViewBag.Message = "No courses found for this user.";
            }
        return View(courses);
    }
    public IActionResult StudentDashboard()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        string userRole = HttpContext.Session.GetString("UserRole");

        if (userId == null || userRole != "Student")
        {
            return Unauthorized();
        }
        var studentData = _userRepository.GetUserById(userId.Value); 
        return View(studentData); 
    }

    public IActionResult Profile() 
    { 
        int? userId = HttpContext.Session.GetInt32("UserId"); 
        if (userId == null) 
        { 
            return Unauthorized(); 
        } 
        var user = _userRepository.GetUserById(userId.Value); 
        if (user == null) 
        { 
            return NotFound(); 
        } 
        ViewBag.Layout = user.UserRole == "Educator" ? "~/Views/Shared/_EducatorLayout.cshtml" : "~/Views/Shared/_StudentLayout.cshtml"; 
        return View(user); 
    } 
    [HttpGet] 
    public IActionResult EditProfile() 
    { 
        int? userId = HttpContext.Session.GetInt32("UserId"); 
        if (userId == null) 
        { 
            return Unauthorized(); 
        } 
        var user = _userRepository.GetUserById(userId.Value); 
        if (user == null) 
        { 
            return NotFound(); 
        } 
        ViewBag.Layout = user.UserRole == "Educator" ? "~/Views/Shared/_EducatorLayout.cshtml" : "~/Views/Shared/_StudentLayout.cshtml"; 
        return View(user); 
        } 
        [HttpPost] 
        public IActionResult EditProfile(User user) 
        { 
            if (ModelState.IsValid) 
            {
                 try 
                { 
                    _userRepository.UpdateUser(user); 
                    TempData["SuccessMessage"] = "Profile updated successfully!"; 
                    return RedirectToAction("Profile"); 
                } 
                catch (Exception ex) 
                { 
                    TempData["ErrorMessage"] = $"Failed to update profile. Please try again. Error: {ex.Message}"; 
                } 
            } 
            ViewBag.Layout = user.UserRole == "Educator" ? "~/Views/Shared/_EducatorLayout.cshtml" : "~/Views/Shared/_StudentLayout.cshtml"; 
            return View(user); 
        }
}


