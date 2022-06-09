import { Moment } from 'moment';

export interface IClientOrganization {
    clientOrganizationId: number;
    clientOrganizationName: String;
    selectedOrganizationId: number;
    clientOrganizationClient: Array<IClientOrganizationClient>;
    IsNew: Boolean;
}

export interface IClientOrganizationClient {
    clientId: number;
    clientCode: String;
    clientName: String;
    isAssigned: Boolean;
    organizationId?: number;

    isSelected: boolean;
}

export interface UpdateOrganizationRequest {
    clientOrganizationId: number;
    clientOrganizationName: string;
    isNew: boolean;
}