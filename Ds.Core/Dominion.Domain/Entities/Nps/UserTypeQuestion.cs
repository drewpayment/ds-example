using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.User;

namespace Dominion.Domain.Entities.Nps
{
    public class UserTypeQuestion : Entity<UserTypeQuestion>
    {
        public UserType UserTypeId { get; set; }
        public int QuestionId { get; set; }

        public virtual UserTypeInfo UserType { get; set; }
        public virtual Question Question { get; set; }
    }
}
