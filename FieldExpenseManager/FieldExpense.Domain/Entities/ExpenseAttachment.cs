namespace FieldExpenseManager.FieldExpense.Domain.Entities
{
    public class ExpenseAttachment : BaseEntity
    {
        public int ExpenseId { get; set; }
        public string FilePathOrUrl { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public virtual Expense Expense { get; set; } 
    
    }
}
