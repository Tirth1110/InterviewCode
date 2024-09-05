using Interview.Entity.DbContextData;
using Interview.Entity.Entities;
using Interview.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Interview.Repository.Repository
{
    public class StudentRepository(DataContext context) : IStudentRepository, IDisposable
    {
        private DataContext _context = context;

        public async Task<Student> AddUpdateAsync(Student student)
        {
            if (await _context.Students.AsNoTracking().SingleOrDefaultAsync(x => x.StudentId == student.StudentId) is null)
                await _context.Students.AddAsync(student);
            else
                _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> GetByIdAsync(Guid studentId)
        {
            return await _context.Students.AsNoTracking().SingleOrDefaultAsync(c => c.StudentId == studentId);
        }
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.AsNoTracking().ToListAsync();
        }


        public async Task<bool> ContactNoAlreadyExists(string contactNo, Guid studentId)
        {
            return await _context.Students.AnyAsync(c => c.StudentId != studentId && c.ContactNo == contactNo);
        }

        public async Task<bool> EmailIdAlreadyExists(string emailId, Guid studentId)
        {
            return await _context.Students.AnyAsync(c => c.StudentId != studentId && c.Email == emailId);
        }

        public async Task<Student?> EmailIdAlreadyExistsForImport(string emailId)
        {

            return await _context.Students.AsNoTracking().SingleOrDefaultAsync(c => c.Email == emailId);
        }
        public async Task<List<Student>> GetByEmails(List<string> emails)
        {
            return await _context.Students
                .AsNoTracking()
                .Where(x => emails.Contains(x.Email))
                .ToListAsync();
        }

        public async Task<bool> AddSubjectMarksAsync(List<StudentSubjects> studentSubjects)
        {
            Guid? studentId = studentSubjects.FirstOrDefault()?.StudentId;
            if (studentId.HasValue)
            {
                IEnumerable<StudentSubjects> studentSubjectsData = await _context.StudentSubjects
                                                                             .Where(x => x.StudentId == studentId)
                                                                             .ToListAsync();
                _context.StudentSubjects.RemoveRange(studentSubjectsData);
                _context.SaveChanges();
            }
            await _context.StudentSubjects.AddRangeAsync(studentSubjects);
            _context.SaveChanges();
            return true;
        }

        public async Task<List<StudentSubjects>> GetSubjectMarksDetailsAsync()
        {
            return await _context.StudentSubjects.AsNoTracking().ToListAsync();
        }


        public async Task<bool> AddImportAsync(List<Student> student)
        {
            await _context.Students.AddRangeAsync(student);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> UpdateImportAsync(List<Student> student)
        {
            _context.Students.UpdateRange(student);
            _context.SaveChanges();
            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_context == null) return;
            _context.Dispose();
            _context = null;
        }
    }
}
