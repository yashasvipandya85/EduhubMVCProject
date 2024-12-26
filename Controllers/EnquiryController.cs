using System;
using System.Linq;
using Eduhub_MVC_Project.ViewModels;
namespace Eduhub_MVC_Project.Controllers
{
    public class EnquiryController : Controller
    {
        private readonly EnquiryRepository _enquiryRepository;
        private readonly CourseRepository _courseRepository;

        public EnquiryController(EnquiryRepository enquiryRepository, CourseRepository courseRepository)
        {
            _enquiryRepository = enquiryRepository;
            _courseRepository = courseRepository;
        }
        
         [HttpGet]
        public IActionResult SubmitEnquiry(int courseId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("StudentLogin", "User");
            }

            var enquiryViewModel = new EnquiryViewModel
            {
                CourseId = courseId,
                UserId = userId.Value
            };

            return View(enquiryViewModel);
        }

        [HttpPost]
        public IActionResult SubmitEnquiry(EnquiryViewModel model)
        {
            Console.WriteLine("SubmitEnquiry POST method called.");

            int? userid = HttpContext.Session.GetInt32("UserId");
            if (!userid.HasValue)
            {
                Console.WriteLine("User is not logged in. Redirecting to StudentLogin.");
                return RedirectToAction("StudentLogin", "User");
            }

            model.UserId = userid.Value;

            Console.WriteLine($"Received ViewModel: Subject={model.Subject}, Message={model.Message}, UserId={model.UserId}, CourseId={model.CourseId}");

            if (ModelState.IsValid)
            {
                var enquiry = new Enquiry
                {
                    UserId = model.UserId,
                    CourseId = model.CourseId,
                    Subject = model.Subject,
                    Message = model.Message,
                    EnquiryDate = DateTime.Now,
                    Status = "In Progress",
                    Response = null
                };

                Console.WriteLine($"Saving enquiry: Subject={enquiry.Subject}, Message={enquiry.Message}, EnquiryDate={enquiry.EnquiryDate}, Status={enquiry.Status}, UserId={enquiry.UserId}");
                _enquiryRepository.SubmitEnquiry(enquiry);
                Console.WriteLine("Enquiry saved successfully. Redirecting to StudentIndex.");
                TempData["SuccessMessage"] = "Enquiry submitted successfully!";
                return RedirectToAction("StudentIndex", "Course");
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"ModelState error: {error.ErrorMessage}");
            }

            Console.WriteLine("Returning view with model due to validation errors.");
            TempData["ErrorMessage"] = "Failed to submit enquiry. Please try again.";
            return View(model);
        }
        
        [HttpGet]
        public IActionResult RespondToEnquiry(int enquiryId)
        {
            var enquiry = _enquiryRepository.GetEnquiryById(enquiryId);
            if (enquiry == null)
            {
                return NotFound();
            }

            var viewModel = new RespondToEnquiryViewModel
            {
                EnquiryId = enquiry.EnquiryId
            };

            ViewBag.StudentName = enquiry.User.Username;
            ViewBag.Subject = enquiry.Subject;
            ViewBag.Message = enquiry.Message;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult RespondToEnquiry(RespondToEnquiryViewModel model)
        {
            Console.WriteLine("RespondToEnquiry POST method called.");

            if (model == null)
            {
                Console.WriteLine("Submitted model is null.");
                TempData["ErrorMessage"] = "The submitted model is null.";
                return View(model);
            }

            Console.WriteLine($"Received ViewModel: EnquiryId={model.EnquiryId}, Response={model.Response}");

            if (ModelState.IsValid)
            {
                var existingEnquiry = _enquiryRepository.GetEnquiryById(model.EnquiryId);
                if (existingEnquiry == null)
                {
                    Console.WriteLine("Enquiry not found.");
                    TempData["ErrorMessage"] = "Enquiry not found.";
                    return View(model);
                }

                Console.WriteLine("Enquiry found. Updating response.");

                existingEnquiry.Response = model.Response;
                existingEnquiry.Status = "Closed";
                _enquiryRepository.UpdateEnquiry(existingEnquiry);

                Console.WriteLine("Enquiry updated successfully.");

                TempData["SuccessMessage"] = "Response submitted successfully!";
                return RedirectToAction("CourseEnquiries");
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"ModelState error: {error.ErrorMessage}");
            }

            Console.WriteLine("Returning view with model due to validation errors.");
            TempData["ErrorMessage"] = "Failed to submit response. Please try again.";
            return View(model);
        }


        // For Students to view their own enquiries
        public IActionResult MyEnquiries()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("StudentLogin", "User");
            }

            var enquiries = _enquiryRepository.GetEnquiriesByUserId(userId.Value);
            return View(enquiries);
        }
       
        public IActionResult CourseEnquiries() 
        { 
            int? userId = HttpContext.Session.GetInt32("UserId"); 
            if (!userId.HasValue) 
            { 
                return RedirectToAction("EducatorLogin", "User"); 
            } 
            var courses = _courseRepository.GetCoursesByUserId(userId.Value); 
            var courseIds = courses.Select(c => c.CourseId).ToList(); 
            var enquiries = _enquiryRepository.GetAllEnquiries().Where(e => courseIds.Contains(e.CourseId)).ToList();
             if (enquiries == null || !enquiries.Any()) 
             { TempData["ErrorMessage"] = "No enquiries found for your courses."; 
             return RedirectToAction("EducatorDashboard"); 
             } 
             return View(enquiries); 
             }
    }

}

