using System;

namespace Dominion.LaborManagement.Dto.Notification
{
    public class ApplicantTrackingCorrespondenceReplacementInfoDto
    {
        public int      ApplicantClientId { get; set; }
        public string   ApplicantFirstName{ get; set; }
        public string   ApplicantLastName { get; set; }
        public string   ApplicantEmail    { get; set; }
        public string   UserName          { get; set; }
        public string   Password          { get; set; }
        public string   Posting           { get; set; }
        public DateTime Date              { get; set; }
        public string   Address           { get; set; }
        public string   Phone             { get; set; }
        public string   CompanyAddress    { get; set; }
        public string   CompanyName       { get; set; }
        public string   CompanyLogo       { get; set; }
        public string   TemplateBody      { get; set; }
    }
}
