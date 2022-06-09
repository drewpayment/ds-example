using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Transform
{
    using Dominion.Utility.ExtensionMethods;

    public class EmptyToNullTransform : Transformer<string>
    {
        public override string Transform(string instance)
        {
            return string.IsNullOrEmpty(instance) ? null : instance;
        }
    }
}
