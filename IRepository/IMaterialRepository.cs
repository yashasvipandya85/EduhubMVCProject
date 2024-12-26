using System.Collections.Generic;

namespace Eduhub_MVC_Project.IRepository
{
    public interface IMaterialRepository
    {
        void AddMaterial(Material material);
        void UpdateMaterial(Material material);
        IEnumerable<Material> GetMaterialsByCourseId(int courseId);
        Material GetMaterialById(int materialId);
    }
}
