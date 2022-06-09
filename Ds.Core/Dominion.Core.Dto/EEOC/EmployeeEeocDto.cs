using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.EEOC
{
    public class EmployeeEeocDto
    {
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Gender { get; set; }
        public EEOCRace? Race { get; set; }
        public int? LocationId { get; set; }
        public EEOCJobCategory? JobCategory { get; set; }
        public bool IsMissingEeocInfo { get; set; }
        public EmployeeProfileImageDto ProfileImage { get; set; }
    }
}
