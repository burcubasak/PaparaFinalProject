using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using MediatR;
using System.Security.Claims;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Commands
{
    public class DeleteCommandHandler:IRequestHandler<DeleteExpenseCommand,Unit>
    {
        private IUnitOfWork _unitOfWork;
        private ILogger<DeleteCommandHandler> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public DeleteCommandHandler(ILogger<DeleteCommandHandler> logger, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var deleterUserId = GetCurrentUserId();
            _logger.LogInformation("Handling DeleteExpenseCommand for Expense ID: {ExpenseId} by User ID: {DeleterUserId}", request.Id, deleterUserId);
            if (deleterUserId == 0)
            {
                _logger.LogError("Could not determine user ID.");
                throw new UnauthorizedAccessException("Deleter User ID could not be determined.");
            }
            var expenseToDelete = await _unitOfWork.ExpenseRepository.GetByIdAsync(request.Id);
            if (expenseToDelete == null)
            {
                _logger.LogError("Delete expense failed: Expense with ID {ExpenseId} not found.", request.Id);
                throw new InvalidOperationException("Expense not found");
            }
         if(!expenseToDelete.IsActive)
            {
                _logger.LogError("Delete expense failed: Expense with ID {ExpenseId} is already inactive.", request.Id);
                return Unit.Value;
            }
            try
            {
                expenseToDelete.IsActive = false;
                expenseToDelete.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.ExpenseRepository.Update(expenseToDelete);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Deleting expense with ID {ExpenseId}", request.Id);
                return Unit.Value;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the expense with ID {ExpenseId}", request.Id);
                throw;
            }
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return 0;
        }
    }
}
