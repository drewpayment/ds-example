namespace Dominion.Core.Dto.Labor
{
    public class JobProfileBasicDto : IValidatableJobProfile
    {
        public int    JobProfileId { get; set; }
        public int    ClientId     { get; set; }
        public string Description  { get; set; }
        public bool   IsActive     { get; set; }
        public int? DirectSupervisorId { get; set; }
    }
}
