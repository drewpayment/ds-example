using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    /// <inheritdoc cref="IXMLGeneratorService"/>
    public class IndeedXMLGeneratorService : IXMLGeneratorService
    {
        private const string IndeedDatePattern = "ddd, dd MMM yyyy HH:mm:ss EDT";
        private const string RequiredString = "required";
        private const string OptionalString = "optional";
        private const string HiddenString = "hidden";

        public IndeedXMLGeneratorService()
        {
        }

        /// <inheritdoc cref="IXMLGeneratorService.BuildIndeedXML"/>
        public IOpResult<XDocument> BuildIndeedXML(IEnumerable<IndeedXmlJobPost> posts, string apiToken, DateTime currentDateTime)
        {
            var result = new OpResult<XDocument>();
            result.TryCatch(() =>
            {
                var baseUrl = TrimIt(ConfigurationManager.AppSettings["legacyRootUrl"]);
                var alternateUrl = TrimIt(ConfigurationManager.AppSettings["baseUrlWeWantPeopleToAccess"]);
                var root = new XElement("source", new XElement("publisher", TrimIt("Dominion ATS")));
                root.Add(new XElement("publisherurl", TrimIt("https://www.dominionsystems.com/applicant-tracking-system/")));
                root.Add(new XElement("lastBuildDate", TrimIt(currentDateTime.ToString(IndeedDatePattern))));
                var sb = new StringBuilder();
                foreach (var post in posts)
                {

                    var description = CreatePostingDescription(post, sb);
                    sb.Clear();

                    var jobUrl = sb.Append(alternateUrl).Append("/applicantPostingListNL.aspx?code=").Append(post.ClientCode).Append("&posting=").Append(post.ReferenceNumber)
                        .ToString();

                    var jobPostElement = new XElement("job");
                    jobPostElement.Add(new XElement("title", new XCData(TrimIt(post.Title))));
                    jobPostElement.Add(new XElement("date", new XCData(TrimIt(post.Date.ToString(IndeedDatePattern)))));
                    jobPostElement.Add(new XElement("referencenumber", new XCData(TrimIt(post.ReferenceNumber.ToString()))));
                    jobPostElement.Add(new XElement("url", new XCData(TrimIt(jobUrl))));
                    jobPostElement.Add(new XElement("company", new XCData(TrimIt(post.Company))));
                    jobPostElement.Add(new XElement("sourcename", new XCData(TrimIt(post.SourceName))));
                    jobPostElement.Add(new XElement("city", new XCData(TrimIt(post.City))));
                    jobPostElement.Add(new XElement("state", new XCData(TrimIt(post.State))));
                    jobPostElement.Add(new XElement("country", new XCData(TrimIt(post.Country))));
                    jobPostElement.Add(new XElement("postalcode", new XCData(TrimIt(post.PostalCode))));
                    jobPostElement.Add(new XElement("email", new XCData(TrimIt(post.ClientEmail))));
                    jobPostElement.Add(new XElement("description", new XCData(TrimIt(description.Replace("\n", "<br/>")))));
                    jobPostElement.Add(new XElement("salary", new XCData(TrimIt(post.Salary))));
                    if (post.Education != null) jobPostElement.Add(new XElement("education", new XCData(TrimIt(post.Education))));
                    jobPostElement.Add(new XElement("jobtype", new XCData(TrimIt(post.JobType))));
                    jobPostElement.Add(new XElement("category", new XCData(TrimIt(post.Category))));
                    jobPostElement.Add(new XElement("experience", new XCData(TrimIt(post.Experience == 0 ? "None" : post.Experience + "+ years"))));
                    sb.Clear();
                    sb.Append("indeed-apply-apiToken=").Append(apiToken).Append("&");
                    sb.Append("indeed-apply-jobTitle=").Append(HttpUtility.UrlEncode(post.Title)).Append("&");
                    sb.Append("indeed-apply-jobId=").Append(post.ReferenceNumber.ToString()).Append("&");
                    sb.Append("indeed-apply-jobCompanyName=").Append(HttpUtility.UrlEncode(post.Company)).Append("&");
                    sb.Append("indeed-apply-jobLocation=").Append(HttpUtility.UrlEncode(post.City)).Append(HttpUtility.UrlEncode(", ")).Append(post.State).Append("&");
                    sb.Append("indeed-apply-jobUrl=").Append(HttpUtility.UrlEncode(jobUrl)).Append("&");
                    sb.Append("indeed-apply-postUrl=").Append(HttpUtility.UrlEncode(baseUrl + "/api/indeedapplication")).Append("&");
                    sb.Append("indeed-apply-phone=").Append(RequiredString).Append("&");
                    sb.Append("indeed-apply-coverletter=").Append(OptionalString).Append("&");
                    sb.Append("indeed-apply-name=").Append("firstlastname").Append("&");
                    sb.Append("indeed-apply-questions=").Append(HttpUtility.UrlEncode(baseUrl + "/api/indeedapplication/application/" + post.ApplicationId + "/posting/" + post.ReferenceNumber)).Append("&");

                    var resumeRequired = "";
                    switch (post.ResumeRequiredId)
                    {
                        case 1:
                            resumeRequired = OptionalString;
                            break;
                        case 3:
                        case 4:
                            resumeRequired = RequiredString;
                            break;
                        default:
                            resumeRequired = HiddenString;
                            break;
                    }

                    sb.Append("indeed-apply-resume=").Append(resumeRequired);

                    jobPostElement.Add(new XElement("indeed-apply-data", new XCData(sb.ToString())));


                    root.Add(jobPostElement);
                    sb.Clear();
                }
                result.Data = new XDocument(
                    new XDeclaration("1.0", "utf-8", null),
                    root
                );
            });

            return result;
        }

        private string CreatePostingDescription(IndeedXmlJobPost post, StringBuilder sb)
        {
            sb.Append("<h3>About This Position</h3><br/>");
            sb.Append(post.Description);
            sb.Append("<hr />");
            if (post.Responsibilities.Any())
            {
                sb.Append(@"<div><h3>Responsibilities</h3><ul>");
                foreach (var responsibility in post.Responsibilities.OrderBy(x => x))
                {
                    sb.Append($"<li>{responsibility}</li>");
                }

                sb.Append(@"</ul><hr /></div>");
            }

            if (post.Skills.Any())
            {
                sb.Append("<div><h3>Skills</h3><ul>");
                foreach (var skill in post.Skills.OrderBy(x => x))
                {
                    sb.Append($"<li>{skill}</li>");
                }
                sb.Append("</ul><hr/></div>");
            }

            if (!string.IsNullOrWhiteSpace(post.WorkingConditions))
            {
                sb.Append("<div><h3>Working conditions</h3>");
                sb.Append($"<p>{post.WorkingConditions}</p>");
                sb.Append("<hr /></div>");
            }

            if (!string.IsNullOrWhiteSpace(post.Benefits))
            {
                sb.Append("<div><h3>Benefits</h3>");
                sb.Append($"<p>{post.Benefits}</p>");
                sb.Append("<hr /></div>");
            }

            var result = sb.ToString();
            return result;
        }

        private string TrimIt(string message)
        {
            return message.Trim();
        }
    }
}
