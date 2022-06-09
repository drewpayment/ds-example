using System;
using System.Security.Claims;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Msg;
using Dominion.Utility.Msg.Identifiers;
using Dominion.Utility.Security;
using NLog;

namespace Dominion.Utility.Logging
{
    public class DsLogger : IDsLogger
    {
        private readonly Logger _logger;

        public DsLogger()
        {
            _logger = LogManager.GetLogger("DsLogger");
        }

        public void Debug(IDsLogEvent logEvent)
        {
            _logger.Log(LogLevel.Debug, (logEvent as DsLogEvent)?.GetLogEventInfo());
        }

        public void Debug(string title, string detail, bool includeCallStack = false)
        {
            _logger.Log(LogLevel.Debug, GetLogEvent(title, detail, includeCallStack).GetLogEventInfo());
        }

        private DsLogEvent GetLogEvent(string title, string detail, bool includeCallStack)
        {
            return new DsLogEvent()
            {
                UserName = GetCurrentIdentity().UserName, 
                Title = title,
                Detail = detail,
                CallStack = includeCallStack ? Environment.StackTrace : string.Empty
            };
        }

        public void Error(IDsLogEvent logEvent)
        {
            _logger.Log(LogLevel.Error, (logEvent as DsLogEvent)?.GetLogEventInfo());
        }

        public void Error(string title, string detail, bool includeCallStack = false)
        {
            _logger.Log(LogLevel.Error, GetLogEvent(title, detail, includeCallStack));
        }

        public void Info(IDsLogEvent logEvent)
        {
            _logger.Log(LogLevel.Info, GetEventFromInterface(logEvent));
        }

        public void Info(string title, string detail, bool includeCallStack = false)
        {
            _logger.Log(LogLevel.Info, GetLogEvent(title, detail, includeCallStack));
        }

        public void Trace(IDsLogEvent logEvent)
        {
            _logger.Log(LogLevel.Trace, logEvent);
        }

        public void Trace(string title, string detail, bool includeCallStack = false)
        {
            _logger.Log(LogLevel.Trace, GetLogEvent(title, detail, includeCallStack));

        }

        public void Warn(IDsLogEvent logEvent)
        {
            _logger.Log(LogLevel.Warn, logEvent);
        }

        public void Warn(string title, string detail, bool includeCallStack = false)
        {
            _logger.Log(LogLevel.Warn, GetLogEvent(title, detail, includeCallStack));
        }

        public void Fatal(IDsLogEvent logEvent)
        {
            _logger.Log(LogLevel.Fatal, logEvent);
        }

        public void Fatal(string title, string detail, bool includeCallStack = false)
        {
            _logger.Log(LogLevel.Fatal, GetLogEvent(title, detail, includeCallStack));
        }

        public void LogMessage(IMsgSimple msg)
        {
            _logger.Log(GetLogLevel(msg.Level), GetLogEvent(msg).GetLogEventInfo());
        }

        private IDsLogIdentity GetCurrentIdentity()
        {
            //May not be the corrent client ID as we use additional logic in 
            // the api session to update the clientId claim 
            return new DsLogIdentity()
            {
                UserName =  ClaimsPrincipal.Current?.Identity?.Name,
                CLientId = ClaimsPrincipal.Current.GetClientIdClaim()
            };
        }

        private LogEventInfo GetEventFromInterface(IDsLogEvent logEvent)
        {
            return (logEvent as DsLogEvent)?.GetLogEventInfo();
        }

        private DsLogEvent GetLogEvent(IMsgSimple msg)
        {
            return new DsLogEvent
            {
                UserName  = GetCurrentIdentity().UserName,
                Title     = $"{msg.GetType()}: {msg.Msg}",
                CallStack = Environment.StackTrace
            };
        }

        private LogLevel GetLogLevel(MsgLevels level)
        {
            switch (level)
            {
                case MsgLevels.Debug:
                    return LogLevel.Debug;
                case MsgLevels.Info:
                    return LogLevel.Info;
                case MsgLevels.Warn:
                    return LogLevel.Warn;
                case MsgLevels.Error:
                    return LogLevel.Error;
                case MsgLevels.Fatal:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Debug;
            }
        }
    }
}