using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientInstructionOneTime : Entity<ClientInstructionOneTime>
    {
        public int ClientId { get; set; }
        public string Instruction { get; set; }
        public int PayrollId { get; set; }
    }
}
