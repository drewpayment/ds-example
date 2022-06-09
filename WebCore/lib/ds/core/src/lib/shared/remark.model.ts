import { Moment } from 'moment';
import { UserInfo } from './user-info.model';

export interface IRemark {
    remarkId: number;
    description: string;
    addedBy: number;
    addedDate: Date|string|Moment;
    user?: UserInfo;
    isSystemGenerated: boolean;
    isArchived?: boolean;
}

/**
 * Used to control UI elements in a *ngFor on the goal-detail component.
 */
export interface ViewRemark extends IRemark {
    editItem?: boolean;
}
