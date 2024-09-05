using AutoMapper;
using Interview.Entity.DTOs;
using Interview.Entity.Entities;

namespace Interview.Entity.Profiles
{
    public class StudentSubjectProfile : Profile
    {
        public StudentSubjectProfile()
        {
            CreateMap<StudentSubjectForCreateDTO, StudentSubjects>();
        }
    }
}
