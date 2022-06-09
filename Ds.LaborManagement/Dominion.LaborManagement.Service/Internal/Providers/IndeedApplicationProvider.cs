using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dominion.Authentication.Interface.Api;
using Dominion.Authentication.Intermediate.Util;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Labor.Enum;
using Dominion.Core.Dto.Location;
using Dominion.Core.Services.Api.Dto;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Internal.Providers.Resources;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Misc;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply;
using Dominion.LaborManagement.Dto.Enums;
using Dominion.Utility.Configs;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query;
using Dominion.Utility.Web;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    /// <inheritdoc cref="IIndeedApplicationProvider"/>
    public class IndeedApplicationProvider : IIndeedApplicationProvider
    {
        private readonly IBusinessApiSession _session;
        private readonly IApplicantTrackingProvider _appTrackProvider;
        private readonly IAzureResourceProvider _azureResourceProvider;

        private const int UnitedStatesId = 1; // 1 is the id for the US entry in the 'Country' table
        private const int MichiganId = 1; // 1 is the id for the Michigan entry in the 'State' table
        private const int SevenDigitNumber = 7; // ex: 225-5612
        private const int TenDigitNumber = 10; // ex: (443) 225-5612
        private const int ElevenDigitNumber = 11; // ex: 1 (443) 225-5612
        private readonly IIndeedApplicationProvider Self;
        private readonly ILoginService _loginService;
        public static Func<string, string> MakeElevenDigitNumberPretty = (string unformatted) => 
        $"{unformatted.Substring(0, 1)} ({unformatted.Substring(1, 3)}) {unformatted.Substring(4, 3)}-{unformatted.Substring(7)}";

        public static Func<string, string> MakeTenDigitNumberPretty = (string unformatted) => 
        $"({unformatted.Substring(0, 3)}) {unformatted.Substring(3, 3)}-{unformatted.Substring(6)}";

        public static Func<string, string> MakeSevenDigitNumberPretty = (string unformatted) =>
        $"{unformatted.Substring(0, 3)}-{unformatted.Substring(3)}";


        /// Indeed does not require users to provide a value for some fields in their application
        /// When an applicant does not provide a value their documentation says that -1 will be provided as the value for the field
        /// (in their example it looks like they leave out the property entirely)

        private const int NotProvidedValue = -1;

        public IndeedApplicationProvider(ILoginService loginService, IBusinessApiSession session, IApplicantTrackingProvider appTrackProvider, IAzureResourceProvider azureResourceProvider)
        {
            _loginService = loginService;
            _session = session;
            _appTrackProvider = appTrackProvider;
            _azureResourceProvider = azureResourceProvider;

            Self = this;
        }

        /// <inheritdoc cref="IIndeedApplicationProvider.SaveApplication"/>
        IOpResult<ApplicantApplicationHeader> IIndeedApplicationProvider.SaveApplication(IndeedApplication indeedApplication, DateTime currentDateTime, int clientId, 
            int? applicantId, Resource resumeInst, IDictionary<string,Resource> resourceInsts)
        {
            var applicantToCreate = applicantId.HasValue ? 
                _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery().ByApplicantId(applicantId.Value).FirstOrDefault() : 
                new Applicant() ;

            var header = applicantToCreate.ApplicantApplicationHeaders.FirstOrDefault() ?? new ApplicantApplicationHeader()
            {
                Applicant = applicantToCreate
            };

            var result = new OpResult<ApplicantApplicationHeader>()
            {
                Data = header
            };

            var indeedApplicant = indeedApplication.Applicant;
            var splitFullName = indeedApplicant.FullName.Split(' ');

            var phoneNumber = indeedApplicant.PhoneNumber;

            var resume = indeedApplication.Applicant.Resume;

            string firstName;
            string lastName;
            string city = null;


            if (resume?.Json != null)
            {
                //Get the data from Indeed's detailed resume
                string stateAbbr;
                var json = resume.Json;


                firstName = json.FirstName;
                lastName = json.LastName;
                if (json.PhoneNumber.IsNotNullOrEmpty())
                {
                    phoneNumber = json.PhoneNumber;
                }

                if (json.Location != null)
                {
                    var country = _session.UnitOfWork.MiscRepository
                        .GetCountryList(
                            new QueryBuilder<Country>().FilterBy(x =>
                                x.Abbreviation == json.Location.Country)).FirstOrDefault();

                    applicantToCreate.CountryId = country?.CountryId ?? 1;

                    var indeedCity = json.Location.City ?? "";
                    var splitCity = indeedCity.Split(',');
                    city = splitCity.First();
                    stateAbbr = splitCity.Last().Trim();

                    // Make sure we aren't trying to get the state from something like 'jkjl, '
                    if (splitCity.Length == 2 && stateAbbr.IsNotNullOrEmpty())
                    {
                        var state = _session.UnitOfWork.MiscRepository.GetStateList(
                                (QueryBuilderAutoMap<State, StateNameIdDto>) new QueryBuilderAutoMap<State, StateNameIdDto>()
                                    .FilterBy(
                                        x => x.Abbreviation == stateAbbr &&
                                             x.CountryId == applicantToCreate.CountryId))
                            .FirstOrDefault();
                        if (state != null)
                        {
                            applicantToCreate.State = state.StateId;
                        }
                    }

                    applicantToCreate.Zip = json.Location.PostalCode;
                }

                #region Employment History

                applicantToCreate.ApplicantEmploymentHistories = new List<ApplicantEmploymentHistory>();

                if (json.Positions.Values != null)
                {
                var i = 0;

                // Initially contains all of the previous employment history
                var originalEmpHist = new Dictionary<string, List<ApplicantEmploymentHistoryWithStateDto>>();

                // Contains all employment history removed from the originalEmpHist in case we need to access those items again
                var removedEmpsHist = new Dictionary<string, List<ApplicantEmploymentHistoryWithStateDto>>();
                var stateAbbrevs = new List<string>();
                foreach (var position in json.Positions.Values)
                {
                    var location = position.Location.Split(',');
                    var posStateAbbr = location.Last().Trim().ToLower();
                    if (location.Length == 2 && posStateAbbr.IsNotNullOrEmpty())
                    {
                        stateAbbrevs.Add(posStateAbbr);
                    }


                    var startMonth = position.StartDateMonth > NotProvidedValue
                        ? position.StartDateMonth.Value.ToString("00")
                        : "01";

                    var endMonth = position.EndDateMonth > NotProvidedValue
                        ? position.EndDateMonth.Value.ToString("00")
                        : "01";

                    string endDate;
                    if (position.EndCurrent)
                    {
                        endDate = "Present";
                    }
                    else
                    {
                        endDate = position.EndDateYear.HasValue && position.EndDateYear.GetValueOrDefault() > NotProvidedValue
                            ? endMonth + "-" + position.EndDateYear
                            : null;
                    }

                    var employmentHistory = new ApplicantEmploymentHistoryWithStateDto()
                    {
                        ApplicantEmploymentId = i,
                        Company = position.Company,
                        Title = position.Title,
                        Responsibilities = position.Description,
                        StartDate = position.StartDateYear.HasValue && position.StartDateYear.GetValueOrDefault() > NotProvidedValue ? startMonth + "-" + position.StartDateYear : null,
                        EndDate = endDate,
                        City = position.Location.Split(',').First(),
                        State = new StateDto()
                        {
                            Abbreviation = posStateAbbr
                        }
                    };

                    if (originalEmpHist.ContainsKey(posStateAbbr))
                    {
                        originalEmpHist[posStateAbbr].Add(employmentHistory);
                    }
                    else
                    {
                        originalEmpHist.Add(posStateAbbr, new List<ApplicantEmploymentHistoryWithStateDto>() { employmentHistory });
                    }

                    i++;
                }

                if (stateAbbrevs.Any())
                {
                    // Set the state ids of the employment histories
                    var states = _session.UnitOfWork.LocationRepository.StateQuery().ByAbbreviation(stateAbbrevs)
                        .ExecuteQuery();

                    foreach (var state in states)
                    {
                        var abbreviation = state.Abbreviation.Trim().ToLower();
                        if (originalEmpHist.ContainsKey(abbreviation))
                        {
                            AssignCountryAndState(originalEmpHist[abbreviation], state);

                            removedEmpsHist.Add(abbreviation, originalEmpHist[abbreviation]);
                            originalEmpHist.Remove(abbreviation);
                        }
                        else if (state.CountryId == UnitedStatesId)
                        { 
                          // some states that are not in the US share the same abbreviation of one of the states in the US
                          // when we find one of these, assume they meant the US state
                            AssignCountryAndState(removedEmpsHist[abbreviation], state);
                        }
                    }
                }

                AddHistoriesToOpResult(result, removedEmpsHist);
                AddHistoriesToOpResult(result, originalEmpHist);

                }

                #endregion

                #region Education History

                applicantToCreate.ApplicantEducationHistories = new List<ApplicantEducationHistory>();

                if (json.Educations.Values != null)
                {

                foreach (var education in json.Educations.Values)
                {
                    var haveStartDate = education.StartDateYear.HasValue && education.StartDateYear > NotProvidedValue;
                    var haveEndDate = education.EndDateYear.HasValue && education.EndDateYear > NotProvidedValue;
                    int? yearsCompleted = null;

                    if (!haveStartDate)
                    {
                        // need to have a start year to try to do any sort calculation of the number of years completed
                    }
                    else if (education.EndCurrent)
                    {
                        // still working on degree
                        // the school year for many schools starts during the fall of one year and end around May/June of the next year
                        yearsCompleted = currentDateTime.Year - education.StartDateYear;
                        yearsCompleted = currentDateTime.Month < 5 ? yearsCompleted - 1 : yearsCompleted;
                    } else if (currentDateTime.Year <= education.StartDateYear || currentDateTime.Year < education.EndDateYear)
                    {
                        // applicant hasn't started this education yet or started to work on it and then stopped
                        yearsCompleted = 0;

                    }
                    else if(haveEndDate)
                    {
                        // no longer working towards a degree, they probably finished it
                        yearsCompleted = education.EndDateYear - education.StartDateYear;
                    }

                    if (yearsCompleted < 0) yearsCompleted = 0;

                    var hasDegree = !education.EndCurrent && education.Degree.IsNotNullOrEmpty() && (yearsCompleted > 0 || !(haveStartDate || haveEndDate)) ? HasDegreeType.Yes :
                        education.EndCurrent ? HasDegreeType.InProgress : HasDegreeType.No;

                    var educationHistory = new ApplicantEducationHistory
                    {
                        Description = education.School,
                        Applicant = applicantToCreate,
                        IsEnabled = true,
                        DateStarted =
                            haveStartDate ? new DateTime(education.StartDateYear.Value, 1, 1) : (DateTime?) null,
                        DateEnded = haveEndDate ? new DateTime(education.EndDateYear.Value, 1, 1) : (DateTime?) null,
                        ExternalDegree = education.Degree,
                        HasDegree = hasDegree,
                        YearsCompleted = yearsCompleted,
                        Studied = education.Field
                    };

                    applicantToCreate.ApplicantEducationHistories.Add(educationHistory);
                    _session.UnitOfWork.RegisterNew(educationHistory);
                    }

                }

                #endregion

                #region Professional Licensing
                applicantToCreate.ApplicantLicenses = new List<ApplicantLicense>();
                if (json.Certifications.Values != null)
                {

                foreach (var cert in json.Certifications.Values)
                {

                    var endDate = cert.EndDateYear > NotProvidedValue && cert.EndDateMonth > NotProvidedValue
                        ? new DateTime(cert.EndDateYear.Value, cert.EndDateMonth.Value, 1)
                        : (DateTime?) null;

                    var startDate = cert.StartDateYear > NotProvidedValue && cert.StartDateMonth > NotProvidedValue
                        ? new DateTime(cert.StartDateYear.Value, cert.StartDateMonth.Value, 1)
                        : (DateTime?) null;

                    var license = new ApplicantLicense()
                    {
                        CountryId = UnitedStatesId,
                        IsEnabled = true,
                        RegistrationNumber = null,
                        StateId = null,
                        Type = cert.Title,
                        ValidFrom = startDate,
                        ValidTo = endDate,
                        Description = cert.Description
                    };

                    applicantToCreate.ApplicantLicenses.Add(license);
                    _session.UnitOfWork.RegisterNew(license);
                    }

                }

                #endregion

            }
            else
            {
                // Get the data from the standard resume
                firstName = splitFullName[0];
                lastName = "";
                int i;
                for (i = 1; i < splitFullName.Length; i++)
                {
                    if (i == 1)
                    {
                        lastName += splitFullName[i];
                    }
                    else
                    {
                        lastName += " " + splitFullName[i];
                    }

                }
            }

            #region Resume Doc
            if (resume?.File != null && resumeInst != null)
            {
                var applicantResume = new ApplicantResume()
                {
                    ApplicantId = applicantId.Value,
                    Applicant = applicantToCreate,
                    LinkLocation = resumeInst.Source,
                    DateAdded = resumeInst.Modified
                };
                _session.UnitOfWork.RegisterNew(applicantResume);

                result.Data.ApplicantResume = applicantResume;
            }
            #endregion

            #region Answers

            result.Data.ApplicantApplicationDetail = new List<ApplicantApplicationDetail>();

            if (indeedApplication.Questions != null)
            {
                // separate indeed file questions
                List<string> fileQuestionIds = new List<string>();
                if(indeedApplication.Questions.Questions!=null && indeedApplication.Questions.Questions.Count()>0)
                    fileQuestionIds = indeedApplication.Questions.Questions.Where(x => x.Type == "file").Select(y => y.Id).ToList();

                var answerDictionary = indeedApplication.Questions.Answers
                    .Where(x => !(x.GetValues().Count() == 1 && string.IsNullOrWhiteSpace(x.GetValues().FirstOrDefault())))
                    .Where(x => x.GetValues().Count() > 0)
                    .Select(answer => new ApplicantApplicationDetail()
                    {
                        QuestionId = int.Parse(string.IsNullOrEmpty(answer.Id) ? "0" : answer.Id),
                        Response = (!fileQuestionIds.Contains(answer.Id)) ? answer.GetValues().Aggregate((a, b) => a + "," + b) : "",
                    })
                    .ToDictionary(detail => detail.QuestionId.Value);

                // only include answers for questions that have been activated for the application associated with this applicant posting
                var questions = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(int.Parse(string.IsNullOrEmpty(indeedApplication.Job.JobId) ? "0" : indeedApplication.Job.JobId))
                    .ExecuteQueryAs(x => x.ApplicantCompanyApplication.ApplicantQuestionSets.Select(y => y.Question))
                    .SelectMany(z => z);

                foreach (var question in questions)
                {
                    answerDictionary.TryGetValue(question.QuestionId, out var answer);
                    if (answer == null) continue;

                    // Make sure to only include answers for questions that have not been deleted
                    // This might happen when an applicant submits an application before Indeed updates a modified job post
                    if (fileQuestionIds.Contains(question.QuestionId.ToString()))
                        answer.IsFlagged = question.IsFlagged && !resourceInsts.ContainsKey(question.QuestionId.ToString());
                    else
                        answer.IsFlagged = question.IsFlagged &&
                                       string.Compare(question.FlaggedResponse, answer.Response,
                                           StringComparison.Ordinal) == 0;
                    answer.SectionId = question.SectionId;
                    answer.ApplicantApplicationHeader = result.Data;

                    _session.UnitOfWork.RegisterNew(answer);
                    result.Data.ApplicantApplicationDetail.Add(answer);
                }
            }
            

            #endregion

            result.Data.ExternalApplicationIdentity = new ExternalApplicationIdentity()
            {
                ApplicantJobSiteId = ApplicantJobSiteEnum.Indeed,
                ExternalApplicationId = indeedApplication.Id,
                Timestamp = DateTime.Now,
                ApplicantApplicationHeader = result.Data,
            };
            _session.UnitOfWork.RegisterNew(result.Data.ExternalApplicationIdentity);
            if (indeedApplication.Applicant.Coverletter.IsNotNullOrEmpty())
            {
                result.Data.CoverLetter = indeedApplication.Applicant.Coverletter;
            }

            if (!applicantId.HasValue)
            {
                applicantToCreate.FirstName = firstName;
                applicantToCreate.LastName = lastName;
                applicantToCreate.EmailAddress = indeedApplicant.Email;
                applicantToCreate.City = city ?? "";
                var numberLength = findExpectedPhoneNumberLength(phoneNumber);
                Func<string, string> formatter = (number) => number;
                if (numberLength == SevenDigitNumber)
                {
                    formatter = MakeSevenDigitNumberPretty;
                }
                else if (numberLength == TenDigitNumber)
                {
                    formatter = MakeTenDigitNumberPretty;
                }
                else if (numberLength == ElevenDigitNumber)
                {
                    formatter = MakeElevenDigitNumberPretty;
                }
                applicantToCreate.PhoneNumber = Self.ParsePhoneNumber(phoneNumber).Select(formatter).FirstOrDefault();
                if (!applicantToCreate.CountryId.HasValue)
                {
                    applicantToCreate.CountryId = 1;
                }
            }

            result.Data.PostingId = int.Parse( string.IsNullOrEmpty(indeedApplication.Job.JobId)? "0": indeedApplication.Job.JobId);
            applicantToCreate.ClientId = clientId;
            result.Data.IsApplicationCompleted = true;
            result.Data.IsRecommendInterview = true;
            result.Data.DateSubmitted = DateTime.Now;
            result.Data.OrigPostingId = int.Parse(string.IsNullOrEmpty(indeedApplication.Job.JobId) ? "0" : indeedApplication.Job.JobId);
            result.Data.ApplicantStatusTypeId = ApplicantStatusType.Applicant;

            if(applicantId.HasValue)    _session.UnitOfWork.RegisterModified(applicantToCreate);
            else                        _session.UnitOfWork.RegisterNew(applicantToCreate);

            _session.UnitOfWork.Commit().MergeInto(result);

            // There is no reference between resources and application details. For saving the resource Ids to ApplicationDetails, RECOMMIT!!!
            if (applicantId.HasValue)
            {
                // separate indeed file questions
                List<string> fileQuestionIds = new List<string>();
                if (indeedApplication.Questions.Questions != null && indeedApplication.Questions.Questions.Count() > 0)
                    fileQuestionIds = indeedApplication.Questions.Questions.Where(x => x.Type == "file").Select(y => y.Id).ToList();
                IEnumerable<ApplicantApplicationDetail> tmp = result.Data.ApplicantApplicationDetail.Where(x => fileQuestionIds.Contains(x.QuestionId.ToString()));

                foreach (ApplicantApplicationDetail detail in tmp)
                {
                    detail.Response = resourceInsts[detail.QuestionId.ToString()].ResourceId.ToString();
                    _session.UnitOfWork.RegisterModified(detail);
                }
                _session.UnitOfWork.Commit().MergeInto(result);
            }

            return result;
        }

        IOpResult<ApplicantApplicationHeader> IIndeedApplicationProvider.CreateUserAndAddApplication(IndeedApplication indeedApplication, DateTime currentDateTime, int clientId)
        {
            IOpResult<ApplicantApplicationHeader> result = new OpResult<ApplicantApplicationHeader>();

            // Validate Application
            if (Self.CheckForConflict(indeedApplication.Id))
                return Self.GetProcessedApplication(indeedApplication.Id);

            var indeedApplicant = indeedApplication.Applicant;
            AddApplicantDto dto = new AddApplicantDto();
            var splitFullName   = indeedApplicant.FullName.Split(' ');
            dto.Phone           = indeedApplicant.PhoneNumber ?? "";
            dto.Email           = indeedApplicant.Email;
            dto.ClientId        = clientId;
            dto.PostingId       = int.Parse(string.IsNullOrEmpty(indeedApplication.Job.JobId) ? "0" : indeedApplication.Job.JobId);
            dto.Address1        = "";
            dto.Address2        = "";
            dto.City            = "";
            dto.ZipCode         = "";
            dto.CellPhone       = "";
            dto.WorkPhone       = "";
            dto.Extension       = "";
            dto.CountryId       = 1;
            dto.State           = 1;
            dto.PostLink        = "";

            var resume = indeedApplication.Applicant.Resume;

            if (resume?.Json != null)
            {
                //Get the data from Indeed's detailed resume
                string stateAbbr;
                var json = resume.Json;


                dto.FirstName = json.FirstName;
                dto.LastName = json.LastName;
                if (json.PhoneNumber.IsNotNullOrEmpty())
                {
                    dto.Phone = json.PhoneNumber;
                }

                if (json.Location != null)
                {
                    var country = _session.UnitOfWork.MiscRepository
                        .GetCountryList(
                            new QueryBuilder<Country>().FilterBy(x =>
                                x.Abbreviation == json.Location.Country)).FirstOrDefault();

                    dto.CountryId = country?.CountryId ?? 1;

                    var indeedCity = json.Location.City ?? "";
                    var splitCity = indeedCity.Split(',');
                    dto.City = splitCity.First();
                    stateAbbr = splitCity.Last().Trim();

                    // Make sure we aren't trying to get the state from something like 'jkjl, '
                    if (splitCity.Length == 2 && stateAbbr.IsNotNullOrEmpty())
                    {
                        var state = _session.UnitOfWork.MiscRepository.GetStateList(
                                (QueryBuilderAutoMap<State, StateNameIdDto>)new QueryBuilderAutoMap<State, StateNameIdDto>()
                                    .FilterBy(
                                        x => x.Abbreviation == stateAbbr &&
                                             x.CountryId == dto.CountryId))
                            .FirstOrDefault();
                        if (state != null)
                        {
                            dto.State = state.StateId;
                        }
                    }

                    dto.ZipCode = json.Location.PostalCode;
                }
            }
            else
            {
                // Get the data from the standard resume
                dto.FirstName = splitFullName[0];
                dto.LastName = "";
                int i;
                for (i = 1; i < splitFullName.Length; i++)
                {
                    if (i == 1)
                        dto.LastName += splitFullName[i];
                    else
                        dto.LastName += " " + splitFullName[i];
                }
            }

            // Set up user name and password
            dto.UserName = GetNewUserName(dto.FirstName+dto.LastName);
            dto.Password = "123";

            // VALIDATION : check resource files compatibility
            //----------------------------------------------------------------------------------
            FileStreamDto resumeStream = null;
            List<FileStreamDto> resources = new List<FileStreamDto>();

            try
            {
                if (resume?.File != null)
                {
                    resumeStream = new FileStreamDto();
                    resumeStream.FileName = resume.File.FileName;
                    resumeStream.FileExtension = resume.File.FileName.Substring(resume.File.FileName.LastIndexOf('.') + 1);
                    resumeStream.MimeType = resume.File.ContentType;
                    resumeStream.FileStream = new MemoryStream(Convert.FromBase64String(resume.File.Data));
                }
                
                List<string> fileQuestions = indeedApplication.Questions.Questions.Where(x => x.Type == "file").Select(x=>x.Id).ToList();
                foreach ( IndeedAnswer answer in indeedApplication.Questions.Answers.Where( x => fileQuestions.Contains(x.Id)) )
                {
                    // No multiple file upload
                    if(answer.Files?.Count() > 0)
                    {
                        FileStreamDto streamDto = new FileStreamDto();
                        streamDto.Id = answer.Id; // Id of the Question for which applicant has answered
                        streamDto.FileName = answer.Files.First().FileName;
                        streamDto.FileExtension = answer.Files.First().FileName.Substring(answer.Files.First().FileName.LastIndexOf('.') + 1);
                        streamDto.MimeType = answer.Files.First().ContentType;
                        streamDto.FileStream = new MemoryStream(Convert.FromBase64String(answer.Files.First().Data));
                        resources.Add(streamDto);
                    }
                }
            }
            catch(Exception ex)
            {
                return result.SetToFail("Unable to retrieve files uploaded.");
            }

            // step 1 : Setup a New User & Applicant.
            IOpResult<AddApplicantDto> opResult = _appTrackProvider.AddNewUser(dto);
            if (opResult.HasError)
            {
                // Disable Applicant User
                DisableApplicantUser(dto.ApplicantId, null, null);
                return result.SetToFail("Unable to add user.");
            }

            // step 2 : Save resume and files in to resources.
            dto = opResult.Data;
            
            Resource resourceTemp = null;
            IDictionary<string,Resource> savedResources = new Dictionary<string,Resource>();
            Resource savedResume = null;
            if (resumeStream != null)
            {
                long totalBytes = ((MemoryStream)resumeStream.FileStream).Length;
                byte[] byteArray = new byte[totalBytes];
                int count = resumeStream.FileStream.Read(byteArray, 0, 20);
                while (count < resumeStream.FileStream.Length) byteArray[count++] = Convert.ToByte(resumeStream.FileStream.ReadByte());

                resourceTemp = SaveApplicantResource(dto.ApplicantId, resumeStream.FileName, byteArray);
                if (resourceTemp == null)
                {
                    // Disable Applicant User
                    DisableApplicantUser(dto.ApplicantId, savedResume, null);
                    return result.SetToFail("Unable to save resume.");
                }

                savedResume = resourceTemp;
            }
            
            if (resources.Count > 0)
            {
                bool resourceSaveFailure = false;
                foreach(FileStreamDto resxStream in resources)
                {
                    long totalBytes = ((MemoryStream)resxStream.FileStream).Length;
                    byte[] byteArray = new byte[totalBytes];
                    int count = resxStream.FileStream.Read(byteArray, 0, 20);
                    while (count < resxStream.FileStream.Length) byteArray[count++] = Convert.ToByte(resxStream.FileStream.ReadByte());

                    resourceTemp = SaveApplicantResource(dto.ApplicantId, resxStream.FileName, byteArray);
                    if (resourceTemp == null)
                    {
                        resourceSaveFailure = true;
                        break;
                    }

                    savedResources.Add(resxStream.Id, resourceTemp);
                }

                if (resourceSaveFailure)
                {
                    // Disable Applicant User
                    DisableApplicantUser(dto.ApplicantId, savedResume, savedResources.Values.ToList());
                    return result.SetToFail("Unable to save applicant files.");
                }
            }
            
            // step 3 : Add ApplicantApplicationHeader along with resume and files
            Self.SaveApplication(indeedApplication, DateTime.Now, clientId, dto.ApplicantId, savedResume, savedResources).MergeAll(result);

            return result;
        }

        private Resource SaveApplicantResource(int applicantId, string fileName, byte[] fileData)
        {
            string resourceLocation = string.Empty;
            ResourceSourceType resourceType = ResourceSourceType.AzureClientFile;
            int azureAccountEnum = 0;
            int clientId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery().ByApplicantId(applicantId)
                .FirstOrDefault().ClientId;

            if (!string.IsNullOrEmpty(ConfigValues.AzureFile))
            {
                if(_azureResourceProvider == null)
                {
                    throw new Exception("service was null");
                }
                var middleStuff = _azureResourceProvider.CreateOrUpdateApplicantAzureFile(clientId, applicantId, fileName, fileData,
                    ConfigValues.AzureFile, true);
                if(middleStuff == null)
                {
                    throw new Exception("middle stuff broke");
                }
                resourceLocation = middleStuff.Data;
                azureAccountEnum = _azureResourceProvider.GetAzureAccountNum(AzureDirectoryType.ApplicantFile);

                if (string.IsNullOrEmpty(resourceLocation))
                {
                    throw new Exception("Azure file failed to create.");
                }
            }
            else
            {
                string filePath = string.Format(@"C:\Upload\{0}\ApplicantResources\{1}\{2}", clientId, applicantId, fileName);
                var fileInfo = new FileInfo(filePath);
                fileInfo.Directory.Create();
                File.WriteAllBytes(filePath, fileData);
                resourceLocation = filePath;
                resourceType = ResourceSourceType.LocalServerFile;
            }

            int userId = _session.LoggedInUserInformation.UserId < 0 ? 0 : _session.LoggedInUserInformation.UserId;
            //create resource and register it
            var resource = new Resource
            {
                ClientId = clientId,
                EmployeeId = null,
                Name = fileName.Trim(),
                SourceType = resourceType,
                Source = resourceLocation,
                AddedById = userId,
                AddedDate = DateTime.Now,
                AzureAccount = azureAccountEnum,
                Modified = DateTime.Now,
                ModifiedBy = userId,
            };

            _session.UnitOfWork.RegisterNew(resource);
            return resource;
        }

        private bool DeleteApplicantResource(Resource resource, int applicantId)
        {
            
            if (resource.Source.Contains(@"C:\"))
            {
                var fileName = Path.GetFileName(resource.Source);
                string ext = Path.GetExtension(resource.Source);
                string mimeType = MimeTypeMap.GetMimeType(ext);
                if (File.Exists(resource.Source))
                {
                    File.Delete(resource.Source);
                }
                else
                {
                    throw new Exception("Unable to find specified resource.");
                }
            }
            else
            {
                IOpResult azureResult = _azureResourceProvider.DeleteBlob(resource.ClientId.Value, applicantId, ConfigValues.AzureFile, resource.Name, AzureDirectoryType.ApplicantFile, resource.Source);
                if (!azureResult.Success)
                {
                    throw new Exception("Unable to find specified resource.");
                }
            }
            _session.UnitOfWork.RegisterDeleted(resource);

            return true;
        }

        private void DisableApplicantUser(int applicantId, Resource savedResume, List<Resource> resources)
        {
            // Remove Resume
            if( savedResume != null )
                DeleteApplicantResource(savedResume, applicantId);

            // Remove Resources
            if (resources != null && resources.Count > 0)
            {
                foreach(Resource dto in resources)
                    DeleteApplicantResource(dto, applicantId);
            }

            // Disable Applicant and User information
            var applicant = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery().ByApplicantId(applicantId).FirstOrDefault();
            applicant.IsDenied = true;
            _session.UnitOfWork.RegisterModified(applicant);

            var usr = _session.UnitOfWork.UserRepository.QueryUsers().ByUserId(applicant.UserId.Value).FirstOrDefault();
            usr.IsUserDisabled = true;
            _session.UnitOfWork.RegisterModified(usr);
            _session.UnitOfWork.Commit();
        }

        private string GetNewUserName(string str)
        {
            str = str.Substring(0, str.Length > 14 ? 14 : str.Length);
            int inc = 1;
            while (inc < 100)
            {
                bool notPresentInAuthDb = _loginService.CheckUserName(str).Success;
                bool notPresentInBizDb = _session.UnitOfWork.UserRepository.GetUserByUserName(str) == null;
                if (notPresentInBizDb && notPresentInAuthDb)
                {
                    return str;
                }
                else
                {
                    // generate a random username ( FirstName + LastName + 1-9 )
                    var digitLength = Math.Floor(Math.Log10(inc) + 1);
                    if(digitLength + str.Length > 14)
                    {
                        var amountToChopOff = (digitLength + str.Length) - 14;

                        str = str.Substring(0, Convert.ToInt32(str.Length - amountToChopOff));
                        
                    }
                    str += inc.ToString();
                    //if (inc == 1) str = (str.Length > 14) ? (str.Substring(0, 14) + inc) : (str + inc);
                    //else str = str.Substring(0, str.Length - 1) + inc;

                    inc++;
                }
            }
            throw new Exception("Plenty of users with same name exists!");
        }

        /// <param name="result">The <see cref="OpResult{ApplicantApplicationHeader}"/> to add employment histories to</param>
        /// <param name="histories">The <see cref="Dictionary{string,List{ApplicantEmploymentHistoryWithStateDto}}" />
        ///     with the employment histories to add to the result</param>
        private void AddHistoriesToOpResult(IOpResult<ApplicantApplicationHeader> result,
            Dictionary<string, List<ApplicantEmploymentHistoryWithStateDto>> histories)
        {
            foreach (var empHistoryList in histories.Values)
            {
                foreach (var empDto in empHistoryList)
                {
                    var empHistory = new ApplicantEmploymentHistory()
                    {
                        IsEnabled = true,
                        Applicant = result.Data.Applicant,
                        Company = EnforceMaxLength(empDto.Company, 300),
                        City = EnforceMaxLength(empDto.City, 50),
                        Title = EnforceMaxLength(empDto.Title, 300),
                        Responsibilities = empDto.Responsibilities,
                        StartDate = empDto.StartDate,
                        EndDate = empDto.EndDate,
                        StateId = empDto.StateId != 0 ? empDto.StateId : MichiganId,
                        CountryId = empDto.CountryId != 0 ? empDto.CountryId : UnitedStatesId,
                        IsContactEmployer = true,
                        IsVoluntaryResign = true
                    };

                    result.Data.Applicant.ApplicantEmploymentHistories.Add(empHistory);
                    _session.UnitOfWork.RegisterNew(empHistory);
                }
            }
        }

        ///<summary>
        /// Assigns the country and state to each dto in <paramref name="dtos"/> using the values found in <paramref name="state"/>
        /// </summary>
        /// <param name="dtos">The collection of dtos to iterate through</param>
        /// <param name="state">The state to assign to each dto</param>
        private static void AssignCountryAndState(IEnumerable<ApplicantEmploymentHistoryWithStateDto> dtos, State state)
        {
            foreach (var dto in dtos)
            {
                dto.StateId = state.StateId;
                dto.CountryId = state.CountryId;
            }
        }

        /// <inheritdoc cref="IIndeedApplicationProvider.ParsePhoneNumber(string)"/>
        IEnumerable<string> IIndeedApplicationProvider.ParsePhoneNumber(string textWithPhoneNumbers)
        {

            var result = new List<string>();
            if (textWithPhoneNumbers.IsNullOrEmpty())
            {
                return result;
            }

            var cleansedPhoneNumbers = Regex.Replace(textWithPhoneNumbers, "[^0-9]", "", RegexOptions.None);

            var numberLength = this.findExpectedPhoneNumberLength(cleansedPhoneNumbers);

            var isValidLength = numberLength.HasValue;

            if (!isValidLength)
            {
                return result;
            }

            while(cleansedPhoneNumbers.Length > 0)
            {
                var localNumber = cleansedPhoneNumbers.Substring(0, numberLength.Value);
                result.Add(localNumber);
                cleansedPhoneNumbers = cleansedPhoneNumbers.Substring(numberLength.Value);
            }

            return result;
        }

        private int? findExpectedPhoneNumberLength(string phoneNumbers)
        {
            if (phoneNumbers.IsNullOrEmpty())
            {
                return null;
            }

            var cleansedPhoneNumbers = Regex.Replace(phoneNumbers, "[^0-9]", "", RegexOptions.None);

            var isSevenDigitNumbers = cleansedPhoneNumbers.Length % SevenDigitNumber == 0;
            var isTenDigitNumbers = cleansedPhoneNumbers.Length % TenDigitNumber == 0;
            var isElevenDigitNumbers = cleansedPhoneNumbers.Length % ElevenDigitNumber == 0;

            if (isSevenDigitNumbers)
            {
                return SevenDigitNumber;
            } else if (isTenDigitNumbers)
            {
                return TenDigitNumber;
            } else if (isElevenDigitNumbers)
            {
                return ElevenDigitNumber;
            } else
            {
                return null;
            }
        }

        /// <inheritdoc cref="IIndeedApplicationProvider.CheckForConflict"/>
        bool IIndeedApplicationProvider.CheckForConflict(string id)
        {
            return null != _session.UnitOfWork.ApplicantTrackingRepository.ExternalApplicationIdentityQuery()
                       .ByExternalApplicationId(id)
                       .ExecuteQueryAs<int?>(x => x.ApplicantApplicationHeaderId)
                       .FirstOrDefault();
        }

        IOpResult<ApplicantApplicationHeader> IIndeedApplicationProvider.GetProcessedApplication(string externalApplicationId)
        {
            IOpResult<ApplicantApplicationHeader> result = new OpResult<ApplicantApplicationHeader>();
            result.TrySetData( () => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                       .ByExternalApplicationId(externalApplicationId)
                       .ExecuteQuery()
                       .FirstOrDefault());
            return result;
        }

        /// <inheritdoc cref="IIndeedApplicationProvider.GetClientIdFromApplicantPost"/>
        int? IIndeedApplicationProvider.GetClientIdFromApplicantPost(int postingId)
        {
            return _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(postingId)
                .ByIsActive(true)
                .ByIsClosed(false)
                .ExecuteQueryAs<int?>(x => x.ClientId)
                .FirstOrDefault();
        }

        private string EnforceMaxLength(string input, int maxLength)
        {
            if(input != null && input.Length > maxLength)
            {
                return input.Substring(0, maxLength);
            }

            return input;
        }
    }
}
