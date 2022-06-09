using System.Collections.Generic;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Performance
{
    public class FeedbackBasicDto
    {
        public int       FeedbackId          { get; set; }
        public string    Body                { get; set; }
        public FieldType FieldType           { get; set; }
        public bool      IsRequired          { get; set; }
        public short?    OrderIndex          { get; set; }
        public bool      IsVisibleToEmployee { get; set; }
        public bool      IsArchived          { get; set; }

        public IEnumerable<FeedbackItemDto> FeedbackItems { get; set; }
        public IEnumerable<int> ReviewProfileEvaluations { get; set; }
        public IEnumerable<int> FeedbackResponses { get; set; }
    }
}