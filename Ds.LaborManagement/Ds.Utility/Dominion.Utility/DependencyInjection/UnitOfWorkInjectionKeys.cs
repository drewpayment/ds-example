namespace Dominion.Utility.DependencyInjection
{
    /// <summary>
    /// Keys used to specify a binding if multiple bindings are defined for a single interface.
    /// </summary>
    public class UnitOfWorkInjectionKeys
    {
        /// <summary>
        /// Used for the entity framework unit of work (or equivalent: ie InMemory).
        /// </summary>
        public const string ENTITY_FRAMEWORK_UNIT_OF_WORK_KEY = "EfUnitOfWork";
    }
}