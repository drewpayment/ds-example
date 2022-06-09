using System;
using System.Linq;

namespace Dominion.Core.Dto.Performance
{
    public class DateFeedbackResponseDto : SingleFeedbackResponseDto<DateTime?>
    {
        public override FeedbackResponseData GetAsResponseData()
        {
            var data = base.GetAsResponseData();
            data.ResponseItems.First().DateValue = Value;
            return data;
        }
    }
}