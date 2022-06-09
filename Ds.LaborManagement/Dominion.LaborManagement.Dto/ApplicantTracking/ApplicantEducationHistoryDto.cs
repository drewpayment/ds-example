using System;
using Dominion.Core.Dto.Labor.Enum;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantEducationHistoryDto
    {
        public int ApplicantEducationId { get; set; }
        public int ApplicantId { get; set; }
        public string Description { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public HasDegreeType HasDegree { get; set; }
        public int? DegreeId { get; set; }
        public string Degree { get; set; }
        public string Studied { get; set; }
        public bool IsEnabled { get; set; }
        public int? YearsCompleted { get; set; }
        public int? ApplicantSchoolTypeId { get; set; }
        public string ApplicantSchoolType { get; set; }
        public int? ApplicationOrder { get; set; }
    }
}