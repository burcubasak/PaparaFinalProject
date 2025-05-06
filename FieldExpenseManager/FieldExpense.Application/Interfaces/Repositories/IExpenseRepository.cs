using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.GenericRepositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;

namespace FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories
{
    public interface IExpenseRepository: IGenericRepository<Expense>
    {
        Task<List<Expense>> GetByUserIdWithDetailsAsync(int userId);
        Task<Expense?> GetByIdWithDetailsAsync(int id);
        Task<List<Expense>> GetAllWithDetailsAsync();
    }
}
