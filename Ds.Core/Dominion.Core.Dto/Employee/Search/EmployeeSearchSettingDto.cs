namespace Dominion.Core.Dto.Employee.Search
{
    public class EmployeeSearchSettingDto
    {
        public int  UserId         { get; set; }
        public int  ClientId       { get; set; }
        public bool IsExcludeTemps { get; set; }
        public bool IsActiveOnly   { get; set; }
        public SearchSortByType SortOrder { get; set; }
    }
}
