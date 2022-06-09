using System.Collections.Generic;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Internal.Security
{
    internal class ApplicantTrackingActionType : ActionType
    {
        private const string BASE_DESIGNATION = "ApplicantTracking";

        /// <summary>
        /// Initializing constructor, intended to be called from w/in static "factory" properties.
        /// </summary>
        /// <param name="localDesignation">Unique identifier for the action relative to this class.
        /// Does not include the base designation.</param>
        /// <param name="label">User-friendly label that describes the action.</param>
        private ApplicantTrackingActionType(string localDesignation, string label, IEnumerable<LegacyRole> roles)
            : base(BASE_DESIGNATION + "." + localDesignation, label, roles)
        {
        }

        /// <summary>
        /// Actions requiring applicant tracking administrator privileges.
        /// See <see cref="ApplicantTrackingActionTypeRegulator"/> for additional constraints.
        /// </summary>
        internal static ApplicantTrackingActionType ApplicantAdmin
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor,
                    LegacyRole.Employee
                };
                return new ApplicantTrackingActionType(nameof(ApplicantAdmin), "Actions requiring applicant tracking administrator privileges.", roles);
            }
        }

        /// <summary>
        /// Actions accessible to internal applicants only.
        /// See <see cref="ApplicantTrackingActionTypeRegulator"/> for additional constraints.
        /// </summary>
        internal static ApplicantTrackingActionType InternalApplicant
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor,
                    LegacyRole.Employee
                };
                return new ApplicantTrackingActionType(nameof(InternalApplicant), "Actions requiring Internal Applicant privileges.", roles);
            }
        }

        /// <summary>
        /// Actions accessible to external applicants only.
        /// See <see cref="ApplicantTrackingActionTypeRegulator"/> for additional constraints.
        /// </summary>
        internal static ApplicantTrackingActionType ExternalApplicant
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.Applicant
                };
                return new ApplicantTrackingActionType(nameof(ExternalApplicant), "Actions requiring External Applicant privileges.", roles);
            }
        }

        /// <summary>
        /// Actions required to update applicant information.
        /// See <see cref="ApplicantTrackingActionTypeRegulator"/> for additional constraints.
        /// </summary>
        internal static ApplicantTrackingActionType WriteApplicantInfo
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor,
                    LegacyRole.Employee,
                    LegacyRole.Applicant
                };
                return new ApplicantTrackingActionType(nameof(WriteApplicantInfo), "Actions required to update applicant information.", roles);
            }
        }

        /// <summary>
        /// Actions required to read applicant information.
        /// See <see cref="ApplicantTrackingActionTypeRegulator"/> for additional constraints.
        /// </summary>
        internal static ApplicantTrackingActionType ReadApplicantInfo
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor,
                    LegacyRole.Employee,
                    LegacyRole.Applicant
                };
                return new ApplicantTrackingActionType(nameof(ReadApplicantInfo), "Actions required to read applicant information.", roles);
            }
        }
    }
}