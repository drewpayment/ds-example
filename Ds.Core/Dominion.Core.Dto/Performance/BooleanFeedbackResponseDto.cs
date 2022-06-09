using System.Linq;

namespace Dominion.Core.Dto.Performance
{
    public class BooleanFeedbackResponseDto : SingleFeedbackResponseDto<bool?>
    {
        public override FeedbackResponseData GetAsResponseData()
        {
            var data = base.GetAsResponseData();
            data.ResponseItems.First().BooleanValue = Value;
            return data;
        }
    }
}