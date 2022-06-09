namespace Dominion.Core.Dto.Core.Search
{
    public interface IHasPaginationPage
    {
        int? Page     { get; set; }
        int? PageSize { get; set; }
    }
}