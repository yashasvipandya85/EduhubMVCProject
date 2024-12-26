namespace Eduhub_MVC_Project.Models
{
   public class AppDbContext:DbContext
    {
       public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}