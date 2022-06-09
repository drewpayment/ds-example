using System.Collections.Generic;

namespace Dominion.Core.Dto.App
{
    public class NavItemDto
    {
        public int                     MenuItemId { get; set; }
        public string                  Name       { get; set; }
        public string                  Icon       { get; set; }
        public int                     Index      { get; set; }
        public string                  Url        { get; set; }
        public int?                    ParentId   { get; set; }
        public IEnumerable<NavItemDto> Items      { get; set; }
        public NavItemDto              Parent     { get; set; }
    }
}
