using Dominion.Utility.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dominion.Core.Dto.Payroll
{
    /// <summary>
    /// Sourced from dbo.EmployeeDeductionMaxType.
    /// 
    /// The extension methods for <see cref="IEmployeeDeductionMaxTypeBasicDto"/> and <see cref="EmployeeDeductionMaxTypeBasicDto"/>
    /// are workarounds to accomodate existing tables that were implemented to reference IDs from this table as byte/tinyint (instead of int).
    /// This was only possible at the time, because FK constraints were not implemented...
    /// </summary>
    public enum EmployeeDeductionMaxType : byte
    {
        [Description("Limit")]
        Limit            = 1,
        [Description("Balance")]
        Balance          = 2,
        [Description("Delinquent (All)")]
        Delinquent_All   = 3,
        [Description("Delinquent (2)")]
        Delinquent_2     = 4,
        [Description("401 K Limit")]
        Four01KLimit     = 5,
        [Description("Catch Up (50+)")]
        CatchUp50Plus    = 6,
        [Description("401 K Base Limit")]
        Four01KBaseLimit = 7,
        [Description("SIMPLE IRA")]
        SimpleIra        = 8,
        [Description("Excess Insurance")]
        ExcessInsurance  = 9,
        [Description("H.S.A Single")]
        HsaSingle        = 10,
        [Description("H.S.A Family")]
        HsaFamily        = 11
    }



    public interface IEmployeeDeductionMaxTypeBasicDto
    {
        int EmployeeDeductionMaxTypeId { get; }

        EmployeeDeductionMaxType? EmployeeDeductionMaxType { get; }
    }

    [Serializable]
    public class EmployeeDeductionMaxTypeBasicDto : IEmployeeDeductionMaxTypeBasicDto
    {
        public EmployeeDeductionMaxTypeBasicDto(int employeeDeductionMaxTypeId)
        {
            EmployeeDeductionMaxTypeId = employeeDeductionMaxTypeId;
        }

        public EmployeeDeductionMaxTypeBasicDto(EmployeeDeductionMaxType employeeDeductionMaxType)
        {
            EmployeeDeductionMaxTypeId = (int)employeeDeductionMaxType;
        }

        public int EmployeeDeductionMaxTypeId { get; set; }

        public EmployeeDeductionMaxType? EmployeeDeductionMaxType => this.GetEmployeeDeductionMaxType();
    }

    public static class EmployeeDeductionMaxTypeBasicDtoExtensions
    {
        public static EmployeeDeductionMaxType? GetEmployeeDeductionMaxType(this IEmployeeDeductionMaxTypeBasicDto dto)
        {
            EmployeeDeductionMaxType result;
            bool isParsable = Enum.TryParse(dto.EmployeeDeductionMaxTypeId.ToString(), out result);
            if (isParsable)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a HashSet of all elements of <paramref name="dtos"/> 
        /// where the element has a non-null value for
        /// <see cref="IEmployeeDeductionMaxTypeBasicDto.EmployeeDeductionMaxType"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// Source <paramref name="dtos"/> is null.
        /// </exception>
        public static HashSet<EmployeeDeductionMaxType> ToHashSetForNonNullElements(this IEnumerable<IEmployeeDeductionMaxTypeBasicDto> dtos)
        {
            return dtos
                .Where((def) => def.EmployeeDeductionMaxType.HasValue)
                .Select((def) => def.EmployeeDeductionMaxType.Value)
                .Distinct()
                .ToHashSet();
        }


    }
}
