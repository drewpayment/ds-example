using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{

    public interface IPreviewEmployeeBenefitImportQuery : IQuery<PreviewEmployeeBenefitImport, IPreviewEmployeeBenefitImportQuery>
    {

        IPreviewEmployeeBenefitImportQuery ByClientId(int clientId);

    }
}
