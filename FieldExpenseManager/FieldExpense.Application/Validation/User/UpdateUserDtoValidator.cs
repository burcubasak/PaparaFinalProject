using FieldExpenseManager.FieldExpense.Application.DTOs.User;
using FluentValidation;

namespace FieldExpenseManager.FieldExpense.Application.Validation.User
{
    public class UpdateUserDtoValidator: AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters.");
            RuleFor(x => x.PhoneNumber)
              .NotEmpty().WithMessage("Phone number is required.")
              .Must(phone=>phone.All(char.IsDigit)).WithMessage("Phone number must contain only digits.")
              .MinimumLength(10).WithMessage("Phone number must be at least 10 digits.")
              .MaximumLength(15).WithMessage("Phone number cannot be longer than 11 characters.");   
            RuleFor(x => x.WorkPhoneNumber)
                   .Must(phone => phone.All(char.IsDigit)).WithMessage("Phone number must contain only digits.")
              .MinimumLength(10).WithMessage("Phone number must be at least 10 digits.")
              .MaximumLength(15).WithMessage("Phone number cannot be longer than 11 characters.")
              .When(x=>!string.IsNullOrEmpty(x.WorkPhoneNumber));
            RuleFor(x => x.IBAN)
                .NotEmpty().WithMessage("IBAN is required.")
                .Must(iban => iban.StartsWith("TR")).WithMessage("IBAN must start with 'TR'.")
                .MaximumLength(26).WithMessage("IBAN cannot be longer than 26 characters.")
                .When(x => !string.IsNullOrEmpty(x.IBAN));
        }
    }
}
