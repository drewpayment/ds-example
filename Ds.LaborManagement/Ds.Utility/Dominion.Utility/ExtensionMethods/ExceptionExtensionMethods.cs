using System;
using System.Collections.Generic;
using System.Text;

namespace Dominion.Utility.ExtensionMethods
{
    public static class ExceptionExtensionMethods
    {
        /// <summary>
        /// This method was created originally for returning as a message to the client when exceptions occur but then it was decided to use just the messages.
        /// This will be useful for a debugging or logging message in the future.
        /// A formatted message for an exception.
        /// Will include the inner exception if there is one to report on.
        /// Includes all stack trace information as well.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="sb">The string builder object. May be null if this is the originating call for this recursive method.</param>
        /// <returns></returns>
        public static string DetailedExceptionReportRecursive(this Exception ex, StringBuilder sb = null)
        {
            if (sb == null)
            {
                sb = new StringBuilder()
                    .AppendLine("--------------------------------------------------")
                    .AppendLine("EXCEPTION")
                    .AppendLine("--------------------------------------------------")
                    .AppendLine();
            }

            sb.AppendLine("ERROR:")
                .AppendLines(2)
                .AppendLine(ex.Message)
                .AppendLines(2)
                .AppendLine(ex.StackTrace);

            if (ex.InnerException != null)
            {
                sb.AppendLine("--------------------------------------------------")
                    .AppendLine("INNER EXCEPTION")
                    .AppendLine("--------------------------------------------------")
                    .AppendLine();

                ex.InnerException.DetailedExceptionReportRecursive(sb);
            }

            return sb.ToString();
        }

        /// <summary>
        /// A formatted message for an exception.
        /// Will include the inner exception if there is one to report on.
        /// This is just the messages from each exception layer.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="sb">The string builder object. May be null if this is the originating call for this recursive method.</param>
        /// <returns></returns>
        public static string ExceptionReport(this Exception ex, StringBuilder sb = null)
        {
            if (sb == null)
                sb = new StringBuilder();

            sb.AppendLine(ex.Message);

            if (ex.InnerException != null)
                ex.InnerException.ExceptionReport(sb);

            return sb.ToString();
        }

        /// <summary>
        /// Return a list of the exception and it's inner exceptions (extracted).
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static IEnumerable<Exception> SplitExceptions(this Exception ex)
        {
            var list = new List<Exception>();
            var curEx = ex;

            list.Add(ex);

            while (curEx.InnerException != null)
            {
                list.Add(curEx.InnerException);
                curEx = curEx.InnerException;
            }

            return list;
        }

    }
}