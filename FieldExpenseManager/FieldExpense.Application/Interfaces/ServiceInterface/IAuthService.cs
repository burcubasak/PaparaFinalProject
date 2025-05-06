using FieldExpenseManager.FieldExpense.Application.DTOs.Auth;

namespace FieldExpenseManager.FieldExpense.Application.Interfaces.ServiceInterface
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto loginDto);
    }
}
