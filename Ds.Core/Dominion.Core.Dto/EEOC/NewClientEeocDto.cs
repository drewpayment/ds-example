using System.Collections.Generic;

namespace Dominion.Core.Services.Api
{
    public class ClientPayroll
    {
        public string Code { get;set;}
        public string CompanyName { get;set;}
        public string FEIN { get;set;}
        public string SelectedPayPeriod { get;set;}
        public List<PayPeriod> PayPeriods {get;set; }
        public int ClientId { get; set; }
    }
}
