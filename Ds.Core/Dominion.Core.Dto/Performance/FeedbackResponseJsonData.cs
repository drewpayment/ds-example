using System.Collections.Generic;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Performance
{
    public class FeedbackResponseJsonData
    {
        public class FeedbackInfoData
        {
            public FieldType FieldType { get; set; }
            public string Body { get; set; }
            public bool IsRequired { get; set; }
            public IEnumerable<FeedbackItemInfo> Items { get; set; }
        }
        public class FeedbackItemInfo
        {
            public int FeedbackItemId { get; set; }
            public string ItemText { get; set; }
        }

        public FeedbackInfoData FeedbackInfo { get; set; }
    }
}