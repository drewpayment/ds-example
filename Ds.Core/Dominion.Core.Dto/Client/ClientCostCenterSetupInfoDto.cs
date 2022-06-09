using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Client
{
    public class ClientCostCenterSetupInfoDto
    {
        public int                   ClientCostCenterId              { get; set; }
        public int                   ClientId                        { get; set; }
        public string                Code                            { get; set; }
        public string                Description                     { get; set; }
        public int?                  DefaultGlAccountId              { get; set; }
        public bool?                 IsSuspenseCostCenter            { get; set; }
        public string                GlClassName                     { get; set; }
        public bool                  IsActive                        { get; set; }
        public bool                  ExcludeFromGlFile               { get; set; }
        public bool                  HasClientGLAssignment           { get; set; }

        public ClientCostCenterRatePremiumDto RatePremiumInfo  { get; set; }
    }
}
