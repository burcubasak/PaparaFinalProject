using FieldExpenseManager.FieldExpense.Application.DTOs.ExpenseCategory;
using MediatR;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.ExpenseCategories.Queries
{
    public record ExpenseCategoryByIdQuery(int Id) : IRequest<CategoryDto>;
    public record GetAllCategoriesQuery() : IRequest<List<CategoryDto>>;
}
