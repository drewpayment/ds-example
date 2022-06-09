namespace Dominion.LaborManagement.Dto.JobCosting
{
    public class ClientJobCostingAssignmentDto
    {
        public int ClientJobCostingAssignmentId { get; set; }
        public int ClientJobCostingId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int? ForeignKeyId { get; set; }
        public bool IsEnabled { get; set; }
    }
}