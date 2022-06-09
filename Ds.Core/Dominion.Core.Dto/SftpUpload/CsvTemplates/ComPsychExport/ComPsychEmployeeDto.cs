using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Interfaces;
using Dominion.Core.Dto.SftpUpload.CsvTemplates.ComPsychExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates
{
    /// <summary>
    /// Intermediate dto for mapping 
    /// from Employee entity
    /// => <see cref="ComPsychEmployeeDto"/> => <see cref="ComPsychExportLayout"/>.
    /// </summary>
    public class ComPsychEmployeeDto : IEmployeeReferenceDatesDto, IPhoneNumbersDto
    {
        #region IEmployeeBasicDto
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public int? ClientId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? JobProfileId { get; set; }
        #endregion

        #region EmployeeMisc
        public string SocialSecurityNumber { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? StateId { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }

        public string Gender { get; set; }

        /// <inheritdoc cref="_GetBinaryGenderCharOrNull(string)"/>
        public char? GenderChar => _GetBinaryGenderCharOrNull(Gender);
        public PayType? PayType { get; set; }
        #endregion

        #region Misc
        public string Email { get; set; }
        public EmployeeStatusType? EmployeeStatusIdEnum { get; set; }
        public bool? IsInactiveEmployeeStatusType => EmployeeStatusIdEnum.HasValue
            ? (EmployeeStatusIdEnum.Value.IsInactiveEmployeeStatusType() as bool?)
            : null
            ;

        /// <summary>
        /// See: <see cref="ComPsychExportLayout.Column_37_AK_Full_Time__Part_Time_Status"/>
        /// </summary>
        public FullTimePartTimeEnum? FullTimePartTime => _GetFullTimePartTimeOrNull(EmployeeStatusIdEnum);

        /// <summary>
        /// See: <see cref="ComPsychExportLayout.Column_37_AK_Full_Time__Part_Time_Status"/>
        /// </summary>
        public char? FullTimePartTimeChar => _GetFullTimePartTimeCharOrNull(FullTimePartTime);

        /// <summary>
        /// See: <see cref="ComPsychExportLayout.Column_46_AT_Scheduled_Hours_per_Week"/>.
        /// </summary>
        public int? ScheduledHoursPerWeek => _GetScheduledHoursPerWeekOrNull(FullTimePartTime);

        /// <summary>
        /// See: <see cref="ComPsychExportLayout.Column_47_AU_Scheduled__Number_of_Days_per_Week"/>
        /// </summary>
        public int? ScheduledDaysPerWeek => _GetScheduledDaysPerWeekOrNull(FullTimePartTime);
        #endregion

        #region IEmployeeReferenceDatesDto
        private readonly IEmployeeReferenceDatesDto _EmployeeReferenceDatesDto = new EmployeeReferenceDatesDto();
        public DateTime? BirthDate { get => _EmployeeReferenceDatesDto.BirthDate; set => _EmployeeReferenceDatesDto.BirthDate = value; }
        public DateTime? HireDate { get => _EmployeeReferenceDatesDto.HireDate; set => _EmployeeReferenceDatesDto.HireDate = value; }
        public DateTime? SeparationDate { get => _EmployeeReferenceDatesDto.SeparationDate; set => _EmployeeReferenceDatesDto.SeparationDate = value; }
        public DateTime? AnniversaryDate { get => _EmployeeReferenceDatesDto.AnniversaryDate; set => _EmployeeReferenceDatesDto.AnniversaryDate = value; }
        public DateTime? RehireDate { get => _EmployeeReferenceDatesDto.RehireDate; set => _EmployeeReferenceDatesDto.RehireDate = value; }
        public DateTime? EligibilityDate { get => _EmployeeReferenceDatesDto.EligibilityDate; set => _EmployeeReferenceDatesDto.EligibilityDate = value; }
        #endregion

        #region IPhoneNumbersDto
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        #endregion

        #region DirectSupervisor
        public int? DirectSupervisorUserId { get; set; }
        #endregion


        #region "PaycheckHistories"
        public IEnumerable<Payroll.PaycheckHistoryDto> PaycheckHistories { get; set; }
        #endregion


        #region "private static mapping functions"
        /// <summary>
        /// See: <see cref="ComPsychExportLayout.Column_15_O_Gender"/>
        /// </summary>
        /// <param name="gender"></param>
        /// <returns><c>('M' | 'F' | null)</c></returns>
        private static char? _GetBinaryGenderCharOrNull(string gender)
        {
            char firstNonBlankChar = gender
                .Trim()
                .Select(char.ToUpperInvariant)
                .FirstOrDefault()
                ;

            return (firstNonBlankChar == 'M' || firstNonBlankChar == 'F')
                ? firstNonBlankChar as char?
                : null
                ;
        }

        public enum FullTimePartTimeEnum
        {
            FullTime = 1,
            PartTime = 2,
        }

        /// <summary>
        /// See: <see cref="ComPsychExportLayout.Column_37_AK_Full_Time__Part_Time_Status"/>
        /// </summary>
        /// <param name="employeeStatusType"></param>
        /// <returns></returns>
        private static FullTimePartTimeEnum? _GetFullTimePartTimeOrNull(EmployeeStatusType? employeeStatusType)
        {
            if (!employeeStatusType.HasValue)
            {
                return null;
            }

            switch (employeeStatusType.Value)
            {
                case EmployeeStatusType.Unknown:
                    return null;
                case EmployeeStatusType.FullTime:
                    return FullTimePartTimeEnum.FullTime;
                case EmployeeStatusType.PartTime:
                    return FullTimePartTimeEnum.PartTime;
                case EmployeeStatusType.CallIn:
                    return null;
                case EmployeeStatusType.Special:
                    return null;
                case EmployeeStatusType.Layoff:
                    return null;
                case EmployeeStatusType.Terminated:
                    return null;
                case EmployeeStatusType.Manager:
                    return null;
                case EmployeeStatusType.LastPay:
                    return null;
                case EmployeeStatusType.FullTimeTemp:
                    return null;
                case EmployeeStatusType.MilitaryLeave:
                    return null;
                case EmployeeStatusType.LeaveOfAbsence:
                    return null;
                case EmployeeStatusType.StudentIntern:
                    return null;
                case EmployeeStatusType.Retired:
                    return null;
                case EmployeeStatusType.Seasonal:
                    return null;
                case EmployeeStatusType.PartTimeTemp:
                    return null;
                case EmployeeStatusType.Severance:
                    return null;
                case EmployeeStatusType.Furlough:
                    return null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// See: <see cref="ComPsychExportLayout.Column_37_AK_Full_Time__Part_Time_Status"/>
        /// </summary>
        /// <param name="fullTimePartTime"></param>
        /// <returns></returns>
        private static char? _GetFullTimePartTimeCharOrNull(FullTimePartTimeEnum? fullTimePartTime)
        {
            if (!fullTimePartTime.HasValue)
            {
                return null;
            }

            switch (fullTimePartTime.Value)
            {
                case FullTimePartTimeEnum.FullTime:
                    return 'F';
                case FullTimePartTimeEnum.PartTime:
                    return 'P';
                default:
                    return null;
            }
        }

        /// <summary>
        /// See: <see cref="ComPsychExportLayout.Column_46_AT_Scheduled_Hours_per_Week"/>
        /// </summary>
        /// <param name="fullTimePartTime"></param>
        /// <returns></returns>
        private static int? _GetScheduledHoursPerWeekOrNull(FullTimePartTimeEnum? fullTimePartTime) => (8 * _GetScheduledDaysPerWeekOrNull(fullTimePartTime));

        /// <summary>
        /// See: <see cref="ComPsychExportLayout.Column_47_AU_Scheduled__Number_of_Days_per_Week"/>
        /// </summary>
        /// <param name="fullTimePartTime"></param>
        /// <returns></returns>
        private static int? _GetScheduledDaysPerWeekOrNull(FullTimePartTimeEnum? fullTimePartTime)
        {
            if (!fullTimePartTime.HasValue)
            {
                return null;
            }

            switch (fullTimePartTime.Value)
            {
                case FullTimePartTimeEnum.FullTime:
                    return 5;
                case FullTimePartTimeEnum.PartTime:
                    return 3;
                default:
                    return null;
            }
        }
        #endregion
    }
}
