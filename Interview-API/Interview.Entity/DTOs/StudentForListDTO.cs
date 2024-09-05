namespace Interview.Entity.DTOs
{
    public class StudentForListDTO
    {
        public Guid StudentId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ContactNo { get; set; }
        public string? CollegeName { get; set; }
        public string? EnrolmentNumber { get; set; }
        public string? ProfileImage { get; set; }
        public int Total { get; set; }
        public decimal Percentage { get; set; }
        public string? Grade { get; set; }
    }
}
