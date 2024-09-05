namespace Interview.Entity.DTOs
{
    public class StudentSubjectForCreateDTO
    {
        public Guid StudentId { get; set; }
        public string? SubjectName { get; set; }
        public int Marks { get; set; }
    }
}