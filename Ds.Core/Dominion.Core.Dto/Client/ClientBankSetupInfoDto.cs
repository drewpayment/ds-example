using System;

namespace Dominion.Core.Dto.Client
{
    public class ClientBankSetupInfoDto
    {
        public int     ClientId               { get; set; }
        public int?    AchBankId              { get; set; }
        public string  BankAccount            { get; set; }
        public int?    RoutingId              { get; set; }
        public string  AltBankAccount         { get; set; }
        public int?    AltRoutingId           { get; set; }
        public string  TaxAccount             { get; set; }
        public int?    TaxRoutingId           { get; set; }
        public string  DebitAccount           { get; set; }
        public int?    DebitRoutingId         { get; set; }
        public string  NachaPrefix            { get; set; }
        public string  TaxAccountNachaPrefix  { get; set; }
        public bool    IsSepChkNumbers        { get; set; }
        public decimal AchLimit               { get; set; }
    }

    public class ClientCalendarBankSetupDto
    { 
        public int TaxManagementAchBankId { get; set; }
    }

    public class ClientBankSetupInfoWithCalendarSetupDto : ClientBankSetupInfoDto
    {
        public ClientCalendarBankSetupDto CalendarSetup { get; set; }
    }
}