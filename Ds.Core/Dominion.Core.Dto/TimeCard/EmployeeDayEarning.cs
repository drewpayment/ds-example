using Dominion.Core.Dto.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeDayEarning : EmployeeDayRecord
    {
        public double Hours { get; set; }
        public int ClientEarningCategoryID { get; set; }
        public bool IsBenefit { get; set; }
        public bool IsWorkedHours { get; set; }
        public string Description { get; set; }
        public string CostCenter
        {
            get
            {
                if ((CostCenters == null))
                    return null;
                return string.Join(", ", CostCenters);
            }
        }
        public string ShiftDescription { get; set; }
        public int EarningID { get; set; }
        /// <summary>
        ///         ''' Whether the department, cost center, and shift description info should be shown in the "Exceptions" output column.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public bool ShowCostCenterInfo { get; set; }
        public string Department { get; set; }
        public string[] CostCenters { get; set; } = new string[] { };
        public string Division { get; set; }
        public EmployeeDayApprovalStatus ApprovalStatus { get; set; } = new EmployeeDayApprovalStatus();
        public EmployeeDayJobCostingInfo JobCostingInfo { get; set; } = new EmployeeDayJobCostingInfo();
        public virtual double? EmployeeRate { get; set; }

        public int? ClockEmployeeBenefitID
        {
            get
            {
                if (IsBenefit)
                    return EarningID;
                else
                    return default(int?);
            }
        }

        /// <summary>
        ///         ''' Whether these hours were "keyed" in. Helps in determining the difference between a input hours punch and a benefit for a "Worked Hours" earning type.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public bool IsInputHours { get; set; }

        /// <summary>
        ///         ''' Calculates whether the earning represents hours that were entered in by a person.
        ///         ''' </summary>
        ///         ''' <param name="isBenefit">Whether the earning is a benefit.</param>
        ///         ''' <param name="clientEarningCategoryID">The earning category ID that the hours are registered as.</param>
        ///         ''' <param name="isOnHoliday">Whether the earning took place on a holiday.</param>
        ///         ''' <returns></returns>
        public static bool CalculateIsKeyedHours(bool isBenefit, int clientEarningCategoryID, bool isOnHoliday, PunchOptionType? punchOption, bool isWorkedHours)
        {
            var isInputHours = punchOption.GetValueOrDefault(PunchOptionType.NoValueSelected) == PunchOptionType.InputHours;
            return (isBenefit && isClientEarningCategory(clientEarningCategoryID)) || (isBenefit && isInputHours && isOnHoliday && isWorkedHours);
        }

        /// <summary>
        /// Expression ported from vb, not exactly sure the significance of it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static bool isClientEarningCategory(int id)
        {
            return id == (int)ClientEarningCategory.Regular || id == (int)ClientEarningCategory.Overtime || id == (int)ClientEarningCategory.Double || id == (int)ClientEarningCategory.HolidayWorked;
        }
    }
}
