using AutoMapper;
using Interview.Entity.DTOs;
using Interview.Entity.Entities;
using Interview.Repository.IRepository;
using Interview.Service.IServices;

namespace Interview.Service.Services
{
    public class StudentService(IStudentRepository studentRepository, IMapper mapper) : IStudentService
    {
        private readonly IStudentRepository _studentRepository = studentRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Student> AddUpdateAsync(StudentForCreateDTO studentForCreateDTO)
        {
            return await _studentRepository.AddUpdateAsync(_mapper.Map<Student>(studentForCreateDTO));
        }

        public async Task<StudentForListDTO?> GetByIdAsync(Guid studentId)
        {
            Student? student = await _studentRepository.GetByIdAsync(studentId);
            if (student is not null)
            {
                IEnumerable<StudentSubjects> studentSubjects = await _studentRepository.GetSubjectMarksDetailsAsync();

                StudentForListDTO? studentDTO = new StudentForListDTO
                {
                    StudentId = student.StudentId,
                    Name = student.Name,
                    Email = student.Email,
                    ContactNo = student.ContactNo,
                    CollegeName = student.CollegeName,
                    EnrolmentNumber = student.EnrolmentNumber,
                    ProfileImage = student.ProfileImage,
                    Total = 0, // Initialize total marks to 0
                    Percentage = 0, // Initialize percentage to 0
                    Grade = "" // Initialize grade to empty string
                };

                // Calculate total marks for the student
                foreach (StudentSubjects subject in studentSubjects.Where(s => s.StudentId == student.StudentId))
                {
                    studentDTO.Total += subject.Marks;
                }

                // Calculate percentage
                if (studentDTO.Total > 0)
                {
                    studentDTO.Percentage = (decimal)studentDTO.Total / studentSubjects.Where(s => s.StudentId == student.StudentId).Count();
                }

                // Determine grade based on percentage
                if (studentDTO.Percentage >= 95)
                    studentDTO.Grade = "A+";
                else if (studentDTO.Percentage >= 85)
                    studentDTO.Grade = "A";
                else if (studentDTO.Percentage >= 75)
                    studentDTO.Grade = "B+";
                else if (studentDTO.Percentage >= 65)
                    studentDTO.Grade = "B";
                else if (studentDTO.Percentage >= 55)
                    studentDTO.Grade = "C+";
                else if (studentDTO.Percentage >= 45)
                    studentDTO.Grade = "C";
                else if (studentDTO.Percentage >= 40)
                    studentDTO.Grade = "D";
                else if (studentDTO.Percentage >= 35)
                    studentDTO.Grade = "E";
                else
                    studentDTO.Grade = "F";
                return studentDTO;
            }
            return new StudentForListDTO();
        }

        public async Task<bool> ContactNoAlreadyExists(StudentForCreateDTO studentForCreateDTO)
        {
            return await _studentRepository.ContactNoAlreadyExists(studentForCreateDTO.ContactNo ?? "", studentForCreateDTO.StudentId);
        }

        public async Task<bool> EmailIdAlreadyExists(StudentForCreateDTO studentForCreateDTO)
        {
            return await _studentRepository.EmailIdAlreadyExists(studentForCreateDTO.Email ?? "", studentForCreateDTO.StudentId);
        }

        public async Task<IEnumerable<StudentForListDTO>> GetAllAsync()
        {
            IEnumerable<Student> students = await _studentRepository.GetAllAsync();
            IEnumerable<StudentSubjects> studentSubjects = await _studentRepository.GetSubjectMarksDetailsAsync();

            List<StudentForListDTO> studentListDTOs = new List<StudentForListDTO>();

            foreach (Student student in students)
            {
                StudentForListDTO? studentDTO = new StudentForListDTO
                {
                    StudentId = student.StudentId,
                    Name = student.Name,
                    Email = student.Email,
                    ContactNo = student.ContactNo,
                    CollegeName = student.CollegeName,
                    EnrolmentNumber = student.EnrolmentNumber,
                    ProfileImage = student.ProfileImage,
                    Total = 0, // Initialize total marks to 0
                    Percentage = 0, // Initialize percentage to 0
                    Grade = "" // Initialize grade to empty string
                };

                // Calculate total marks for the student
                foreach (var subject in studentSubjects.Where(s => s.StudentId == student.StudentId))
                {
                    studentDTO.Total += subject.Marks;
                }

                // Calculate percentage
                if (studentDTO.Total > 0)
                {
                    studentDTO.Percentage = (decimal)studentDTO.Total / studentSubjects.Where(s => s.StudentId == student.StudentId).Count();
                }

                // Determine grade based on percentage
                if (studentDTO.Percentage >= 95)
                    studentDTO.Grade = "A+";
                else if (studentDTO.Percentage >= 85)
                    studentDTO.Grade = "A";
                else if (studentDTO.Percentage >= 75)
                    studentDTO.Grade = "B+";
                else if (studentDTO.Percentage >= 65)
                    studentDTO.Grade = "B";
                else if (studentDTO.Percentage >= 55)
                    studentDTO.Grade = "C+";
                else if (studentDTO.Percentage >= 45)
                    studentDTO.Grade = "C";
                else if (studentDTO.Percentage >= 40)
                    studentDTO.Grade = "D";
                else if (studentDTO.Percentage >= 35)
                    studentDTO.Grade = "E";
                else
                    studentDTO.Grade = "F";
                studentListDTOs.Add(studentDTO);
            }
            return studentListDTOs;
        }

        public async Task<bool> AddSubjectMarksAsync(List<StudentSubjectForCreateDTO> studentSubjectForCreateDTOs)
        {
            return await _studentRepository.AddSubjectMarksAsync(_mapper.Map<List<StudentSubjects>>(studentSubjectForCreateDTOs));
        }

        public async Task<bool> UploadStudentDataViaExcel(List<StudentDetailsImport> studentDetailsImports)
        {
            List<Student> studentListNotExists = [];
            List<Student> studentListExists = [];
            List<Student> updateStudentList = [];
            List<string> employeeEmails = studentDetailsImports
                .Where(x => !string.IsNullOrEmpty(x.Email))
                .Select(x => x.Email!)
                .ToList();
            studentListExists = await _studentRepository.GetByEmails(employeeEmails);

            foreach (StudentDetailsImport studentDetailsImport in studentDetailsImports)
            {
                //Student? studentsExists = await _studentRepository.EmailIdAlreadyExistsForImport(studentDetailsImport.Email ?? "");
                Student? studentsExists = studentListExists
                    .Where(x => string.Equals(studentDetailsImport.Email, x.Email, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                if (studentsExists is null)
                {
                    Student student = new()
                    {
                        Name = studentDetailsImport.Name ?? "",
                        Email = studentDetailsImport.Email ?? "",
                        ContactNo = studentDetailsImport.ContactNo ?? "",
                        CollegeName = studentDetailsImport.CollegeName ?? "",
                        EnrolmentNumber = studentDetailsImport.EnrolmentNumber ?? "",
                        StudentId = Guid.NewGuid(),
                        ProfileImage = "Images\\userprofile.png"
                    };
                    studentListNotExists.Add(student);
                }
                else
                {
                    studentsExists.StudentId = studentsExists.StudentId;
                    studentsExists.Name = studentDetailsImport.Name ?? "";
                    studentsExists.Email = studentDetailsImport.Email ?? "";
                    studentsExists.ContactNo = studentDetailsImport.ContactNo ?? "";
                    studentsExists.CollegeName = studentDetailsImport.CollegeName ?? "";
                    studentsExists.EnrolmentNumber = studentDetailsImport.EnrolmentNumber ?? "";
                    studentsExists.ProfileImage = studentsExists.ProfileImage ?? "";
                    updateStudentList.Add(studentsExists);
                }
            }
            await _studentRepository.AddImportAsync(studentListNotExists);
            await _studentRepository.UpdateImportAsync(updateStudentList);
            return true;
        }
    }
}