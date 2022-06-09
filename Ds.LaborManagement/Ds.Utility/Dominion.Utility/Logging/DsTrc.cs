using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dominion.Utility.ExtensionMethods;
using Newtonsoft.Json;

namespace Dominion.Utility.Logging
{
    public class DsTrc
    {
        #region DS.DEFAULT

        private static DsTrc _instance = null;
        public static DsTrc I => _instance ?? new DsTrc("DS.DEFAULT");

        #endregion

        #region DS.GLOBAL.ASAX

        private static DsTrc _instanceGlobal = null;
        public static DsTrc IG => _instanceGlobal ?? new DsTrc("DS.GLOBAL.ASAX");

        #endregion

        #region DS.STS

        private static DsTrc _instanceSTS = null;
        public static DsTrc ISTS => _instanceSTS ?? new DsTrc("DS.STS");

        #endregion

        private readonly TraceSource _ts;// = new TraceSource("STS.WebSite");

        public DsTrc(string sourceName)
        {
            _ts = new TraceSource(sourceName);
        }

        [DebuggerStepThrough]
        public void Start(string message)
        {
            TraceEvent(TraceEventType.Start, message);
        }

        [DebuggerStepThrough]
        public void Stop(string message)
        {
            TraceEvent(TraceEventType.Stop, message);
        }

        [DebuggerStepThrough]
        public void Information(string message)
        {
            TraceEvent(TraceEventType.Information, message);
        }

        [DebuggerStepThrough]
        public void InformationFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Information, message, objects);
        }

        [DebuggerStepThrough]
        public void Warning(string message)
        {
            TraceEvent(TraceEventType.Warning, message);
        }

        [DebuggerStepThrough]
        public void WarningFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Warning, message, objects);
        }

        [DebuggerStepThrough]
        public void Error(string message)
        {
            TraceEvent(TraceEventType.Error, message);
        }

        [DebuggerStepThrough]
        public void ErrorVerbose(string message, [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            TraceEventFormat(TraceEventType.Error, "{0}\n\nMethod: {1}\nFilename: {2}\nLine number: {3}", message,
                memberName, filePath, lineNumber);
        }

        [DebuggerStepThrough]
        public void ErrorFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Error, message, objects);
        }

        [DebuggerStepThrough]
        public void Verbose(string message)
        {
            TraceEvent(TraceEventType.Verbose, message);
        }

        [DebuggerStepThrough]
        public void VerboseFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Verbose, message, objects);
        }

        [DebuggerStepThrough]
        public void Transfer(string message, Guid activity)
        {
            _ts.TraceTransfer(0, message, activity);
        }

        [DebuggerStepThrough]
        public void TraceEventFormat(TraceEventType type, string message, params object[] objects)
        {
            var format = string.Format(message, objects);
            TraceEvent(type, format);
        }

        [DebuggerStepThrough]
        public void TraceEvent(TraceEventType type, string message)
        {
            if (Trace.CorrelationManager.ActivityId == Guid.Empty)
            {
                Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            }

            _ts.TraceEvent(type, 0, message);
        }

        [DebuggerStepThrough]
        public void ErrorJson(object obj)
        {
            this.LogJson(TraceEventType.Error, JsonConvert.SerializeObject(obj));
        }

        [DebuggerStepThrough]
        public void InformationJson(object obj)
        {
            this.LogJson(TraceEventType.Information, JsonConvert.SerializeObject(obj));
        }

        [DebuggerStepThrough]
        public void LogJson(TraceEventType eventType, object obj)
        {
            try
            {
                this.TraceEvent(eventType, JsonConvert.SerializeObject(obj));
            }
            catch (Exception ex)
            {
                TraceEvent(TraceEventType.Error, $"JSON SERIALIZATION FAILED: {ex.DetailedExceptionReportRecursive()}");
            }
        }

    }
}
