using FieldExpenseManager.FieldExpense.Application.Cqrs.Reporting.Queries;
using FieldExpenseManager.FieldExpense.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FieldExpenseManager.FieldExpense.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController: ControllerBase
    {
        private readonly IMediator _mediator;
        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("MyExpenseHistory")]
        public async Task<IActionResult> GetMyExpenseHistory([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetPersonnelExpenseReportQuery(startDate, endDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("PaymentDensity")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetPaymentDensity(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate,
            [FromQuery] ReportPeriod period
            )
        {
            var query = new GetPaymentDensityReportQuery(startDate, endDate,period);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("PersonnelSpending")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetPersonnelSpendingReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] ReportPeriod period,
            [FromQuery] int? personnelId = null
            )
        {
            var query = new GetPersonnelSpendingReportQuery(startDate, endDate, period, personnelId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("ApprovalStatus")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetApprovalStatusReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] ReportPeriod period
            )
        {
            var query = new GetApprovalStatusReportQuery(startDate, endDate, period);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
