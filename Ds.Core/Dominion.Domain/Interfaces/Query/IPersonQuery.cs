using Dominion.Domain.Entities.Contact;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPersonQuery : IQuery<Person, IPersonQuery>
    {
        IPersonQuery ByPersonId(int periodId);
        IPersonQuery ByFein(int fein);
    }
}