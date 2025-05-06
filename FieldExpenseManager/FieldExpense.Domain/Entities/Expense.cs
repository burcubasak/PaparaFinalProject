using FieldExpenseManager.FieldExpense.Domain.Enums;

namespace FieldExpenseManager.FieldExpense.Domain.Entities
{
    public class Expense : BaseEntity
    {
        public int UserId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? Location { get; set; }
        public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;
        public string? RejectionReason { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public int? ApproverUserId { get; set; }

        public virtual User User { get; set; }
        public virtual ExpensePayments? Payments { get; set; }
        public virtual ExpenseCategory ExpenseCategory { get; set; }
        public virtual ICollection<ExpenseAttachment> Attachments { get; set; } = new List<ExpenseAttachment>();

    }
}
