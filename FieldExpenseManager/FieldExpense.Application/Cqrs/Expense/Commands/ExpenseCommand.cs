using FieldExpenseManager.FieldExpense.Application.DTOs.Expense;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Commands
{
    public record CreateExpenseCommand(
        CreateExpenseDto ExpenseData,
        IFormFile? AttachmentFile
        ) : IRequest<ExpenseDto>;
    public record DeleteExpenseCommand(int Id) : IRequest<Unit>;
}
