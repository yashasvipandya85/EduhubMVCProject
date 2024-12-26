namespace Eduhub_MVC_Project.Models
{
    public class Material
    {
        public int MaterialId {get; set;}
        public int CourseId {get; set;}
        public string Title {get; set;}
        public string Description {get; set;}
        public string URL {get; set;}
        public DateTime UploadDate {get; set;}
        public string? ContentType {get; set;} 
        public Course Course { get; set; }
    }
}