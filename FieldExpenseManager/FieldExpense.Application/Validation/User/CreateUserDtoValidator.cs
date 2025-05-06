using FieldExpenseManager.FieldExpense.Application.DTOs.User;
using FluentValidation;

namespace FieldExpenseManager.FieldExpense.Application.Validation.User
{
    public class CreateUserDtoValidator :AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters.");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email cannot be longer than 100 characters.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(50).WithMessage("Password cannot be longer than 50 characters.");
            RuleFor(x => x.PhoneNumber)
              .NotEmpty().WithMessage("Phone number is required.")
              .Must(phone => phone.All(char.IsDigit)).WithMessage("Phone number must contain only digits.")
              .MinimumLength(10).WithMessage("Phone number must be at least 10 digits.")
              .MaximumLength(15).WithMessage("Phone number cannot be longer than 11 characters.");
            RuleFor(x => x.WorkPhoneNumber)
                .Must(phone => string.IsNullOrEmpty(phone) || phone.All(char.IsDigit)).WithMessage("Work phone number must contain only digits.")
                .MinimumLength(10).WithMessage("Work phone number must be at least 10 digits.")
                .MaximumLength(15).WithMessage("Work phone number cannot be longer than 11 characters.")
                .When(x => !string.IsNullOrEmpty(x.WorkPhoneNumber));
            RuleFor(x => x.IBAN)
               .NotEmpty().WithMessage("IBAN is required.")
               .Must(iban => iban.StartsWith("TR")).WithMessage("IBAN must start with 'TR'.")
               .MaximumLength(26).WithMessage("IBAN cannot be longer than 26 characters.")
               .When(x => !string.IsNullOrEmpty(x.IBAN));
        }       
    }
}
