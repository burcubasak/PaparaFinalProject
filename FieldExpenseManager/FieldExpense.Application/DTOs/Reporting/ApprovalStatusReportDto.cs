using FieldExpenseManager.FieldExpense.Domain.Enums;

namespace FieldExpenseManager.FieldExpense.Application.DTOs.Reporting
{
    public class ApprovalStatusReportDto
    {
        public DateTime Period { get; set; }
        public decimal TotalApprovedAmount { get; set; }
        public decimal TotalRejectedAmount { get; set; }
    }
}
