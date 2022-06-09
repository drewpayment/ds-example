using Dominion.Utility.Msg;

namespace Dominion.Utility.Logging
{
    public interface IDsLogger
    {
        void Debug(IDsLogEvent logEvent);
        void Debug(string title, string detail, bool includeCallStack = false);

        void Error(IDsLogEvent logEvent);
        void Error(string title, string detail, bool includeCallStack = false);

        void Info(IDsLogEvent logEvent);
        void Info(string title, string detail, bool includeCallStack = false);

        void Trace(IDsLogEvent logEvent);
        void Trace(string title, string detail, bool includeCallStack = false);

        void Warn(IDsLogEvent logEvent);
        void Warn(string title, string detail, bool includeCallStack = false);

        void Fatal(IDsLogEvent logEvent);
        void Fatal(string title, string detail, bool includeCallStack = false);

        void LogMessage(IMsgSimple msg);

    }
}