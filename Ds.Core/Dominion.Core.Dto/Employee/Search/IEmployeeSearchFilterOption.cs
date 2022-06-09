using System;
using System.Linq;

namespace Dominion.Core.Dto.Employee.Search
{
    public interface IEmployeeSearchFilterOption
    {
        EmployeeSearchFilterType FilterType { get; }
        int                     Id        { get; }
        string                  Name      { get; }

        IEmployeeSearchFilterOption ParentOption { get; }
        bool Evaluate(EmployeeSearchDto dto);
    }

    public static class FilterFns
    {
        public static Func<EmployeeSearchDto, IEmployeeSearchFilterOption, bool> EvaluateById = 
            (dto, option) => dto.Groups.Any(g => g.FilterType == option.FilterType && g.Id == option.Id);
    }
}