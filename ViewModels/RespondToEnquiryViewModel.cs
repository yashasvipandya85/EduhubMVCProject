using System.ComponentModel.DataAnnotations;

namespace Eduhub_MVC_Project.ViewModels
{
    public class RespondToEnquiryViewModel
    {
        public int EnquiryId { get; set; }
        [Required]
        public string Response { get; set; }
    }
}
