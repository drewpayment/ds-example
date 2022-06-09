using System.Collections.Generic;
using Dominion.Utility.ExtensionMethods;
using NPOI.SS.Formula.Functions;

namespace Dominion.Core.Dto.App
{
    public interface IMenuNode
    {
        IEnumerable<MenuItemDto> Items { get; set; }
    }

    public static class MenuNodeExtensions
    {
        public static TNode RemoveItem<TNode>(this TNode node, MenuItemDto item) where TNode : IMenuNode
        {
            var copy = node.Items.ToOrNewList();
            copy.Remove(item);
            node.Items = copy;
            return node;
        }
    }
}