namespace Dominion.Core.Dto.Employee.Search
{
    public enum LegacySearchCategoryType : byte
    {
        AllEmployees          = 0,
        CostCenterCode        = 1,
        CostCenterDescription = 2,
        DepartmentCode        = 3,
        DepartmentDescription = 4,
        GroupCode             = 5,
        GroupDescription      = 6,
        Shift                 = 7,
        EmployeeStatus        = 8,
        Supervisor            = 9,
        TimePolicy            = 10
    }
}