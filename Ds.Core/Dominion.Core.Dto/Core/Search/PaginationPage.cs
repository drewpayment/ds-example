namespace Dominion.Core.Dto.Core.Search
{
    public class PaginationPage : IHasPaginationPage
    {
        public int? Page     { get; set; }
        public int? PageSize { get; set; }
    }
}