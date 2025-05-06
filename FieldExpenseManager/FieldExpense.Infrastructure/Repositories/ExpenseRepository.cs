using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using FieldExpenseManager.FieldExpense.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Repositories
{
    public class ExpenseRepository: GenericRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(FieldExpenseDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<Expense>> GetAllWithDetailsAsync()
        {
            return await _dbContext.Expenses
                .Include(e => e.User)
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Attachments)
                .Include(e=>e.Payments)
                .AsNoTracking()
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }
        public Task<Expense?> GetByIdWithDetailsAsync(int id)
        {
            return _dbContext.Expenses
                .Include(e => e.User)
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Attachments)
                .Include(e => e.Payments)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        public Task<List<Expense>> GetByUserIdWithDetailsAsync(int userId)
        {
         return _dbContext.Expenses
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Attachments)
                .Where(e => e.UserId == userId)
                .Include(e=>e.Payments)
                .AsNoTracking() //performans için ekledim
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }
    }
}
