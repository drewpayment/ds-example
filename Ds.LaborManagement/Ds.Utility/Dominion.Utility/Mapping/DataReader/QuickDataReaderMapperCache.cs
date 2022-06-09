using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

using Dominion.Utility.Constants;

namespace Dominion.Utility.Mapping.DataReader
{
    /// <summary>
    /// Manages cached mapping functions used by <see cref="QuickDataReaderMapper{TDest}"/>s.
    /// </summary>
    /// <typeparam name="TDest"></typeparam>
    public sealed class QuickDataReaderMapperCache<TDest> : IQuickDataReaderMapProvider<TDest>
    {
        #region Static

        private static readonly QuickDataReaderMapperCache<TDest> _instance = new QuickDataReaderMapperCache<TDest>();

        /// <summary>
        /// Instance providing access to mapping cache for <see cref="TDest"/>.
        /// </summary>
        public static QuickDataReaderMapperCache<TDest> Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// The cached <see cref="IDataReader"/> object creators.
        /// </summary>
        private readonly Dictionary<int, Func<IDataReader, TDest>> _cache = new Dictionary<int, Func<IDataReader, TDest>>();

        /// <summary>
        /// Make private to hide ability to instantiate elsewhere.
        /// </summary>
        private QuickDataReaderMapperCache()
        {
        }

        /// <summary>
        /// Returns the cached function capable of mapping from a <see cref="IDataReader"/> to a <see cref="TDest"/> 
        /// object.  Function is created and cached if it has not been accessed before.
        /// </summary>
        public Func<IDataReader, TDest> GetObjectCreator(IDataReader reader, string mapperToken = null)
        {
            var key = GetCacheKey(reader, mapperToken);

            Func<IDataReader, TDest> creator;
            if (this._cache.TryGetValue(key, out creator))
            {
                // found in cache so return
                return creator;
            }

            // not found so create, add to cache (for next time) and return
            lock (this._cache)
            {
                // TPR-156 One thread was waiting for lock while another thread was adding the item both threads were 
                // looking for.  In this case we need to check the cache again.
                if (this._cache.TryGetValue(key, out creator))
                {
                    // found in cache so return
                    return creator;
                }
                creator = PropertyMapper.GetInstanceCreator<TDest>(reader, CultureInfo.CurrentCulture, false);
                this._cache.Add(key, creator);
                return creator;
            }
        }

        /// <summary>
        /// Returns a hash of the column names contained within a <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="reader">Reader to generate the cache key from.</param>
        /// <param name="mapperToken">Optional unique token used distinguish uniqueness if two readers have the same column names.</param>
        /// <returns></returns>
        private static int GetCacheKey(IDataRecord reader,  string mapperToken = null)
        {
            var key = mapperToken ?? CommonConstants.EMPTY_STRING;
            
            for(var i = 0; i < reader.FieldCount; i++)
            {
                key += reader.GetName(i);
            }

            return key.GetHashCode();
        }
    }
}
