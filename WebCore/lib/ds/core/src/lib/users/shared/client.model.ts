
/**
 * TODO: This needs to be moved to the right module.
 * This is the DTO that comes back from an 'api/clients' call.
 */
export interface IClientInfo {
    clientStatus: string;
    clientStatusCode: string;
    clientStatusId: number;
    clientName: string;
    clientId: number;
    clientCode: string;
    isCurrentlyActive: boolean;
}