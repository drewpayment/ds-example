using System;

namespace Dominion.Core.Dto.Payroll
{
    /// <summary>
    /// Pay frequency DTO containing indication if it is actively being used for payroll for the client.
    /// </summary>
    [Serializable]
    public class ClientPayFrequencyDto : PayFrequencyDto
    {
        public int  ClientId                 { get; set; }
        public bool IsActivePayrollFrequency { get; set; }
    }
}