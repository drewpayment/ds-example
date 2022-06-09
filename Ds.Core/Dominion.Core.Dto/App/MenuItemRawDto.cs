namespace Dominion.Core.Dto.App
{
    public class MenuItemRawDto
    {
        public int    MenuId     { get; set; }
        public int    MenuItemId { get; set; }
        public string Title      { get; set; }
        public short  Index      { get; set; }
        public int?   ResourceId { get; set; }
        public int?   ParentId   { get; set; }
    }

    public class MenuItemRawDto_WithResourceData : MenuItemRawDto
    {
        public ApplicationResourceDto Resource { get; set; }
    }
}