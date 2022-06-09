using System;
using System.Collections.Generic;

namespace Dominion.Utility.Mapping
{
    public abstract class Mapper<TSource, TDest> : IMapper<TSource, TDest>
    {
        public Type SourceType
        {
            get { return typeof(TSource); }
        }

        public Type DestinationType
        {
            get { return typeof(TDest); }
        }

        public abstract TDest Map(TSource obj);

        public abstract IEnumerable<TDest> Map(IEnumerable<TSource> source);
    }
}
