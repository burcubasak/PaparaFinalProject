using AutoMapper;
using FieldExpenseManager.FieldExpense.Application.DTOs.ExpenseCategory;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using FluentValidation;
using MediatR;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.ExpenseCategories.Commands
{
    public class ExpenseCategoryCommandHandler:
        IRequestHandler<CreateCategoryCommand, CategoryDto>,
        IRequestHandler<UpdateCategoryCommand, Unit>,
        IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExpenseCategoryCommandHandler> _logger;
        private readonly IValidator<CreateCategoryDto> _createCategoryValidator;
        private readonly IValidator<UpdateCategoryDto> _updateCategoryValidator;
        public ExpenseCategoryCommandHandler(
            ILogger<ExpenseCategoryCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateCategoryDto> createCategoryValidator,
            IValidator<UpdateCategoryDto> updateCategoryValidator
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createCategoryValidator = createCategoryValidator;
            _updateCategoryValidator = updateCategoryValidator;
        }
        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateCategoryCommand for category name: {CategoryName}", request.Category.Name);
           var validationResult= await _createCategoryValidator.ValidateAsync(request.Category, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Create category validation failed: {@ValidationErrors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }
            var nameExists = await _unitOfWork.ExpenseCategoryRepository.CategoryNameExistsAsync(request.Category.Name);
            if (nameExists)
            {
                _logger.LogWarning("Category name already exists: {CategoryName}", request.Category.Name);
                throw new InvalidOperationException($"Category name '{request.Category.Name}' already exists.");
            }
            try
            {
                var newCategory = _mapper.Map<ExpenseCategory>(request.Category);
                newCategory.IsActive = true;
                await _unitOfWork.ExpenseCategoryRepository.AddAsync(newCategory);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Category '{CategoryName}' created successfully with ID: {CategoryId}", newCategory.Name, newCategory.Id);
                return _mapper.Map<CategoryDto>(newCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the new expense category '{CategoryName}'.", request.Category.Name);

                throw;
            }
        }
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateCategoryCommand for category ID: {CategoryId}", request.Id);
            var validationResult = await _updateCategoryValidator.ValidateAsync(request.Category, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Update category validation failed for ID {CategoryId}: {@ValidationErrors}.", request.Id, validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }
            var categoryToUpdate = await _unitOfWork.ExpenseCategoryRepository.GetByIdAsync(request.Id);
            if (categoryToUpdate == null )
            {
                _logger.LogWarning("Update category failed, {CategoryId} not found.", request.Id);
                throw new InvalidOperationException($"ID:{request.Id} not found");
            }
            var nameExists = await _unitOfWork.ExpenseCategoryRepository
                .AnyAsync(c => c.Name.ToLower() == request.Category.Name.ToLower() 
                                                 && c.IsActive
                                                 && c.Id != request.Id);
            if (nameExists)
            {
                _logger.LogWarning("Category name already exists: {CategoryName}", request.Category.Name);
                throw new InvalidOperationException($"Category name '{request.Category.Name}' already exists.");
            }
            try
            {
             _mapper.Map(request.Category, categoryToUpdate);
                categoryToUpdate.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.ExpenseCategoryRepository.Update(categoryToUpdate);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Category updated successfully with ID: {CategoryId}", request.Id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the expense category with ID '{CategoryId}'.", request.Id);
                throw;
            }
        }
        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteCategoryCommand for category ID: {CategoryId}", request.Id);
            var categoryToDelete = await _unitOfWork.ExpenseCategoryRepository.GetByIdAsync(request.Id);
            if (categoryToDelete == null)
            {
                _logger.LogWarning("Delete category failed, {CategoryId} not found.", request.Id);
                throw new InvalidOperationException( $"{ request.Id } not found");
            }
            if(!categoryToDelete.IsActive)
            {
                _logger.LogWarning("Delete category failed, {CategoryId} already inactive.", request.Id);
                return Unit.Value;
            }
            var hasExpenses = await _unitOfWork.ExpenseCategoryRepository.HasAssociatedActiveExpensesAsync(request.Id);
            if (hasExpenses)
            {
                    _logger.LogWarning("Delete category failed, {CategoryId} has active expenses.", request.Id);
                throw new InvalidOperationException($"Category with ID '{request.Id}' has active expenses and cannot be deleted.");
            }
            try
            {
                categoryToDelete.IsActive = false;
                categoryToDelete.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.ExpenseCategoryRepository.Update(categoryToDelete);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Category deleted successfully with ID: {CategoryId}", request.Id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the expense category with ID '{CategoryId}'.", request.Id);
                throw;
            }
        }
    }
}
