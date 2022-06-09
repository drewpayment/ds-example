using System.Collections.Generic;

namespace Dominion.Core.Dto.Contact.Search
{
    public class ContactSearchResult
    {
        public int  TotalCount  { get; set; }
        public int? PageCount   { get; set; }
        public int? Page        { get; set; }
        public int? PageSize    { get; set; }
        public int  FilterCount { get; set; }

        public IEnumerable<ContactSearchDto> Results { get; set; }
    }
}