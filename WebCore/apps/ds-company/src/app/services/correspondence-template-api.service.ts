import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IApplicantEmailTemplateData, ApplicantCorrespondenceTypeEnum, 
        IApplicantCorrespondenceTypeData, ICustomizeSenderData } from "apps/ds-company/src/app/models/correspodence-template-data";
import { ClientBankInfoModule } from '@ds/core/clients/client-bank-info';
import { IEmailData } from '@ajs/applicantTracking/shared/models';
import { ResourceType } from '@ajs/core/ds-resource/models';
import { IAzureViewDto } from '@ds/core/resources/shared/azure-view-dto.model';
import { Observable } from 'rxjs/internal/Observable';
//import { Client } from './models/client';
//import { ClientPayroll } from './models/clientPayroll';

@Injectable({
  providedIn: 'root'
})
export class CorrespondenceTemplateApiService {

    constructor(private httpClient: HttpClient) { }

    getApplicantCompanyCorrespondences(clientId, correspondenceTypeId, isText = false) {
        let params = new HttpParams();
        params = params.append('clientId', clientId);
        params = params.append('correspondenceTypeId', correspondenceTypeId || "");
        params = params.append('isText', isText.toString());
        params = params.append('isApplicantAdmin', 'true');

        return this.httpClient.get<IApplicantEmailTemplateData[]>('api/applicant-tracking/company-correspondences', { params: params });
        
    }

    getOnboardingCompanyCorrespondences(clientId, correspondenceTypeId, isText = false) {
        let params = new HttpParams();
        params = params.append('clientId', clientId);
        params = params.append('correspondenceTypeId', correspondenceTypeId || "");
        params = params.append('isText', isText.toString());
        params = params.append('isApplicantAdmin', 'false');

        return this.httpClient.get<IApplicantEmailTemplateData[]>('api/applicant-tracking/company-correspondences', { params: params });
        
    }
    
    getClientSMTPSetting(clientId) {
        return this.httpClient.get<ICustomizeSenderData>(
            `api/client/clientSMTPSetting/${clientId}`);
    }

    saveClientSMTPSetting(smtpSetting) {
        return this.httpClient.post<ICustomizeSenderData>(
            `api/client/updateClientSMTPSetting` , smtpSetting);               
    }

    testSMTPSetting(smtpSetting) {
        return this.httpClient.post<IEmailData>(
            `api/client/testSMTPSetting` , smtpSetting); 
    }

    getClientResource(
        resourceType:ResourceType,
        clientId:number,
        name:string
    ):Observable<IAzureViewDto> {
        return this.httpClient.get<IAzureViewDto>(
            `api/resources/${+resourceType}/clients/${clientId}/files/${name}`);
    }

    saveApplicantCompanyCorrespondence(dto) {
        var compCorrespondenceId = dto.applicantCompanyCorrespondenceId || "";

        return this.httpClient.post<IApplicantEmailTemplateData>(
            `api/applicant-tracking/company-correspondences/${compCorrespondenceId}` , dto); 
    }

    deleteApplicantCompanyCorrespondence(dto) {
        return this.httpClient.delete<boolean>(
            `api/applicant-tracking/company-correspondences/${dto.applicantCompanyCorrespondenceId}/isapplicant-admin/${dto.isApplicantAdmin}`); 
    }

}
