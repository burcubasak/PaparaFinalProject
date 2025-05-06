namespace FieldExpenseManager.FieldExpense.Domain.Entities
{
    public class ExpensePayments : BaseEntity
    {
        public int ExpenseId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string Description { get; set; } = string.Empty;

        public virtual Expense Expense { get; set; }

    }
}
