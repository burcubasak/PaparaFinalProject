namespace FieldExpenseManager.FieldExpense.Application.DTOs.ExpensePayment
{
    public class ExpensePaymentDto
    {
        public DateTime PaymentDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string? TransactionId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
