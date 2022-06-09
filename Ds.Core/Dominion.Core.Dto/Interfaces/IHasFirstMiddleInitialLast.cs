using Dominion.Core.Dto.Utility.ValueObjects;

namespace Dominion.Core.Dto.Interfaces
{
    public interface IHasFirstMiddleInitialLast
    {
// <TEntity> : IEntity<TEntity> where TEntity : class
        string FirstName { get; set; }
        string MiddleInitial { get; set; }
        string LastName { get; set; }
    }

    public static class IHasFirstMiddleInitialLastExtensionMethods
    {
        public static string LastFirst(this IHasFirstMiddleInitialLast nameData)
        {
            var obj = new PersonName(nameData);
            return obj.LastFirst;
        }

        public static string FirstLast(this IHasFirstMiddleInitialLast nameData)
        {
            var obj = new PersonName(nameData);
            return obj.FirstLast;
        }

        public static string FirstMidLast(this IHasFirstMiddleInitialLast nameData)
        {
            var obj = new PersonName(nameData);
            return obj.FirstMidLast;
        }
    }
}