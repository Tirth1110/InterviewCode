using System.ComponentModel.DataAnnotations.Schema;

namespace Interview.Entity.Entities
{
    public class StudentSubjects
    {
        [Column("StudentSubjectsId", TypeName = "uniqueidentifier")]
        public Guid StudentSubjectsId { get; set; }
        [Column("StudentId", TypeName = "uniqueidentifier")]
        public Guid StudentId { get; set; }
        public Student Students { get; set; }

        [Column("SubjectName", TypeName = "varchar(20)")]
        public string SubjectName { get; set; } = default!;
        [Column("Marks", TypeName = "int")]
        public int Marks { get; set; }
    }
}