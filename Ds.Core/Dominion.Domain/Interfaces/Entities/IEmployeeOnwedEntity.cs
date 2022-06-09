using System;
using System.Linq.Expressions;

namespace Dominion.Domain.Interfaces.Entities
{
    public interface IEmployeeOwnedEntity<TEntity>
        where TEntity : class
    {
        Expression<Func<TEntity, TEntity>> GetEmployeeIdView();
        int EmployeeId { get; set; }
    }
}