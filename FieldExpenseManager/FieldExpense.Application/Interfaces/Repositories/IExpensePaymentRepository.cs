using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.GenericRepositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;

namespace FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories
{
    public interface IExpensePaymentRepository :IGenericRepository<ExpensePayments>
    {
    }
}
