namespace Dominion.Utility.Dto
{
    public static class IStateManagedDtoExtensions
    {
        /// <summary>
        /// Indicates if the DTO is marked for deletion.
        /// </summary>
        /// <param name="dto">DTO to check.</param>
        /// <returns></returns>
        public static bool IsDtoDeleted(this IStateManagedDto dto)
        {
            return dto.DtoState == DtoState.Deleted;
        }

        /// <summary>
        /// Indicates if the DTO is marked to be added.
        /// </summary>
        /// <param name="dto">DTO to check.</param>
        /// <returns></returns>
        public static bool IsDtoAdded(this IStateManagedDto dto)
        {
            return dto.DtoState == DtoState.Added;
        }

        /// <summary>
        /// Indicates if the DTO is marked as modified.
        /// </summary>
        /// <param name="dto">DTO to check.</param>
        /// <returns></returns>
        public static bool IsDtoModified(this IStateManagedDto dto)
        {
            return dto.DtoState == DtoState.Modified;
        }
    }
}
