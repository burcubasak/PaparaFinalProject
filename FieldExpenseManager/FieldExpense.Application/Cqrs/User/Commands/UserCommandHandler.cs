using AutoMapper;
using FieldExpenseManager.FieldExpense.Application.DTOs.User;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using FieldExpenseManager.FieldExpense.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.Identity.Client;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.User.Commands
{
    public class UserCommandHandler :
        IRequestHandler<CreateUserCommand, UserDto>,
        IRequestHandler<UpdateUserCommand, Unit>,
        IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IValidator<UpdateUserDto> _updateUserValidator;
        private readonly ILogger<UserCommandHandler> _logger;

        public UserCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateUserDto> createUserValidator,
            IValidator<UpdateUserDto> updateUserValidator,
            ILogger<UserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
            _logger = logger;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateUserCommand for email:{UserEmail}", request.UserData.Email);
            var validationResult = await _createUserValidator.ValidateAsync(request.UserData, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }
            var emailUnique = await _unitOfWork.UserRepository.IsEmailUniqueAsync(request.UserData.Email, null);
            if (emailUnique)
            {
                _logger.LogWarning("Email already exists: {Email}", request.UserData.Email);
                throw new InvalidOperationException("Email already exists");
            }
            if (request.UserData.Role == UserRole.Admin && !string.IsNullOrEmpty(request.UserData.IBAN))
            {
                _logger.LogWarning("Create user failed: Admin role cannot have an IBAN.");
                throw new InvalidOperationException("Admin user should not have IBAN");
            }
            try
            {
                var newUser = _mapper.Map<Domain.Entities.User>(request.UserData);
                newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.UserData.Password);
                newUser.IsActive = true;

                await _unitOfWork.UserRepository.AddAsync(newUser);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("User created successfully with ID: {UserId}", newUser.Id);
                return _mapper.Map<UserDto>(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the new user '{UserEmail}'.", request.UserData.Email);
                throw;
            }
        }
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateUserCommand for user ID: {UserId}", request.Id);
            var validationResult = await _updateUserValidator.ValidateAsync(request.UserData, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", request.Id);
                throw new InvalidOperationException("User not found");
            }
            if(!user.IsActive)
            {
                _logger.LogWarning("User with ID: {UserId} is already inactive.", request.Id);
                throw new InvalidOperationException("User is inactive and cannot be updated.");
            }
            if (user.Role == UserRole.Admin && !string.IsNullOrEmpty(request.UserData.IBAN))
            {
                _logger.LogWarning("Update user failed: Admin role cannot have an IBAN.");
                throw new InvalidOperationException("Admin user should not have IBAN");
            }
            try
            {
                _mapper.Map(request.UserData, user);
                user.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("User updated successfully with ID: {UserId}", user.Id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user with ID '{UserId}'.", request.Id);
                throw;
            }
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteUserCommand for user ID: {UserId}", request.Id);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", request.Id);
                throw new InvalidOperationException("User not found");
            }
            if(!user.IsActive)
            {
                _logger.LogWarning("User with ID: {UserId} is already inactive.", request.Id);
                return Unit.Value;
            }
            if (user.Role == UserRole.Admin)
            {
                _logger.LogWarning("Delete user failed: Cannot delete an admin user.");
                throw new InvalidOperationException("Cannot delete an admin user");
            }
            try
            {
                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("User deleted successfully with ID: {UserId}", request.Id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user with ID '{UserId}'.", request.Id);
                throw;
            }
        }
    }
}