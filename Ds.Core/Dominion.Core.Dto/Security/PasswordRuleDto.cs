using Dominion.Utility.Validation;

namespace Dominion.Core.Dto.Security
{
    /// <summary>
    /// DTO representing a single password rule configuration.
    /// </summary>
    public class PasswordRuleDto
    {
        public TextValidationRuleType RuleType { get; set; }
        public string RegularExpression { get; set; }
        public string Description { get; set; }
        public string ErrorMessage { get; set; }
        public bool? IsValid { get; set; }
    }
}