using FieldExpenseManager.FieldExpense.Application.DTOs.User;
using MediatR;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.User.Commands
{
    public record CreateUserCommand(CreateUserDto UserData):IRequest<UserDto>;
    public record UpdateUserCommand(int Id, UpdateUserDto UserData) : IRequest<Unit>;
    public record DeleteUserCommand(int Id) : IRequest<Unit>;
}
