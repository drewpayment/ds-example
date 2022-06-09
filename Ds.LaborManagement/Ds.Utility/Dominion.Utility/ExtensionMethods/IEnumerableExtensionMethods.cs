using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.ExtensionMethods
{
    public static class IEnumerableExtensionMethods
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        /// <summary>
        /// Returns null if the original enumerable is null; otherwise ToList()'s the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static List<T> NullCheckToList<T>(this IEnumerable<T> e)
        {
            return e == null ? null : (e as List<T> ?? e.ToList());
        } 

        /// <summary>
        /// Returns a new <see cref="List{T}"/> if the original enumerable is null; otherwise ToList()'s the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static List<T> ToOrNewList<T>(this IEnumerable<T> e)
        {
            return e.NullCheckToList() ?? new List<T>();
        } 

        /// <summary>
        /// Splits a collection into chunks of the specified size.  Last chunk will have remaining elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Collection to split.</param>
        /// <param name="size">Number of elements to include in each chunk.</param>
        /// <returns></returns>
        /// <remarks>
        /// REFERENCE: 
        /// http://stackoverflow.com/a/438208
        /// </remarks>
        //public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> source, int size)
        //{
        //    T[] array = null;
        //    var count = 0;
        //    foreach (T item in source)
        //    {
        //        if (array == null)
        //        {
        //            array = new T[size];
        //        }
        //        array[count] = item;
        //        count++;
        //        if (count == size)
        //        {
        //            yield return new ReadOnlyCollection<T>(array);
        //            array = null;
        //            count = 0;
        //        }
        //    }
        //    if (array != null)
        //    {             
        //        Array.Resize(ref array, count);
        //        yield return new ReadOnlyCollection<T>(array);
        //    }
        //}
        // BETTER PERFORMANCE AS IT ISN'T RECREATING ARRAYS IN MEMORY AND JUST MOVING THE POINTER.
        public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> source, int size)
        {
            while (source.Any())
            {
                yield return source.Take(size);
                source = source.Skip(size);
            }
        }

        public static IOpResult CompareObjectValues<T>(this List<T> one, List<T> two) where T : class
        {
            var opResult = new OpResult.OpResult();
            for(int i = 0; i < one.Count(); i++)
            {
                var result = new OpResult.OpResult();
                var first = one[i];
                first.CompareObjectValues(two[i]);

                opResult.CombineMessages(result);
            }

            return opResult;
        }

        /// <summary>
        /// Add a range of items only if the comparison is true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static List<T> AddRangeDistinct<T>(
            //------------------------
            this List<T> list, 
            IEnumerable<T> items, 
            Func<T, T, bool> criteria)
            //------------------------
            where T : class
        {
            foreach (var obj in items)
            {
                if(!list.Any(x => criteria(x, obj)))
                    list.Add(obj);
            }
        
            return list;
        }
        
        /// <summary>
        /// Does exactly what AddRange does but returns the list for chaining.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<T> AddRangeEx<T>(this List<T> list, IEnumerable<T> items)
            where T : class
        {
            list.AddRange(items);
            return list;
        }   

        /// <summary>
        /// Converts a collection to a <see cref="DataSet"/>. Adds <see cref="DataTable"/> column names based on the 
        /// property names of the object.
        /// </summary>
        /// <typeparam name="T">Type of collection object.</typeparam>
        /// <param name="collection">Collection to convert.</param>
        /// <returns>Dataset representation of the collection.</returns>
        /// <remarks>
        /// Modified From: http://stackoverflow.com/questions/1245662/convert-generic-list-to-dataset-in-c-sharp
        /// 
        /// Currently this extension is used in unit tests only. If used in production remove ExcludeFromCodeCoverage 
        /// attribute and add tests accordingly.
        /// </remarks>
        [ExcludeFromCodeCoverage]
        public static DataSet ToDataSet<T>(this IEnumerable<T> collection)
        {
            var elementType = typeof(T);
            var ds          = new DataSet();
            var t           = new DataTable();

            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                var columnType = propInfo.PropertyType.GetUnderlyingType();
                var col = t.Columns.Add(propInfo.Name, columnType);

                col.AllowDBNull = propInfo.PropertyType.IsNullableType() || propInfo.PropertyType.Equals<string>();
            }

            //go through each property on T and add each value to the table
            foreach (T item in collection)
            {
                var row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }

        public static IList<T> AddIfNotNull<T>(this IList<T> list, T item)
            where T : class
        {
            if(item != null)
                list.Add(item);

            return list;
        }

        /// <summary>
        /// Removes items from the list that are said to be bad.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="removeCriteria"></param>
        public static IEnumerable<T> RemoveWhere<T>(this IEnumerable<T> items, Func<T, bool> removeCriteria)
        {
            var list = items.ToList();
            for (var i = list.Count() - 1; i >= 0; i--)
            {
                if (removeCriteria(list[i]))
                    list.RemoveAt(i);
            }

            return list;
        }


        /// <summary>
        /// Removes items from the list that are said to be bad but ONLY from the end of the list.
        /// Once it finds a 'good' row no more items will be removed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="removeCriteria"></param>
        public static IEnumerable<T> RemoveFromEndWhere<T>(this IEnumerable<T> items, Func<T, bool> removeCriteria)
        {
            var list = items.ToList();
            for (var i = list.Count() - 1; i >= 0; i--)
            {
                if(removeCriteria(list[i]))
                    list.RemoveAt(i);
                else
                    break;
            }

            return list;
        }

        /// <summary>
        /// It's meant to serve as a shortcut.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> list)
        {
            return list.ToList().AsReadOnly();
        }

        /// <summary>
        /// http://stackoverflow.com/questions/489258/linqs-distinct-on-a-particular-property
        /// selects a distinct list of objects matching the provided field("Selector")
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Takes a collection of item sets and returns the possible item combinations when taking one item per set.
        /// </summary>
        /// <typeparam name="T">Item type being combined.</typeparam>
        /// <param name="itemSets">Item sets to get unique combinations of.</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GetItemCombinations<T>(this IEnumerable<IEnumerable<T>> itemSets)
        {
            return itemSets != null ? Combinator(itemSets.ToList()) : null;
        }

        /// <summary>
        /// Recursively determines all possible unique combinations of items in the <see cref="itemSets"/> and returns the 
        /// different combos.
        /// </summary>
        /// <typeparam name="T">Item type being combined.</typeparam>
        /// <param name="itemSets">Item sets to get unique combinations of.</param>
        /// <param name="activeSetIndex">Index of the set items to currently take from.</param>
        /// <param name="activeResultSet">The result set currently being filled.</param>
        /// <param name="resultSets">The collection of unique item combinations.</param>
        /// <returns></returns>
        private static IEnumerable<IEnumerable<T>> Combinator<T>(IList<IEnumerable<T>> itemSets, int activeSetIndex = 0, T[] activeResultSet = null, List<IEnumerable<T>> resultSets = null)
        {
            activeResultSet = activeResultSet ?? new T[itemSets.Count];
            resultSets      = resultSets ?? new List<IEnumerable<T>>();

            if(itemSets.Count > 0)
            {
                var activeItemSet = itemSets[activeSetIndex];

                Action handleResultSet = () =>
                    {
                        if(activeSetIndex + 1 == itemSets.Count)
                        {
                            //done with current result set, so copy it to a new list and add it 
                            //have to copy it as the items in the activeResultSet are overwritten while sequencing
                            resultSets.Add(new List<T>(activeResultSet));
                        }
                        else
                        {
                            //resursively add next set's items
                            Combinator(itemSets, activeSetIndex + 1, activeResultSet, resultSets);
                        }
                    };

                var wasSetHandled = false;
                if(activeItemSet != null)
                {
                    foreach(var item in activeItemSet)
                    {
                        activeResultSet[activeSetIndex] = item;
                        handleResultSet();
                        wasSetHandled = true;
                    }
                }

                if(!wasSetHandled)
                {
                    activeResultSet[activeSetIndex] = default(T);
                    handleResultSet();
                }
            }

            return resultSets;
        }


        public static string ToSeparatedList<T>(this IEnumerable<T> list, string separator = ",")
        {
            var sb = new StringBuilder();

            foreach (var item in list)
                sb.Append($"{item}{separator}");

            //this shortens the string so that the last separator isn't included.
            sb.Length = sb.Length - separator.Length;

            return sb.ToString();
        }

        //public static IList<T> AddIfNotNull<T>(this IList<T> list, T obj)
        //{
        //    if(obj != null)
        //        list.Add(obj);

        //    return list;
        //} 

        public static IList<T> AddIf<T>(this IList<T> list, T val, bool criteria)
        {
            if(criteria)
                list.Add(val);

            return list;
        }

        /// <summary>
        /// Returns a particular new object if IEnumerable objects are null. Created to use 
        /// with FirstOrDefault.
        /// 
        /// https://stackoverflow.com/questions/12972295/firstordefault-default-value-other-than-null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="alternate"></param>
        /// <returns></returns>
        public static T IfDefaultUse<T>(this T value, T alternate)
        {
            return value.Equals(default(T)) ? alternate : value;
        }

        /// <summary>
        /// Returns a Hashset from the IEnumerable. Hashset automatically eliminates duplicate values.
        /// 
        /// https://stackoverflow.com/questions/3471899/how-to-convert-linq-results-to-hashset-or-hashedset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        {
            return new HashSet<T>(source, comparer);
        }

        public static Dictionary<TKey, TElement> ToSafeDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer = null)
        {
            if (source == null)
                throw new ArgumentException("source");
            if (keySelector == null)
                throw new ArgumentException("keySelector");
            if (elementSelector == null)
                throw new ArgumentException("elementSelector");

            Dictionary<TKey, TElement> d = new Dictionary<TKey, TElement>(comparer);
            foreach (TSource element in source)
            {
                if (!d.ContainsKey(keySelector(element)))
                    d.Add(keySelector(element), elementSelector(element));
            }

            return d;
        }

    }
}
