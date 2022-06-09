import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import {
  IClientAccrual,
  ClientEarning,
} from "@ds/core/employee-services/models";
import { EmployeePayType } from "../../models/leave-management/employee-pay-type";
import { User } from "@ds/core/employee-services/models/user.model";
import { Observable, of } from "rxjs";
import { ClientAccrualDropdownsDto, IClientCalendar } from "../../models/leave-management/client-accrual-dropdowns-dto";
import { IPayrollHistoryInfo } from "@ds/payroll/shared";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: "root",
})
export class LeaveManagementApiService {
  private readonly api = "api/leave-management";
  private readonly clientAccrualApi = `${this.api}/client-accrual`;
  private readonly clientApi = "api/clients";
  private readonly payrollApi = "api/payroll";
  private readonly userInfoApi = "api/account/userInfo";

  constructor(private http: HttpClient) {}

  // Inserts new, or updates existing ClientAccrual.
  // Default autoApplyAccrualPolicyOptionId sets it to NOT auto apply.
  postClientAccrual = (dto: IClientAccrual): Observable<IClientAccrual> => {
    const url = `${this.clientAccrualApi}/${
      dto.clientAccrualId || 0
    }/auto-apply-option/${dto.autoApplyAccrualPolicyOptionId || 0}`;
    return this.http.post<IClientAccrual>(url, dto);
  }

  deleteClientAccrualLeaveManagementPendingAwards = (clientAccrualId: number): Observable<string> => {
    const url = `${this.clientAccrualApi}/${clientAccrualId}/leave-management-pending-awards/delete-not-adjustments`;
    return this.http.delete<string>(url);
  }

  deleteClientAccrual = (clientAccrualId: number): Observable<string> => {
    const url = `${this.clientAccrualApi}/${clientAccrualId}`;
    return this.http.delete<string>(url);
  }

  getClientAccrual = (
    clientAccrualId: number,
    setDefaultsForFrontEnd: boolean
  ): Observable<IClientAccrual> => {
    const url = `${this.clientAccrualApi}/${clientAccrualId}?setDefaultsForFrontEnd=${setDefaultsForFrontEnd}`;
    return this.http.get<IClientAccrual>(url);
  }

  getClientAccrualsList = (
    clientId: number,
    setDefaultsForFrontEnd: boolean
  ) => {
    const url = `${this.clientApi}/${clientId}/client-accruals?setDefaultsForFrontEnd=${setDefaultsForFrontEnd}`;
    return this.http.get<IClientAccrual[]>(url);
  }

  getClientAccrualsDropdownList = (clientId: number) => {
    const url = `${this.clientApi}/${clientId}/client-accruals/dropdown-list`;
    return this.http.get<IClientAccrual[]>(url);
  }

  getClientEarningsList = (clientId: number, orderByDescription: boolean) => {
    const url = `${this.clientApi}/${clientId}/client-earnings?orderByDescription=${orderByDescription}`;
    return this.http.get<ClientEarning[]>(url);
  }

  // Backend currently excludes {clientEarnings, employeePayTypes, companyAdmins} from result.
  getClientAccrualDropdownDtos = (): Observable<ClientAccrualDropdownsDto> => {
    const url = `${this.clientAccrualApi}/dropdown-dtos`;
    return this.http.get<ClientAccrualDropdownsDto>(url);
  }

  getEmployeePayTypeList = () => {
    const url = `${this.payrollApi}/pay/info`;
    return this.http.get<EmployeePayType[]>(url);
  }

  getCompanyAdminList = (clientId: number, orderByLastnameFirstname: boolean) => {
    const url = `${this.userInfoApi}/CompanyAdminByClientId/${clientId}?orderByLastnameFirstname=${orderByLastnameFirstname}`;
    return this.http.get<User[]>(url);
  }

  getClientCalendar = (clientId: number) => {
    const url = `api/payroll/clientCalendar/${clientId}`;
    return this.http.get<IClientCalendar>(url)
    .pipe(map(clientCalendar => {
      const result = {
        calendarFrequencyAltWeekId: clientCalendar.calendarFrequencyAltWeekId,
        calendarFrequencyBiWeeklyId: clientCalendar.calendarFrequencyBiWeeklyId,
        calendarFrequencyMonthlyId: clientCalendar.calendarFrequencyMonthlyId,
        calendarFrequencyQuarterlyId: clientCalendar.calendarFrequencyQuarterlyId,
        calendarFrequencySemiMonthlyId: clientCalendar.calendarFrequencySemiMonthlyId,
        calendarFrequencyWeeklyId: clientCalendar.calendarFrequencyWeeklyId,
      } as IClientCalendar;
      return result;
    }));
  }
}
