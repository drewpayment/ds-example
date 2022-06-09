using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dominion.Utility.Security
{
	/// <summary>
	/// List of action types organized by a specialized identifier.
	/// </summary>
	public class ActionTypeList : IEnumerable<ActionType>, ICollection<ActionType>
	{
		private SortedList<string, ActionType> _actionTypes;


		/// <summary>
		/// Default constructor.
		/// </summary>
		public ActionTypeList()
		{
			_actionTypes = new SortedList<string,ActionType>();
		}


		//-----------------------------------------------------------------------------------
		#region ICollection implementation

		/// <summary>
		/// Get the number of elements in the list.
		/// </summary>
		public int Count
		{
			get { return _actionTypes.Count; }
		}

		/// <summary>
		/// Get the value indicating whether the list is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Add the given action to the list.
		/// </summary>
		/// <param name="actionType">The object to add to the list.</param>
		/// <returns>True if the object is added, false if the object is already in the list.</returns>
		public bool Add( ActionType actionType )
		{
			string key = GetHash( actionType );

			if( ! _actionTypes.ContainsKey(key) )
			{
				_actionTypes.Add( key, actionType );
				return true;
			}
			else
			{
				return false;
			}

		}// Add()


		/// <summary>
		/// Add the given action to the list.
		/// </summary>
		/// <param name="actionType">The object to add to the list.</param>
		void ICollection<ActionType>.Add( ActionType actionType )
		{
			this.Add( actionType );
		}


		/// <summary>
		/// Remove all items from the list.
		/// </summary>
		public void Clear()
		{
			_actionTypes.Clear();
		}


		/// <summary>
		/// Determine whether given action type is in the list.
		/// </summary>
		/// <param name="actionType">The action type to find.</param>
		/// <returns>True if the given action type is in the list.</returns>
		public bool Contains( ActionType actionType )
		{
			return _actionTypes.ContainsKey( GetHash(actionType) );
		}


		/// <summary>
		/// Copy the elements to the given array.
		/// </summary>
		/// <param name="array">The array that is the destination of the copied elements.</param>
		/// <param name="arrayIndex">The index in the array at which copying begins.</param>
		public void CopyTo( ActionType[] array, int arrayIndex )
		{
			for( int i = arrayIndex; i < arrayIndex + _actionTypes.Count; i++ )
			{
				array[i] = _actionTypes.Values[i];
			}
		}


		/// <summary>
		/// Get an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An enumerator that iterates through the collection.</returns>
		public IEnumerator<ActionType> GetEnumerator()
		{
			return _actionTypes.Values.GetEnumerator();
		}


		/// <summary>
		/// Get an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An enumerator that iterates through the collection.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _actionTypes.Values.GetEnumerator();
		}


		/// <summary>
		/// Remove the given action from the list.
		/// </summary>
		/// <param name="actionType">The action to be removed.</param>
		public bool Remove( ActionType actionType )
		{
			string key = GetHash( actionType );

			if( _actionTypes.ContainsKey(key) )
			{
				return _actionTypes.Remove( key );
			}
			else
			{
				return false;
			}

		}// Remove()

		#endregion // ICollection implementation


		/// <summary>
		/// Create a unique, reversable hash (eg: dictionary key) value for the given action type.
		/// </summary>
		/// <param name="actionType">The action type for which the hash is created.</param>
		/// <returns>The hash value for the given action type.</returns>
		private string GetHash( ActionType actionType )
		{
			// this simply uses the action type designation value, at least for now. the designation
			// is expected to be unique across all derived action type classes.
			return actionType.Designation;
		}

	}// class ActionTypeList
}
