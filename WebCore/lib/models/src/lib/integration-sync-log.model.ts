import { Moment } from 'moment';
import { EnSyncDirectionType, EnSyncStatusType, EnSyncType } from './employee-navigator-sync-log.model';


export interface IntegrationSyncLog {
    integrationSyncLogId: number;
    employeeId: number;
    clientId: number;
    syncType: EnSyncType;
    syncDirection: EnSyncDirectionType;
    syncStatus: EnSyncStatusType;
    hasRetriedSync: boolean;
    createdAt: Date|Moment|string;
    modifiedBy: number;
    modified: Date|Moment|string;
}
