using FieldExpenseManager.FieldExpense.Domain.Enums;

namespace FieldExpenseManager.FieldExpense.Application.DTOs.User
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }= null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? WorkPhoneNumber { get; set; }
        public string? IBAN { get; set; }
        public UserRole Role { get; set; } = UserRole.Personnel;
    }
}
