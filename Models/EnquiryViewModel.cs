using System;
using System.ComponentModel.DataAnnotations;

namespace Eduhub_MVC_Project.Models
{
    public class EnquiryViewModel
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
