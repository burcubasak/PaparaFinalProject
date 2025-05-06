namespace FieldExpenseManager.FieldExpense.Application.DTOs.Expense
{
    public class ExpenseAttachmentDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public string FilePathOrUrl { get; set; } = null!;
        public long FileSize { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
