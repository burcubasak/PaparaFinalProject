using FieldExpenseManager.FieldExpense.Application.DTOs.Reporting;
using FieldExpenseManager.FieldExpense.Domain.Enums;

namespace FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories
{
    public interface IReportingRepository
    {
        Task<IEnumerable<PersonnelExpenseReportDto>> GetPersonnelExpenseHistoryAsync(

            int personnelUserId,
            DateTime? startDate,
            DateTime? endDate);

        Task<IEnumerable<PaymentDensityReportDto>> GetPaymentDensityReportAsync(
            ReportPeriod period,
            DateTime startDate,
            DateTime endDate);

        Task<IEnumerable<PersonnelSpendingReportDto>> GetPersonnelSpendingReportAsync(

                DateTime startDate,
                DateTime endDate,
                ReportPeriod period,
                int? personnelId = null);

        Task<IEnumerable<ApprovalStatusReportDto>> GetApprovalStatusReportAsync(
            DateTime startDate,
            DateTime endDate,
            ReportPeriod period);
    }
}
