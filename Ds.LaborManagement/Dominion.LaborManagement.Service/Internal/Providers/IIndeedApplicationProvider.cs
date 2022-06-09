using System;
using System.Collections;
using System.Collections.Generic;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Core;
using Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    /// <summary>
    /// Provides the functionality needed to allow Indeed's 'Indeed Apply' service to send us an application
    /// (an <see cref="IndeedApplication"/> object) and store it in our database in our own representation of an application
    /// (an <see cref="ApplicantApplicationHeader"/> and it's related entities)
    /// 
    /// 
    /// <see href="http://opensource.indeedeng.io/api-documentation/docs/xml-feed-ia/#adding-indeed-apply-ia"/>
    /// This link to their documentation contains all the details on what is found in an application from indeed 
    /// (Indeed Apply is Indeed's service that supplies us with the applications)
    /// </summary>

    public interface IIndeedApplicationProvider
    {
        /// <summary>
        /// Maps the provided <see cref="IndeedApplication"/> to our representation of an application which 
        /// includes an <see cref="ApplicantApplicationHeader"/>, <see cref="Applicant"/>,
        ///  <see cref="IEnumerable{ApplicantEmploymentHistory}"/>, <see cref="IEnumerable{ApplicantEducationHistory}"/>,
        /// and <see cref="IEnumerable{ApplicantLicense}"/>
        /// </summary>
        /// <param name="indeedApplication">The application received from Indeed</param>
        /// <param name="currentDateTime">The current date (this is made a parameter for testing purposes)</param>
        /// <param name="clientId">The clientId of the associated <see cref="ApplicantPosting"/> for the provided application</param>
        /// <param name="applicantId">The applicantId if already present else the param can be null</param>
        /// <param name="resume">The resource object of the resume if present else the param can be null</param>
        /// <param name="resources">The resources if present else the param can be null</param>
        /// <returns>An <see cref="IOpResult{ApplicantApplicationHeader}"/> which contains the application created in our database</returns>
        IOpResult<ApplicantApplicationHeader> SaveApplication(IndeedApplication indeedApplication, DateTime currentDateTime, int clientId, 
            int? applicantId = null, Resource applicantResume = null, IDictionary<string, Resource> resources = null);

        /// <summary>
        /// Creates a new User, a new Applicant and an application header for an application
        /// </summary>
        /// <param name="indeedApplication"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<ApplicantApplicationHeader> CreateUserAndAddApplication(IndeedApplication indeedApplication, DateTime currentDateTime, int clientId);

        /// <summary>
        /// Determines whether an <see cref="ExternalApplicationIdentity"/> with the same <see cref="ExternalApplicationIdentity.ExternalApplicationId"/> already exists in our database
        /// </summary>
        /// <param name="id">The id of the <see cref="ExternalApplicationIdentity"/> to search for</param>
        /// <returns>True if the application already exists, otherwise false.</returns>
        bool CheckForConflict(string id);

        /// <summary>
        /// Determines whether an <see cref="ApplicantPosting"/> exists in the db
        /// </summary>
        /// <param name="postingId">The id of the job post in the provided application</param>
        /// <returns>The clientId of the found <see cref="ApplicantPosting"/> if the post exists, otherwise null.
        /// 
        /// 
        /// The clientId is needed in the <see cref="SaveApplication"/> method of this class. So returning a nullable 
        /// int instead of a boolean of the clientId was an optimization decision that may avoid another call to the db.
        /// </returns>
        int? GetClientIdFromApplicantPost(int postingId);

        /// <summary>
        /// Attempts to get the phone numbers from the provided string.
        /// </summary>
        /// <param name="textWithPhoneNumbers">The text we want to try to get the phone number(s) from.  Each number 
        /// must have the same amount of digits. Non numeric characters (anything that's not 0-9) are ignored.</param>
        /// <returns>A collection of phone numbers found in the provided string.  Each string is a number with the correct 
        /// number of digits to make a phone number.</returns>
        IEnumerable<string> ParsePhoneNumber(string textWithPhoneNumbers);
        /// <summary>
        /// Retrive the Application Header already processed via an indeed Feed
        /// </summary>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        IOpResult<ApplicantApplicationHeader> GetProcessedApplication(string externalApplicationId);
    }
}