using AutoMapper;
using FieldExpenseManager.FieldExpense.Application.DTOs.User;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using MediatR;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.User.Queries
{
    public class UserQueryHandler:
        IRequestHandler<GetAllUsersQuery, List<UserDto>>,
        IRequestHandler<UserByIdQuery, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserQueryHandler> _logger;
        public UserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving all users.");
                var users = await _unitOfWork.UserRepository.GetAllAsync();
                var userDtos = _mapper.Map<List<UserDto>>(users);
                _logger.LogInformation("Successfully retrieved users.");
                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all users.");
                throw;
            }
        }
        public async Task<UserDto> Handle(UserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving user with ID: {UserId}", request.Id);
                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", request.Id);
                    throw new InvalidOperationException($"ID'si {request.Id} olan kullanıcı bulunamadı.");
                }

                var userDto = _mapper.Map<UserDto>(user);
                _logger.LogInformation("Successfully retrieved user with ID {UserId}.", request.Id);
                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user with ID {UserId}.", request.Id);
                throw;
            }
        }
    }
}
