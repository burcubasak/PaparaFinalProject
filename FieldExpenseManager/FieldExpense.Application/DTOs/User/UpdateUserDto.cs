namespace FieldExpenseManager.FieldExpense.Application.DTOs.User
{
    public class UpdateUserDto
    {
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? WorkPhoneNumber { get; set; }
        public string? IBAN { get; set; }
    }
}
