using Dominion.Core.Dto.InstantPay;

namespace Dominion.Core.Dto.Employee
{
    public class InsertEmployeeBankDto
    {
        public int AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public bool isPreNote { get; set; }
        public int EmployeeID { get; set; }
        public int ClientID { get; set; }
        public int ModifiedBy { get; set; }
        public InstantPayProvider? InstantPayProvider { get; set; }
    }
}