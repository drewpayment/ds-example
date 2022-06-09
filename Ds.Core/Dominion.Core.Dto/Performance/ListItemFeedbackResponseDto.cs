using System.Collections.Generic;
using System.Linq;

namespace Dominion.Core.Dto.Performance
{
    public class ListItemFeedbackResponseDto : SingleFeedbackResponseDto<FeedbackItemDto>
    {
        public IEnumerable<FeedbackItemDto> FeedbackItems { get; set; }
        public override FeedbackResponseData GetAsResponseData()
        {
            var data = base.GetAsResponseData();

            var responseItem = data.ResponseItems.First();

            responseItem.FeedbackItem = Value;
            responseItem.FeedbackItemId = Value?.FeedbackItemId;

            data.FeedbackItems = FeedbackItems;
            return data;
        }
    }
}