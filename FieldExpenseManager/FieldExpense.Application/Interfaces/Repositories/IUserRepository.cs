using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.GenericRepositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;

namespace FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<User?> FindByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllPersonnelAsync();
        Task<bool> IsEmailUniqueAsync(string email, int? userId);
    }
}
