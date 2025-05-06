using FieldExpenseManager.FieldExpense.Application.DTOs.Expense;
using FluentValidation;

namespace FieldExpenseManager.FieldExpense.Application.Validation.Expense
{
    public class RejectExpenseDtoValidator:AbstractValidator<RejectExpenseDto>
    {
        public RejectExpenseDtoValidator()
        {
            RuleFor(x => x.Reason)
                    .NotEmpty().WithMessage("Rejection reason cannot be empty.")
                    .MaximumLength(500).WithMessage("Rejection reason cannot be longer than 500 characters.")
                    .MinimumLength(10).WithMessage("Rejection reason must be at least 10 characters long.");
        }
    }
}
