using FluentValidation;
using Interview.Entity.DTOs;

namespace Interview.Entity.Validatons
{
    public class StudentSubjectForCreateValidator : AbstractValidator<StudentSubjectForCreateDTO>
    {
        public StudentSubjectForCreateValidator()
        {
            RuleFor(s => s.SubjectName)
                                                 .NotEmpty()
                                                 .WithMessage("Subject Name is required")
                                                 .Matches("^[a-zA-Z]+$")
                                                 .WithMessage("Name must contain only letters");

            RuleFor(s => s.Marks)
                                               .NotEmpty()
                                               .WithMessage("Marks is required");
        }
    }
}