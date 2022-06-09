using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominion.Core.Dto.App
{
    public class MenuItemDto : IMenuNode
    {
        public int    MenuItemId { get; set; }
        public string Title      { get; set; }
        public short  Index      { get; set; }
        public bool   IsHidden   { get; set; }
        public bool IsAngularRoute { get; set; }

        public MenuItemDto Parent { get; set; }
        public ApplicationResourceDto   Resource { get; set; }
        public IEnumerable<MenuItemDto> Items    { get; set; }

        public bool HasTitle(string title)
        {
            return string.Equals(Title, title, StringComparison.OrdinalIgnoreCase);
        }

        public MenuItemDto()
        {
            if (Items?.ToList().Any() ?? false)
            {
                foreach(var item in Items)
                {
                    IsAngularRoute = item.Resource.RouteUrl.IndexOf('#') > 0;
                }
            }
        }
    }
}