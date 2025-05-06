using FieldExpenseManager.FieldExpense.Application.DTOs.ExpenseCategory;
using MediatR;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.ExpenseCategories.Commands
{
    public record CreateCategoryCommand(CreateCategoryDto Category) : IRequest<CategoryDto>;
    public record UpdateCategoryCommand(int Id,UpdateCategoryDto Category) : IRequest<Unit>;
    public record DeleteCategoryCommand(int Id) : IRequest<Unit>;
}
