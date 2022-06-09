using System;
using System.Collections.Generic;
using System.Text;

namespace Dominion.Utility.OpResult
{
    public class OpResultException : Exception
    {
        public OpResultException()
            : base("OpResult Exception")
        {

        }

        public OpResultException(string message)
            : base(message)
        {

        }
    }
}
