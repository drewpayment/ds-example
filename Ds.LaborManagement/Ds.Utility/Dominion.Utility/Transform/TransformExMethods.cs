using Dominion.Utility.Msg;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Transform
{
    public static class TransformExMethods
    {
        public static string TransformAndValidate<T>(
            this T transformer, 
            string value, 
            IOpResult result, 
            IMsgSimple msg)
            where T : ITransformer<string>
        {
            value = transformer.Transform(value);

            if(string.IsNullOrWhiteSpace(value))
                result.AddMessage(msg).SetToFail();

            //return string.IsNullOrEmpty(value) ? null : value;
            return value;
        }
    }
}
