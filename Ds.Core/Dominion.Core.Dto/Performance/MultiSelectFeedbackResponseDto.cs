using System.Linq;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public class MultiSelectFeedbackResponseDto : SingleFeedbackResponseDto<string>
    {
        public IEnumerable<FeedbackItemDto> FeedbackItems { get; set; }
        public override FeedbackResponseData GetAsResponseData()
        {
            var data = base.GetAsResponseData();
            data.ResponseItems.First().TextValue = Value;

            string[] itemIds = string.IsNullOrEmpty(Value) ? new string[] { } : Value.Split(',');
            foreach (FeedbackItemDto m in FeedbackItems)
                if (itemIds.Contains(m.FeedbackItemId.ToString()))
                    m.Checked = true;

            data.FeedbackItems = FeedbackItems;
            return data;
        }
    }
}