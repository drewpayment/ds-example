namespace Dominion.Core.Dto.Security
{
    /// <summary>
    /// Data transfer object for user secret question information.
    /// </summary>
    public class SecretQuestionDto
    {
        public virtual int SecretQuestionId { get; set; }
        public virtual string Text { get; set; }
    }
}