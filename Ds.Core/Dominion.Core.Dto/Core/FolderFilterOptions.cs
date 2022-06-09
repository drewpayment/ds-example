namespace Dominion.Core.Dto.Core
{
    /// <summary>
    /// Options used to filter attachment folders.
    /// </summary>
    public class FolderFilterOptions
    {
        public int? FolderId                   { get; set; }
        public int? ClientId                   { get; set; }
        public int? EmployeeId                 { get; set; }
        public int? OwnerId                    { get; set; }
        public bool IncludeSystemFolders       { get; set; }
        public bool IncludeCompanyFolders      { get; set; }
        public bool IncludeEmployeeFolders     { get; set; }
        public bool IncludeEmployeeVisibleOnly { get; set; }
        public bool IncludeOnboardingFolders   { get; set; }
    }
}
