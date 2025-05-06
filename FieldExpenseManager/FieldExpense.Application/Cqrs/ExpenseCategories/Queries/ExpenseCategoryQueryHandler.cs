using AutoMapper;

using FieldExpenseManager.FieldExpense.Application.DTOs.ExpenseCategory;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using MediatR;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.ExpenseCategories.Queries
{
    public class ExpenseCategoryQueryHandler:
        IRequestHandler<ExpenseCategoryByIdQuery, CategoryDto>,
        IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExpenseCategoryQueryHandler> _logger;
        public ExpenseCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ExpenseCategoryQueryHandler> logger)
        {
        
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<CategoryDto> Handle(ExpenseCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching category with ID {Id}", request.Id);
                var category = await _unitOfWork.ExpenseCategoryRepository.GetByIdAsync(request.Id);
                if (category == null || !category.IsActive )
                {
                    _logger.LogWarning("Category with ID {Id} not found or not active", request.Id);
                    throw new InvalidOperationException($"Id{ request.Id } not found");
                }
                var categoryDto=_mapper.Map<CategoryDto>(category);
                _logger.LogInformation("Successfully retrieved expense category with ID {CategoryId}.", request.Id);
                return categoryDto;
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while retrieving expense category with ID {CategoryId}.", request.Id);
               throw;

            }
         
        }
        public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving all active expense categories");
                var categories = await _unitOfWork.ExpenseCategoryRepository
                                         .GetAllActiveAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
                _logger.LogInformation("Successfully retrieved all active categories.");
                return categoryDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving active expense categories.");
                throw;
            }
        }
    }
}
