import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { IEmployeeTaxAdmin, IBasicTax, IEmployeeTaxCostCenterConfiguration, IEmployeeNonFederalTax, KeyValue } from '@models';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Injectable({
  providedIn: "root",
})
export class EmployeeTaxesService {
  private api = `api/employee/taxes`;

  constructor(
    private http: HttpClient, 
    private msg: NgxMessageService
  ) {}

  getEmployeeTaxInfo(clientId: number, employeeId: number) {
    const url = `${this.api}/info/${clientId}/${employeeId}`;
    return this.http.get<IEmployeeTaxAdmin>(url);
  }

  getClientSutaStateList(clientId: number, clientTaxId?: number) {
    const url = `${this.api}/sutaStateList/${clientId}/${clientTaxId}`;
    return this.http.get<IBasicTax[]>(url);
  }

  saveEmployeeTaxes(employeeTaxInfo: IEmployeeTaxAdmin) {
    const url = `${this.api}/saveTaxInfo`;
    return this.http.post<IEmployeeTaxAdmin>(url, employeeTaxInfo);
  }

  saveReminderDates(effectiveDate: string, employeeTaxInfo: IEmployeeTaxAdmin) {
    const url = `${this.api}/saveReminderDates/${effectiveDate}`;
    return this.http.post<boolean>(url, employeeTaxInfo);
  }

  getWotcReasons() {
    const url = `${this.api}/wotcReasons`;
    return this.http.get<KeyValue[]>(url);
  }

  getDefaultFilingStatuses() {
    const url = `${this.api}/defaultFilingStatusList`;
    return this.http.get<KeyValue[]>(url);
  }

  deleteEmployeeTax(employeeTaxId: number) {
    const url = `${this.api}/delete/${employeeTaxId}`;
    return this.http.post<boolean>(url, employeeTaxId);
  }

  getCostCenterList(clientId: number, employeeTaxId: number) {
    const url = `${this.api}/costCenters/${clientId}/${employeeTaxId}`;
    return this.http.get<IEmployeeTaxCostCenterConfiguration>(url);
  }

  saveEmployeeTaxCostCenters(
    clientId: number,
    employeeId: number,
    employeeTaxId: number,
    selectedCostCenters: KeyValue[]
  ) {
    const url = `${this.api}/saveEmployeeTaxCostCenters/${clientId}/${employeeId}/${employeeTaxId}`;
    return this.http.post<boolean>(url, selectedCostCenters);
  }

  getAvailableClientTaxList(clientId: number, employeeId: number) {
    const url = `${this.api}/getAvailableClientTaxList/${clientId}/${employeeId}`;
    return this.http.get<KeyValue[]>(url);
  }

  saveNewEmployeeTax(clientId: number, employeeId: number, clientTaxId: number) {
    const url = `${this.api}/saveNewEmployeeTax/${clientId}/${employeeId}/${clientTaxId}`;
    return this.http.post<IEmployeeNonFederalTax[]>(url, clientTaxId);
  }
}
