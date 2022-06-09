namespace Dominion.Utility.Pdf
{
    public interface IPdf
    {
        void AddToBuilder(IPdfBuilder builder, params int[] pages);
    }
}
