using FieldExpenseManager.FieldExpense.Application.DTOs.Expense;
using FluentValidation;

namespace FieldExpenseManager.FieldExpense.Application.Validation.Expense
{
    public class CreateExpenseDtoValidator : AbstractValidator<CreateExpenseDto>
    {
        public CreateExpenseDtoValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Expense amount is required.")
                .GreaterThan(0).WithMessage("Expense amount must be greater than 0.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Expense description is required.")
                .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters.");
            RuleFor(x => x.ExpenseCategoryId)
                .NotEmpty().WithMessage("Expense category selection is required.")
                .GreaterThan(0).WithMessage("A valid expense category must be selected.");
            RuleFor(x => x.ExpenseDate)
                .NotEmpty().WithMessage("Expense date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Expense date cannot be in the future.")
                .GreaterThan(DateTime.UtcNow.AddYears(-1)).WithMessage("Expense date cannot be older than 1 year.");
            RuleFor(x => x.Location)
                .MaximumLength(200).WithMessage("Location cannot be longer than 200 characters.");
        }
    }
 
}
