using System.Linq;

namespace Dominion.Core.Dto.Performance
{
    public class TextFeedbackResponseDto : SingleFeedbackResponseDto<string>
    {
        public override FeedbackResponseData GetAsResponseData()
        {
            var data = base.GetAsResponseData();
            data.ResponseItems.First().TextValue = Value;
            return data;
        }
    }
}