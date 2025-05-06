using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.GenericRepositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;

namespace FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories
{
    public interface IExpenseCategoryRepository :IGenericRepository<ExpenseCategory>
    {
        Task<bool> CategoryNameExistsAsync(string categoryName);
        Task<bool> HasAssociatedActiveExpensesAsync(int categoryId);
        Task<List<ExpenseCategory>>GetAllActiveAsync();
    }
}
