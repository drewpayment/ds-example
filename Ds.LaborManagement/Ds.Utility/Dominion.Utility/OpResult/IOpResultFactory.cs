using Dominion.Utility.Logging;

namespace Dominion.Utility.OpResult
{
    public interface IOpResultFactory
    {
        IOpResult GetOpResult();

        IOpResult<T> GetOpResult<T>();

        void UpdateLogger(IDsLogger logger);
    }
}