using Dapper;
using FieldExpenseManager.FieldExpense.Application.DTOs.Reporting;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories;
using FieldExpenseManager.FieldExpense.Domain.Entities;
using FieldExpenseManager.FieldExpense.Domain.Enums;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FieldExpenseManager.FieldExpense.Infrastructure.Repositories
{
    public class ReportingRepository : IReportingRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReportingRepository> _logger;
        public ReportingRepository(
            IConfiguration configuration,
            ILogger<ReportingRepository> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IEnumerable<PersonnelExpenseReportDto>> GetPersonnelExpenseHistoryAsync(int personnelUserId, DateTime? startDate, DateTime? endDate)
        {
            _logger.LogInformation("Fetching expense history report for User ID: { UserId}", personnelUserId);

            var sql = @"
                    SELECT 
                        e.Id AS ExpenseId,
                        e.Description,
                        e.ExpenseDate,
                        e.Amount,
                        e.Status,
                        ec.Name AS CategoryName,
                        e.ProcessedDate,
                        e.RejectionReason
                    FROM 
                        Expenses e
                    INNER JOIN 
                        ExpenseCategories ec ON e.ExpenseCategoryId = ec.Id
                   WHERE e.UserId = @UserId
                     AND e.IsActive = 1
                     AND (@StartDate IS NULL OR e.ExpenseDate >= @StartDate) 
                     AND (@EndDate IS NULL OR e.ExpenseDate <= @EndDate)    
                    ORDER BY e.ExpenseDate DESC;
               ";
            _logger.LogDebug("Executing Dapper query for personnel expense history: {Sql}", sql);
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    dbConnection.Open();

                    var result = await dbConnection.QueryAsync<PersonnelExpenseReportDto>(
                       sql,
                       new { UserId = personnelUserId, StartDate = startDate, EndDate = endDate }
                   );

                    _logger.LogInformation("Successfully retrieved {Count} expense history records for User ID: {UserId}", result.Count(), personnelUserId);
                    return result;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error executing Dapper query for personnel expense history for User ID: {UserId}", personnelUserId);
                throw;
            }
        }
        public async Task<IEnumerable<PaymentDensityReportDto>> GetPaymentDensityReportAsync(ReportPeriod period, DateTime startDate, DateTime endDate)
        {
            string sql;

            if (period == ReportPeriod.Daily)

            {
                sql = @"
                    SELECT 
                    CONVERT(date, PaymentDate) AS Period,
                    SUM(Amount) AS TotalAmountPaid,
                    COUNT(*) AS PaymentCount
                    FROM ExpensePaymentses
                    WHERE IsSuccessful = 1
                    AND  PaymentDate >= @StartDate AND PaymentDate <= @EndDate
                    AND IsActive = 1
                    GROUP BY CONVERT(date, PaymentDate)
                    ORDER BY Period;
                ";
            }
            else if (period == ReportPeriod.Weekly)
            {
                sql = @"
                    SELECT
                    DATEADD(wk, DATEDIFF(wk, 0, PaymentDate), 0) AS Period,
                    SUM(Amount) AS TotalAmountPaid,
                    COUNT(*) AS PaymentCount
                    FROM ExpensePaymentses
                    WHERE IsSuccessful = 1
                    AND PaymentDate >= @StartDate AND PaymentDate <= @EndDate
                    AND IsActive = 1
                    GROUP BY DATEADD(wk, DATEDIFF(wk, 0, PaymentDate), 0)
                    ORDER BY Period;
                ";

            }
            else
            {
                sql = @"
                    SELECT 
                    DATEFROMPARTS(YEAR(PaymentDate), MONTH(PaymentDate), 1) AS Period,
                    SUM(Amount) AS TotalAmountPaid,
                    COUNT(*) AS PaymentCount
                    FROM ExpensePaymentses
                    WHERE IsSuccessful = 1
                    AND PaymentDate >= @StartDate AND PaymentDate <= @EndDate
                    AND IsActive = 1
                    GROUP BY DATEFROMPARTS(YEAR(PaymentDate), MONTH(PaymentDate), 1)
                    ORDER BY Period;
                ";
            }
            var endOfDay = endDate.AddDays(1).AddTicks(-1);
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    dbConnection.Open();

                    var result = await dbConnection.QueryAsync<PaymentDensityReportDto>(
                        sql,
                        new { StartDate = startDate, EndDate = endOfDay }
                    );

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating payment density report for period: {Period}", period);
                throw;
            }
        }
        public async Task<IEnumerable<PersonnelSpendingReportDto>> GetPersonnelSpendingReportAsync(
                DateTime startDate,
                DateTime endDate,
                ReportPeriod period,
                int? personnelId = null)
        {
            string sqlQuery;
            string groupByClause;
            string selectPeriodClause;
            object parameters;

            if (period == ReportPeriod.Daily)
            {
                groupByClause = "CONVERT(date, e.ExpenseDate)";
                selectPeriodClause = "CONVERT(date, e.ExpenseDate) AS Period";
            }
            else if (period == ReportPeriod.Weekly)
            {
                groupByClause = "DATEADD(wk, DATEDIFF(wk, 0, e.ExpenseDate), 0)";
                selectPeriodClause = "DATEADD(wk, DATEDIFF(wk, 0, e.ExpenseDate), 0) AS Period";
            }
            else
            {
                groupByClause = "DATEFROMPARTS(YEAR(e.ExpenseDate), MONTH(e.ExpenseDate), 1)";
                selectPeriodClause = "DATEFROMPARTS(YEAR(e.ExpenseDate), MONTH(e.ExpenseDate), 1) AS Period";
            }
            var endOfDay = endDate.AddDays(1).AddTicks(-1);
            if (personnelId.HasValue)
            {
                _logger.LogInformation("Filtering by PersonnelId: {PersonnelId}", personnelId);
                sqlQuery = $@"
                    SELECT 
                        e.UserId As PersonnelId,
                        u.FirstName +' ' + u.LastName AS PersonnelName,
                        {selectPeriodClause},
                        SUM(e.Amount) AS TotalAmountSpent,
                        COUNT(e.Id) AS ExpenseCount
                    FROM 
                        Expenses e
                    INNER JOIN 
                        Users u ON e.UserId = u.Id
                    WHERE 
                        e.UserId = @PersonnelId
                        AND e.IsActive = 1
                        AND e.ExpenseDate >= @StartDate
                        AND e.ExpenseDate <= @EndDate
                    GROUP BY 
                        e.UserId,u.FirstName, u.LastName, {groupByClause}
                        
                    ORDER BY 
                        Period, PersonnelName;
                ";
                parameters = new { PersonnelId = personnelId.Value, StartDate = startDate, EndDate = endOfDay };
            }
            else
            {
                sqlQuery = $@"
                    SELECT 
                        e.UserId AS PersonnelId,
                        u.FirstName + ' ' + u.LastName AS PersonnelName,
                        {selectPeriodClause},
                        SUM(e.Amount) AS TotalAmountSpent,
                        COUNT(e.Id) AS ExpenseCount
                    FROM 
                        Expenses e
                    INNER JOIN 
                        Users u ON e.UserId = u.Id
                    WHERE 
                        e.IsActive = 1
                        AND e.ExpenseDate >= @StartDate AND e.ExpenseDate <= @EndDate
                    GROUP BY 
                       e.UserId, u.FirstName, u.LastName, {groupByClause}
                    ORDER BY 
                        Period,PersonnelName;
                ";
                parameters = new { StartDate = startDate, EndDate = endOfDay };
            }
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    dbConnection.Open();
                    var result = await dbConnection.QueryAsync<PersonnelSpendingReportDto>(

                        sqlQuery, parameters
                    );
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating expense category report.");
                throw;
            }
        }
        public async Task<IEnumerable<ApprovalStatusReportDto>> GetApprovalStatusReportAsync(
               DateTime startDate,
               DateTime endDate,
               ReportPeriod period)
        {
            string groupByClause;
            string selectPeriodClause;

            if (period == ReportPeriod.Daily)
            {
                selectPeriodClause = "CONVERT(date, ProcessedDate) AS Period";
                groupByClause = "CONVERT(date, ProcessedDate)";
            }
            else if (period == ReportPeriod.Weekly)
            {
                selectPeriodClause = "DATEADD(wk, DATEDIFF(wk, 0, ProcessedDate), 0) AS Period";
                groupByClause = "DATEADD(wk, DATEDIFF(wk, 0, ProcessedDate), 0)";
            }
            else 
            {
                selectPeriodClause = "DATEFROMPARTS(YEAR(ProcessedDate), MONTH(ProcessedDate), 1) AS Period";
                groupByClause = "DATEFROMPARTS(YEAR(ProcessedDate), MONTH(ProcessedDate), 1)";
            }

            string sql = $@"
           SELECT 
            {selectPeriodClause},
            ISNULL(SUM(CASE WHEN Status = {(int)ExpenseStatus.Approved} THEN Amount ELSE 0 END), 0) AS TotalApprovedAmount,
            ISNULL(SUM(CASE WHEN Status = {(int)ExpenseStatus.Rejected} THEN Amount ELSE 0 END), 0) AS TotalRejectedAmount
           FROM Expenses
           WHERE 
            ProcessedDate IS NOT NULL
            AND IsActive = 1
            AND ProcessedDate >= @StartDate
            AND ProcessedDate <= @EndDate
           GROUP BY {groupByClause}
           ORDER BY Period;
            ";

            var endOfDay = endDate.AddDays(1).AddTicks(-1);
            try
            {
                using var dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                dbConnection.Open();

                var result = await dbConnection.QueryAsync<ApprovalStatusReportDto>(
                    sql,
                    new { StartDate = startDate, EndDate = endOfDay }
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching approval status report.");
                throw;
            }
        }
    }
}


