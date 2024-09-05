using System.ComponentModel.DataAnnotations.Schema;

namespace Interview.Entity.Entities
{
    public class Student
    {
        [Column("StudentId", TypeName = "uniqueidentifier")]
        public Guid StudentId { get; set; }
        [Column("Name", TypeName = "varchar(20)")]
        public string Name { get; set; } = default!;
        [Column("Email", TypeName = "varchar(30)")]
        public string Email { get; set; } = default!;
        [Column("ContactNo", TypeName = "varchar(15)")]
        public string ContactNo { get; set; } = default!;
        [Column("CollegeName", TypeName = "varchar(30)")]
        public string CollegeName { get; set; } = default!;
        [Column("EnrolmentNumber", TypeName = "varchar(30)")]
        public string EnrolmentNumber { get; set; } = default!;
        [Column("ProfileImage", TypeName = "nvarchar(max)")]
        public string? ProfileImage { get; set; } = default!;
    }
}