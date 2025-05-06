using FieldExpenseManager.FieldExpense.Application.DTOs.ExpenseCategory;
using FluentValidation;

namespace FieldExpenseManager.FieldExpense.Application.Validation.ExpenseCategory
{
    public class CreateCategoryDtoValidator: AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters.")
            .MinimumLength(3)
                .WithMessage("Name must be at least 3 characters long.");

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .WithMessage("Description must not exceed 250 characters.");
        }
    }
   
}
