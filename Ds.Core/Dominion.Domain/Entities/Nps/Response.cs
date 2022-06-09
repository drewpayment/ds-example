using Dominion.Domain.Entities.Base;
using System;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.User;

namespace Dominion.Domain.Entities.Nps
{
    public class Response : Entity<Response>
    {
        public int      ResponseId   { get; set; }
        public int      QuestionId   { get; set; }
        public int      UserId       { get; set; }
        public UserType UserTypeId   { get; set; }
        public int      ClientId     { get; set; }
        public DateTime ResponseDate { get; set; }
        public int      Score        { get; set; }
        public string   Feedback     { get; set; }
        public bool? IsResolved       { get; set; }
        public int? ResolvedByUserId  { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public virtual Question Question { get; set; }
        public virtual User.User User { get; set; }
        public virtual UserTypeInfo UserType { get; set; }
        public virtual Client Client { get; set; }
        public virtual User.User ResolvedByUser { get; set; }
    }
}
