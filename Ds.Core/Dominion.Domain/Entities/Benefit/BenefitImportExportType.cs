using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Benefit
{
    public class BenefitImportExportType : Entity<BenefitImportExportType>
    {
        public int BenefitImportExportTypeId { get; set; }
        public int BenefitImportExportFormatId { get; set; }
        public int ClientId { get; set; }
        public int ClientRelationId { get; set; }

        public virtual BenefitImportExportFormat Format { get; set; }
        public virtual Client Client { get; set; }
        public virtual ClientOrganization ClientRelation { get; set; }
    }
}