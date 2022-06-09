namespace Dominion.LaborManagement.Dto.JobCosting
{
    public class ClientJobCostingAssignmentSelectedDto
    {
        public int ClientJobCostingAssignmentSelectedId { get; set; }
        public int ClientJobCostingAssignmentId { get; set; }
        public int? ClientJobCostingAssignmentId_Selected { get; set; }
        public int ClientId { get; set; }
        public int? ClientJobCostingId_Selected { get; set; }
        public int? ForeignKeyId_Selected { get; set; }
        public bool IsEnabled { get; set; }
    }
}