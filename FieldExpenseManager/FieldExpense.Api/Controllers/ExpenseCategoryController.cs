using FieldExpenseManager.FieldExpense.Application.Cqrs.ExpenseCategories.Commands;
using FieldExpenseManager.FieldExpense.Application.Cqrs.ExpenseCategories.Queries;
using FieldExpenseManager.FieldExpense.Application.DTOs.ExpenseCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FieldExpenseManager.FieldExpense.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseCategoryController: ControllerBase
    {
        private readonly IMediator _mediator;
        public ExpenseCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllExpenseCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetExpenseCategoryById([FromRoute] int id)
        {
            var query = new ExpenseCategoryByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize (Roles ="Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createDto)
        {
            var command = new CreateCategoryCommand(createDto);
            var resultDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetExpenseCategoryById), new { id = resultDto.Id }, resultDto);

        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id,[FromBody] UpdateCategoryDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var command = new UpdateCategoryCommand(id,updateDto);
           await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory([FromRoute]int id)
        {
            var command = new DeleteCategoryCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
