using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using FieldExpenseManager.FieldExpense.Infrastructure.Persistence.DbContext;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Repositories
{
    public class ExpensePaymentRepository :GenericRepository<ExpensePayments>, IExpensePaymentRepository
    {
        public ExpensePaymentRepository(FieldExpenseDbContext context) : base(context)
        {
        }
    }
}
