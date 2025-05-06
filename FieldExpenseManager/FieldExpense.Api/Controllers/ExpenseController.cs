using FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Commands;
using FieldExpenseManager.FieldExpense.Application.Cqrs.Expense.Queries;
using FieldExpenseManager.FieldExpense.Application.DTOs.Expense;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace FieldExpenseManager.FieldExpense.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllPersonnel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllExpenses()
        {
            var query = new GetAllExpensesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("my")]
        [Authorize(Roles = "Personnel,Admin")]
        public async Task<IActionResult> GetMyExpenses()
        {
            var query = new GetMyExpensesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("ExpenseById/{id}")]
        public async Task<IActionResult> GetExpenseById([FromRoute] int id)
        {
            var query = new ExpenseByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Personnel")]
        public async Task<IActionResult> CreateExpense([FromForm] CreateExpenseRequest request)
        {
            var createDto = new CreateExpenseDto
            {
                ExpenseCategoryId = request.ExpenseCategoryId,
                Description = request.Description,
                Amount = request.Amount,
                ExpenseDate = request.ExpenseDate,
                Location = request.Location
            };

            var command = new CreateExpenseCommand(createDto, request.AttachmentFile);
            var resultDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetExpenseById), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectExpense([FromRoute] int id, [FromBody] RejectExpenseDto rejectExpense)
        { 
        var command= new RejectExpenseCommand(id, rejectExpense);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveExpense([FromRoute] int id)
        {
            var command = new ApproveExpenseCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteExpense([FromRoute] int id)
        {
            var command = new DeleteExpenseCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
