using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class PerformanceReviewRating : Entity<PerformanceReviewRating>, IHasModifiedData
    {
        public int PerformanceReviewRatingId { get; set; }
        public int ClientId { get; set; }
        public int Rating { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public virtual Client Client { get; set; }
    }
}
