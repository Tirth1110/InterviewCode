using Interview.Entity.Entities;

namespace Interview.Repository.IRepository
{
    public interface IStudentRepository
    {
        Task<Student> AddUpdateAsync(Student student);
        Task<Student?> GetByIdAsync(Guid studentId);
        Task<bool> ContactNoAlreadyExists(string contactNo, Guid studentId);
        Task<bool> EmailIdAlreadyExists(string emailId, Guid studentId);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<bool> AddSubjectMarksAsync(List<StudentSubjects> studentSubjects);
        Task<List<StudentSubjects>> GetSubjectMarksDetailsAsync();
        Task<Student?> EmailIdAlreadyExistsForImport(string emailId);
        Task<List<Student>> GetByEmails(List<string> emails);
        Task<bool> AddImportAsync(List<Student> student);
        Task<bool> UpdateImportAsync(List<Student> student);
    }
}