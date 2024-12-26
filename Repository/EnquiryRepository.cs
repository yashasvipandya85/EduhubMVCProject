using Eduhub_MVC_Project.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace Eduhub_MVC_Project.Repository
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly AppDbContext _context;

        public EnquiryRepository(AppDbContext context)
        {
            _context = context;
        }

        
        public void SubmitEnquiry(Enquiry model){
            _context.Enquiries.Add(model);
            _context.SaveChanges();
 
        }

        public IEnumerable<Enquiry> GetEnquiriesByUserId(int userId)
        {
            return _context.Enquiries.Include(e => e.Course).Where(e => e.UserId == userId).ToList();
        }
        public void UpdateEnquiry(Enquiry enquiry) 
        { 
            var existingEnquiry = _context.Enquiries.Find(enquiry.EnquiryId); 
            if (existingEnquiry != null) 
            { 
                existingEnquiry.Response = enquiry.Response; 
                existingEnquiry.Status = enquiry.Status; 
                _context.SaveChanges(); 
            } 
        }
        public IEnumerable<Enquiry> GetEnquiriesByCourseId(int courseId)
        {
            return _context.Enquiries.Include(e => e.User).Where(e => e.CourseId == courseId).ToList();
        }

        public Enquiry GetEnquiryById(int enquiryId) 
        { 
            return _context.Enquiries .Include(e => e.Course) .Include(e => e.User) .FirstOrDefault(e => e.EnquiryId == enquiryId); 
        }
        public void RespondToEnquiry(int enquiryId, string response) 
        { 
            var enquiry = _context.Enquiries.FirstOrDefault(e => e.EnquiryId == enquiryId); 
            if (enquiry != null) 
            { 
                enquiry.Response = response; 
                enquiry.Status = "Responded"; 
                _context.SaveChanges(); 
            } 
        }
        public IEnumerable<Enquiry> GetAllEnquiries()
        {
            return _context.Enquiries.Include(e => e.Course).Include(e => e.User).ToList();
        }
    }
}
