using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.TimeClock;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Labor
{
    /// <summary>
    /// Properties are <c>byte?</c> representations of <see cref="DayOfWeekType"/>.
    /// </summary>
    public interface IHasClockClientRulesStartingDayOfWeekIds
    {
        byte? WeeklyStartingDayOfWeekId { get; }
        byte? BiWeeklyStartingDayOfWeekId { get; }
        byte? SemiMonthlyStartingDayOfWeekId { get; }
        byte? MonthlyStartingDayOfWeekId { get; }
    }

    public static class IHasClockClientRulesStartingDayOfWeekIdsExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self">
        /// Object which has had <see cref="IHasClockClientRulesStartingDayOfWeekIds"/> properties set. 
        /// Generally sourced from a <see cref="Dominion.Domain.Entities.TimeClock.ClockClientRules"/> entity.
        /// </param>
        /// <param name="employeePayFrequencyType">
        /// <see cref="PayFrequencyType"/> used to determine the appropriate <see cref="DayOfWeekType"/> to use as the StartingDayOfWeek.
        /// </param>
        /// <returns></returns>
        public static DayOfWeekType GetStartingDayOfWeekType(this IHasClockClientRulesStartingDayOfWeekIds self, PayFrequencyType employeePayFrequencyType)
        {
            byte startingDayOfWeekTypeAsByte = self.GetStartingDayOfWeekTypeAsByte(employeePayFrequencyType);
            DayOfWeekType startingDayOfWeekType = DayOfWeekTypeExtensions.FromByteAsDayOfWeekType(startingDayOfWeekTypeAsByte);
            return startingDayOfWeekType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self">
        /// Object which has had <see cref="IHasClockClientRulesStartingDayOfWeekIds"/> properties set. 
        /// Generally sourced from a <see cref="Dominion.Domain.Entities.TimeClock.ClockClientRules"/> entity.
        /// </param>
        /// <param name="employeePayFrequencyType">
        /// <see cref="PayFrequencyType"/> used to determine the appropriate <see cref="DayOfWeekType"/> to use as the StartingDayOfWeek.
        /// </param>
        /// <returns></returns>
        public static DayOfWeekType? GetStartingDayOfWeekTypeNullable(this IHasClockClientRulesStartingDayOfWeekIds self, PayFrequencyType employeePayFrequencyType)
        {
            byte? startingDayOfWeekTypeAsByte = self.GetStartingDayOfWeekTypeAsNullableByte(employeePayFrequencyType);
            DayOfWeekType? startingDayOfWeekType = (DayOfWeekType?)startingDayOfWeekTypeAsByte;
            return startingDayOfWeekType;
        }

        /// <summary>
        /// Wraps <see cref="GetStartingDayOfWeekTypeAsNullableByte"/>, providing additional validation and defaulting logic
        /// in order to narrow the result from <c>byte?</c> to <c>byte</c>.
        /// 
        /// <seealso cref="spGetClockClientRulesByEmployeeID"/>
        /// </summary>
        /// <param name="self">
        /// Object which has had <see cref="IHasClockClientRulesStartingDayOfWeekIds"/> properties set. 
        /// Generally sourced from a <see cref="Dominion.Domain.Entities.TimeClock.ClockClientRules"/> entity.
        /// </param>
        /// <param name="employeePayFrequencyType">
        /// <see cref="PayFrequencyType"/> used to determine the appropriate <see cref="DayOfWeekType"/> to use as the StartingDayOfWeek.
        /// </param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// Returns <see cref="GetStartingDayOfWeekTypeAsNullableByte"/> result if it was not <c>null</c>.
        /// </item>
        /// 
        /// <item>
        /// Else, if <paramref name="employeePayFrequencyType"/> was either:
        ///     <list type="bullet">
        ///         <item>invalid</item>
        ///         <item><see cref="PayFrequencyType.Quarterly"/></item>
        ///         <item><see cref="PayFrequencyType.Annually"/></item>
        ///     </list>
        /// returns <c>12</c> (Originated from <see cref="Dominion.LaborManagement.Service.Mapping.Clock.ClockClientRulesMaps.ToClockClientRulesSummaryDto.GetClockClientRulesSummaryDto"/>)
        /// See commit <c>eff9a53d</c>.
        /// </item>
        /// 
        /// <item>For all other <c>null</c> conditions, returns <see cref="DayOfWeekType.Sunday"/></item>
        /// </list>
        /// </returns>
        public static byte GetStartingDayOfWeekTypeAsByte(this IHasClockClientRulesStartingDayOfWeekIds self, PayFrequencyType employeePayFrequencyType)
        {
            byte startingDayOfWeekTypeAsByte;
            byte? startingDayOfWeekTypeAsNullableByte = self.GetStartingDayOfWeekTypeAsNullableByte(employeePayFrequencyType);
            
            if (startingDayOfWeekTypeAsNullableByte.HasValue)
            {
                startingDayOfWeekTypeAsByte = startingDayOfWeekTypeAsNullableByte.Value;
            }
            else
            {
                switch (employeePayFrequencyType)
                {
                    case PayFrequencyType.Weekly:
                    case PayFrequencyType.BiWeekly:
                    case PayFrequencyType.AlternateBiWeekly:
                    case PayFrequencyType.SemiMonthly:
                    case PayFrequencyType.Monthly:
                        startingDayOfWeekTypeAsByte = (byte)DayOfWeekType.Sunday;
                        break;
                    case PayFrequencyType.Quarterly:
                    case PayFrequencyType.Annually:
                    default:
                        /// TA-541: Not sure what this represents yet (how its used...).
                        /// Originated from <see cref="Dominion.LaborManagement.Service.Mapping.Clock.ClockClientRulesMaps.ToClockClientRulesSummaryDto.GetClockClientRulesSummaryDto"/>
                        startingDayOfWeekTypeAsByte = 12;
                        break;
                }
            }
            return startingDayOfWeekTypeAsByte;
        }

        /// <summary>
        /// Gets the appropriate <see cref="DayOfWeekType"/> cast as a <c>byte?</c> for a given employee <see cref="PayFrequencyType"/>.
        /// </summary>
        /// <param name="self">
        /// Object which has had <see cref="IHasClockClientRulesStartingDayOfWeekIds"/> properties set. 
        /// Generally sourced from a <see cref="Dominion.Domain.Entities.TimeClock.ClockClientRules"/> entity.
        /// </param>
        /// <param name="employeePayFrequencyType">
        /// <see cref="PayFrequencyType"/> used to determine the appropriate <see cref="DayOfWeekType"/> to use as the StartingDayOfWeek.
        /// </param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// One of StartingDayOfWeekIds set on self
        /// </item>
        /// 
        /// <item>
        /// Or <c>null</c> if <paramref name="employeePayFrequencyType"/> was either:
        ///     <list type="bullet">
        ///         <item>invalid</item>
        ///         <item><see cref="PayFrequencyType.Quarterly"/></item>
        ///         <item><see cref="PayFrequencyType.Annually"/></item>
        ///     </list>
        /// </item>
        /// 
        /// <item>
        /// Also returns null if the <paramref name="employeePayFrequencyType"/> maps to a property <see cref="IHasClockClientRulesStartingDayOfWeekIds"/> that was <c>null</c>.
        /// </item>
        /// </list>
        /// </returns>
        public static byte? GetStartingDayOfWeekTypeAsNullableByte(this IHasClockClientRulesStartingDayOfWeekIds self, PayFrequencyType employeePayFrequencyType)
        {
            byte? startingDayOfWeekType;
            switch (employeePayFrequencyType)
            {
                case PayFrequencyType.Weekly:
                    startingDayOfWeekType = self.WeeklyStartingDayOfWeekId;
                    break;
                case PayFrequencyType.BiWeekly:
                case PayFrequencyType.AlternateBiWeekly:
                    startingDayOfWeekType = self.BiWeeklyStartingDayOfWeekId;
                    break;
                case PayFrequencyType.SemiMonthly:
                    startingDayOfWeekType = self.SemiMonthlyStartingDayOfWeekId;
                    break;
                case PayFrequencyType.Monthly:
                    startingDayOfWeekType = self.MonthlyStartingDayOfWeekId;
                    break;
                case PayFrequencyType.Quarterly:
                case PayFrequencyType.Annually:
                default:
                    startingDayOfWeekType = null;
                    break;
            }
            return startingDayOfWeekType;
        }
    }
}
