using Interview.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interview.Entity.DbContextData
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentSubjects> StudentSubjects { get; set; }
    }
}