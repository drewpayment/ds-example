using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewRatingDto
    {
        public int ReviewRatingId { get; set; }
        public int ClientId { get; set; }
        public int Rating { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        //RELATIONSHIPS

        public virtual ClientDto Client { get; set; }
    }
}
