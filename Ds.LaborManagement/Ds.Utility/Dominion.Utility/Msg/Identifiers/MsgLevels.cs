using Serilog.Events;

namespace Dominion.Utility.Msg.Identifiers
{
    /// <summary>
    /// Levels of message importance.
    /// </summary>
    public enum MsgLevels
    {
        // note that the integer value is important for determining relative significance.
        Debug = 0, 
        Info = 1, 
        Warn = 2, 
        Error = 3, 
        Fatal = 4
    }

    public static class MsgLevelsExtensions
    {
        public static LogEventLevel GetLogEventLevel(this MsgLevels self)
        {
            switch (self)
            {
                case MsgLevels.Debug:
                    return LogEventLevel.Debug;
                case MsgLevels.Info:
                    return LogEventLevel.Information;
                case MsgLevels.Warn:
                    return LogEventLevel.Warning;
                case MsgLevels.Error:
                    return LogEventLevel.Error;
                case MsgLevels.Fatal:
                    return LogEventLevel.Fatal;
                default:
                    return LogEventLevel.Verbose;
            }
        }
    }
    
    public static class LogEventLevelExtensions
    {
        public static MsgLevels GetMsgLevelsFromLogEventLevel(this LogEventLevel self)
        {
            switch (self)
            {
                case LogEventLevel.Debug:
                    return MsgLevels.Debug;
                case LogEventLevel.Information:
                    return MsgLevels.Info;
                case LogEventLevel.Warning:
                    return MsgLevels.Warn;
                case LogEventLevel.Error:
                    return MsgLevels.Error;
                case LogEventLevel.Fatal:
                    return MsgLevels.Fatal;
                // MsgLevels.Debug is as "verbose" as these go currently.
                case LogEventLevel.Verbose:
                default:
                    return MsgLevels.Debug;
            }
        }
    }
}