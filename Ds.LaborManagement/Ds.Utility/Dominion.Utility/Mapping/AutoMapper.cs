using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Dominion.Utility.Mapping
{
    /// <summary>
    /// Mapper which uses <see cref="AutoMapper"/> to define the mapping.
    /// </summary>
    /// <typeparam name="TSource">Source type to map FROM.</typeparam>
    /// <typeparam name="TDest">Destination type to map TO.</typeparam>
    [Obsolete("From this point on out (2015.03.04) we will use Fast Mapper. Look for 'FastMapperDs'; it replaces this.")]
    public class AutoMapper<TSource, TDest> : Mapper<TSource, TDest>, IMapperBuilder
    {
        /// <summary>
        /// Defines and registers the <see cref="AutoMapper"/> mapping.  By default this calls 
        /// <see cref="Mapper.CreateMap{TSource,TDest}()"/>. This should only be called once per 
        /// application instance to resister the mapping statically with AutoMapper.
        /// </summary>
        public virtual void Build()
        {
            this.CreateMap();
        }

        /// <summary>
        /// Maps an instance of the source-type to the destination-type using Mapper.Map().
        /// </summary>
        /// <param name="obj">Object containing the source information to map from.</param>
        /// <returns>Object containing the mapped information.</returns>
        public override TDest Map(TSource obj)
        {
            return Mapper.Map<TSource, TDest>(obj);
        }

        /// <summary>
        /// Maps a collection of source-type objects to a collection of destination-type objects using 
        /// <see cref="AutoMapper"/> mappings.  If the source is <see cref="IQueryable"/>, the results are 
        /// Project()'ed to the destination type.
        /// </summary>
        /// <param name="source">Object collection containing the information to map from.</param>
        /// <returns></returns>
        public override IEnumerable<TDest> Map(IEnumerable<TSource> source)
        {
            if(source is IQueryable<TSource>)
                return (source as IQueryable<TSource>).Project().To<TDest>();

            return Mapper.Map<IEnumerable<TSource>, IEnumerable<TDest>>(source);
        }

        /// <summary>
        /// Returns the constructed mapping between <see cref="TSource"/> and <see cref="TDest"/> using AutoMapper's 
        /// <see cref="Mapper.CreateMap{TSource,TDest}()"/>.
        /// </summary>
        /// <param name="memberList">MemberList to validate. If not provided (default), no member validation will occur.</param>
        /// <returns></returns>
        protected IMappingExpression<TSource, TDest> CreateMap(MemberList? memberList = null)
        {
            return memberList.HasValue ? 
                Mapper.CreateMap<TSource, TDest>(memberList.Value) : 
                Mapper.CreateMap<TSource, TDest>();
        }
    }
}
