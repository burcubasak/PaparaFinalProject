using FieldExpenseManager.FieldExpense.Application.DTOs.Expense;
using MediatR;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Queries
{
    public record ExpenseByIdQuery(int Id) : IRequest<ExpenseDto>;
    public record GetAllExpensesQuery() : IRequest<List<ExpenseDto>>;
    public record GetMyExpensesQuery() : IRequest<List<ExpenseDto>>;
}
