import { MOMENT_FORMATS, UserType } from '@ds/core/shared';
import { EmployeeStatusType } from '@ds/employees/models/employee-status.enum';
import { EmployeeBasic } from './employee.model';
import * as bcrypt from 'bcryptjs';
import * as moment from 'moment';

const SALT = bcrypt.genSaltSync(10);

export interface UserProfile {
  // GENERAL
  userId: number;
  displayName?: string;
  userType: UserType;
  firstName: string;
  lastName: string;
  employeeId?: number;
  employeeStatusType?: EmployeeStatusType;
  employee?: EmployeeBasic;

  // ACCOUNT SETTINGS
  username: string;
  password?: string;
  verifyPassword?: string;
  email: string;
  forceUserPasswordReset?: boolean;
  isAccountEnabled: boolean;
  isUserDisabled: boolean;

  // APPLICATION SETTINGS
  sessionTimeout?: number;
  userPin?: number;
  viewEmployeesType?: UserViewEmployeePayType | string;
  viewRatesType?: UserViewEmployeePayType | string;
  isEssViewOnly: boolean;
  blockHr: boolean;
  hasEssSelfService: boolean;
  hasEmployeeAccess: boolean;
  isReportingAccessOnly: boolean;
  hasGLAccess: boolean;
  blockPayrollAccess: boolean;
  hasTaxPacketsAccess: boolean;
  isApplicantTrackingAdmin: boolean;
  hasTimeAndAttAccess: boolean;
  isEmployeeNavigatorAdmin: boolean;
  isTimeclockAppOnly: boolean;
  hasTempAccess?: boolean;
  fromDate?: Date | null;
  toDate?: Date | null;
}

export interface UpdateUserProfileAccountDisableRequest {
  userId: number;
  isAccountEnabled?: boolean;
  isDisabled?: boolean;
}

export enum UserViewEmployeePayType {
  hourly = 1,
  salary,
  both,
  none
}

export class NewUserRequest {
  firstName: string;
  lastName: string;
  username: string;
  encryptedPassword: string;
  password1: string;
  password2: string;
  email: string;
  dsUserType: UserType;
  employeeId: number;
  clientId: number;
  viewEmployees: number;
  viewRates: number;
  securitySettings = true;
  allowUnsafePass = false;
  isStruckOut: boolean;
  viewOnly: boolean;
  employeeSelfServiceOnly: boolean;
  reportingOnly: boolean;
  blockPayrollAccess: boolean;
  timeclock: boolean;
  isTimeClockDeviceUser: boolean;
  blockHR: boolean;
  employeeOnly: boolean;
  applicantAdmin: boolean;
  fromDate?: Date | string | null;
  toDate?: Date | string | null;
  timeout: number;
  viewTaxPackets: boolean;
  mustChangePwd: boolean;
  editGl: boolean;
  permissions: UserPermission;
  dsModifiedBy: number = 0;
  dsUserId: number;

  constructor(user: any = null) {
    if (user) {
      if (user.userId) this.dsUserId = user.userId;
      this.firstName = user.firstName;
      this.lastName = user.lastName;
      this.dsUserType = user.userType;
      this.username = user.username;

      if (user.passwords.password) {
        this.encryptedPassword = this.hashPassword(user.passwords.password);
      }

      this.password1 = user.passwords.password;
      this.password2 = user.passwords.verifyPassword;
      this.email = user.email;
      this.employeeId = user.employeeId;
      this.clientId = user.clientId;
      this.securitySettings = true;
      this.allowUnsafePass = false;
      this.isStruckOut = false;
      this.viewOnly = false;
      this.employeeSelfServiceOnly = user.hasEssSelfService;
      this.reportingOnly = user.isReportingAccessOnly;
      this.blockPayrollAccess = user.blockPayrollAccess;
      this.blockHR = user.blockHr;
      this.employeeOnly = user.isEssViewOnly;
      this.timeclock = user.hasTimeAndAttAccess;
      this.applicantAdmin = user.isApplicantTrackingAdmin;
      this.timeout = user.sessionTimeout;

      if (user.hasTempAccess && user.tempAccess) {
        this.fromDate = moment(user.tempAccess.fromDate).hours(0).minutes(0).seconds(0).format(MOMENT_FORMATS.API);
        this.toDate = moment(user.tempAccess.toDate).hours(23).minutes(59).seconds(59).format(MOMENT_FORMATS.EOD);
      }

      this.viewTaxPackets = user.hasTaxPacketsAccess;
      this.mustChangePwd = user.forceUserPasswordReset;
      this.editGl = user.hasGLAccess;
      this.permissions = {
        isEmployeeNavigatorAdmin: user.isEmployeeNavigatorAdmin,
        userId: user.userId,
      };

      this.setViewEmployeeAndRates(user);
    }
  }

  private hashPassword(pw: string): string {
    return bcrypt.hashSync(pw, SALT);
  }

  private setViewEmployeeAndRates(user: any) {
    if (!!user && user.userType == UserType.systemAdmin) {
      this.viewEmployees = UserViewEmployeePayType.both;
      this.viewRates = UserViewEmployeePayType.both;
      this.employeeSelfServiceOnly = false;
      this.reportingOnly = false;
    } else if (!!user &&
      user.userType == UserType.companyAdmin ||
      user.userType == UserType.supervisor
    ) {
      this.viewEmployees = user.viewEmployeesType;
      this.viewRates = user.viewRatesType;
    } else {
      this.viewEmployees = UserViewEmployeePayType.none;
      this.viewRates = UserViewEmployeePayType.none;
      this.reportingOnly = false;
    }
  }
}

export interface UserPermission {
  userId?: number;
  isEmployeeNavigatorAdmin: boolean;
}
