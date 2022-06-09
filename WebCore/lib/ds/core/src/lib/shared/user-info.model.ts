import { UserType } from './user-type';
import { UserBetaFeature } from '.';


export interface UserInfo {
    userId: number;
    userTypeId: UserType;
    userName: string;
    employeeId: number | null;
    firstName: string;
    lastName: string;
    middleInitial: string;
    clientId: number | null;
    lastClientId: number | null;
    lastClientCode: string;
    lastClientName: string;
    lastEmployeeId: number | null;
    lastEmployeeFirstName: string | null;
    lastEmployeeMiddleInitial: string | null;
    lastEmployeeLastName: string | null;
    timeoutMinutes?: number | null;
    emailAddress: string;
    $isEmulating?: boolean;
    /** When the user is a company admin we sometimes lose the company admin's employeeId.
     * This property will take that value when it is lost.
     * @see AccountService.updateDataStoreUser */
    userEmployeeId?: number;
    userFirstName: string;
    userLastName: string;
    betaFeatures: UserBetaFeature[];

    certifyI9?: boolean;
    addEmployee?: boolean;
    isInOnboarding?: boolean;

    isEmployeeSelfServiceViewOnly: boolean;

    isBillingAdmin: boolean;
    isHrBlocked: boolean;

    changeRequestRequired: boolean;

    isApplicantAdmin: boolean;
    isArAdmin: boolean;
    isPayrollAccessBlocked: boolean;

    isAllowedToAddSystemAdmin: boolean;
    
    /**
     * Return the active client ID for the current user.  First
     * checks {@link UserInfo.lastClientId} then {@link UserInfo.clientId}.
     */
    selectedClientId(): number | null;
    /**
     * Return the active employee ID for the current user. First
     * checks {@link UserInfo.lastEmployeeId} then {@link UserInfo.employeeId}.
     */
    selectedEmployeeId(): number | null;
    /**
     * Pass a usertype to ensure the user is at a minimum user type level.
     */
    isInMinimumRole(type: UserType): boolean;
    /**
     * Pass one or more UserType to check if the user is a specific UserType.
     * @param types
     */
    isRole(...types: UserType[]): boolean;
}
