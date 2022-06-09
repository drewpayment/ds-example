using System;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class EmailBodyDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string TemplateBody { get; set; }
    }
}