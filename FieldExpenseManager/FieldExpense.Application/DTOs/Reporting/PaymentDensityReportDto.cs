namespace FieldExpenseManager.FieldExpense.Application.DTOs.Reporting
{
    public class PaymentDensityReportDto
    {
        public DateTime Period { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public int PaymentCount { get; set; }
    }
}
