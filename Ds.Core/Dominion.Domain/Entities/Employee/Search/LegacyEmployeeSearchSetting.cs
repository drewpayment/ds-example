using Dominion.Core.Dto.Employee.Search;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Employee.Search
{
    public partial class LegacyEmployeeSearchSetting : Entity<LegacyEmployeeSearchSetting>
    {
        public virtual int                              EmployeeHeaderSettingsId             { get; set; } 
        public virtual LegacySearchEmployeeActiveType?  SearchOption                         { get; set; } 
        public virtual LegacySearchEmployeeActiveType?  PaySourceFilterOption                { get; set; } 
        public virtual LegacySearchEmployeeActiveType?  LaborSourceFilterOption              { get; set; } 
        public virtual int                              UserId                               { get; set; } 
        public virtual bool                             IsExcludeTemps                       { get; set; } 
        public virtual bool                             IsHideInactive                       { get; set; } 
        public virtual LegacySearchCategoryType?        PaySourceFilterCategoryOption        { get; set; } 
        public virtual LegacySearchCategoryType?        LaborSourceFilterCategoryOption      { get; set; } 
        public virtual string                           PaySourceFilterCategorySearchText    { get; set; } 
        public virtual string                           LaborSourceFilterCategorySearchText  { get; set; } 
        public virtual SearchSortByType           PaySourceOrderBy                     { get; set; } 
        public virtual SearchSortByType           LaborSourceOrderBy                   { get; set; } 
    }
}
