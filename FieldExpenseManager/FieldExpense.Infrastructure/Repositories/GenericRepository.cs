using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.GenericRepositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using FieldExpenseManager.FieldExpense.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T : BaseEntity
    {
        protected readonly FieldExpenseDbContext _dbContext;
       
        public GenericRepository(FieldExpenseDbContext dbContext) 
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
         
        }
        public virtual async Task<T> AddAsync(T entity)
        {
           await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AnyAsync(predicate);
        }
        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
           return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }
        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public virtual async Task<List<T>> GetAllAsync()
        {
           return await _dbContext.Set<T>().ToListAsync();
        }
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);  
        }
        public virtual void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}
