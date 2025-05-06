using FieldExpenseManager.FieldExpense.Application.DTOs.User;
using MediatR;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.User.Queries
{
    public record UserByIdQuery(int Id) : IRequest<UserDto>;
    public record GetAllUsersQuery() : IRequest<List<UserDto>>;
}
