using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.Authentication.Intermediate.Util;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Labor.Enum;
using Dominion.Core.Services.Api.Dto;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Entities;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply;
using Dominion.LaborManagement.Dto.Enums;
using Dominion.LaborManagement.EF.Query;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query;
using Dominion.Testing.Util.Common;
using Dominion.Testing.Util.Helpers.Mapping;
using Dominion.Testing.Util.Helpers.Prototyping;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ApplicantStatusType = Dominion.LaborManagement.Dto.ApplicantTracking.ApplicantStatusType;
using Dominion.Authentication.Interface.Api;

namespace Dominion.LaborManagement.Test.Internal.Providers
{
    [TestFixture]
    public class IndeedApplicationProviderUnitTests
    {
        protected IIndeedApplicationProvider _provider { get; set; }
        protected Mock<IBusinessApiSession> _mockSession { get; set; }
        private const int UnitedStatesId = 1; // this is the id of the US in justinDev's db

        [SetUp]
        public virtual void SetUp()
        {
            _mockSession = new Mock<IBusinessApiSession>();

            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(x => x.CheckUserName(It.IsAny<string>())).Returns(() => new OpResult());
            _provider = new IndeedApplicationProvider(loginServiceMock.Object, _mockSession.Object, null, null);
            _mockSession.Setup(x => x.UnitOfWork);
        }

        #region standardResume

        [Test]
        public void Test_ApplicantApplicationHeader_Created()
        {
            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.LocationRepository.StateQuery().ByAbbreviation(It.IsAny<List<string>>()).ExecuteQuery())
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "A Name",
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob()
            }, DateTime.Now, 1);

            Assert.NotNull(result.Data);
        }

        [Test]
        public void Test_Application_Committed()
        {
            _mockSession.Setup(x => x.UnitOfWork.Commit());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "A Name",
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob()
            }, DateTime.Now, 1);

            _mockSession.Verify(x => x.UnitOfWork.Commit(), Times.AtLeastOnce);
        }

        [Test]
        public void Test_Applicant_Created()
        {

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "A Name",
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob()
            }, DateTime.Now, 1);

            Assert.NotNull(result.Data.Applicant);
        }

        [Test]
        public void Test_Applicant_FirstName_Should_Be_John()
        {
            var fakePost = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "John Doe",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob()
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(fakePost, DateTime.Now, 1);
            Assert.AreEqual("John", result.Data.Applicant.FirstName, "Applicant has wrong FirstName!");
        }

        [Test]
        public void Test_Applicant_LastName_Should_Be_Doe()
        {
            var fakePost = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "John Doe",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob()
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(fakePost, DateTime.Now, 1);
            Assert.AreEqual("Doe", result.Data.Applicant.LastName, "Applicant has wrong LastName!");
        }

        [Test]
        public void Test_Applicant_Cover_Letter_Should_Be_Added()
        {
            var coverLetter = "A cover letter showing I am reeeeeeeely interested";
            var fakePost = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume(),
                    Coverletter = coverLetter
                },
                Job = new IndeedJob()
                {
                    JobId = "1"
                }
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var result = _provider.SaveApplication(fakePost, DateTime.Now, 1);
            Assert.AreEqual(coverLetter, result.Data.CoverLetter);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Test_Applicant_Cover_Letter_Should_Not_Be_Added_When_Not_Provided(string coverLetter)
        {
            var fakePost = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume(),
                    Coverletter = coverLetter
                },
                Job = new IndeedJob()
                {
                    JobId = "1"
                }
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var result = _provider.SaveApplication(fakePost, DateTime.Now, 1);
            Assert.Null(result.Data.CoverLetter);
        }

        /// <summary>
        /// It is possible for first names and last names to have multiple names in them.  Unfortunately Indeed splits the first name 
        /// and last name by a space.  This means that if your first name and last name consist of three names with a space in between each one
        ///  it is impossible to tell whether the middle name goes with the first or last name.  Fortunately this is a pretty unlikely scenario
        /// </summary>
        [Test]
        public void Test_Applicant_LastName_Includes_All_Names_After_First_Name()
        {
            var fakePost = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "Peter De Vries",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob()
                {
                    JobId="1234"
                }
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(fakePost, DateTime.Now, 1);
            Assert.AreEqual("De Vries", result.Data.Applicant.LastName, "Applicant has wrong LastName!  It should have two names!");
            Assert.AreEqual("Peter", result.Data.Applicant.FirstName, "Applicant has wrong FirstName!");
        }

        [Test]
        public void Test_Applicant_Email_Should_Be_JohnDoeAtEmailDotCom()
        {
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "A Name",
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob()
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);
            Assert.AreEqual("john.doe@email.com", result.Data.Applicant.EmailAddress, "Applicant has the wrong EmailAddress!");
        }

        #endregion

        #region IndeedResume

        #region IndeedResumeRoot

        [Test]
        public void Test_IndeedResume_FirstName_Assigned_Instead_Of_Extracted_Value_From_Full_Name()
        {

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var testFirstName = "Michael";
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "A Name",
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = testFirstName,
                            Location = new IndeedLocation()
                            {
                                City = "a city, MI"
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(testFirstName, result.Data.Applicant.FirstName, "The Applicant's FirstName was not retrieved from Indeed's specialized resume!");
        }

        [Test]
        public void Test_IndeedResume_PhoneNumber_Assigned_Instead_Of_Applicant_PhoneNumber_When_Indeed_Resume_Provided_And_Indeed_Resume_Phone_Number_Is_Not_Null()
        {

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var testFirstName = "Michael";
            var actualPhoneNumber = "9999999999";
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    PhoneNumber = "1234567890",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = testFirstName,
                            Location = new IndeedLocation()
                            {
                                City = ""
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }, PhoneNumber = actualPhoneNumber
                        }
                    }
                },
                Job = new IndeedJob()
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual("(999) 999-9999", result.Data.Applicant.PhoneNumber);
        }

        [Test]
        public void Test_Applicant_PhoneNumber_Assigned_When_Indeed_Resume_Not_Provided()
        {

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var phoneNumber = "9999999999";
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    PhoneNumber = phoneNumber,
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob()
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual("(999) 999-9999", result.Data.Applicant.PhoneNumber);
        }

        [Test]
        public void Test_Applicant_Zip_Assigned_When_Indeed_Resume_Provided()
        {

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var zip = "49555";
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            },
                            Location = new IndeedLocation()
                            {
                                City = "",
                                Country = "",
                                PostalCode = zip
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(zip, result.Data.Applicant.Zip);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Test_Applicant_PhoneNumber_Assigned_When_Indeed_Resume_Provided_But_Indeed_Resume_Phone_Number_Is_Null_Or_Empty(string number)
        {

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var phoneNumber = "9999999999";
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    PhoneNumber = phoneNumber,
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            Location = new IndeedLocation()
                            {
                                City = ""
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            },
                            PhoneNumber = number
                        }
                    }
                },
                Job = new IndeedJob()
            };

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual("(999) 999-9999", result.Data.Applicant.PhoneNumber);
        }

        [Test]
        public void Test_IndeedResume_FirstName_Not_Assigned_Instead_Of_Extracted_Value_From_Full_Name_When_Json_Is_Null()
        {
            string indeedResumeFirstName = "Michael";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(indeedApplicantFirstName, result.Data.Applicant.FirstName, "The Applicant's FirstName was retrieved from Indeed's specialized resume!");
        }

        [Test]
        public void Test_IndeedResume_LastName_Assigned_Instead_Of_Extracted_Value_From_Full_Name()
        {
            var testFirstName = "Michael";
            var testLastName = "Jordan";

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "A Name",
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = testFirstName,
                            LastName = testLastName,
                            Location = new IndeedLocation()
                            {
                                City = "a city, MI"
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(testLastName, result.Data.Applicant.LastName, "The Applicant's LastName was not retrieved from Indeed's specialized resume!");
        }

        [Test]
        public void Test_IndeedResume_LastName_Not_Assigned_Instead_Of_Extracted_Value_From_Full_Name_When_Json_Is_Null()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(indeedApplicantLastName, result.Data.Applicant.LastName, "The Applicant's LastName was retrieved from Indeed's specialized resume!");
        }

        [Test]
        public void Test_IndeedResume_City_Splits_And_Assigns_City_To_Applicant_City()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var city = "Midland";
            var wholeCityString = city + ", TX";

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Location = new IndeedLocation()
                            {
                                City = wholeCityString
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(city, result.Data.Applicant.City, "The Applicant's city was wrong!");
        }

        [Test]
        public void Test_IndeedResume_City_Splits_And_Assigns_Country_To_Applicant_Country()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var city = "Midland";
            var state = "TX";
            var stateId = 45; // stateId of Texas in JustinDEV db
            var wholeCityString = city + ", " + state;
            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilder<Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Location = new IndeedLocation()
                            {
                                City = wholeCityString,
                                Country = "US"
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(UnitedStatesId, result.Data.Applicant.CountryId, "The Applicant's countryId was wrong!");
        }

        [Test]
        public void Test_IndeedResume_City_Splits_And_Assigns_Nothing_To_Applicant_Country_When_No_Country_Found()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var city = "Midland";
            var state = "TX";
            var stateId = 45; // stateId of Texas in JustinDEV db
            var wholeCityString = city + ", " + state;
            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Location = new IndeedLocation()
                            {
                                City = wholeCityString,
                                Country = "US"
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(1, result.Data.Applicant.CountryId, "The Applicant's countryId was not null!");
        }

        [Test]
        public void Test_IndeedResume_City_Splits_And_Assigns_Texas_To_Applicant_State()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var city = "Midland";
            var state = "TX";
            var stateId = 45; // stateId of Texas in JustinDEV db
            var wholeCityString = city + ", " + state;
            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, StateNameIdDto>>()))
                .Returns(() => new List<StateNameIdDto>() { new StateNameIdDto() { StateId = 45 } });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Location = new IndeedLocation()
                            {
                                City = wholeCityString,
                                Country = "US"
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(stateId, result.Data.Applicant.State, "The Applicant's stateId was not 45 (Texas)!");
        }

        [Test]
        public void Test_IndeedResume_State_Not_Assigned_When_Not_Found()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var city = "Midland";
            var state = "TX";
            var stateId = 45; // stateId of Texas in JustinDEV db
            var wholeCityString = city + ", " + state;
            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Location = new IndeedLocation()
                            {
                                City = wholeCityString,
                                Country = "US"
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.Null(result.Data.Applicant.State, "The Applicant's stateId was not null!");
        }

        [Test]
        public void Test_IndeedResume_City_Assigned_But_State_Not_With_Invalid_City()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var city = "Midland";
            var state = "TX";
            var stateId = 45; // stateId of Texas in JustinDEV db
            var wholeCityString = "jkjl";
            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Location = new IndeedLocation()
                            {
                                City = wholeCityString,
                                Country = "US"
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(wholeCityString, result.Data.Applicant.City, "The Applicant's city was not correct!");
            Assert.Null(result.Data.Applicant.State, "The Applicant's state was defined!");
            _mockSession.Verify(x => x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()), Times.Never);
        }

        [Test]
        public void Test_IndeedResume_No_Call_To_Db_For_State_when_Comma_Present_But_StateAbbrevation_Not_Provided()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var wholeCityString = "jkjl, ";
            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Location = new IndeedLocation()
                            {
                                City = wholeCityString,
                                Country = "US"
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            _mockSession.Verify(x => x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()), Times.Never);
        }

        [Test]
        public void Test_IndeedResume_City_State_Country_Not_Assigned_When_IndeedResume_Not_Provided()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.IsEmpty(result.Data.Applicant.City);
            Assert.Null(result.Data.Applicant.State);
            Assert.AreEqual(UnitedStatesId, result.Data.Applicant.CountryId, "The Applicant's countryId was wrong!");
        }

        #endregion

        #region Employment History

        [Test]
        public void Test_ApplicantEmploymentHistory_Has_Correct_Values_When_Only_One_Position_Provided()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var positionTitle = "title";
            var positionCompany = "company";
            var positionCity = "Austin";
            var positionState = "TX";
            var positionLocation = positionCity + ", " + positionState;
            var positionDescription = "A description";
            var positionStartDateMonth = 1;
            var positionEndDateMonth = 2;
            var positionStartDateYear = 2000;
            var positionEndDateYear = 2001;
            var positionEndCurrent = false;
            var stateId = 45;

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.LocationRepository.StateQuery().ByAbbreviation(It.IsAny<List<string>>()).ExecuteQuery())
                .Returns(() => new List<State>() { new State()
            {
                StateId = stateId,
                Abbreviation = "TX"
            }
        });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEmploymentHistory>()));

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                                {
                                    new IndeedPosition()
                                    {
                                        Title = positionTitle,
                                        Company = positionCompany,
                                        Description = positionDescription,
                                        EndCurrent = positionEndCurrent,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = positionEndDateYear,
                                        Location = positionLocation,
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = positionStartDateYear
                                    }
                                }
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories.First();
            Assert.AreEqual(positionTitle, actualPosition.Title);
            Assert.AreEqual(positionCompany, actualPosition.Company);
            Assert.AreEqual(positionDescription, actualPosition.Responsibilities);
            Assert.AreEqual(positionStartDateMonth.ToString("00") + "-" + positionStartDateYear, actualPosition.StartDate);
            Assert.AreEqual(positionEndDateMonth.ToString("00") + "-" + positionEndDateYear, actualPosition.EndDate);
            Assert.AreEqual(positionCity, actualPosition.City);
            Assert.AreEqual(stateId, actualPosition.StateId);

            _mockSession.Verify(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEmploymentHistory>()), Times.AtLeastOnce);
        }

        [Test]
        public void Test_ApplicantEmploymentHistory_Has_Multiple_Past_Employment_Positions_When_Multiple_Previous_Positions_Provided()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var positionTitle = "title";
            var positionOneTitle = "I was promoted, woot!  I have a new title";
            var positionCompany = "company";
            var positionCity = "Austin";
            var positionState = "TX";
            var positionLocation = positionCity + ", " + positionState;
            var positionDescription = "A description";
            var positionStartDateMonth = 1;
            var positionEndDateMonth = 2;
            var positionStartDateYear = 2000;
            var positionEndDateYear = 2001;
            var positionEndCurrent = false;
            var stateId = 45;

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.LocationRepository.StateQuery().ByAbbreviation(It.IsAny<List<string>>()).ExecuteQuery())
                .Returns(() => new List<State>() { new State()
            {
                StateId = stateId,
                Abbreviation = "TX"
            }
        });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                                {
                                    new IndeedPosition()
                                    {
                                        Title = positionTitle,
                                        Company = positionCompany,
                                        Description = positionDescription,
                                        EndCurrent = positionEndCurrent,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = positionEndDateYear,
                                        Location = positionLocation,
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = positionStartDateYear
                                    },
                                    new IndeedPosition()
                                    {
                                        Title = positionOneTitle,
                                        Company = positionCompany,
                                        Description = positionDescription,
                                        EndCurrent = positionEndCurrent,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = positionEndDateYear,
                                        Location = positionLocation,
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = positionStartDateYear
                                    }
                                }
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories;
            Assert.AreEqual(2, actualPosition.Count);
            Assert.AreEqual(positionTitle, actualPosition.First().Title);
            Assert.AreEqual(positionOneTitle, actualPosition.Last().Title);
        }

        [Test]
        public void Test_ApplicantEmploymentHistory_Has_No_Employment_Positions_When_No_Previous_Positions_Provided()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var stateId = 45;

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>() { new State()
            {
                StateId = stateId
            }
        });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new List<IndeedPosition>()
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories;
            Assert.AreEqual(0, actualPosition.Count);
        }

        [Test]
        public void Test_ApplicantEmploymentHistory_Michigan_Assigned_To_StateId_When_State_Not_Provided()
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var positionTitle = "title";
            var positionCompany = "company";
            var positionCity = "jkjl";
            var positionState = "";
            var positionLocation = positionCity + ", " + positionState;
            var positionDescription = "A description";
            var positionStartDateMonth = 1;
            var positionEndDateMonth = 2;
            var positionStartDateYear = 2000;
            var positionEndDateYear = 2001;
            var positionEndCurrent = false;

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = indeedResumeFirstName,
                            LastName = indeedResumeLastName,
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                                {
                                    new IndeedPosition()
                                    {
                                        Title = positionTitle,
                                        Company = positionCompany,
                                        Description = positionDescription,
                                        EndCurrent = positionEndCurrent,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = positionEndDateYear,
                                        Location = positionLocation,
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = positionStartDateYear
                                    }
                                }
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);
            
            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories.First();
            Assert.AreEqual(1, actualPosition.StateId);
        }

        [Test]
        public void Test_ApplicantEmploymentHistory_Not_Created_When_No_Indeed_Resume()
        {
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>());

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories;
            Assert.Null(actualPosition);
        }

        [Test]
        public void Test_ApplicantEmploymentHistory_One_Query_To_Db_For_States_When_Multiple_Positions()
        {
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";

            var positionTitle = "title";
            var positionOneTitle = "I was promoted, woot!  I have a new title";
            var positionCompany = "company";
            var positionCity = "Austin";
            var positionState = "TX";
            var positionLocation = positionCity + ", " + positionState;
            var positionDescription = "A description";
            var positionStartDateMonth = 1;
            var positionEndDateMonth = 2;
            var positionStartDateYear = 2000;
            var positionEndDateYear = 2001;
            var positionEndCurrent = false;

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.LocationRepository.StateQuery().ByAbbreviation(It.IsAny<List<string>>()).ExecuteQuery())
                .Returns(() => new List<State>(){
                    new State()
                    {
                        StateId = 1,
                        Abbreviation = "TX"
                    }
                });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Values = new List<IndeedPosition>()
                                {
                                    new IndeedPosition()
                                    {
                                        Title = positionTitle,
                                        Company = positionCompany,
                                        Description = positionDescription,
                                        EndCurrent = positionEndCurrent,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = positionEndDateYear,
                                        Location = positionLocation,
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = positionStartDateYear
                                    },
                                    new IndeedPosition()
                                    {
                                        Title = positionOneTitle,
                                        Company = positionCompany,
                                        Description = positionDescription,
                                        EndCurrent = positionEndCurrent,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = positionEndDateYear,
                                        Location = positionLocation,
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = positionStartDateYear
                                    }
                                }
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories;
            _mockSession.Verify(x => x.UnitOfWork.LocationRepository.StateQuery().ByAbbreviation(It.IsAny<List<string>>()).ExecuteQuery(), Times.AtLeastOnce); //Times.Exactly(1));
        }

        [Test]
        public void Test_All_ApplicantEmploymentHistories_Included_When_One_Has_State_And_One_Does_Not()
        {
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";

            var positionTitle = "title";
            var positionOneTitle = "I was promoted, woot!  I have a new title";
            var positionCompany = "company";
            var positionCity = "Austin";
            var positionState = "TX";
            var positionLocation = positionCity + ", " + positionState;
            var positionDescription = "A description";
            var positionStartDateMonth = 1;
            var positionEndDateMonth = 2;
            var positionStartDateYear = 2000;
            var positionEndDateYear = 2001;
            var positionEndCurrent = false;

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.LocationRepository.StateQuery().ByAbbreviation(It.IsAny<List<string>>())
                .ExecuteQuery())
                .Returns(() => new List<State>(){
                    new State()
                    {
                        StateId = 1,
                        Abbreviation = "TX",
                        CountryId = UnitedStatesId
                    }
                });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Values = new List<IndeedPosition>()
                                {
                                    new IndeedPosition()
                                    {
                                        Title = positionTitle,
                                        Company = positionCompany,
                                        Description = positionDescription,
                                        EndCurrent = positionEndCurrent,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = positionEndDateYear,
                                        Location = positionLocation,
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = positionStartDateYear
                                    },
                                    new IndeedPosition()
                                    {
                                        Title = positionOneTitle,
                                        Company = positionCompany,
                                        Description = positionDescription,
                                        EndCurrent = positionEndCurrent,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = positionEndDateYear,
                                        Location = "jkjl",
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = positionStartDateYear
                                    }
                                }
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPositions = result.Data.Applicant.ApplicantEmploymentHistories;
            Assert.AreEqual(2, actualPositions.Count);
            Assert.AreEqual(positionTitle, actualPositions.First().Title);
            Assert.AreEqual(1, actualPositions.First().StateId);
            Assert.AreEqual(1, actualPositions.First().CountryId);

            
            Assert.AreEqual("jkjl", actualPositions.Last().City);
            Assert.AreEqual(1, actualPositions.Last().StateId);
            Assert.AreEqual(1, actualPositions.Last().CountryId);
        }

        [Test]
        public void Test_All_ApplicantEmploymentHistories_US_State_Assigned_When_Multiple_States_Found()
        {
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";

            var positionTitle = "title";
            var positionCompany = "company";
            var positionCity = "cityInMinnesota";
            var positionState = "MN";
            var positionLocation = positionCity + ", " + positionState;
            var positionDescription = "A description";
            var positionStartDateMonth = 1;
            var positionEndDateMonth = 2;
            var positionStartDateYear = 2000;
            var positionEndDateYear = 2001;
            var positionEndCurrent = false;

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.LocationRepository.StateQuery().ByAbbreviation(It.IsAny<List<string>>()).ExecuteQuery())
                .Returns(() => new List<State>(){
                    new State()
                    {
                        StateId = 2,
                        Name = "Manipur",
                        Abbreviation = "MN",
                        CountryId = 17
                    },
                    new State()
                    {
                        StateId = 1,
                        Name = "Minnesota",
                        Abbreviation = "MN",
                        CountryId = UnitedStatesId
                    }
                });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Values = new List<IndeedPosition>()
                                {
                                    new IndeedPosition()
                                    {
                                        Title = positionTitle,
                                        Company = positionCompany,
                                        Description = positionDescription,
                                        EndCurrent = positionEndCurrent,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = positionEndDateYear,
                                        Location = positionLocation,
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = positionStartDateYear
                                    }
                                }
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories;
            Assert.AreEqual(1, actualPosition.First().StateId);
        }

        [TestCase(-1, -1)]
        [TestCase(null, null)]
        public void Test_ApplicantEmploymentHistory_Has_No_StartDate_When_No_StartYear_Provided_And_No_EndDate_When_No_EndYear_Provided(int? startYear, int? endYear)
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var positionStartDateMonth = 1;
            var positionEndDateMonth = 2;
            var stateId = 45;

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>() { new State() });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                                {
                                    new IndeedPosition()
                                    {
                                        Title = "",
                                        Company = "",
                                        Description = "",
                                        EndCurrent = false,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = endYear,
                                        Location = "",
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = startYear
                                    }
                                }
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories.First();
            Assert.Null(actualPosition.StartDate);
            Assert.Null(actualPosition.EndDate);
        }

        [TestCase(-1, -1)]
        [TestCase(null, null)]
        public void Test_ApplicantEmploymentHistory_Has_January_In_StartDate_When_No_StartMonth_Provided_And_January_In_EndDate_When_No_EndMonth_Provided(int? startMonth, int? endMonth)
        {
            var indeedResumeFirstName = "Micheal";
            string indeedResumeLastName = "Jordan";
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";
            var positionStartDateMonth = 1;
            var positionEndDateMonth = 2;
            var stateId = 45;

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>() { new State() });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                                {
                                    new IndeedPosition()
                                    {
                                        Title = "",
                                        Company = "",
                                        Description = "",
                                        EndCurrent = false,
                                        EndDateMonth = endMonth,
                                        EndDateYear = 2012,
                                        Location = "",
                                        StartDateMonth = startMonth,
                                        StartDateYear = 2010
                                    }
                                }
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories.First();
            Assert.AreEqual("01-2010", actualPosition.StartDate);
            Assert.AreEqual("01-2012", actualPosition.EndDate);
        }

        [Test]
        public void Test_ApplicantEmploymentHistory_Has_Present_As_EndDate_When_EndCurrent_Is_True()
        {
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>() { new State() });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 1,
                                Values = new List<IndeedPosition>()
                                {
                                    new IndeedPosition()
                                    {
                                        Title = "",
                                        Company = "",
                                        Description = "",
                                        EndCurrent = true,
                                        EndDateMonth = 1,
                                        EndDateYear = 2012,
                                        Location = "",
                                        StartDateMonth = 2,
                                        StartDateYear = 2010
                                    }
                                }
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories.First();
            Assert.AreEqual("Present", actualPosition.EndDate);
        }

        [Test]
        public void Test_When_Casing_Of_Found_State_Abbreviation_Does_Not_Match_Casing_Of_State_Abbreviation_Returned_From_Database()
        {
            var expectedStateId = 1;
            var indeedApplicantFirstName = "John";
            var indeedApplicantLastName = "Doe";

            var positionTitle = "title";
            var positionOneTitle = "I was promoted, woot!  I have a new title";
            var positionCompany = "company";
            var positionCity = "Austin";
            var positionState = "mi";
            var positionLocation = positionCity + ", " + positionState;
            var positionDescription = "A description";
            var positionStartDateMonth = 1;
            var positionEndDateMonth = 2;
            var positionStartDateYear = 2000;
            var positionEndDateYear = 2001;
            var positionEndCurrent = false;

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>());

            _mockSession
                .Setup(x =>
                    x.UnitOfWork.LocationRepository.StateQuery().ByAbbreviation(It.IsAny<List<string>>()).ExecuteQuery())
                .Returns(() => new List<State>(){
                    new State()
                    {
                        StateId = expectedStateId,
                        Abbreviation = "MI",
                        CountryId = UnitedStatesId
                    }
                });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = indeedApplicantFirstName + " " + indeedApplicantLastName,
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Values = new List<IndeedPosition>()
                                {
                                    new IndeedPosition()
                                    {
                                        Title = positionTitle,
                                        Company = positionCompany,
                                        Description = positionDescription,
                                        EndCurrent = positionEndCurrent,
                                        EndDateMonth = positionEndDateMonth,
                                        EndDateYear = positionEndDateYear,
                                        Location = positionLocation,
                                        StartDateMonth = positionStartDateMonth,
                                        StartDateYear = positionStartDateYear
                                    }
                                }
                            },
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new List<IndeedEducation>()
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            var actualPosition = result.Data.Applicant.ApplicantEmploymentHistories;
            Assert.AreEqual(1, actualPosition.Count);
            Assert.AreEqual(expectedStateId, actualPosition.First().StateId);
        }
        #endregion

        #region Education History
        [TestCase(false, "B.A.")] // graduated
        [TestCase(false, null)] // not working on degree and did not received one
        public void Test_Education_History_Maps_Values_Correctly(bool actualEndCurrent, string actualDegree)
        {
            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEducationHistory>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actualField = "Computer Science";
            var actualSchool = "University of Texas";
            var startDateYear = 2007;
            var endDateYear = 2011;
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 1,
                                Values = new List<IndeedEducation>()
                                {
                                    new IndeedEducation()
                                    {
                                        Degree = actualDegree,
                                        Field = actualField,
                                        School = actualSchool,
                                        Location = "Austin, Tx",
                                        StartDateYear = startDateYear,
                                        EndDateYear = endDateYear,
                                        EndCurrent = actualEndCurrent
                                    }
                                }
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };
            HasDegreeType hasDegree;
            if (!actualEndCurrent && actualDegree.IsNotNullOrEmpty())
            {
                hasDegree = HasDegreeType.Yes;
            } else if (actualEndCurrent)
            {
                hasDegree = HasDegreeType.InProgress;
            }
            else
            {
                hasDegree = HasDegreeType.No;
            }

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(actualDegree, result.Data.Applicant.ApplicantEducationHistories.First().ExternalDegree);
            Assert.AreEqual(actualField, result.Data.Applicant.ApplicantEducationHistories.First().Studied);
            Assert.AreEqual(actualSchool, result.Data.Applicant.ApplicantEducationHistories.First().Description);
            Assert.AreEqual(endDateYear - startDateYear, result.Data.Applicant.ApplicantEducationHistories.First().YearsCompleted);
            Assert.AreEqual(hasDegree, result.Data.Applicant.ApplicantEducationHistories.First().HasDegree);
            Assert.True(result.Data.Applicant.ApplicantEducationHistories.First().IsEnabled);
            Assert.Null(result.Data.Applicant.ApplicantEducationHistories.First().ApplicantSchoolTypeId);

            _mockSession.Verify(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEducationHistory>()), Times.AtLeastOnce);
        }

        [TestCase(-1, -1, true)]
        public void Test_Education_History_Handles_Date_Years_Correctly_StartYear_And_End_Year_Not_Provided(int? startYear, int? endYear, bool endCurrent)
        {
            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEducationHistory>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 1,
                                Values = new List<IndeedEducation>()
                                {
                                    new IndeedEducation()
                                    {
                                        Degree = "",
                                        Field = "",
                                        School = "",
                                        Location = "",
                                        StartDateYear = startYear,
                                        EndDateYear = endYear,
                                        EndCurrent = endCurrent
                                    }
                                }
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);
            Assert.Null(result.Data.Applicant.ApplicantEducationHistories.First().YearsCompleted);
            Assert.AreEqual(HasDegreeType.InProgress, result.Data.Applicant.ApplicantEducationHistories.First().HasDegree);
            Assert.Null(result.Data.Applicant.ApplicantEducationHistories.First().DateStarted);
            Assert.Null(result.Data.Applicant.ApplicantEducationHistories.First().DateEnded);
        }

        [TestCase(-1, 2010, true)]
        public void Test_Education_History_Handles_Date_Years_Correctly_StartYear_Not_Provided(int? startYear, int? endYear, bool endCurrent)
        {
            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEducationHistory>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 1,
                                Values = new List<IndeedEducation>()
                                {
                                    new IndeedEducation()
                                    {
                                        Degree = "",
                                        Field = "",
                                        School = "",
                                        Location = "",
                                        StartDateYear = startYear,
                                        EndDateYear = endYear,
                                        EndCurrent = endCurrent
                                    }
                                }
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);
            Assert.Null(result.Data.Applicant.ApplicantEducationHistories.First().YearsCompleted);
            Assert.AreEqual(HasDegreeType.InProgress, result.Data.Applicant.ApplicantEducationHistories.First().HasDegree);
            Assert.Null(result.Data.Applicant.ApplicantEducationHistories.First().DateStarted);
            Assert.AreEqual(new DateTime(endYear.Value, 1, 1), result.Data.Applicant.ApplicantEducationHistories.First().DateEnded);
        }

        [TestCase(2010, -1, true)]
        public void Test_Education_History_Handles_Date_Years_Correctly_EndYear_Not_Provided_And_Working_Towards_Degree(int? startYear, int? endYear, bool endCurrent)
        {
            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEducationHistory>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 1,
                                Values = new List<IndeedEducation>()
                                {
                                    new IndeedEducation()
                                    {
                                        Degree = "",
                                        Field = "",
                                        School = "",
                                        Location = "",
                                        StartDateYear = startYear,
                                        EndDateYear = endYear,
                                        EndCurrent = endCurrent
                                    }
                                }
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, new DateTime(2011, 7, 1), 1);
            Assert.AreEqual(1, result.Data.Applicant.ApplicantEducationHistories.First().YearsCompleted);
            Assert.AreEqual(new DateTime(startYear.Value, 1, 1), result.Data.Applicant.ApplicantEducationHistories.First().DateStarted);
            Assert.Null(result.Data.Applicant.ApplicantEducationHistories.First().DateEnded);
        }

        [Test]
        public void Test_Education_History_Handles_Date_Years_Correctly_EndYear_Not_Provided_And_Not_Working_Towards_Degree()
        {
            var startYear = 2010;
            var endYear = -1;
            var endCurrent = false;


            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEducationHistory>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 1,
                                Values = new List<IndeedEducation>()
                                {
                                    new IndeedEducation()
                                    {
                                        Degree = "",
                                        Field = "",
                                        School = "",
                                        Location = "",
                                        StartDateYear = startYear,
                                        EndDateYear = endYear,
                                        EndCurrent = endCurrent
                                    }
                                }
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);
            Assert.Null(result.Data.Applicant.ApplicantEducationHistories.First().YearsCompleted);
            Assert.AreEqual(new DateTime(startYear, 1, 1), result.Data.Applicant.ApplicantEducationHistories.First().DateStarted);
            Assert.Null(result.Data.Applicant.ApplicantEducationHistories.First().DateEnded);
        }

        [TestCase(2010, 2012, false)]
        public void Test_Education_History_Handles_Date_Years_Correctly_Schooling_Completed(int? startYear, int? endYear, bool endCurrent)
        {
            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEducationHistory>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 1,
                                Values = new List<IndeedEducation>()
                                {
                                    new IndeedEducation()
                                    {
                                        Degree = "Thienthe",
                                        Field = "",
                                        School = "",
                                        Location = "",
                                        StartDateYear = startYear,
                                        EndDateYear = endYear,
                                        EndCurrent = endCurrent
                                    }
                                }
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);
            Assert.AreEqual(2, result.Data.Applicant.ApplicantEducationHistories.First().YearsCompleted);
            Assert.AreEqual(HasDegreeType.Yes, result.Data.Applicant.ApplicantEducationHistories.First().HasDegree);
            Assert.AreEqual(new DateTime(startYear.Value, 1, 1), result.Data.Applicant.ApplicantEducationHistories.First().DateStarted);
        }

        [TestCase(2017, 1, 1, true)]
        [TestCase(2016, 10, 1, true)]
        [TestCase(2016, 1, 1, true)]
        [TestCase(2016, 1, 1, false)]
        public void Test_Education_History_Handles_Date_Years_And_HasDegree_Correctly_Schooling_In_Progress_First_Year_Not_Completed(int currentYear, int currentMonth, int currentDay, bool endCurrent)
        {
            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEducationHistory>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var startYear = new DateTime(2016, 8, 1).Year;
            var endYear = new DateTime(2020, 4, 1).Year;
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 1,
                                Values = new List<IndeedEducation>()
                                {
                                    new IndeedEducation()
                                    {
                                        Degree = "Math",
                                        Field = "",
                                        School = "",
                                        Location = "",
                                        StartDateYear = startYear,
                                        EndDateYear = endYear,
                                        EndCurrent = endCurrent
                                    }
                                }
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, new DateTime(currentYear, currentMonth, currentDay), 1);
            Assert.AreEqual(0, result.Data.Applicant.ApplicantEducationHistories.First().YearsCompleted);
            Assert.AreEqual(endCurrent ? HasDegreeType.InProgress : HasDegreeType.No, result.Data.Applicant.ApplicantEducationHistories.First().HasDegree);
            Assert.AreEqual(new DateTime(startYear, 1, 1), result.Data.Applicant.ApplicantEducationHistories.First().DateStarted);
            Assert.AreEqual(new DateTime(endYear, 1, 1), result.Data.Applicant.ApplicantEducationHistories.First().DateEnded);
        }



        [TestCase(2017, 7, 1, true)]
        [TestCase(2017, 10, 1, true)]
        [TestCase(2017, 10, 1, false)]
        public void Test_Education_History_Handles_Date_Years_Correctly_Schooling_In_Progress_First_Year_Completed(int currentYear, int currentMonth, int currentDay, bool endCurrent)
        {
            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEducationHistory>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var startYear = new DateTime(2016, 8, 1).Year;
            var endYear = new DateTime(2020, 4, 1).Year;
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 1,
                                Values = new List<IndeedEducation>()
                                {
                                    new IndeedEducation()
                                    {
                                        Degree = "",
                                        Field = "",
                                        School = "",
                                        Location = "",
                                        StartDateYear = startYear,
                                        EndDateYear = endYear,
                                        EndCurrent = endCurrent
                                    }
                                }
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, new DateTime(currentYear, currentMonth, currentDay), 1);
            Assert.AreEqual(endCurrent ? 1 : 0, result.Data.Applicant.ApplicantEducationHistories.First().YearsCompleted);
            Assert.AreEqual(endCurrent ? HasDegreeType.InProgress : HasDegreeType.No, result.Data.Applicant.ApplicantEducationHistories.First().HasDegree);
            Assert.AreEqual(new DateTime(startYear, 1, 1), result.Data.Applicant.ApplicantEducationHistories.First().DateStarted);
            Assert.AreEqual(new DateTime(endYear, 1, 1), result.Data.Applicant.ApplicantEducationHistories.First().DateEnded);
        }

        [TestCase(2018, 6, 1)]
        [TestCase(2018, 10, 1)]
        public void Test_Education_History_Handles_Date_Years_Correctly_Schooling_In_Progress_Second_Year_Completed(int currentYear, int currentMonth, int currentDay)
        {
            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantEducationHistory>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var startYear = new DateTime(2016, 8, 1).Year;
            var endYear = new DateTime(2020, 4, 1).Year;
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 1,
                                Values = new List<IndeedEducation>()
                                {
                                    new IndeedEducation()
                                    {
                                        Degree = "",
                                        Field = "",
                                        School = "",
                                        Location = "",
                                        StartDateYear = startYear,
                                        EndDateYear = endYear,
                                        EndCurrent = true
                                    }
                                }
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 0,
                                Values = new IndeedCertification[0]
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, new DateTime(currentYear, currentMonth, currentDay), 1);
            Assert.AreEqual(2, result.Data.Applicant.ApplicantEducationHistories.First().YearsCompleted);
        }
#endregion

        #region Professional Licensing

        [Test]
        public void Test_Certification_Mapped_Correctly_To_Applicant_License()
        {

            var title = "PMP";
            var description = "Project Management Professional certification";
            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2020, 1, 1);

            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantLicense>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new IndeedEducation[0]
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 1,
                                Values = new List<IndeedCertification>()
                                {
                                    new IndeedCertification()
                                    {
                                        Title = title,
                                        Description = description,
                                        StartDateMonth = startDate.Month,
                                        StartDateYear = startDate.Year,
                                        EndDateMonth = endDate.Month,
                                        EndDateYear = endDate.Year,
                                        EndCurrent = true
                                    }
                                }
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            var result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(title, result.Data.Applicant.ApplicantLicenses.First().Type);
            Assert.AreEqual(startDate, result.Data.Applicant.ApplicantLicenses.First().ValidFrom );
            Assert.AreEqual(endDate, result.Data.Applicant.ApplicantLicenses.First().ValidTo );
            Assert.True(result.Data.Applicant.ApplicantLicenses.First().IsEnabled);
            Assert.AreEqual(1, result.Data.Applicant.ApplicantLicenses.First().CountryId);
            Assert.Null(result.Data.Applicant.ApplicantLicenses.First().StateId);
            Assert.Null(result.Data.Applicant.ApplicantLicenses.First().RegistrationNumber);
            Assert.AreEqual(description, result.Data.Applicant.ApplicantLicenses.First().Description);

            _mockSession.Verify(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantLicense>()), Times.AtLeastOnce);
        }

        [TestCase(-1, -1)]
        [TestCase(null, null)]
        public void Test_Certification_Creates_License_Without_ValidTo(int? endMonth, int? endYear)
        {

            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantLicense>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new IndeedEducation[0]
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 1,
                                Values = new List<IndeedCertification>()
                                {
                                    new IndeedCertification()
                                    {
                                        Title = "",
                                        Description = "",
                                        StartDateMonth = 1,
                                        StartDateYear = 2010,
                                        EndDateMonth = endMonth,
                                        EndDateYear = endYear,
                                        EndCurrent = false
                                    }
                                }
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            var result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.Null(result.Data.Applicant.ApplicantLicenses.First().ValidTo);
        }

        [TestCase(-1, -1)]
        [TestCase(null, null)]
        public void Test_Certification_Creates_License_Without_ValidFrom(int? startMonth, int? startYear)
        {

            _mockSession.Setup(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantLicense>()));

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                    {
                        Json = new IndeedResumeData()
                        {
                            FirstName = "",
                            LastName = "",
                            Educations = new IndeedArray<IndeedEducation>()
                            {
                                Total = 0,
                                Values = new IndeedEducation[0]
                            },
                            Positions = new IndeedArray<IndeedPosition>()
                            {
                                Total = 0,
                                Values = new IndeedPosition[0]
                            },
                            Certifications = new IndeedArray<IndeedCertification>()
                            {
                                Total = 1,
                                Values = new List<IndeedCertification>()
                                {
                                    new IndeedCertification()
                                    {
                                        Title = "",
                                        Description = "",
                                        StartDateMonth = startMonth,
                                        StartDateYear = startYear,
                                        EndDateMonth = 1,
                                        EndDateYear = 210,
                                        EndCurrent = false
                                    }
                                }
                            }
                        }
                    }
                },
                Job = new IndeedJob()
            };

            var result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.Null(result.Data.Applicant.ApplicantLicenses.First().ValidFrom);
        }

        #endregion
        #endregion

        [Test]
        public void Test_ExternalApplication_Identity_Mapped_Correctly()
        {
            var id = "jfionlvnesf5adsf38vcsef5sd35f5";
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "A Name",
                    Email = "john.doe@email.com",
                    Resume = new IndeedResume()
                },
                Id = id,
                Job = new IndeedJob()
                {
                    JobId = "1234"
                }
            };

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

           Assert.AreEqual(id, result.Data.ExternalApplicationIdentity.ExternalApplicationId);
           Assert.AreEqual(ApplicantJobSiteEnum.Indeed, result.Data.ExternalApplicationIdentity.ApplicantJobSiteId);
        }

        [Test]
        public void Test_Created_Applicant_Has_ApplicantStatusType()
        {
            var actual = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob()
                {
                    JobId = "0"
                }
            };

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, int>>>())).Returns(() => new List<int>());

            IOpResult<ApplicantApplicationHeader> result = _provider.SaveApplication(actual, DateTime.Now, 1);

            Assert.AreEqual(ApplicantStatusType.Applicant, result.Data.ApplicantStatusTypeId);
        }

        [Test]
        public void Test_CheckForConflict_Returns_True_When_Duplicate_Application_Found()
        {
            _mockSession
                .Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ExternalApplicationIdentityQuery()
                    .ByExternalApplicationId(It.IsAny<string>())
                    .ExecuteQueryAs<int?>(It.IsAny<Expression<Func<ExternalApplicationIdentity, int?>>>()))
                .Returns(() => new List<int?>(){1});

            bool aConflictExists = _provider.CheckForConflict("");

            Assert.True(aConflictExists);
        }

        [Test]
        public void Test_CheckForConflict_Returns_False_When_Duplicate_Application_Is_Not_Found()
        {
            _mockSession
                .Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ExternalApplicationIdentityQuery()
                    .ByExternalApplicationId(It.IsAny<string>()).ExecuteQuery())
                .Returns(() => new List<ExternalApplicationIdentity>());

            bool aConflictExists = _provider.CheckForConflict("");

            Assert.False(aConflictExists);
        }

        [Test]
        public void Test_GetClientIdFromApplicantPost_Returns_Integer_When_ApplicantPosting_With_Matching_Id_Exists()
        {
            _mockSession.Setup(x =>
                x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>())
                    .ByIsActive(true)
                    .ByIsClosed(false)
                    .ExecuteQueryAs<int?>(It.IsAny<Expression<Func<ApplicantPosting, int?>>>())).Returns(() => new List<int?>(){1});

            var posting = _provider.GetClientIdFromApplicantPost(0);

            Assert.NotNull(posting);
        }

        [Test]
        public void Test_GetClientIdFromApplicantPost_Returns_Null_When_ApplicantPosting_With_Matching_Id_Exists()
        {
            _mockSession.Setup(x =>
                x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>())
                    .ByIsActive(true)
                    .ByIsClosed(false)
                    .ExecuteQueryAs<int?>(It.IsAny<Expression<Func<ApplicantPosting, int?>>>())).Returns(() => new List<int?>());

            var posting = _provider.GetClientIdFromApplicantPost(0);

            Assert.Null(posting);
        }

        [Test]
        public void Test_Answer_Maps_QuestionID_And_Response_To_ApplicantApplicationDetail()
        {
            var application = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob(),
                Questions = new IndeedScreenerQuestions()
                {
                    Answers = new[]
                    {
                        new IndeedAnswer()
                        {
                            Id = "11",
                            Values = new[]
                            {
                                "m"
                            }
                        }
                    }
                }
            };

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, IEnumerable<ApplicantQuestionControl>>>>()))
                    .Returns(() => new[] { new ApplicantQuestionControl[] {
                        new ApplicantQuestionControl {
                            QuestionId = 11,
                            SectionId = 5
                    } } });
            

            var result = _provider.SaveApplication(application, DateTime.Now, 1);

            Assert.AreEqual(1, result.Data.ApplicantApplicationDetail.Count);

            var firstAnswer = result.Data.ApplicantApplicationDetail.First();

            Assert.AreEqual(11, firstAnswer.QuestionId);
            Assert.AreEqual("m", firstAnswer.Response);
            Assert.AreEqual(5, firstAnswer.SectionId);

            _mockSession.Verify(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantApplicationDetail>()), Times.AtLeastOnce);
        }

        [Test]
        public void Test_Multiple_Answers_Create_Multiple_Details()
        {
            var application = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob(),
                Questions = new IndeedScreenerQuestions()
                {
                    Answers = new[]
                    {
                        new IndeedAnswer()
                        {
                            Id = "11",
                            Values = new[]
                            {
                                "m"
                            }
                        },
                        new IndeedAnswer()
                        {
                            Id = "12",
                            Values = new[]
                            {
                                "b"
                            }
                        }
                    }
                }
            };

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, IEnumerable<ApplicantQuestionControl>>>>()))
                    .Returns(() => new[] { new ApplicantQuestionControl[] {
                        new ApplicantQuestionControl()
                    {
                        QuestionId = 11
                    },
                        new ApplicantQuestionControl()
                        {
                            QuestionId = 12
                        } } });

            var result = _provider.SaveApplication(application, DateTime.Now, 1);

            Assert.AreEqual(2, result.Data.ApplicantApplicationDetail.Count);

            _mockSession.Verify(x => x.UnitOfWork.RegisterNew(It.IsAny<ApplicantApplicationDetail>()), Times.AtLeastOnce); //Times.Exactly(2));
        }

        [Test]
        public void Test_Answer_With_Multiple_Values_Creates_Header_With_CSV_Response()
        {
            var application = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob(),
                Questions = new IndeedScreenerQuestions()
                {
                    Answers = new[]
                    {
                        new IndeedAnswer()
                        {
                            Id = "11",
                            Values = new[]
                            {
                                "m",
                                "n"
                            }
                        }
                    }
                }
            };

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, IEnumerable<ApplicantQuestionControl>>>>()))
                    .Returns(() => new[] { new ApplicantQuestionControl[] {
                        new ApplicantQuestionControl()
                    {
                        QuestionId = 11
                    }
                    } });

            var result = _provider.SaveApplication(application, DateTime.Now, 1);

            Assert.AreEqual("m,n", result.Data.ApplicantApplicationDetail.First().Response);
        }

        [Test]
        public void Test_Answer_With_Empty_Values_List_Creates_No_Header()
        {
            var application = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob(),
                Questions = new IndeedScreenerQuestions()
                {
                    Answers = new[]
                    {
                        new IndeedAnswer()
                        {
                            Id = "11",
                            Values = new string[]
                            { }
                        }
                    }
                }
            };

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, IEnumerable<ApplicantQuestionControl>>>>()))
                    .Returns(() => new[] { new ApplicantQuestionControl[] {
                        new ApplicantQuestionControl()
                    {
                        QuestionId = 11
                    }
                    } });

            var result = _provider.SaveApplication(application, DateTime.Now, 1);

            Assert.AreEqual(0, result.Data.ApplicantApplicationDetail.Count);
        }

        [Test]
        public void Test_Answer_With_Null_Values_List_Creates_No_Header()
        {
            var application = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob(),
                Questions = new IndeedScreenerQuestions()
                {
                    Answers = new[]
                    {
                        new IndeedAnswer()
                        {
                            Id = "11",
                            Values = null
                        }
                    }
                }
            };

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, IEnumerable<ApplicantQuestionControl>>>>()))
                    .Returns(() => new[] { new ApplicantQuestionControl[] {
                        new ApplicantQuestionControl()
                    {
                        QuestionId = 11
                    }
                    } });

            var result = _provider.SaveApplication(application, DateTime.Now, 1);

            Assert.AreEqual(0, result.Data.ApplicantApplicationDetail.Count);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void Test_Answer_With_Empty_String_Value_Creates_No_Header(string value)
        {
            var application = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob(),
                Questions = new IndeedScreenerQuestions()
                {
                    Answers = new[]
                    {
                        new IndeedAnswer()
                        {
                            Id = "11",
                            Value = value
                        }
                    }
                }
            };

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, IEnumerable<ApplicantQuestionControl>>>>()))
                    .Returns(() => new[] { new ApplicantQuestionControl[] {
                        new ApplicantQuestionControl()
                    {
                        QuestionId = 11
                    }
                    } });

            var result = _provider.SaveApplication(application, DateTime.Now, 1);

            Assert.AreEqual(0, result.Data.ApplicantApplicationDetail.Count);
        }

        [Test]
        public void Test_Detail_IsFlagged_Is_True_When_Response_Matches_FlaggedResponse_And_Question_IsFlagged_Is_True()
        {

            var flaggedResponse = "m";
            var application = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob(),
                Questions = new IndeedScreenerQuestions()
                {
                    Answers = new[]
                    {
                        new IndeedAnswer()
                        {
                            Id = "11",
                            Values = new[]
                            {
                                flaggedResponse
                            }
                        }
                    }
                }
            };

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, IEnumerable<ApplicantQuestionControl>>>>()))
                    .Returns(() => new[] { new ApplicantQuestionControl[] {
                        new ApplicantQuestionControl()
                    {
                        QuestionId = 11,
                        IsFlagged = true,
                        FlaggedResponse = flaggedResponse
                    } } });

            var result = _provider.SaveApplication(application, DateTime.Now, 1);

            Assert.True(result.Data.ApplicantApplicationDetail.First().IsFlagged);
        }

        [Test]
        public void Test_Detail_First_IsFlagged_Is_True_And_Second_IsFlagged_Is_False()
        {

            var flaggedResponse = "m";
            var notFlaggedResponse = flaggedResponse + "n";
            var application = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob(),
                Questions = new IndeedScreenerQuestions()
                {
                    Answers = new[]
                    {
                        new IndeedAnswer()
                        {
                            Id = "11",
                            Values = new[]
                            {
                                flaggedResponse
                            }
                        },
                        new IndeedAnswer()
                        {
                            Id = "12",
                            Values = new[]
                            {
                                notFlaggedResponse
                            }
                        }
                    }
                }
            };

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, IEnumerable<ApplicantQuestionControl>>>>()))
                    .Returns(() => new[] { new ApplicantQuestionControl[] {
                        new ApplicantQuestionControl()
                    {
                        QuestionId = 11,
                        IsFlagged = true,
                        FlaggedResponse = flaggedResponse
                    },
                    new ApplicantQuestionControl()
                    {
                        QuestionId = 12,
                        IsFlagged = false
                    } } });

            var result = _provider.SaveApplication(application, DateTime.Now, 1);

            Assert.True(result.Data.ApplicantApplicationDetail.First().IsFlagged);
            Assert.False(result.Data.ApplicantApplicationDetail.Last().IsFlagged);
        }

        [Test]
        public void Test_One_Call_Made_To_Get_All_Questions_For_Answers()
        {
            var application = new IndeedApplication()
            {
                Applicant = new IndeedApplicant()
                {
                    FullName = "",
                    Email = "",
                    Resume = new IndeedResume()
                },
                Job = new IndeedJob(),
                Questions = new IndeedScreenerQuestions()
                {
                    Answers = new[]
                    {
                        new IndeedAnswer()
                        {
                            Id = "11",
                            Values = new[]
                            {
                                ""
                            }
                        },
                        new IndeedAnswer()
                        {
                            Id = "12",
                            Values = new[]
                            {
                                ""
                            }
                        }
                    }
                }
            };

            var responses = new Stack<IEnumerable<ApplicantQuestionControl>>();

            responses.Push(new List<ApplicantQuestionControl>()
            {
                new ApplicantQuestionControl()
                {
                    QuestionId = 12,
                    IsFlagged = false
                }
            });

            responses.Push(new List<ApplicantQuestionControl>()
            {
                new ApplicantQuestionControl()
                {
                    QuestionId = 11,
                    IsFlagged = true,
                    FlaggedResponse = ""
                }
            });



            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, IEnumerable<ApplicantQuestionControl>>>>()))
                    .Returns(() => new[] { responses.Pop() });

            var result = _provider.SaveApplication(application, DateTime.Now, 1);

            _mockSession.Verify(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>()).ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, IEnumerable<ApplicantQuestionControl>>>>()), Times.AtLeastOnce);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ParsePhoneNumber_Returns_Empty_List_When_Provided_Number_Is_Empty_Or_Null(string number)
        {
            var result = _provider.ParsePhoneNumber(number);
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        [TestCase("(443)225-5612")]
        [TestCase("4432255612")]
        [TestCase("(443)225 - 5612")]
        [TestCase("443 225 5612")]
        public void ParsePhoneNumber_Returns_List_With_One_Number_When_Input_Is_One_Valid_Number(string number)
        {
            var result = _provider.ParsePhoneNumber(number);
            Assert.AreEqual(1, result.Count(), "The list of phone numbers should have had only 1 item!");
            Assert.AreEqual("4432255612", result.FirstOrDefault(), "The list of phone numbers had an incorrect phone number.");
        }

        [Test]
        [TestCase("123")]
        public void ParsePhoneNumber_Returns_Empty_List_When_Number_Does_Not_Have_Enough_Digits(string number)
        {
            var result = _provider.ParsePhoneNumber(number);
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        [TestCase("(443)225-5612 (989)122-5344 (123)456-7890")]
        [TestCase("(443)225-5612 or (989)122-5344 or (123)456-7890")]
        [TestCase("(443)225-5612,(989)122-5344,(123)456-7890")]

        [TestCase("(443)225-5612(989)122-5344(123)456-7890")]
        public void ParsePhoneNumber_Returns_Multiple_Phone_Numbers_When_Input_Is_Multiple_Numbers(string number)
        {
            var result = _provider.ParsePhoneNumber(number);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("4432255612", result.FirstOrDefault());
            Assert.AreEqual("9891225344", result.ElementAt(1));
            Assert.AreEqual("1234567890", result.ElementAt(2));
        }

        [Test]
        [TestCase("225-5612 122-5344 456-7890")]
        public void ParsePhoneNumber_Returns_Multiple_Phone_Numbers_When_Input_Is_Multiple_Numbers_With_Seven_Digits(string number)
        {
            var result = _provider.ParsePhoneNumber(number);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("2255612", result.FirstOrDefault());
            Assert.AreEqual("1225344", result.ElementAt(1));
            Assert.AreEqual("4567890", result.ElementAt(2));
        }

        [Test]
        [TestCase("1 443 225-5612")]
        public void ParsePhoneNumber_Returns_One_Phone_Number_When_Input_Is_Eleven_Digits(string number)
        {
            var result = _provider.ParsePhoneNumber(number);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("14432255612", result.FirstOrDefault());
        }

        [Test]
        [TestCase("14432255612")]
        public void MakeElevenDigitNumberPretty_Returns_Pretty_PhoneNumber(string number)
        {
            var result = IndeedApplicationProvider.MakeElevenDigitNumberPretty(number);
            Assert.AreEqual("1 (443) 225-5612", result);
        }

        [Test]
        [TestCase("4432255612")]
        public void MakeTenDigitNumberPretty_Returns_Pretty_PhoneNumber(string number)
        {
            var result = IndeedApplicationProvider.MakeTenDigitNumberPretty(number);
            Assert.AreEqual("(443) 225-5612", result);
        }

        [Test]
        [TestCase("2255612")]
        public void MakeSevenDigitNumberPretty_Returns_Pretty_PhoneNumber(string number)
        {
            var result = IndeedApplicationProvider.MakeSevenDigitNumberPretty(number);
            Assert.AreEqual("225-5612", result);
        }
    }
}
