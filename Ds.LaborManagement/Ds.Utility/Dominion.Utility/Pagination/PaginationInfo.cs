using System;

namespace Dominion.Utility.Pagination
{
    /// <summary>
    /// Containter class used to perform pagination on a collection set
    /// </summary>
    public class PaginationInfo
    {
        public const int NO_PAGE_SELECTED = -1;

        public int ItemCountPerPage { get; set; }
        public int ActivePageNumber { get; set; }
        public int TotalItemCount { get; set; }

        public int TotalPages
        {
            get { return (int) Math.Ceiling((decimal) TotalItemCount/ItemCountPerPage); }
        }

        public int SkipCount
        {
            get { return (ActivePageNumber - 1)*ItemCountPerPage; }
        }

        public bool IsValid
        {
            get
            {
                if (ItemCountPerPage > 0 && ActivePageNumber <= TotalPages)
                    return true;
                return false;
            }
        }
    }
}