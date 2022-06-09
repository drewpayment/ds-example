using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Api
{
    public delegate IOpResult<T> ServiceFn<T, in U>(U parameters);

    
    public sealed class MethodClass<T, U>
    {
        private readonly ServiceFn<T, U> _function;
        private readonly U _parameters;
        private MethodClass() { }
        public MethodClass(ServiceFn<T, U> method, U parameters)
        {
            _function = method;
            _parameters = parameters;
        }

        public IOpResult<T> Invoke()
        {
            return _function(_parameters);
        }
    }
}
