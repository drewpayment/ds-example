using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dominion.Utility.Dto
{
    public class DtoList<T> : DtoObject, IEnumerable<T>
    {
        private List<T> _dtos;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public DtoList()
        {
            _dtos = new List<T>();
        }

        /// <summary>
        /// Initializing constructor.
        /// </summary>
        /// <param name="dtos">Set of DTO objects with which to init this list.</param>
        public DtoList(IEnumerable<T> dtos)
        {
            _dtos = dtos.ToList();
        }

        /// <summary>
        /// Add a range of dtos to the list.
        /// </summary>
        /// <param name="list"></param>
        public DtoList<T> AddDtos(IEnumerable<T> list)
        {
            _dtos.AddRange(list);
            return this;
        }

        #region IEnumberable IMPLEMENTATION

        /// <summary>
        /// Get an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that iterates through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _dtos.GetEnumerator();
        }

        /// <summary>
        /// Get an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that iterates through the collection.</returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dtos.GetEnumerator();
        }

        #endregion // IEnumberable IMPLEMENTATION
    }
}