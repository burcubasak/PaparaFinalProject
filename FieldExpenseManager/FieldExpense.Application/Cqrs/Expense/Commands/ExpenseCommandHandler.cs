using AutoMapper;
using FieldExpenseManager.FieldExpense.Application.DTOs.Expense;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using FieldExpenseManager.FieldExpense.Domain.Enums;
using FluentValidation;
using MediatR;
using System.Security.Claims;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Commands
{
    public class ExpenseCommandHandler:
        IRequestHandler<CreateExpenseCommand, ExpenseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateExpenseDto> _validator;
        private readonly ILogger<ExpenseCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;
        public ExpenseCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateExpenseDto> validator,
            ILogger<ExpenseCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper ;
            _validator = validator ;
            _logger = logger ;
            _httpContextAccessor = httpContextAccessor ;
            _env = env ;
        }
        public async Task<ExpenseDto> Handle(CreateExpenseCommand request,CancellationToken cancellationToken)
        { 
            var currentUserId= GetCurrentUserId();
            _logger.LogInformation("Current User ID: {UserId}", currentUserId);
            if (currentUserId==0)
            {
                _logger.LogError("User not found");
                throw new UnauthorizedAccessException("User not found");
            }

            var validationResult = await _validator.ValidateAsync(request.ExpenseData, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            var category = await _unitOfWork.ExpenseCategoryRepository
                .GetByIdAsync(request.ExpenseData.ExpenseCategoryId);
            if (category == null || !category.IsActive)
            {
                _logger.LogError("Expense category not found or inactive");
                throw new InvalidOperationException("Expense category not found or inactive");
            }

            ExpenseAttachment? newAttachment = null;
            string? relativeUrl = null;
            if (request.AttachmentFile != null && request.AttachmentFile.Length > 0)
            { 
                _logger.LogInformation("Attachment file found");
                try
                {
                    var uploadsRootFolder = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, "uploads");
                    var targetSubFolder= "expenses-attachment";
                    var targetFolderPath = Path.Combine(uploadsRootFolder, targetSubFolder);
                    if (!Directory.Exists(targetFolderPath))
                    {
                        Directory.CreateDirectory(targetFolderPath);
                        _logger.LogInformation("Directory created: {Path}", targetFolderPath);
                    }
                    var uniqueFileName = $"{Guid.NewGuid().ToString()} { Path.GetExtension(request.AttachmentFile.FileName)}";
                    var targetFilePath = Path.Combine(targetFolderPath, uniqueFileName);

                    using (var stream = new FileStream(targetFilePath, FileMode.Create))
                    {
                        await request.AttachmentFile.CopyToAsync(stream, cancellationToken);
                        _logger.LogInformation("File copied to: {Path}", targetFilePath);
                    }
                    relativeUrl = Path.Combine("/uploads",targetSubFolder, uniqueFileName).Replace('\\','/');

                    newAttachment = new ExpenseAttachment
                    {
                        FileName = Path.GetFileName(request.AttachmentFile.FileName),
                        ContentType = request.AttachmentFile.ContentType,
                        FileSize = request.AttachmentFile.Length,
                        FilePathOrUrl = relativeUrl,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };
                    _logger.LogInformation("Attachment created: {FileName}", newAttachment.FileName);
                }
                catch (Exception ex)
                {

                    throw new InvalidOperationException("Error while saving the file", ex);
                }
            }
            try
            {
                var newExpense = _mapper.Map<Domain.Entities.Expense>(request.ExpenseData);
                newExpense.UserId = currentUserId;
                newExpense.Status = ExpenseStatus.Pending;
                newExpense.IsActive = true;

                if (newAttachment != null)
                {
                    newExpense.Attachments.Add(newAttachment);
                }
                await _unitOfWork.ExpenseRepository.AddAsync(newExpense);
                await _unitOfWork.CompleteAsync();
                var createdExpenseWithDetails = await _unitOfWork.ExpenseRepository
                    .GetByIdWithDetailsAsync(newExpense.Id);
                if (createdExpenseWithDetails == null)
                {
                    _logger.LogError("Created expense not found");
                    throw new InvalidOperationException("Created expense not found");
                }
                var expenseDto = _mapper.Map<ExpenseDto>(createdExpenseWithDetails);
                return expenseDto;
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
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return 0;
        }
    }
}
