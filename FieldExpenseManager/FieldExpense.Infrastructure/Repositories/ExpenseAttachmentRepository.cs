using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using FieldExpenseManager.FieldExpense.Infrastructure.Persistence.DbContext;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Repositories
{
    public class ExpenseAttachmentRepository:GenericRepository<ExpenseAttachment>, IExpenseAttachmentRepository
    {
        public ExpenseAttachmentRepository(FieldExpenseDbContext context) : base(context)
        {
        }
    }
}
