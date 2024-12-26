using System.Collections.Generic;
using System.Linq;
using Eduhub_MVC_Project.IRepository;
namespace Eduhub_MVC_Project.Repository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _context;

        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddMaterial(Material material)
        {
            _context.Materials.Add(material);
            _context.SaveChanges();
        }

        public void UpdateMaterial(Material material)
        {
            var existingMaterial = _context.Materials.Find(material.MaterialId);
            if (existingMaterial != null)
            {
                existingMaterial.Title = material.Title;
                existingMaterial.Description = material.Description;
                existingMaterial.URL = material.URL;
                existingMaterial.ContentType = material.ContentType;
                existingMaterial.UploadDate = material.UploadDate != DateTime.MinValue ? material.UploadDate : DateTime.Now; // Ensure a valid UploadDate
                _context.SaveChanges();
            }
        }
        public IEnumerable<Material> GetMaterialsByCourseId(int courseId)
        {
            return _context.Materials.Where(m => m.CourseId == courseId).ToList();
        }

        public Material GetMaterialById(int materialId)
        {
            return _context.Materials.FirstOrDefault(m => m.MaterialId == materialId);
        }
    }
}
