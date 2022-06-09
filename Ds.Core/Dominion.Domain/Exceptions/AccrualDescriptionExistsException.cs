using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Exceptions
{
    /// <summary>
    /// Declare an exception to be thrown when we are trying to add a duplicate client name
    /// </summary>
    public class AccrualDescriptionExistsException : Exception
    {
        public AccrualDescriptionExistsException() 
            : base("The specified accrual description has already been used.")
        {
        }
    }
}
