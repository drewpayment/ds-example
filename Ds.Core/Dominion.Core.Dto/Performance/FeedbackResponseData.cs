using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Dto.Contact.Search;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Performance
{
    public class FeedbackResponseData : IHasFeedbackResponseData
    {
        public int       ResponseId          { get; set; }
        public int       FeedbackId          { get; set; }
        public FieldType FieldType           { get; set; }
        public bool      IsRequired          { get; set; }
        public string    FeedbackBody        { get; set; }
        public string    JsonData            { get; set; }
        public short?    OrderIndex          { get; set; }
        public bool      IsVisibleToEmployee { get; set; }
        public ApprovalProcessStatus? ApprovalProcessStatusId { get; set; }
        public bool IsEditedByApprover { get; set; }
        public EvaluationRoleType EvaluationRoleType { get; set; }		

        public ContactSearchDto ResponseByContact { get; set; }
        public IEnumerable<FeedbackResponseItemData> ResponseItems { get; set; }
        public IEnumerable<FeedbackItemDto> FeedbackItems { get; set; }
        public IEnumerable<RemarkDto> ActivityFeed { get; set; }

        public FeedbackResponseData GetAsResponseData()
        {
            return this;
        }

        public IHasFeedbackResponseData GetAsTypedResponseDto()
        {
            IHasFeedbackResponseData dto;

            var firstItem = ResponseItems?.FirstOrDefault();
            var firstItemId = firstItem?.ResponseItemId ?? 0;
            switch (FieldType)
            {
                case FieldType.Boolean:
                    dto = new BooleanFeedbackResponseDto { ResponseItemId = firstItemId, Value = firstItem?.BooleanValue };
                    break;
                case FieldType.List:
                    dto = new ListItemFeedbackResponseDto
                    {
                        ResponseItemId = firstItemId,
                        FeedbackItems  = FeedbackItems,
                        Value = firstItem != null && firstItem.FeedbackItemId.HasValue ? new FeedbackItemDto
                        {
                            FeedbackId     = FeedbackId,
                            FeedbackItemId = firstItem.FeedbackItemId ?? 0,
                            ItemText       = firstItem.FeedbackItem?.ItemText
                        } : null
                    };
                    break;
                case FieldType.MultipleSelection:
                    dto = new MultiSelectFeedbackResponseDto
                    {
                        ResponseItemId = firstItemId,
                        FeedbackItems = FeedbackItems,
                        Value = firstItem?.TextValue,
                    };
                    if (!string.IsNullOrEmpty(firstItem?.TextValue))
                    {
                        MultiSelectFeedbackResponseDto k = (MultiSelectFeedbackResponseDto)dto;
                        string[] itemIds = firstItem.TextValue.Split(',');
                        foreach (FeedbackItemDto m in k.FeedbackItems)
                            if (itemIds.Contains(m.FeedbackItemId.ToString()))
                                m.Checked = true;
                    }
                    break;
                case FieldType.Date:
                    dto = new DateFeedbackResponseDto { ResponseItemId = firstItemId, Value = firstItem?.DateValue };
                    break;
                case FieldType.Text:
                    dto = new TextFeedbackResponseDto { ResponseItemId = firstItemId, Value = firstItem?.TextValue };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            dto.ResponseId              = ResponseId;
            dto.FeedbackId              = FeedbackId;
            dto.FieldType               = FieldType;
            dto.FeedbackBody            = FeedbackBody;
            dto.ResponseByContact       = ResponseByContact;
            dto.IsRequired              = IsRequired;
            dto.OrderIndex              = OrderIndex;
            dto.IsVisibleToEmployee     = IsVisibleToEmployee;
            dto.ActivityFeed            = ActivityFeed;
            dto.ApprovalProcessStatusId = ApprovalProcessStatusId;
            dto.IsEditedByApprover      = IsEditedByApprover;
            

            return dto;
        }

        public void SetResponseId(int responseId)
        {
            ResponseId = responseId;
            if (ResponseItems == null) return;
            foreach(var item in ResponseItems)
                item.ResponseId = responseId;
        }

        public void SetResponseItemId(int responseItemId, FeedbackResponseItemData item)
        {
            item.ResponseItemId = responseItemId;
        }

        public bool HasResponseValue()
        {
            return ResponseItems.Any(i => !string.IsNullOrWhiteSpace(i.TextValue) || i.BooleanValue.HasValue || i.DateValue.HasValue || i.FeedbackItemId != null);
        }
    }
}