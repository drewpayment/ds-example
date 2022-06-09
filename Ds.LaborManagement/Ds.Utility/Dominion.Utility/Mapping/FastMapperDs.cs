using System;
using System.Collections.Generic;
using System.Linq;
using FastMapper;

namespace Dominion.Utility.Mapping
{
    /// <summary>
    /// Mapper which uses <see cref="FastMapper"/> to define the mapping.
    /// Fast mapper is faster and requires no configuration unless you need a custom mapping.
    /// https://fmapper.codeplex.com/
    /// </summary>
    /// <typeparam name="TSource">Source type to map FROM.</typeparam>
    /// <typeparam name="TDest">Destination type to map TO.</typeparam>
    public class FastMapperDs<TSource, TDest> : Mapper<TSource, TDest>, IMapperBuilder
    {
        private readonly Lazy<TypeAdapterConfig<TSource, TDest>> _lazyConfig = new Lazy<TypeAdapterConfig<TSource, TDest>>(TypeAdapterConfig<TSource, TDest>.NewConfig); 

        /// <summary>
        /// FastMapper configuration used to customize the mapping.
        /// </summary>
        protected TypeAdapterConfig<TSource, TDest> Config
        {
            get { return _lazyConfig.Value; }
        }


        /// <summary>
        /// Defines and registers the <see cref="FastMapper"/> mapping.  By default this calls 
        /// application instance to resister the mapping statically with AutoMapper.
        /// </summary>
        public virtual void Build()
        {
        }

        /// <summary>
        /// Maps an instance of the source-type to the destination-type.
        /// </summary>
        /// <param name="obj">Object containing the source information to map from.</param>
        /// <returns>Object containing the mapped information.</returns>
        public override TDest Map(TSource obj)
        {
            return TypeAdapter.Adapt<TSource, TDest>(obj);
        }

        /// <summary>
        /// Maps a collection of source-type objects to a collection of destination-type objects using 
        /// <see cref="FastMapper"/> mappings.  If the source is <see cref="IQueryable"/>, the results are 
        /// Project()'ed to the destination type.
        /// </summary>
        /// <param name="source">Object collection containing the information to map from.</param>
        /// <returns></returns>
        public override IEnumerable<TDest> Map(IEnumerable<TSource> source)
        {
            if(source is IQueryable<TSource>)
                return source.AsQueryable().Project().To<TDest>();

            return TypeAdapter.Adapt<IEnumerable<TSource>, IEnumerable<TDest>>(source);
        }
    }
}
