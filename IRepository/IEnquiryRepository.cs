
using System.Collections.Generic;

namespace Eduhub_MVC_Project.IRepository
{
    public interface IEnquiryRepository
    {
        void SubmitEnquiry(Enquiry model);
        IEnumerable<Enquiry> GetEnquiriesByUserId(int userId);
        IEnumerable<Enquiry> GetEnquiriesByCourseId(int courseId);
        Enquiry GetEnquiryById(int enquiryId);
        void RespondToEnquiry(int enquiryId, string response);
        IEnumerable<Enquiry> GetAllEnquiries();
         public void UpdateEnquiry(Enquiry enquiry);
        //public void SubmitEnquiry(int courseId, int userId, string subject, string message, string status);
    }
}
