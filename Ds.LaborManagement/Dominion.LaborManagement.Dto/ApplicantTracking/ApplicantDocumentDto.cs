using System;
namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantDocumentDto
    {
        public int ApplicantDocumentId { get; set; }
        public string DocumentName { get; set; }
        public int ApplicationHeaderId { get; set; }
        public string LinkLocation { get; set; }
        public DateTime DateAdded { get; set; }
    }
}