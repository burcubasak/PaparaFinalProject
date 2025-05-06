using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using FieldExpenseManager.FieldExpense.Infrastructure.Persistence.DbContext;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IUserRepository? _userRepository;
        private IExpenseCategoryRepository? _expenseCategoryRepository;
        private IExpenseRepository? _expenseRepository;
        private IExpenseAttachmentRepository? _expenseAttachmentRepository;
        private IExpensePaymentRepository? _expensePaymentRepository;

        private readonly FieldExpenseDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(FieldExpenseDbContext context,ILogger<UnitOfWork>logger)
        {
         _context = context ;
            _logger = logger ;
        }
        
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public IExpenseCategoryRepository ExpenseCategoryRepository => _expenseCategoryRepository ??= new ExpenseCategoryRepository(_context);

        public IExpenseRepository ExpenseRepository => _expenseRepository ??= new ExpenseRepository(_context);

        public IExpenseAttachmentRepository ExpenseAttachmentRepository => _expenseAttachmentRepository ??=new ExpenseAttachmentRepository(_context);

        public IExpensePaymentRepository ExpensePaymentRepository => _expensePaymentRepository ??=new ExpensePaymentRepository(_context);

        public async Task CompleteAsync()
        {
            using (var transaction=await _context.Database.BeginTransactionAsync())
            {
                _logger.LogInformation("Transaction started");
                try
                {
                  var result=  await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    _logger.LogInformation("Transaction committed successfully", result);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while saving changes");
                    await transaction.RollbackAsync();
                    _logger.LogInformation("Transaction rolled back");
                    throw;
                }
            }
        }
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _logger.LogInformation("DbContext disposed");
                }
              
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
