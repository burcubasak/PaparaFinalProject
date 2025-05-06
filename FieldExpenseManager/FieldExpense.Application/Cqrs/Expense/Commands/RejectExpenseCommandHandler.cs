using FieldExpenseManager.FieldExpense.Application.DTOs.Expense;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using FieldExpenseManager.FieldExpense.Application.Interfaces.ServiceInterface;
using FieldExpenseManager.FieldExpense.Domain.Enums;
using FluentValidation;
using MediatR;
using System.Security.Claims;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Commands
{
    public class RejectExpenseCommandHandler:IRequestHandler<RejectExpenseCommand,Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RejectExpenseCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPaymentService _paymentService;
        private readonly IValidator<RejectExpenseDto> _rejectValidator;
        public RejectExpenseCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<RejectExpenseCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor,
            IValidator<RejectExpenseDto> rejectValidator,
            IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _paymentService = paymentService;
            _rejectValidator = rejectValidator;
        }
        public async Task<Unit> Handle(RejectExpenseCommand request, CancellationToken cancellationToken)
        { 
        var approverUserId = GetCurrentUserId();
            if(approverUserId==0)
            {
                _logger.LogError("Could not determine approver user ID.");
                throw new UnauthorizedAccessException("Approver User ID could not be determined.");
            }
            var validationResult = await _rejectValidator.ValidateAsync(request.RejectionData, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }
            var expenseToReject = await _unitOfWork.ExpenseRepository.GetByIdAsync(request.Id);
            if (expenseToReject == null)
            {
                _logger.LogWarning("Reject expense failed: Expense with ID {ExpenseId} not found.", request.Id);
                throw new InvalidOperationException("Expense not found");
            }
            if (expenseToReject.Status != ExpenseStatus.Pending)
            {
                _logger.LogWarning("Expense with ID {ExpenseId} is not in pending status.", request.Id);
                throw new InvalidOperationException("Expense is not in pending status.");
            }
            try
            {
                expenseToReject.Status = ExpenseStatus.Rejected;
                expenseToReject.RejectionReason = request.RejectionData.Reason;
                expenseToReject.ApproverUserId = approverUserId;
                expenseToReject.ProcessedDate = DateTime.UtcNow;
                expenseToReject.UpdatedAt = DateTime.UtcNow;

               _unitOfWork.ExpenseRepository.Update(expenseToReject);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Expense with ID {ExpenseId} rejected successfully.", request.Id);
                return Unit.Value;
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred while rejecting the expense with ID {ExpenseId}", request.Id);
                throw;
            }

        }
        private int GetCurrentUserId()
        { 
        var userIdClaim=_httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim !=null && int.TryParse(userIdClaim.Value,out var userId))
            {
                return userId;
            }
         _logger.LogError("Could not parse User ID from claims in RejectExpenseCommandHandler.");
            return 0;
        }
    }
}
