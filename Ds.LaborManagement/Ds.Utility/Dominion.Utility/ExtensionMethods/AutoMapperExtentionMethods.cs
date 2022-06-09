using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Dominion.Utility.ExtensionMethods
{
    public static class AutoMapperExtentionMethods
    {
        /// <summary>
        /// Used to shorthand the automapper code for turning a list of pocos to a list of dto pocos.
        /// The automapping configuration must exist for this work. 
        /// </summary>
        /// <typeparam name="TSource">The poco type.</typeparam>
        /// <typeparam name="TDto">The dto type.</typeparam>
        /// <param name="source">The source list.</param>
        /// <returns></returns>
        public static IEnumerable<TDto> ToDtos<TSource, TDto>(this IEnumerable<TSource> source) 
            where TSource : class
            where TDto : class
        {
            var data = Mapper.Map<List<TDto>>(source);
            return data;
        }
    }
}
