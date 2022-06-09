namespace Dominion.Core.Dto.Core
{
    public enum ResourceSourceType : byte
    {
        LocalServerFile = 1,
        Url             = 2,
        Form            = 3,
        Video           = 4,
        AzureProfileImage = 5,
        AzureClientImage = 6,
        AzureClientFile = 7
    }
}
