using Dominion.Core.Dto.Contact.Search;
using Dominion.Core.Dto.Core;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public interface IHasFeedbackResponseData
    {
        int              FeedbackId          { get; set; }
        int              ResponseId          { get; set; }
        string           FeedbackBody        { get; set; }
        ContactSearchDto ResponseByContact   { get; set; }
        FieldType        FieldType           { get; set; }
        bool             IsRequired          { get; set; }
        short?           OrderIndex          { get; set; }
        bool             IsVisibleToEmployee { get; set; }
        ApprovalProcessStatus? ApprovalProcessStatusId { get; set; }
        bool IsEditedByApprover { get; set; }
        IEnumerable<RemarkDto> ActivityFeed { get; set; }

        FeedbackResponseData GetAsResponseData();
        IHasFeedbackResponseData GetAsTypedResponseDto();
        void SetResponseId(int responseId);
        void SetResponseItemId(int responseItemId, FeedbackResponseItemData item);
        bool HasResponseValue();
    }
}