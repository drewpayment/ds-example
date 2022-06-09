using System;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class PostingFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClientId { get; set; }
        public int? UserId { get; set; }
        public int? StatusId { get; set; }
        public int? PostingId { get; set; }
        public int? PostingCategoryId { get; set; }
        public int? PostingOwnerId { get; set; }
        public int? PostingNumber { get; set; }
    }
}