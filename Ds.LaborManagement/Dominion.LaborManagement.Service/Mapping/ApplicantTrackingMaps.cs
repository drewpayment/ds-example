using System;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.User;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class ApplicantTrackingMaps
    {
        public class FromApplicantReference
        {
            public class ToApplicantReferenceDto : ExpressionMapper<ApplicantReference, ApplicantReferenceDto>
            {
                #region Instance

                private static readonly Lazy<ToApplicantReferenceDto> _instance = new Lazy<ToApplicantReferenceDto>();

                /// <summary>
                /// Static instance of the mapper.
                /// </summary>
                public static ToApplicantReferenceDto Instance => _instance.Value;

                #endregion 

                public override Expression<Func<ApplicantReference, ApplicantReferenceDto>> MapExpression
                {
                    get
                    {
                        return x => new ApplicantReferenceDto
                        {
                            ApplicantReferenceId = x.ApplicantReferenceId,
                            ApplicantId = x.ApplicantId,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            PhoneNumber = x.PhoneNumber,
                            EmailAddress = x.EmailAddress,
                            Relationship = x.Relationship,
                            YearsKnown = x.YearsKnown,
                            IsEnabled = x.IsEnabled
                        };
                    }
                }
            }
        }

        public class FromUserReference
        {
            public class ToApplicationDto : ExpressionMapper<User, ApplicantDto>
            {
                private static readonly Lazy<ToApplicationDto> _instance = new Lazy<ToApplicationDto>();

                /// <summary>
                /// Static instance of the mapper.
                /// </summary>
                public static ToApplicationDto Instance => _instance.Value;

                public override Expression<Func<User, ApplicantDto>> MapExpression
                {
                    get
                    {
                        return x => new ApplicantDto
                        {
                            ApplicantId = 0,
                            FirstName = x.Employee != null ? x.Employee.FirstName : "",
                            MiddleInitial = x.Employee != null ? x.Employee.MiddleInitial : "",
                            LastName = x.Employee != null ? x.Employee.LastName : "",
                            UserId = x.UserId,
                            Address = x.Employee != null ? x.Employee.AddressLine1 : "",
                            AddressLine2 = x.Employee != null ? x.Employee.AddressLine2 : "",
                            City = x.Employee != null ? x.Employee.City : "",
                            State = x.Employee != null ? x.Employee.StateId.Value : 0,
                            Zip = x.Employee != null ? x.Employee.PostalCode : "",
                            PhoneNumber = x.Employee != null ? x.Employee.HomePhoneNumber : "",
                            CellPhoneNumber = x.Employee != null ? x.Employee.CellPhoneNumber : "",
                            EmployeeId = x.EmployeeId,
                            CountryId = x.Employee != null ? x.Employee.CountryId : 0,
                            IsDenied = false
                        };
                    }
                }
            }
        }
        public class FromApplicantOnboardingProcess
        {
            public class ToApplicantOnboardingProcessDto : ExpressionMapper<ApplicantOnBoardingProcess, ApplicantOnBoardingProcessDto>
            {
                #region Instance

                private static readonly Lazy<ToApplicantOnboardingProcessDto> _instance = new Lazy<ToApplicantOnboardingProcessDto>();

                /// <summary>
                /// Static instance of the mapper.
                /// </summary>
                public static ToApplicantOnboardingProcessDto Instance => _instance.Value;

                #endregion 

                public override Expression<Func<ApplicantOnBoardingProcess, ApplicantOnBoardingProcessDto>> MapExpression
                {
                    get
                    {
                        return x => new ApplicantOnBoardingProcessDto
                        {
                            ApplicantOnboardingProcessId = x.ApplicantOnboardingProcessId,
                            Description = x.Description,
                            ClientId = x.ClientId,
                            CustomToPostingId = x.CustomToPostingId,
                            ApplicantOnBoardingProcessTypeId = x.ApplicantOnBoardingProcessTypeId,
                            ProcessPhaseId = x.ProcessPhaseId,
                            IsEnabled = x.IsEnabled,
                            ApplicantOnBoardingProcessSets = x.ApplicantOnBoardingProcessSets
                                                                 .Select(y => new ApplicantOnBoardingProcessSetDto()
                                                                 {
                                                                     ApplicantOnboardingProcessId = y.ApplicantOnboardingProcessId,
                                                                     ApplicantOnboardingTaskId = y.ApplicantOnboardingTaskId,
                                                                     Description = y.ApplicantOnBoardingTask.Description,
                                                                     OrderId = y.OrderId
                                                                 }).ToList()
                        };
                    }
                }
            }
        }

        public class FromApplicationControl
        {
            public class ToApplicationControlDto : ExpressionMapper<ApplicantCompanyApplication, ApplicantCompanyApplicationDto>
            {
                #region Instance

                private static readonly Lazy<ToApplicationControlDto> _instance = new Lazy<ToApplicationControlDto>();

                /// <summary>
                /// Static instance of the mapper.
                /// </summary>
                public static ToApplicationControlDto Instance => _instance.Value;

                #endregion 

                public override Expression<Func<ApplicantCompanyApplication, ApplicantCompanyApplicationDto>> MapExpression
                {
                    get
                    {
                        return x => new ApplicantCompanyApplicationDto
                        {
                            CompanyApplicationId = x.CompanyApplicationId,
                            Description = x.Description,
                            ClientId = x.ClientId,
                            IsEnabled = x.IsEnabled,
                            YearsOfEmployment = x.YearsOfEmployment,
                            ReferenceNo = x.ReferenceNo,
                            Education = x.Education,
                            IsCurrentEmpApp = x.IsCurrentEmpApp,
                            IsExcludeHistory = x.IsExcludeHistory,
                            IsExcludeReferences = x.IsExcludeReferences,
                            IsFlagReferenceCheck = x.IsFlagReferenceCheck,
                            IsFlagVolResign = x.IsFlagVolResign,
                            IsFlagNoEmail = x.IsFlagNoEmail,
                            IsExperience= x.IsExperience,
                            ApplicantQuestionSets = x.ApplicantQuestionSets
                                            .Select(y=> new ApplicantQuestionSetsDto {
                                                ApplicationId = y.ApplicationId,
                                                QuestionId =y.QuestionId,
                                                OrderId =y.OrderId}).ToList()
                        };
                    }
                }
            }
        }

        //public class FromQuestionSection
        //{
        //    public class ToQuestionSectionDto : ExpressionMapper<ApplicationQuestionSection, ApplicationQuestionSectionDto>
        //    {
        //        public override Expression<Func<ApplicationQuestionSection, ApplicationQuestionSectionDto>> MapExpression
        //        {
        //            get
        //            {
        //                return x => new ApplicationQuestionSectionDto
        //                {
        //                    SectionId=x.SectionId,
        //                    Description =x.Description,
        //                    TotalQuestions = x.ApplicantQuestionControl.Count()
        //                };
        //            }
        //        }
        //    }
        //}
    }
}
