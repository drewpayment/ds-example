using System;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantNoteDto
    {
        public int ApplicantNoteId { get; set; } // now remarkid
        public int ApplicantId { get; set; }
        public int? PostingId { get; set; }
        public int UserId { get; set; } 
        public string PostedBy { get; set; }
        public string Note { get; set; }
        public DateTime DateSubmitted { get; set; }
    }
}