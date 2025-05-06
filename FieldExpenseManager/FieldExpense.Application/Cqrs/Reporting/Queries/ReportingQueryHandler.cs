using FieldExpenseManager.FieldExpense.Application.DTOs.Reporting;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories;
using MediatR;
using System.Security.Claims;

namespace FieldExpenseManager.FieldExpense.Application.Cqrs.Reporting.Queries
{
    public class ReportingQueryHandler:
        IRequestHandler<GetPersonnelExpenseReportQuery,IEnumerable<PersonnelExpenseReportDto>>,
        IRequestHandler<GetPaymentDensityReportQuery,IEnumerable<PaymentDensityReportDto>>,
        IRequestHandler<GetPersonnelSpendingReportQuery, IEnumerable<PersonnelSpendingReportDto>>,
        IRequestHandler<GetApprovalStatusReportQuery, IEnumerable<ApprovalStatusReportDto>>
    {
        private readonly IReportingRepository _reportingRepository;
        private readonly ILogger<ReportingQueryHandler> _logger;
        public IHttpContextAccessor _httpContextAccessor;
        public ReportingQueryHandler(
            IReportingRepository reportingRepository,
            ILogger<ReportingQueryHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _reportingRepository = reportingRepository ;
            _logger = logger ;
            _httpContextAccessor = httpContextAccessor ;
        }
        public async Task<IEnumerable<PersonnelExpenseReportDto>>Handle(GetPersonnelExpenseReportQuery request, CancellationToken cancellationToken)
        {
            var personnelUserId = GetCurrentUserId();
            if (personnelUserId == 0)
            {
                _logger.LogWarning("Could not determine current user ID for personnel expense report.");
                throw new UnauthorizedAccessException("User ID could not be determined from token.");
            }
            _logger.LogInformation("Handling GetPersonnelExpenseReportQuery for User ID: {UserId}", personnelUserId);
            try
            {
                var report = await _reportingRepository.GetPersonnelExpenseHistoryAsync(

                    personnelUserId,
                    request.StartDate,
                    request.EndDate
                    );
                _logger.LogInformation("Successfully retrieved personnel expense report for User ID: {UserId}", personnelUserId);
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating personnel expense report for User ID: {UserId}", personnelUserId);
                throw;
            }
        }

        public async Task<IEnumerable<PaymentDensityReportDto>>Handle(GetPaymentDensityReportQuery request, CancellationToken cancellationToken)
        {
           _logger.LogInformation("Handling GetPaymentDensityReportQuery");
            if (request.StartDate>request.EndDate)
            {
                _logger.LogWarning("Start date is greater than end date.");
                throw new ArgumentException("Start date cannot be greater than end date.");
            }
            try
            {
                var report = await _reportingRepository.GetPaymentDensityReportAsync(

                    request.Period,
                    request.StartDate,
                    request.EndDate
                    );
                _logger.LogInformation("Successfully retrieved payment density report.");
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating payment density report.");
                throw;
            }
        }
        public async Task<IEnumerable<PersonnelSpendingReportDto>> Handle(GetPersonnelSpendingReportQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate > request.EndDate)
            { 
                _logger.LogWarning("Start date is greater than end date.");
                throw new ArgumentException("Start date cannot be greater than end date.");
            }
            try
            {
                var report = await _reportingRepository.GetPersonnelSpendingReportAsync(
                    request.StartDate,
                    request.EndDate,
                    request.Period,
                    request.PersonnelId
                    );
                _logger.LogInformation("Successfully retrieved personnel spending report.");
                return report;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating personnel spending report.");
                throw;
            }
        
        }

        public async Task<IEnumerable<ApprovalStatusReportDto>>Handle(GetApprovalStatusReportQuery request,CancellationToken cancellationToken)
        {
            if (request.StartDate > request.EndDate)
            {
                _logger.LogWarning("Start date is greater than end date.");
                throw new ArgumentException("Start date cannot be greater than end date.");
            }
            try
            {
                var report = await _reportingRepository.GetApprovalStatusReportAsync(
                    request.StartDate,
                    request.EndDate,
                    request.Period
                    );
                _logger.LogInformation("Successfully retrieved approval status report.");
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating approval status report.");
                throw;
            }
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            _logger.LogWarning("Could not parse User ID from claims in ReportingQueryHandler.");
            return 0;
        }
    }
}
