using Microsoft.AspNetCore.Http;

namespace Interview.Entity.DTOs
{
    public class StudentForCreateDTO
    {
        public Guid StudentId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ContactNo { get; set; }
        public string? CollegeName { get; set; }
        public string? EnrolmentNumber { get; set; }
        public string? profileImage { get; set; }
        public IFormFile? imageFile { get; set; }
    }
}