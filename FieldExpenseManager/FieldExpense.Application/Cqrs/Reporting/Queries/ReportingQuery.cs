using FieldExpenseManager.FieldExpense.Application.DTOs.Reporting;
using FieldExpenseManager.FieldExpense.Domain.Enums;
using MediatR;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Reporting.Queries
{
    public record GetPersonnelExpenseReportQuery(
        DateTime? StartDate,
        DateTime? EndDate
    ) : IRequest<IEnumerable<PersonnelExpenseReportDto>>;

    public record GetPaymentDensityReportQuery(
        DateTime StartDate,
        DateTime EndDate,
        ReportPeriod Period
    ) : IRequest<IEnumerable<PaymentDensityReportDto>>;

    public record GetPersonnelSpendingReportQuery(
        DateTime StartDate,
        DateTime EndDate,
        ReportPeriod Period,
        int? PersonnelId = null
    ) : IRequest<IEnumerable<PersonnelSpendingReportDto>>;

    public record GetApprovalStatusReportQuery(
        DateTime StartDate,
        DateTime EndDate,
        ReportPeriod Period
    ) : IRequest<IEnumerable<ApprovalStatusReportDto>>;
}
