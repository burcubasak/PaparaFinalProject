using FieldExpenseManager.FieldExpense.Domain.Enums;

namespace FieldExpenseManager.FieldExpense.Application.DTOs.Reporting
{
    public class PersonnelExpenseReportDto
    {
        public int ExpenseId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }=null!;
        public decimal Amount { get; set; }
        public string CategoryName { get; set; } = null!;
        public ExpenseStatus Status { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string? RejectionReason { get; set; }
    }
}
