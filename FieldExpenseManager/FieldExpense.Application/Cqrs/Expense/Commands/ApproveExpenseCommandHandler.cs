using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using FieldExpenseManager.FieldExpense.Application.Interfaces.ServiceInterface;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using FieldExpenseManager.FieldExpense.Domain.Enums;
using MediatR;
using System.Security.Claims;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Commands
{
    public class ApproveExpenseCommandHandler:IRequestHandler<ApproveExpenseCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ApproveExpenseCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPaymentService _paymentService;

        public ApproveExpenseCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<ApproveExpenseCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor,
             IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _paymentService = paymentService;
        }
        public async Task<Unit> Handle(ApproveExpenseCommand request, CancellationToken cancellationToken)
        {
            var approverUserId = GetCurrentUserId();
            _logger.LogInformation("Current User ID: {UserId}", approverUserId);
            if (approverUserId == 0)
            {
                _logger.LogError("Could not determine approver user ID.");
                throw new UnauthorizedAccessException("Approver User ID could not be determined.");
            }
            var expenseToAppove = await _unitOfWork.ExpenseRepository.GetByIdWithDetailsAsync(request.Id);
            if (expenseToAppove == null)
            {
                _logger.LogError("Approve expense failed: Expense with ID {ExpenseId} not found.", request.Id);
                throw new InvalidOperationException("Expense not found");
            }

            if(expenseToAppove.Status != ExpenseStatus.Pending)
            {
                _logger.LogError("Expense with ID {ExpenseId} is not in pending status.", request.Id);
                throw new InvalidOperationException("Expense is not in pending status." );
            }

            _logger.LogInformation("Approving expense with ID {ExpenseId}", request.Id);
            var paymentSuccess = await _paymentService.ProcessPaymentAsync(expenseToAppove);

            ExpensePayments payment = new ExpensePayments
            {
                ExpenseId = expenseToAppove.Id,
                Amount = expenseToAppove.Amount,
                PaymentDate = DateTime.UtcNow,
                IsSuccessful = paymentSuccess,
                ErrorMessage = paymentSuccess ? null : "Payment failed",
                CreatedAt = DateTime.UtcNow,
                TransactionId = Guid.NewGuid().ToString(),
            };

            await _unitOfWork.ExpensePaymentRepository.AddAsync(payment);
            _logger.LogInformation("Payment record created for expense with ID {ExpenseId}", request.Id);

            if (!paymentSuccess)
            {
                _logger.LogError("Payment processing failed for expense with ID {ExpenseId}", request.Id);
                throw new InvalidOperationException("Payment processing failed.");
            }

            try
            {
                expenseToAppove.Status = ExpenseStatus.Approved;
                expenseToAppove.ApproverUserId = approverUserId;
                expenseToAppove.ProcessedDate = DateTime.UtcNow;
                expenseToAppove.RejectionReason = null;
                expenseToAppove.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.ExpenseRepository.Update(expenseToAppove);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Expense with ID {ExpenseId} approved successfully.", request.Id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the expense status to Approved for Expense ID {ExpenseId}.", request.Id);

                throw;
            }
        }
        private int GetCurrentUserId()
        {
           var userIdClaim= _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            _logger.LogWarning("Could not parse User ID from claims in ApproveExpenseCommandHandler");
            return 0;

        }
    }
}
