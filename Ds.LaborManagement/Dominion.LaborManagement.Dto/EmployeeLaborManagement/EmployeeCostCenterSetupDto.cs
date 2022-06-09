using Dominion.Core.Dto.Labor;

namespace Dominion.LaborManagement.Dto.EmployeeLaborManagement
{
    public class EmployeeCostCenterSetupDto
    {
        public int ClientId { get; set; }
        public int CostCenterId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int ClockEmployeeCostCenterId { get; set; }
        public string Code { get; set; }
        public int EmployeeId { get; set; }

        public virtual ClientCostCenterDto CostCenter { get; set; }
    }

    public class EmployeeFullWithCostCenterSetupDto : EmployeeCostCenterSetupDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string EmployeeNumber { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientDivisionId { get; set; }
    }
}
