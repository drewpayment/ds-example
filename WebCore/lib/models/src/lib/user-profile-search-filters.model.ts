import { UserType } from '@ds/core/shared';


export interface UserProfileSearchFilters {
  userType: UserType;
  includeTerminated: boolean;
}
