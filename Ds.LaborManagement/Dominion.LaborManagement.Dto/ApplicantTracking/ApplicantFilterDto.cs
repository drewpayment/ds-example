using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantFilterDto
    {
        public int? PostingId { get; set; }
        public int? JobTypeId { get; set; }
        public int? PostingCategoryId { get; set; }
        public int? PostingOwnerId { get; set; }
        public int? JobProfileId { get; set; }
        public int? DivisionId { get; set; }
        public int? DepartmentId { get; set; }
        public string Keyword { get; set; }
        public string NameSearch { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool ViewDenied { get; set; }
        //public bool ViewRecommended { get; set; }
        public int? PostingNumber { get; set; }
        public int? ClientId { get; set; }
        public int? UserId { get; set; }
        //public int? OrderBy { get; set; }
        //public bool Ascending { get; set; }
        //public bool IsClosed { get; set; }
        public int? StatusId { get; set; }
        public int? ApplicantRejectionReasonId { get; set; }
        public List<bool> RatingSelection { get; set; }
    }
}