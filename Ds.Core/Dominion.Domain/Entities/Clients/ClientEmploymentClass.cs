using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Pay.Dto.Earnings;

namespace Dominion.Domain.Entities.Clients
{
    /// <summary>
    /// Defines an entity that represents the dbo.ClientEmploymentClass table.
    /// </summary>
    public class ClientEmploymentClass: Entity<ClientEmploymentClass>, IModifiableEntity<ClientEmploymentClass>, IEquatable<ClientEmploymentClassDto>
    {
        public int ClientEmploymentClassId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public bool IsEnabled { get; set; }

        public DateTime LastModifiedDate { get; set; }
        public int LastModifiedByUserId { get; set; }
        public virtual string LastModifiedByDescription { get; set; }
        public virtual User.User LastModifiedByUser { get; set; }
        public virtual Client Client { get; set; }

        public void SetLastModifiedValues(int lastModifiedByUserId, string lastModifiedByUserName, DateTime lastModifiedDate)
        {
            LastModifiedByUserId = lastModifiedByUserId;
            LastModifiedByDescription = lastModifiedByUserName;
            LastModifiedDate = lastModifiedDate;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() == typeof(ClientEmploymentClassDto)) return EqualsImpl((ClientEmploymentClassDto) obj);
            if (obj.GetType() != this.GetType()) return false;
            return EqualsImpl((ClientEmploymentClass) obj);
        }
        public bool Equals(ClientEmploymentClassDto other) => Equals((object) other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Code != null ? Code.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsEnabled.GetHashCode();
                return hashCode;
            }
        }

        protected bool EqualsImpl(ClientEmploymentClassDto other)
        {
            return string.Equals(Description, other.Description)
                && string.Equals(Code, other.Code)
                && IsEnabled == other.IsEnabled;
        }

        protected bool EqualsImpl(ClientEmploymentClass other)
        {
            return string.Equals(Description, other.Description)
                && string.Equals(Code, other.Code)
                && IsEnabled == other.IsEnabled;
        }
    }
}
