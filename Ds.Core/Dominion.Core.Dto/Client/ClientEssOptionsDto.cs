using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    [Serializable]
    public partial class ClientEssOptionsDto
    {
        public int ClientId { get; set; } 
        public int? DirectDepositLimit { get; set; } 
        public bool AllowDirectDeposit { get; set; } 
        public bool AllowCheck { get; set; } 
        public bool AllowPaycard { get; set; }
        public bool AllowPaystubEmails { get; set; }
        public bool AllowImageUpload { get; set; }
        public string WelcomeMessage { get; set; }
        public string FinalDisclaimerMessage { get; set; }
        public string FinalDisclaimerAgreementText { get; set; }
        public bool IsCustomMessage { get; set; }
        public bool ManageDirectDepositAmount { get; set; }
        public bool ManageDirectDepositAmountAndAccountInfo { get; set; }
        public string DirectDepositDisclaimer { get; set; }
        public bool DoInsert { get; set; }
    }
}
