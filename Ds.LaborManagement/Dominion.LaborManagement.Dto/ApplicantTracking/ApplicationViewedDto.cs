using System;
namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicationViewedDto
    {
        public int ApplicationViewedId { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public int ApplicationHeaderId { get; set; }
        public DateTime ViewedOn { get; set; }
    }
}