using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicationSectionInstructionDto
    {
        public int SectionInstructionId { get; set; }
        public int SectionId { get; set; }
        public int ClientId { get; set; }
        public string Instruction { get; set; }
    }
}