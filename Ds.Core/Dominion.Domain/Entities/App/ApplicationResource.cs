using System;
using Dominion.Core.Dto.App;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.App
{
    public class ApplicationResource : Entity<ApplicationResource>, IHasModifiedData
    {
        public int                     ResourceId            { get; set; }
        public string                  Name                  { get; set; }
        public string                  RouteUrl              { get; set; }
        public ApplicationSourceType   ApplicationSourceType { get; set; }
        public ApplicationResourceType ResourceType          { get; set; }
        public DateTime                Modified              { get; set; }
        public int                     ModifiedBy            { get; set; }
    }
}
