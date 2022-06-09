import { Moment } from 'moment';
import { UserType } from '@ds/core/shared/user-type';
import { UserInfo } from '@ds/core/shared/user-info.model';

export interface IAlert {
  isNew: boolean;
  hovered: boolean;

  alertId: number;
  datePosted: Date;
  alertText: string;
  alertLink: string;
  dateStartDisplay: Date;
  dateEndDisplay: Date;
  alertType: AlertType;
  securityLevel: SecurityLevel;
  clientId?: number;
  modified: Date;
  modifiedBy: string;
  alertCategoryId: AlertCategory;
  title: string;
  isExpired: boolean;
}

export enum SecurityLevel {
  SystemAdminOnly = 1,
  CompanyAdminHigher = 2,
  AllEmployees = 3,
  SupervisorsAndHigher = 4,
  AllApplicants = 5,
  TimeOffOnly = 9,
}
export enum AlertType {
  Dominion = 1,
  Company = 2,
}
export enum AlertCategory {
  Attachment = 1,
  Memo = 2,
  Link = 3,
}
