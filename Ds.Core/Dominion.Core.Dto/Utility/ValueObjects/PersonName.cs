using Dominion.Core.Dto.Interfaces;
using Dominion.Utility.Constants;

namespace Dominion.Core.Dto.Utility.ValueObjects
{
    /// <summary>
    /// Represents different formats of a person's name.
    /// Focus of this class is to have a central location for definition of the possible acceptable formats.
    /// </summary>
    public class PersonName
    {
        #region Variables and Properties

        public string First { get; private set; }
        public string Middle { get; private set; }
        public string Last { get; private set; }

        /// <summary>
        /// Construct a string consisting of 'lastname, firstname'.
        /// </summary>
        public string LastFirst
        {
            get
            {
                var separator =
                    (string.IsNullOrEmpty(Last) || string.IsNullOrEmpty(First))
                        ? string.Empty
                        : CommonConstants.COMMA_SPACE;

                var val =
                    Last +
                    separator +
                    First;

                return val;
            }
        }

        public string FirstLast
        {
            get
            {
                var separator =
                    (string.IsNullOrEmpty(Last) || string.IsNullOrEmpty(First))
                        ? string.Empty
                        : CommonConstants.COMMA_SPACE;

                var val =
                    First +
                    separator +
                    Last;

                return val;
            }
        }

        /// <summary>
        /// Construct a string consisting of 'lastname mid firstname'.
        /// </summary>
        public string FirstMidLast
        {
            get
            {
                var mid =
                    string.IsNullOrEmpty(Middle)
                        ? CommonConstants.SINGLE_SPACE
                        : CommonConstants.SINGLE_SPACE + Middle + CommonConstants.SINGLE_SPACE;

                var val =
                    First +
                    mid +
                    Last;

                return val.Trim();
            }
        }

        public string LastFirstMid
        {
            get
            {
                var separator =
                    (string.IsNullOrEmpty(Last) || string.IsNullOrEmpty(First))
                        ? string.Empty
                        : CommonConstants.COMMA_SPACE;

                var val =
                    Last +
                    separator +
                    First;

                if(!string.IsNullOrEmpty(Middle))
                    val += CommonConstants.SINGLE_SPACE + Middle;

                return val;
            }
        }

        #endregion

        #region Constructors and Initializers

        public PersonName()
        {
        }

        public PersonName(IHasFirstMiddleInitialLast data)
            : this(data.FirstName, data.MiddleInitial, data.LastName)
        {
        }

        public PersonName(string first, string middle, string last)
        {
            First = string.IsNullOrEmpty(first) ? string.Empty : first;
            Middle = string.IsNullOrEmpty(middle) ? string.Empty : middle.Trim();
            Last = string.IsNullOrEmpty(last) ? string.Empty : last;
        }

        #endregion
    }
}