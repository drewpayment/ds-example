namespace Dominion.Utility.Mapping
{
    /// <summary>
    /// Defines mappings and provides a mechanism to register them.
    /// </summary>
    public interface IMapperBuilder
    {
        /// <summary>
        /// Defines and configures the mapping.  This should only be called once per 
        /// application instance to resister the mapping statically.
        /// </summary>
        void Build();
    }
}