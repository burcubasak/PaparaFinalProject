using Microsoft.AspNetCore.Mvc;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Commands
{
    public class CreateExpenseRequest
    {
        [FromForm]
        public int ExpenseCategoryId { get; set; }

        [FromForm]
        public string Description { get; set; }

        [FromForm]
        public decimal Amount { get; set; }

        [FromForm]
        public DateTime ExpenseDate { get; set; }

        [FromForm]
        public string? Location { get; set; }

        [FromForm]
        public IFormFile? AttachmentFile { get; set; }
    }

}
