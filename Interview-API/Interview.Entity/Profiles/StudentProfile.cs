using AutoMapper;
using Interview.Entity.DTOs;
using Interview.Entity.Entities;

namespace Interview.Entity.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentForCreateDTO, Student>();
        }
    }
}
