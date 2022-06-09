using System;

namespace Dominion.Utility.Transform
{
    public class GenericTransformer<T> : Transformer<T>
    {
        private readonly Func<T, T> _transformAction;

        public GenericTransformer(Func<T, T> transformAction)
        {
            _transformAction = transformAction;
        }

        public override T Transform(T instance)
        {
            return _transformAction.Invoke(instance);
        }
    }
}