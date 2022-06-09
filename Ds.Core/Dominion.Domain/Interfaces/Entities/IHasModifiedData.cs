using System;

using Dominion.Utility.Containers;

namespace Dominion.Domain.Interfaces.Entities
{
    public interface IHasModifiedData
    {
        int         ModifiedBy  { get; set; }
        DateTime    Modified    { get; set; }
    }

    public interface IHasModifiedOptionalData
    {
        int?         ModifiedBy  { get; set; }
        DateTime?   Modified    { get; set; }
    }
	
	public interface IHasOptionalSyncData
    {
        DateTime? LastSync { get; set; }
        DateTime? LastSyncAttempt { get; set; }
    }

    /// <summary>
    /// Represents an Entity with Modified/ModifiedBy, where ModifiedBy is the user name, not the numeric user ID.
    /// 
    /// Some system processes can break this rule.
    /// </summary>
    public interface IHasModifiedUserNameData
    {
        string   ModifiedBy { get; set; }
        DateTime Modified   { get; set; }
    }

    /// <summary>
    /// Represents an Entity with Modified/ModifiedBy, where ModifiedBy is the numeric User ID.
    /// 
    /// Some system processes can break this rule.
    /// </summary>
    public interface IHasModifiedStringUserIdData
    {
        string   ModifiedBy { get; set; }
        DateTime Modified   { get; set; }
    }

    public static class HasModifiedExtensions
    {
        /// <summary>
        /// Includes <see cref="IHasModifiedData.Modified"/> and <see cref="IHasModifiedData.ModifiedBy"/> in the property list.
        /// </summary>
        /// <typeparam name="TModified"></typeparam>
        /// <param name="properties">Existing property list which may or may not include other properties.</param>
        /// <returns></returns>
        public static PropertyList<TModified> IncludeModifiedProperties<TModified>(this PropertyList<TModified> properties) where TModified : class, IHasModifiedData
        {
            return properties.Include(x => x.Modified).Include( x => x .ModifiedBy);
        }

        /// <summary>
        /// Includes <see cref="IHasModifiedOptionalData.Modified"/> and <see cref="IHasModifiedOptionalData.ModifiedBy"/> in the property list.
        /// </summary>
        /// <typeparam name="TModified"></typeparam>
        /// <param name="properties">Existing property list which may or may not include other properties.</param>
        /// <returns></returns>
        public static PropertyList<TModified> IncludeModifiedOptionalProperties<TModified>(this PropertyList<TModified> properties) where TModified : class, IHasModifiedOptionalData
        {
            return properties.Include(x => x.Modified).Include( x => x .ModifiedBy);
        }

        /// <summary>
        /// Includes <see cref="IHasModifiedOptionalData.Modified"/> and <see cref="IHasModifiedOptionalData.ModifiedBy"/> in the property list.
        /// </summary>
        /// <typeparam name="TModified"></typeparam>
        /// <param name="properties">Existing property list which may or may not include other properties.</param>
        /// <returns></returns>
        public static PropertyList<TModified> IncludeModifiedStringUserIdProperties<TModified>(this PropertyList<TModified> properties) where TModified : class, IHasModifiedStringUserIdData
        {
            return properties.Include(x => x.Modified).Include(x => x.ModifiedBy);
        }

        /// <summary>
        /// Includes <see cref="IHasModifiedOptionalData.Modified"/> and <see cref="IHasModifiedOptionalData.ModifiedBy"/> in the property list.
        /// </summary>
        /// <typeparam name="TModified"></typeparam>
        /// <param name="properties">Existing property list which may or may not include other properties.</param>
        /// <returns></returns>
        public static PropertyList<TModified> IncludeModifiedUserNamerProperties<TModified>(this PropertyList<TModified> properties) where TModified : class, IHasModifiedUserNameData
        {
            return properties.Include(x => x.Modified).Include(x => x.ModifiedBy);
        }
    }
}
