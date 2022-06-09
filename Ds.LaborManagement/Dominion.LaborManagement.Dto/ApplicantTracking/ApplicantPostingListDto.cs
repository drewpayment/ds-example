using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;


namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class ApplicantPostingListDto
    {
        public int ApplicationId { get; set; }
        public int PostingId { get; set; }
        public string Description { get; set; }
        public PostingType PostingTypeId { get; set; }
        public int PostingNumber { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int PostingCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int JobTypeId { get; set; }
        public string Location { get; set; }
        public bool IsEnabled { get; set; }
        public int ClientId { get; set; }
       
        public DateTime? PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
      
        public bool IsPublished { get; set; }
        public bool IsClosed { get; set; }
        public List<ApplicantPostingOwnerDto> PostingOwners { get; set; }
        //public int? PostingOwnerId { get; set; }
        //public string PostingOwnerName { get; set; }
        public IEnumerable<ApplicantApplicationHeaderDto> Applications { get; set; }
        public int NumOfPositions { get; set; }

    }
}
