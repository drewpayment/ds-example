using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IMeetingQuery : IQuery<Meeting, IMeetingQuery>
    {
       IMeetingQuery ByMeetingId(int meetingId);
    }
}
