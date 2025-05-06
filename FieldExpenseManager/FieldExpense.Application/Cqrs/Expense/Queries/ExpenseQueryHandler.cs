using AutoMapper;
using FieldExpenseManager.FieldExpense.Application.DTOs.Expense;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using FieldExpenseManager.FieldExpense.Domain.Enums;
using MediatR;
using System.Security.Claims;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Queries
{
    public class ExpenseQueryHandler:
        IRequestHandler<GetAllExpensesQuery, List<ExpenseDto>>,
        IRequestHandler<ExpenseByIdQuery, ExpenseDto>,
        IRequestHandler<GetMyExpensesQuery, List<ExpenseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExpenseQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<ExpenseDto>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var expenses = await _unitOfWork.ExpenseRepository.GetAllWithDetailsAsync();
                var expenseDto= _mapper.Map<List<ExpenseDto>>(expenses);
                return expenseDto;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<ExpenseDto> Handle(ExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var currentUserRole = GetCurrentUserRole();
                if(currentUserId==0)
                {
                    throw new UnauthorizedAccessException("User not found");
                }
                var expense = await _unitOfWork.ExpenseRepository.GetByIdWithDetailsAsync(request.Id);
                if (expense == null)
                {
                    throw new InvalidOperationException("Expense not found");
                }
                if (currentUserRole !=UserRole.Admin && expense.UserId !=currentUserId)
                {
                    throw new UnauthorizedAccessException("You do not have permission to access this expense");
                }
                var expenseDto = _mapper.Map<ExpenseDto>(expense);
                return expenseDto;
            }
            catch (InvalidOperationException)
            {

                throw;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ExpenseDto>> Handle(GetMyExpensesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
              
                if (currentUserId == 0)
                {
                    throw new UnauthorizedAccessException("User ID could not be determined from token.");
                }
                var expenses = await _unitOfWork.ExpenseRepository.GetByUserIdWithDetailsAsync(currentUserId);
            
                var expenseDto = _mapper.Map<List<ExpenseDto>>(expenses);
                return expenseDto;
            }
        
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private int GetCurrentUserId()
        {
           var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value,out var userId) )
            {
                return userId;

            }
            return 0;
        }
        private UserRole? GetCurrentUserRole()
        {
            var role = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Role);
            if (role != null && Enum.TryParse<UserRole>(role.Value,true,out var userRole))
            {
                return userRole;
            }
            return null;
        }
    }
}
