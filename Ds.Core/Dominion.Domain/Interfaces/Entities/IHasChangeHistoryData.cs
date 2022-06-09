using System;
using Dominion.Core.Dto.Common;

namespace Dominion.Domain.Interfaces.Entities
{
    /// <summary>
    /// Entity containing change history data.
    /// </summary>
    public interface IHasChangeHistoryData 
    {
        int    ChangeId   { get; set; }
        string ChangeMode { get; set; }
    }

    public interface IHasChangeHistoryDataWithEnum
    {
        int ChangeId { get; set; }
        ChangeModeType ChangeMode { get; set; }
        DateTime ChangeDate { get; set; }
    }
}
