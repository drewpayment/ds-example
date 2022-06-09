using Dominion.Utility.Dto;

namespace Dominion.Core.Dto.Tax
{
    /// <summary>
    /// Data Transfer Object representation of the Filing Status entity.
    /// </summary>
    public class FilingStatusDto : DtoObject
    {
        public int    FilingStatusId { get; set; }
        public string Description    { get; set; }
        public int?    DisplayOrder   { get; set; }
    }
}