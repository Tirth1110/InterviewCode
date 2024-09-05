using FluentValidation;
using Interview.Entity.DTOs;

namespace Interview.Entity.Validatons
{
    public class StudentForCreateValidator : AbstractValidator<StudentForCreateDTO>
    {
        public StudentForCreateValidator()
        {
            RuleFor(s => s.Name)
                                                   .NotEmpty()
                                                   .WithMessage("Name is required")
                                                   .Matches("^[a-zA-Z]+$")
                                                   .WithMessage("Name must contain only letters");

            RuleFor(s => s.Email)
                                                .NotEmpty()
                                                .WithMessage("Email id is required")
                                                .EmailAddress()
                                                .WithMessage("Enter valid email id");

            RuleFor(s => s.ContactNo)
                                                .NotEmpty()
                                                .WithMessage("Contact is required")
                                                .Matches(@"^[0-9]{10}$")
                                                .WithMessage("Invalid contact number. Please enter a 10-digit Indian contact number.");

            RuleFor(s => s.CollegeName)
                                                 .NotEmpty()
                                                 .WithMessage("College name is required")
                                                 .Matches("^[a-zA-Z]+$")
                                                 .WithMessage("College name must contain only letters");

            RuleFor(s => s.EnrolmentNumber)
                                                .NotEmpty()
                                                .WithMessage("College name is required");
        }
    }
}