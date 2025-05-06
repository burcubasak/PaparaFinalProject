using FieldExpenseManager.FieldExpense.Application.Cqrs.User.Commands;
using FieldExpenseManager.FieldExpense.Application.Cqrs.User.Queries;
using FieldExpenseManager.FieldExpense.Application.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace FieldExpenseManager.FieldExpense.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles="Admin")]
    public class UserController:ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var query = new UserByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var command = new CreateUserCommand(createUserDto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
        }
        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var command = new UpdateUserCommand(id, updateUserDto);
            var result = await _mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var command = new DeleteUserCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
