using FieldExpenseManager.FieldExpense.Application.DTOs.ExpensePayment;
using FieldExpenseManager.FieldExpense.Domain.Enums;

namespace FieldExpenseManager.FieldExpense.Application.DTOs.Expense
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int ExpenseCategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? Location { get; set; }
        public ExpenseStatus Status { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public int? ApproverUserId { get; set; }
        public string? ApproverUserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public List<ExpenseAttachmentDto> Attachments { get; set; } = new List<ExpenseAttachmentDto>();
        public ExpensePaymentDto? Payments { get; set; }

    }
}
