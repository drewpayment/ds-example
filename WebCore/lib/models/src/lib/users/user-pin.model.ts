import { ClientContact } from '@ds/admin/client-statistics/shared/models/clientContact';
import { UserInfo } from '@ds/core/shared';


export interface UserPin {
  userPinId?: number;
  userId: number | null;
  clientContactId?: number | null;
  clientId: number;
  pin: string;
  modified?: Date;
  user?: UserInfo;
  clientContact?: ClientContact;
}
