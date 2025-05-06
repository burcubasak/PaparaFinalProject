using FieldExpenseManager.FieldExpense.Application.Interfaces.ServiceInterface;
using FieldExpenseManager.FieldExpense.Domain.Entities;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Services
{
    public class FakeBankPaymentService : IPaymentService
    {
        private readonly ILogger<FakeBankPaymentService> _logger;
        public FakeBankPaymentService(ILogger<FakeBankPaymentService> logger)
        {
            _logger = logger;
        }
        public async Task<bool> ProcessPaymentAsync(Expense expense)
        {
            _logger.LogInformation("--Payment Simulation Start--");
            _logger.LogInformation("Payment processed successfully for expense {ExpenseId}", expense.Id);
            _logger.LogInformation("Amount: {Amount} {Currency} ", expense.Amount, "TRY");
          
            if(expense.User !=null && !string.IsNullOrWhiteSpace(expense.User.IBAN))
            {
                _logger.LogInformation("Payment sent to IBAN: {IBAN}", expense.User.IBAN);
                _logger.LogInformation("Recipent: {FirstName} {LastName}",expense.User.FirstName, expense.User.LastName);

                await Task.Delay(500);

                _logger.LogInformation("Payment simulation completed successfully.");
                _logger.LogInformation("--Payment Simulation End--");
                return true;
            }
            else
            {
                _logger.LogWarning("Payment simulation failed for Expense ID: {ExpenseId}. User or IBAN information is missing.", expense.UserId);
                return false;
            }
        }
    }
}
