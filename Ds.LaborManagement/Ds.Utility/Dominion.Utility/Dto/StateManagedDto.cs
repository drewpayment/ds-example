using System;

namespace Dominion.Utility.Dto
{
    [Serializable]
    public abstract class StateManagedDto : IStateManagedDto
    {
        public DtoState DtoState { get; set; }
    }
}