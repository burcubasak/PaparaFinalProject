using FieldExpenseManager.FieldExpense.Domain.Entities;

namespace FieldExpenseManager.FieldExpense.Application.Interfaces.ServiceInterface
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(Expense expense);
    }
}
