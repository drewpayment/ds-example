using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface ICompanyResourceFolderQuery : IQuery<CompanyResourceFolder, ICompanyResourceFolderQuery>
    {
        ICompanyResourceFolderQuery ByClientId(int clientId);
        ICompanyResourceFolderQuery ByFolderId(int? folderId);
        ICompanyResourceFolderQuery OrderByFolderDescription(SortDirection direction);
        ICompanyResourceFolderQuery ByDescription(string description);
    }
}
