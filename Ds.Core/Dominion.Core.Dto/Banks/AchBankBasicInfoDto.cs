namespace Dominion.Core.Dto.Banks
{
    public class AchBankBasicInfoDto
    {
        public int    AchBankId           { get; set; }
        public string DisplayName         { get; set; }
        public string DsiBankCode         { get; set; }
        public bool   IsTaxManagementBank { get; set; }
    }
}