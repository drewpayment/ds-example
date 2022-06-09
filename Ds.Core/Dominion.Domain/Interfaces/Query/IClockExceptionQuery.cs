using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockExceptionQuery : IQuery<ClockExceptionTypeInfo, IClockExceptionQuery>
    {
        IClockExceptionQuery ByClockException(int[] clockExceptionId);
    }
}
