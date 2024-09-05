using Interview.Entity.DTOs;
using Interview.Entity.Entities;

namespace Interview.Service.IServices
{
    public interface IStudentService
    {
        Task<Student> AddUpdateAsync(StudentForCreateDTO studentForCreateDTO);
        Task<StudentForListDTO?> GetByIdAsync(Guid studentId);
        Task<bool> ContactNoAlreadyExists(StudentForCreateDTO studentForCreateDTO);
        Task<bool> EmailIdAlreadyExists(StudentForCreateDTO studentForCreateDTO);
        Task<IEnumerable<StudentForListDTO>> GetAllAsync();
        Task<bool> AddSubjectMarksAsync(List<StudentSubjectForCreateDTO> studentSubjectForCreateDTOs);
        Task<bool> UploadStudentDataViaExcel(List<StudentDetailsImport> studentDetailsImports);
    }
}