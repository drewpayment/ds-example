namespace Dominion.Utility.Msg.Specific
{
    /// <summary>
    /// The message object data gathered during the entity framework's configuration validation phase.
    /// This is executed when SaveChanges is called on the entity framework context.
    /// </summary>
    public class EntityFrameworkConfigValidationMsg : PropertyValidationContainerMsg
    {
        public EntityFrameworkConfigValidationMsg()
        {
        }
    }
}