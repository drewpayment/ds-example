import { UserType } from "@ds/core/shared";
import { IClientAccessInfo } from "./client-access-info.model";
import { UserSupervisorAccessInfo } from "../../../../../../models/src/lib/users/user-supervisor-access-info.model";
import { IClaimType } from './claim-type.model';

export interface IUserAccessInfo {
    userId: number;
    authUserId: number;
    username: string;
    lastClientId: number;
    lastEmployeeId: number;
    employeeId: number;
    employeeClientId: number;
    isApplicantTrackingAdmin: boolean;
    userType: UserType;
    isReportingOnly: boolean;
    isAnonymous: boolean;
    isBlockHr: boolean;
    isEssViewOnly: boolean;

    claims: IClaimType[];
    clientAccessInfo: IClientAccessInfo;
    supervisorAccessInfo: UserSupervisorAccessInfo;
}
