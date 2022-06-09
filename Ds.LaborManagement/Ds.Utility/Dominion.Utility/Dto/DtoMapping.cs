using System;
using System.Collections.Generic;
using AutoMapper;

namespace Dominion.Utility.Dto
{
    public class DtoMapping
    {
        /// <summary>
        /// Quick dirty way to do mapping.
        /// Registers a map for both the source to dest specified and an IEnumerable<> for both.
        /// Helpful for a list of entities to dto.
        /// </summary>
        /// <typeparam name="TSource">Source object.</typeparam>
        /// <typeparam name="TDest">Dto object.</typeparam>
        public static void MapEntityWithList<TSource, TDest>()
            where TSource : class
            where TDest : class
        {
            Mapper.CreateMap<TSource, TDest>();
            Mapper.CreateMap<IEnumerable<TSource>, IEnumerable<TDest>>();
        }

        /// <summary>
        /// Quick dirty way to do mapping.
        /// Registers a map for both the source to dest specified and an IEnumerable<> for both.
        /// Executes an action on the dto after the mapping is completed.
        /// If it's a list all the items in the list will also have the action executed afterwards.
        /// </summary>
        /// <typeparam name="TSource">Source object.</typeparam>
        /// <typeparam name="TDest">Dto object.</typeparam>
        public static void MapEntityWithListExecuteActionAfterOnDestination<TSource, TDest>(Action<TDest> action)
            where TSource : class
            where TDest : class
        {
            Mapper.CreateMap<TSource, TDest>().AfterMap((s, d) => action(d));
            Mapper.CreateMap<IEnumerable<TSource>, IEnumerable<TDest>>();
        }

        /// <summary>
        /// Quick dirty way to do mapping.
        /// Registers both the source to dest and an IEnumberable for both.
        /// Returns the created map for further customization.
        /// </summary>
        /// <typeparam name="TSource">Source object type.</typeparam>
        /// <typeparam name="TDest">Destination object type.</typeparam>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDest> MapEntityWithListCustom<TSource, TDest>()
            where TSource : class
            where TDest : class
        {
            var map = Mapper.CreateMap<TSource, TDest>();
            Mapper.CreateMap<IEnumerable<TSource>, IEnumerable<TDest>>();
            return map;
        }

        /// <summary>
        /// Helpful for debugging, not for production.
        /// </summary>
        /// <returns>Inforamtion about the maps resitered.</returns>
        public static TypeMap[] ShowConfiguredMaps()
        {
            return Mapper.GetAllTypeMaps();
        }
    }
}