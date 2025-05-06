namespace FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IExpenseCategoryRepository ExpenseCategoryRepository { get; }
        IUserRepository UserRepository { get; }
        IExpenseRepository ExpenseRepository { get; }
        IExpenseAttachmentRepository ExpenseAttachmentRepository { get; }
        IExpensePaymentRepository ExpensePaymentRepository { get; }

        Task CompleteAsync();
    }
}
