using FieldExpenseManager.FieldExpense.Domain.Entities;
using System.Linq.Expressions;

namespace FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.GenericRepositories
{
    public interface IGenericRepository<T> where T :BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
    
        Task<List<T>> GetAllAsync();
        Task<List<T>>FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
 
}
