using System.Net.Http;
using System.Text;

namespace Dominion.Core.Dto.Http
{
    public class HttpRequestMessageDto
    {
        /// <summary>
        /// The referring URL.
        /// </summary>
        public string ReferrerUri { get; set; }

        /// <summary>
        /// The request URL.
        /// </summary>
        public string RequestUri { get; set; }

        /// <summary>
        /// The user agent data for the request.
        /// This tells us the browser and computer environment they're using.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public HttpRequestMessageDto()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="request">A HTTP request object.</param>
        public HttpRequestMessageDto(HttpRequestMessage request)
        {
            ReferrerUri = request.Headers.Referrer.ToString();
            RequestUri = request.RequestUri.ToString();
            UserAgent = request.Headers.UserAgent.ToString();
        }

        /// <summary>
        /// Method that builds the default message.
        /// </summary>
        /// <returns></returns>
        public string BuildMsg()
        {
            var sb = new StringBuilder()
                .AppendLine("Referrer Url: " + ReferrerUri)
                .AppendLine("Request Url: " + RequestUri)
                .AppendLine("User Agent: " + UserAgent);

            return sb.ToString();
        }
    }
}