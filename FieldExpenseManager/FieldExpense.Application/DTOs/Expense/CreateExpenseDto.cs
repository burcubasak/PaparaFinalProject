namespace FieldExpenseManager.FieldExpense.Application.DTOs.Expense
{
    public class CreateExpenseDto
    {
        public int ExpenseCategoryId { get; set; }
        public string Description { get; set; }= null!;
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? Location { get; set; }
    }
}
