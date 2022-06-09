using Dominion.Core.Dto.Core.Search;
using Dominion.Core.Dto.Employee.Search;
using Dominion.Core.Dto.User.Search;

namespace Dominion.Core.Dto.Contact.Search
{
    public enum ContactNameSource
    {
        Employee,
        User
    }

    public class ContactSearchOptions : IHasPaginationPage
    {
        public int?   ClientId             { get; set; }
        public bool?  IncludeUsers         { get; set; }
        public bool?  IncludeEmployees     { get; set; }
        public bool   IncludeProfileImages { get ;set; }
        public int?   PageSize             { get; set; }
        public int?   Page                 { get; set; }
        public string SearchText           { get; set; }

        /// <summary>
        /// Determines what First/Last name to use if both user and employee
        /// records exist.  Default is to use Employee info if available.
        /// </summary>
        public ContactNameSource? PreferredContactNameSource { get; set; }

        public UserSearchOptions UserSearchOptions { get; set; }
        public EmployeeSearchQueryOptions EmployeeSearchOptions { get; set; }
    }
}