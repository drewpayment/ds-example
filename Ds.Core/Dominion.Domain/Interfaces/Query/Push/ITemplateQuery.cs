using Dominion.Domain.Entities.Push;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Push
{
    public interface ITemplateQuery : IQuery<PushTemplate, ITemplateQuery>
    {
        /// <summary>
        /// Filters PushUserInfo entities by device serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        ITemplateQuery BySerialNumber(string serialNumber);

        /// <summary>
        /// Filters PushUserInfo entities by device serial numbers
        /// </summary>
        /// <param name="serialNumbers"></param>
        /// <returns></returns>
        ITemplateQuery BySerialNumbers(string[] serialNumbers);
    }
}
