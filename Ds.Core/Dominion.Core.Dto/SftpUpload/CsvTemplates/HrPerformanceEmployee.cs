using System;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates
{
    public class HrPerformanceEmployee
    {
        public string Username { get; set; }
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string EmailAddress { get; set; }
        public string AccessLevel { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionName { get; set; }
        public string Location { get; set; }
        public double? Rate { get; set; }
        public int EmployeeStatusId { get; set; }
        public int PayTypeId { get; set; }
        public int PayFrequencyId { get; set; }
        public string JobTitle { get; set; }
        public string JobTitleDescription { get; set; }
        public string IsAppraiser { get; set; }
        public string Appraiser1UserName { get; set; }
        public string Appraiser1EmployeeId { get; set; }
        public string Appraiser1Weight { get; set; }
        public string Appraiser2UserName { get; set; }
        public string Appraiser2EmployeeId { get; set; }
        public string Appraiser2Weight { get; set; }
        public string AppraisalCycle { get; set; }
        public string AppraisalFrequency { get; set; }
        public string AppraisalStartDate { get; set; }
        public string AppraisalEndDate { get; set; }
        public string DueDate { get; set; }
        public string OverallGoalWeight { get; set; }
        public string DownlineView { get; set; }
        public string DownlineEdit { get; set; }
        public string IsMultipleAppraiser { get; set; }
        public DateTime? HireDate { get; set; }
        public string LastAppraisalDate { get; set; }
        public string InPosition { get; set; }
        public double? Hours { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public int? RaceId { get; set; }
        public string IncentivePay { get; set; }
        public string ChangeAmount { get; set; }
        public string Percentage { get; set; }
        public string LastPayChangeDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string CustomField1 { get; set; }
        public string SortField { get; set; }
        public DateTime? SeparationDate { get; set; }
        public double? HourlyRate { get; set; }
        public DateTime? DefaultEffectiveDate { get; set; }
    }
}