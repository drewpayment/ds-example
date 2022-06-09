using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Banks;
using Dominion.Domain.Interfaces.Query.Payroll;
using Dominion.Pay.Dto.Pay;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository used to manipulate various Payroll related Entities in a data store.
    /// </summary>
    public interface IPayrollRepository : IRepository, IDisposable
    {
        /// <summary>
        /// Queries check stub configuration client settings. <see cref="CheckStubConfiguration"/>
        /// </summary>
        /// <returns></returns>
        ICheckStubConfigurationQuery QueryCheckStubConfigurations();

        /// <summary>
        /// Provides a way to build and execute a query on payrolls.
        /// <see cref="Payroll"/>
        /// </summary>
        /// <returns></returns>
        IPayrollQuery QueryPayrolls();

        /// <summary>
        /// Creates a new query on <see cref="PayData"/>
        /// </summary>
        /// <returns></returns>
        IPayrollPayDataQuery QueryPayData();

        /// <summary>
        /// Provides a way to build and execute a query on payroll pay data interface (import) types.
        /// <see cref="PayrollPayDataInterface"/>
        /// </summary>
        /// <returns></returns>
        IPayrollPayDataInterfaceQuery QueryPayrollPayDataInterfaces();

        /// <summary>
        /// Provides a way to build and execute a query on a client specific interface (import) configurations.
        /// <see cref="ClientPayDataInterface"/>
        /// </summary>
        /// <returns></returns>
        IClientPayDataInterfaceQuery QueryClientPayDataInterfaces();

        /// <summary>
        /// Creates a new query on <see cref="ClientEarning"/>
        /// </summary>
        /// <returns></returns>
        IClientEarningQuery QueryClientEarnings();

        /// <summary>
        /// Creates a new query on <see cref="EmployeePay"/>
        /// </summary>
        /// <returns></returns>
        IEmployeePayQuery QueryEmployeePay();

        /// <summary>
        /// Provides a way to query an employee's pay info history.
        /// <see cref="EmployeePayChangeHistory"/>
        /// </summary>
        /// <returns></returns>
        IEmployeePayChangeHistoryQuery QueryEmployeePayChangeHistory();

        /// <summary>
        /// Provides a way to query a paycheck's <see cref="PreviewPaycheckEarning"/> records.
        /// </summary>
        /// <returns></returns>
        IPreviewPaycheckEarningQuery QueryPreviewPaycheckEarnings();

        /// <summary>
        /// Provides a way to query <see cref="ClientEarningWithholdingOverride"/> records.
        /// </summary>
        /// <returns></returns>
        IClientEarningWithholdingOverrideQuery QueryClientEarningWithholdingOverrides();

        /// <summary>
        /// Creates a new query on <see cref="PayFrequency"/> data.
        /// </summary>
        /// <returns></returns>
        IPayFrequencyQuery QueryPayFrequencies(); 

        /// <summary>
        /// Creates a new query on <see cref="ClientVendor" /> data.
        /// </summary>
        /// <returns></returns>
        IClientVendorQuery QueryClientVendors();

        /// <summary>
        /// Creates a new query on <see cref="Plan"/>
        /// </summary>
        /// <returns></returns>
        IClientPlanQuery QueryPlans();

        /// <summary>
        /// Creates a new query on <see cref="PayrollCheckSeq"/>
        /// </summary>
        /// <returns></returns>
        IPayrollCheckSeqQuery PayrollCheckSeqQuery();

        /// <summary>
        /// Creates a new query on <see cref="CheckStockOrder"/> data.
        /// </summary>
        /// <returns></returns>
        ICheckStockOrdersQuery QueryCheckStockOrders();
        
        /// <summary>
        /// Creates a new query on <see cref="PayGrade"/> data.
        /// </summary>
        /// <returns></returns>
        IPayGradeQuery QueryPayGrades();

        /// <summary>
        /// Creates a new query on <see cref="PaycheckPayDataHistory"/>
        /// </summary>
        /// <returns></returns>
        IPaycheckPayDataHistoryQuery QueryPaycheckPayDataHistory();

        /// <summary>
        /// Creates a new query on <see cref="PaycheckTaxHistory"/>
        /// </summary>
        /// <returns></returns>
        IPaycheckTaxHistoryQuery QueryPaycheckTaxHistory();

        /// <summary>
        /// Creates a new query on <see cref="PaycheckSUTAHistory"/>
        /// </summary>
        /// <returns></returns>
        IPaycheckSUTAHistoryQuery QueryPaycheckSUTAHistory();

        /// <summary>
        /// Creates a new query on <see cref="TaxDefermentHistory"/>
        /// </summary>
        /// <returns></returns>
        ITaxDefermentHistoryQuery QueryTaxDefermentHistory();

        /// <summary>
        /// Creates a new query on <see cref="PayrollFederalTaxCredit"/>
        /// </summary>
        /// <returns></returns>
        IPayrollFederalTaxCreditQuery QueryPayrollFederalTaxCredit();

        /// <summary>
        /// Creates a new query on <see cref="ClientDeduction"/>
        /// </summary>
        /// <returns></returns>
        IClientDeductionQuery QueryClientDeduction();

        /// <summary>
        /// Creates a new query on <see cref="ClientMatch"/>
        /// </summary>
        /// <returns></returns>
        IClientMatchQuery QueryClientMatch();

        /// <summary>
        /// Creates a new query on <see cref="AdditionalAmountTypeInfo"/> data.
        /// </summary>
        /// <returns></returns>
        IAdditionalAmountTypeInfoQuery QueryAdditionalAmountTypeInformation();

        /// <summary>
        /// Creates a new query on <see cref="EmployeeDeductionAmountTypeInfo"/>
        /// </summary>
        /// <returns></returns>
        IEmployeeDeductionAmountTypeInfoQuery QueryEmployeeDeductionAmountTypeInfoQuery();

        /// <summary>
        /// Creates a new query <see cref="ClientDeductionMappingPlanInfo"/>
        /// </summary>
        /// <returns></returns>
        IClientDeductionMappingPlanQuery QueryClientDeductionMappingPlan();

        /// <summary>
        /// Creates a new query <see cref="PaycheckEarningHistory"/>
        /// </summary>
        /// <returns></returns>
        IPaycheckEarningHistoryQuery QueryPaycheckEarningHistory();

        /// <summary>
        /// Creates a new query <see cref="BankHoliday"/>
        /// </summary>
        /// <returns></returns>
        IBankHolidayQuery QueryBankHoliday();

        /// <summary>
        /// Creates a new query <see cref="PayrollControlTotal"/>
        /// </summary>
        /// <returns></returns>
        IPayrollControlTotalQuery QueryPayrollControlTotals();
		
        /// <summary>
        /// Creates a new query on <see cref="PaycheckHistory"/>
        /// </summary>
        /// <returns></returns>
        IPaycheckHistoryQuery PaycheckHistoryQuery();

        /// <summary>
        /// Creates new query on <see cref="PaycheckDeductionHistory"/>
        /// </summary>
        /// <returns></returns>
        IPaycheckDeductionHistoryQuery PaycheckDeductionHistoryQuery();

        /// <summary>
        /// Creates new query on <see cref="RemoteCheckHistory"/>
        /// </summary>
        /// <returns></returns>
        IRemoteCheckHistoryQuery RemoteCheckHistoryQuery();

        IPayrollHistoryQuery QueryPayrollHistory();

        IVendorPaymentHistoryQuery QueryVendorPaymentHistory();
        IDominionVendorQuery QueryDominionVendor();
        ITaxVendorQuery QueryTaxVendor();
		IVendorQuery QueryVendor();
        ITaxFrequencyQuery QueryTaxFrequency();
        IPayrollEmailLogQuery QueryPayrollEmailLog();
        IPayrollRunQuery QueryPayrollRun();
        IOneTimeEarningQuery QueryOneTimeEarnings();
        IMeritIncreaseQuery QueryMeritIncreases();

        /// <summary>
        /// Wraps stored procedure [dbo].[spGetCurrentPaycheck].  Returns ALL paychecks for a given employee ordered by most recent first.  
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        IEnumerable<PaycheckDto> GetCurrentPaychecksSproc(int employeeId);

        /// <summary>
        /// Wraps stored procedure [dbo].[spReportPaycheck_Deductions].  Returns an enumerable of 
        /// ReportyPayCheckDeductionsDto
        /// </summary>
        /// <param name="genPaycheckHistoryId"></param>
        /// <returns></returns>
        IEnumerable<PaycheckDeductionDto> GetPaycheckDeductionsSproc(int genPaycheckHistoryId);

        /// <summary>
        /// Wraps stored procedure [dbo].[spReportPaycheck_Earnings].  Returns an enumerable of 
        /// ReportPaycheckEarningsDto
        /// </summary>
        /// <param name="genPaycheckHistoryId"></param>
        /// <returns></returns>
        IEnumerable<PaycheckEarningDto> GetPaycheckEarningsSproc(int genPaycheckHistoryId);

        /// <summary>
        /// Wraps stored procedure [dbo].[spReportPaycheck_MiddleStubRight].  Returns an enumerable of 
        /// ReportPaycheckDisbursementsDto
        /// </summary>
        /// <param name="genPaycheckHistoryId"></param>
        /// <returns></returns>
        IEnumerable<PaycheckDisbursementDto> GetPaycheckMiddleStubRightSproc(int genPaycheckHistoryId);

        /// <summary>
        /// Wraps stored procedure [dbo].[spReportPaycheck_MiddleStubRight_PaidBenefits].  Returns an enumerable of 
        /// ReportPaycheckCompanyPaidBenefits
        /// 
        /// </summary>
        /// <param name="genPaycheckHistoryId"></param>
        /// <returns></returns>
        IEnumerable<PaycheckCompanyPaidBenefitDto> GetPaycheckMiddleStubRightPaidBenefitsSproc(int genPaycheckHistoryId);

        /// <summary>
        /// Wraps stored procedure [dbo].[spReportPaycheck_Earnings_Detail].  Returns an enumerable of 
        /// ReportPaycheckEarningsDto
        /// </summary>
        /// <param name="genPaycheckHistoryId"></param>
        /// <returns></returns>
        IEnumerable<PaycheckEarningDetailDto> GetPaycheckEarningsDetailSproc(int genPaycheckHistoryId);

        /// <summary>
        /// Wraps stored procedure [dbo].[spReportPaycheck_MiddleStub].  Returns an enumerable of 
        /// ReportPaycheckEarningsDto
        /// </summary>
        /// <param name="genPaycheckHistoryId"></param>
        /// <returns></returns>
        IEnumerable<PayCheckMiddleStubDto> GetPaycheckEarningsHoursSproc(int genPaycheckHistoryId);

        /// <summary>
        /// Wraps stored procedure [dbo].[spReportPaycheck_MiddleStub].  Returns an enumerable of 
        /// ReportPaycheckEarningsDto
        /// </summary>
        /// <param name="genPaycheckHistoryId"></param>
        /// <returns></returns>
        IEnumerable<PaystubOptionDto> GetPaystubOptionsSproc(int genPaycheckHistoryId);

        /// <summary>
        /// Provides a way to build and execute queries on PayrollAdjustments
        /// </summary>
        /// <returns></returns>
        IPayrollAdjustmentQuery QueryPayrollAdjustment();

        /// <summary>
        /// Provides a way to build and execute queries on PayrollEmployeeOverride
        /// </summary>
        /// <returns></returns>
        IPayrollEmployeeOverrideQuery QueryPayrollEmployeeOverride();

        IGenW2ClientHistoryQuery QueryGenW2ClientHistory();
    }
}