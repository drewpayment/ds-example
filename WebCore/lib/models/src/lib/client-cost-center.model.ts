export interface ClientCostCenter {
    clientCostCenterId: number;
    clientId: number;
    code: string;
    description: string;
    isDefaultGlCostCenter: boolean | null;
    modified: Date | null;
    isActive: boolean;
}