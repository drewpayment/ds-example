using NLog;

namespace Dominion.Utility.Logging
{
    public class DsLogEvent : IDsLogEvent
    {
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string CallStack { get; set; }

        public LogEventInfo GetLogEventInfo()
        {
            var info = new LogEventInfo {Message = Title};
            info.Message = Title;
            info.Properties["UserName"] = UserName;
            info.Properties["Title"] = Title;
            info.Properties["Detail"] = Detail;
            info.Properties["CallStack"] = CallStack;
            return info;
        }

    }
}