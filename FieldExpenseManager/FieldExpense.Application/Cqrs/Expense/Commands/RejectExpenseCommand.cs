using FieldExpenseManager.FieldExpense.Application.DTOs.Expense;
using MediatR;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Commands
{
    public record ApproveExpenseCommand(int Id) : IRequest<Unit>;
    public record RejectExpenseCommand(int Id, RejectExpenseDto RejectionData) : IRequest<Unit>;
}
