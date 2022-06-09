import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import {  IClientOrganization,IClientOrganizationClient, UpdateOrganizationRequest } from './organization.model'

@Injectable({
    providedIn: 'root'
})
export class OrganizationService {

    static readonly CLIENT_API_BASE = "api/client";    

    constructor(private httpClient: HttpClient) {

    }

    getOrganizationClientsList(clientRelationId) {
        return this.httpClient.get<IClientOrganizationClient[]>(
            `${OrganizationService.CLIENT_API_BASE}/clientorganizationclients/${clientRelationId}`);
    }
    
    deleteOrganizationClient(organizationId, clientId) {
        return this.httpClient.put<boolean>(
            `${OrganizationService.CLIENT_API_BASE}/deleteOrganizationClient/organizationId/${organizationId}` + 
            `/clientId/${clientId}`, {});
    }

    updateOrganization(dto: UpdateOrganizationRequest) {
        return this.httpClient.put<IClientOrganization[]>(
            `${OrganizationService.CLIENT_API_BASE}/updateOrganization` , dto);
    }

    deleteOrganization(organizationId) {
        return this.httpClient.put<boolean>(
            `${OrganizationService.CLIENT_API_BASE}/deleteOrganization/organizationId/${organizationId}`, {});
    }

    getOrganizationsList() {
        return this.httpClient.get<IClientOrganization[]>(
            `${OrganizationService.CLIENT_API_BASE}/organizations`);        
    }

    getOrganizationByClientId(clientId) {
        return this.httpClient.get<IClientOrganization>(
            `${OrganizationService.CLIENT_API_BASE}/organization/${clientId}`);
    }
    
    updateOrganizationClients(dto) {
        return this.httpClient.put<IClientOrganizationClient>(
            `${OrganizationService.CLIENT_API_BASE}/updateOrganizationClients` , dto);
    }
	
    getUnAssignedClients() {
        return this.httpClient.get<IClientOrganizationClient[]>(
            `${OrganizationService.CLIENT_API_BASE}/unAssignedclients`);  
    }
}
