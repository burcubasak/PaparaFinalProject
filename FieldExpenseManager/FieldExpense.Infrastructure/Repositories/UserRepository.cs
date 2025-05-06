using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using FieldExpenseManager.FieldExpense.Domain.Enums;
using FieldExpenseManager.FieldExpense.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Repositories
{
    public class UserRepository :GenericRepository<User>, IUserRepository
    {
      public UserRepository(FieldExpenseDbContext context): base(context) 
        {
        }
        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u=>u.Email.ToLower()==email.ToLower()  && u.IsActive);
        }
        public async Task<IEnumerable<User>> GetAllPersonnelAsync()
        {
            return await _dbContext.Users.Where(u => u.Role == UserRole.Personnel && u.IsActive).ToListAsync();
        }
        public async Task<bool> IsEmailUniqueAsync(string email, int? userId)
        {
            return await _dbContext.Users
         .AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != userId);
        }
    }
}
