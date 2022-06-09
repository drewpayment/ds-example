using Dominion.Core.Dto.Contact.Search;
using Dominion.Core.Dto.Core;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public class SingleFeedbackResponseDto<TValue> : ISingleFeedbackResponse
    {
        public int       ResponseId          { get; set; }
        public int       ResponseItemId      { get ;set; }
        public int       FeedbackId          { get; set; }
        public string    FeedbackBody        { get; set; }
        public FieldType FieldType           { get; set; }
        public bool      IsRequired          { get; set; }
        public TValue    Value               { get; set; }
        public short?    OrderIndex          { get; set; }
        public bool      IsVisibleToEmployee { get; set; }
        public ApprovalProcessStatus? ApprovalProcessStatusId { get; set; }
        public IEnumerable<RemarkDto> ActivityFeed { get; set; }

        public ContactSearchDto ResponseByContact { get; set; }
        public bool IsEditedByApprover { get; set; }

        public virtual FeedbackResponseData GetAsResponseData()
        {
            return new FeedbackResponseData
            {
                FeedbackId              = FeedbackId,
                FeedbackBody            = FeedbackBody,
                FieldType               = FieldType,
                ResponseByContact       = ResponseByContact,
                ResponseId              = ResponseId,
                IsRequired              = IsRequired,
                OrderIndex              = OrderIndex,
                IsVisibleToEmployee     = IsVisibleToEmployee,
                ActivityFeed            = ActivityFeed,
                ApprovalProcessStatusId = ApprovalProcessStatusId,
                IsEditedByApprover      = IsEditedByApprover,
                ResponseItems           = new []
                {
                    new FeedbackResponseItemData { ResponseItemId = ResponseItemId, ResponseId = ResponseId }
                }
            };
        }

        public IHasFeedbackResponseData GetAsTypedResponseDto()
        {
            return this;
        }

        public void SetResponseId(int responseId)
        {
            ResponseId = responseId;
        }

        public void SetResponseItemId(int responseItemId, FeedbackResponseItemData item)
        {
            ResponseItemId = responseItemId;
        }

        public virtual bool HasResponseValue()
        {
            return Value != null;
        }
    }
}