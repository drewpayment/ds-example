using System;
using System.Collections.Generic;

using Dominion.Aca.Dto;
using Dominion.Aca.Dto.Forms;
using Dominion.Core.Dto;
using Dominion.Domain.Entities.Aca;
using Dominion.Domain.Interfaces.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface to a repository used to query ACA data.
    /// </summary>
    public interface IAcaRepository
    {   
        /// <summary>
        /// Query 1095-C employee data.
        /// </summary>
        /// <returns></returns>
        IAca1095CLineItemQuery QueryAca1095CLineItems();

        /// <summary>
        /// Query 1095-C covered individual (eg: dependents) data.
        /// </summary>
        /// <returns></returns>
        IAca1095CCoveredIndividualQuery QueryAca1095CCoveredIndividuals();

        /// <summary>
        /// Query 1095-C box code information.
        /// </summary>
        /// <returns></returns>
        IAca1095CBoxCodeTypeQuery Query1095CBoxCodeTypeInfo();

        /// <summary>
        /// Query 1095-C box code options.
        /// </summary>
        /// <returns></returns>
        IAca1095CBoxCodeOptionQuery Query1095CBoxCodeOption();

        /// <summary>
        /// Query 1095-C data.
        /// </summary>
        /// <returns></returns>
        IAca1095CQuery Query1095Cs();


        /// <summary>
        /// Query 1095 Consent informaition
        /// </summary>
        /// <returns></returns>
        IAca1095CEmployeeConsentQuery Query1095CEmployeeConsent();

        /// <param name="clientId">Client to get eligible employees for.</param>
        /// <param name="start">Start date to begin checking eligibility.</param>
        /// <param name="end">End date to end checking eligibility.</param>
        /// <param name="employeeId">If specified checks only the specified employee's eligibility.</param>
        /// <returns></returns>
        IEnumerable<AcaEligibleEmployeeDto> GetEmployeesEligibleListForAca(int clientId, DateTime start, DateTime end, int? employeeId = null); 
        
        /// <summary>
        /// </summary>
        /// <returns></returns>
        IAcaAleMemberQuery          AcaAleMemberQuery();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IAcaAleMemberClientQuery    AcaAleMemberClientQuery();

        /// <summary>
        /// THIS IS BASICALLY STATIC CONFIGURATION FOR THE MANIFEST.
        /// CONFUSING: SEE AcaClientConfiguration stufff.
        /// lowfix: jay: aca: rename this two, this is confusing. 
        /// </summary>
        /// <returns></returns>
        IAcaConfigurationQuery AcaConfigurationQuery();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IAcaTransmissionQuery AcaTransmissionQuery();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IAcaTransmissionSubmissionQuery AcaTransmissionSubmissionQuery();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IAcaTransmissionRecordQuery AcaTransmissionRecordQuery();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IAcaCompanyApprovalStatusInfoQuery QueryAcaCompanyApprovalStatusInfo();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IDominionContactQuery DominionContactQuery();

        /// <summary>
        /// Queries ACA client configuration data.
        /// This is per year, per client.
        /// DO NOT CONFUSE WITH AcaConfiguration WHICH IS CONFUSING, BUT DO NOT BE CONFUSED BECAUSE I SAID.
        /// lowfix: jay: aca: rename this two, this is confusing. 
        /// </summary>
        /// <returns></returns>
        IAcaClientConfigurationQuery AcaClientConfigurationsQuery();
        
        /// <summary>
        /// Query Employee Approval by Client an Year.
        /// </summary>
        /// <returns></returns>
        IAcaEmployeeApprovalQuery AcaEmployeeApprovalQuery();

        IAcaCompanyStatusQuery AcaCompanyStatusQuery();

        /// <summary>
        /// Query transmission status type info.
        /// </summary>
        /// <returns></returns>
        IAcaTransmissionStatusTypeQuery AcaTransmissionStatusTypeQuery();
        
        /// <summary>
        /// Query submission status type info.
        /// </summary>
        /// <returns></returns>
        IAcaTransmissionSubmissionStatusTypeQuery AcaTransmissionSubmissionStatusTypeQuery();
        
        /// <summary>
        /// Query record status type info.
        /// </summary>
        /// <returns></returns>
        IAcaTransmissionRecordStatusTypeQuery AcaTransmissionRecordStatusTypeQuery();

        /// <summary>
        /// Query error codes.
        /// </summary>
        /// <returns></returns>
        IAcaErrorCodeQuery AcaErrorCodeQuery();

        /// <summary>
        /// Query transmission errors.
        /// </summary>
        /// <returns></returns>
        IAcaTransmissionErrorQuery AcaTransmissionErrorQuery();

        /// <summary>
        /// Query ACA corrections.
        /// </summary>
        /// <returns></returns>
        IAcaCorrectionQuery AcaCorrectionQuery();

        /// <summary>
        /// Constructs a new query on <see cref="Aca1095CResource"/>(s).
        /// </summary>
        /// <returns></returns>
        IAca1095CResourceQuery Aca1095CResourceQuery();

    }
}