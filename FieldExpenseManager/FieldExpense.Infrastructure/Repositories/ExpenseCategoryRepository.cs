using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using FieldExpenseManager.FieldExpense.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Repositories
{
    public class ExpenseCategoryRepository: GenericRepository<ExpenseCategory>, IExpenseCategoryRepository
    {
        public ExpenseCategoryRepository(FieldExpenseDbContext context) : base(context)
        {
        }
        public async Task<List<ExpenseCategory>> GetAllActiveAsync()
        {
            return await _dbContext.ExpenseCategories
                .Where(c => c.IsActive)
                .ToListAsync();
        }
        public async Task<bool> CategoryNameExistsAsync(string name)
        {
            var query = _dbContext.ExpenseCategories
                .Where(c => c.Name.ToLower() == name.ToLower() && c.IsActive);
            return await query.AnyAsync();
        }
        public async Task<bool> HasAssociatedActiveExpensesAsync(int categoryId)
        {
            var query = _dbContext.Expenses
                .Where(e => e.ExpenseCategoryId == categoryId && e.IsActive);
            return await query.AnyAsync();
        }
    }
}
