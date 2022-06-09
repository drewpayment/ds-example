using System.Collections.Generic;

namespace Dominion.Core.Dto.App
{
    public class MenuBasicDto : IMenuNode
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public IEnumerable<MenuItemDto> Items { get; set; }
    }
}