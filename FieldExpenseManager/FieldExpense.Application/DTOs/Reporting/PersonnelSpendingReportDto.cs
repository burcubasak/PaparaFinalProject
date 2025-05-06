namespace FieldExpenseManager.FieldExpense.Application.DTOs.Reporting
{
    public class PersonnelSpendingReportDto
    {
        public int PersonnelId { get; set; }
        public string PersonnelName { get; set; } = null!;
        public DateTime Period { get; set; }
        public decimal TotalAmountSpent { get; set; }
        public int ExpenseCount { get; set; }
    }
}
