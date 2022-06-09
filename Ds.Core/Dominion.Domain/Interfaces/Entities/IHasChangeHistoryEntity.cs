using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Interfaces.Entities
{
    /// <summary>
    /// Represents an entity which has a corresponding change history entity of type <see cref="TChangeHistory"/>.
    /// </summary>
    /// <typeparam name="TChangeHistory"></typeparam>
    public interface IHasChangeHistoryEntity<TChangeHistory> where TChangeHistory : Entity<TChangeHistory>, IHasChangeHistoryData, new()
    {
    }

    public interface IHasChangeHistoryEntityWithEnum<TChangeHistory> where TChangeHistory : Entity<TChangeHistory>, IHasChangeHistoryDataWithEnum, new()
    {
    }


}
