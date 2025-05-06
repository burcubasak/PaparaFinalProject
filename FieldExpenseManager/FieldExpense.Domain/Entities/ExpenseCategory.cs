namespace FieldExpenseManager.FieldExpense.Domain.Entities
{
    public class ExpenseCategory : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
