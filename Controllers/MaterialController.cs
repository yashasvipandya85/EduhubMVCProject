using System;

namespace Eduhub_MVC_Project.Controllers
{
    public class MaterialController : Controller
    {
        private readonly MaterialRepository _materialRepository;
        private readonly CourseRepository _courseRepository;

        public MaterialController(MaterialRepository materialRepository, CourseRepository courseRepository)
        {
            _materialRepository = materialRepository;
            _courseRepository = courseRepository;
        }

        // For educators to view materials for their courses
        public IActionResult CourseMaterials(int courseId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("EducatorLogin", "User");
            }

            var course = _courseRepository.GetCourseById(courseId);
            if (course == null || course.UserId != userId.Value)
            {
                return NotFound("Course not found or you are not authorized to view materials for this course.");
            }

            var materials = _materialRepository.GetMaterialsByCourseId(courseId);
            ViewBag.CourseId = courseId;
            return View(materials);
        }

        // For educators to add new material
        [HttpGet]
        public IActionResult AddMaterial(int courseId)
        {
            var material = new Material
            {
                CourseId = courseId,
                UploadDate = DateTime.Now
            };

            return View(material);
        }
        [HttpPost]
        public IActionResult AddMaterial(Material material)
        {
            Console.WriteLine("AddMaterial POST method called.");
            Console.WriteLine($"Received material: CourseId={material.CourseId}, Title={material.Title}, Description={material.Description}, URL={material.URL}, ContentType={material.ContentType}");

            // Exclude Course from ModelState validation
            ModelState.Remove("Course");

            if (ModelState.IsValid)
            {
                material.UploadDate = DateTime.Now;
                _materialRepository.AddMaterial(material);
                Console.WriteLine("Material added successfully.");
                TempData["SuccessMessage"] = "Material uploaded successfully!";
                return RedirectToAction("CourseMaterials", new { courseId = material.CourseId });
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"ModelState error: {error.ErrorMessage}");
            }

            TempData["ErrorMessage"] = "Failed to upload material. Please try again.";
            return View(material);
        }
       
        // For educators to edit existing material
        [HttpGet]
        public IActionResult EditMaterial(int materialId)
        {
            var material = _materialRepository.GetMaterialById(materialId);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        [HttpPost]
        public IActionResult EditMaterial(Material material)
        {
            Console.WriteLine("EditMaterial POST method called.");
            Console.WriteLine($"Received material: MaterialId={material.MaterialId}, CourseId={material.CourseId}, Title={material.Title}, Description={material.Description}, URL={material.URL}, ContentType={material.ContentType}");

            // Exclude Course from ModelState validation
            ModelState.Remove("Course");

            if (ModelState.IsValid)
            {
                _materialRepository.UpdateMaterial(material);
                Console.WriteLine("Material updated successfully.");
                TempData["SuccessMessage"] = "Material updated successfully!";
                return RedirectToAction("CourseMaterials", new { courseId = material.CourseId });
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"ModelState error: {error.ErrorMessage}");
            }

            TempData["ErrorMessage"] = "Failed to update material. Please try again.";
            return View(material);
        }

        // For students to view materials for their courses
        public IActionResult ViewMaterials(int courseId)
        {
            var materials = _materialRepository.GetMaterialsByCourseId(courseId);
            ViewBag.CourseId = courseId;
            return View(materials);
        }
    }
}
