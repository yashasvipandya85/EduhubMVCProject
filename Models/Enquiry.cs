using System;
using System.ComponentModel.DataAnnotations;

namespace Eduhub_MVC_Project.Models
{
    public class Enquiry
    {
        public int EnquiryId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string? Status { get; set; }
        public string? Response { get; set; }
        public User User { get; set; }
        public Course Course { get; set; }
    }
}
