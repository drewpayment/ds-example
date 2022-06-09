import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ClientDivisionDto } from '@ajs/ds-external-api/models/client-division-dto.model';
import {
  GeneralLedgerType,
  ClientGLControl,
  ClientGLSettings,
  ClientDepartment,
  ClientDeduction,
  ClientCostCenter,
  ClientGroupDto,
  ClientGLCustomClass,
  ClientGLAssignmentCustom,
  AssignmentFilterOptions,
  MappingFilterOptions,
  ClientGLMappingItem,
  ClientGLClassGroup,
  GeneralLedgerAccount,
} from '@models';

@Injectable({
  providedIn: 'root',
})
export class GeneralLedgerService {
  constructor(private http: HttpClient) {}

  private api = `api/general-ledger`;
  private clientAPI = `api/client`;
  private payrollAPI = `api/payroll`;

  getGeneralLedgerTypes(): Observable<GeneralLedgerType[]> {
    const url = `${this.api}/get/gl/types`;
    let params = new HttpParams();

    return this.http.get<GeneralLedgerType[]>(url, { params: params });
  }

  getClientGLControl(): Observable<ClientGLControl> {
    const url = `${this.api}/get/client/gl/control`;
    let params = new HttpParams();

    return this.http.get<ClientGLControl>(url, { params: params });
  }

  getClientGLSettings(clientId: number): Observable<ClientGLSettings> {
    const url = `${this.api}/get/client/${clientId}/gl/settings`;
    let params = new HttpParams();
    params = params.append('clientId', clientId.toString());

    return this.http.get<ClientGLSettings>(url, { params: params });
  }

  getClientDepartments(clientId: number): Observable<ClientDepartment[]> {
    const url = `${this.clientAPI}/${clientId}/client-departments-all`;
    let params = new HttpParams();
    params = params.append('clientId', clientId.toString());

    return this.http.get<ClientDepartment[]>(url, { params: params });
  }

  getClientDeductions(): Observable<ClientDeduction[]> {
    const url = `${this.payrollAPI}/clientDeductions`;
    let params = new HttpParams();

    return this.http.get<ClientDeduction[]>(url, { params: params });
  }

  getClientCostCenters(clientId: number): Observable<ClientCostCenter[]> {
    const url = `${this.clientAPI}/${clientId}/client-cost-center-all`;
    let params = new HttpParams();
    params = params.append('clientId', clientId.toString());

    return this.http.get<ClientCostCenter[]>(url, { params: params });
  }

  getClientGroups(clientId: number): Observable<ClientGroupDto[]> {
    const url = `${this.clientAPI}/clientGroups/${clientId}`;
    let params = new HttpParams();
    params = params.append('clientId', clientId.toString());

    return this.http.get<ClientGroupDto[]>(url, { params: params });
  }

  getClientDivisions(clientId: number): Observable<ClientDivisionDto[]> {
    const url = `${this.clientAPI}/${clientId}/client-divisions`;
    let params = new HttpParams();
    params = params.append('clientId', clientId.toString());

    return this.http.get<ClientDivisionDto[]>(url, { params: params });
  }

  getClientDivisionsWithDepartments(): Observable<ClientDivisionDto[]> {
    const url = `${this.api}/get/client/gl/divisions`;
    let params = new HttpParams();

    return this.http.get<ClientDivisionDto[]>(url, { params: params });
  }

  getClientGLCustomClass(): Observable<ClientGLCustomClass[]> {
    const url = `${this.api}/get/client/gl/custom-class`;
    let params = new HttpParams();

    return this.http.get<ClientGLCustomClass[]>(url, { params: params });
  }

  getClientGLAssignmentItems(
    assignmentId: number,
    foreignKeyId: number
  ): Observable<ClientGLAssignmentCustom> {
    const url = `${this.api}/get/client/gl/assignment/${assignmentId}/key/${foreignKeyId}`;
    let params = new HttpParams();
    params = params.append('assignmentId', assignmentId.toString());
    params = params.append('foreignKeyId', foreignKeyId.toString());
    return this.http.get<ClientGLAssignmentCustom>(url, { params: params });
  }

  getAssignmentFilterOptions(): Observable<AssignmentFilterOptions> {
    const url = `${this.api}/get/client/gl/assignment/filter/options`;
    let params = new HttpParams();

    return this.http.get<AssignmentFilterOptions>(url, { params: params });
  }

  getMappingFilterOptions(): Observable<MappingFilterOptions> {
    const url = `${this.api}/get/client/gl/mapping/filter/options`;
    let params = new HttpParams();

    return this.http.get<MappingFilterOptions>(url, { params: params });
  }

  getMappingItems(
    mappingAssignmentId: number,
    generalLedgerTypeId: number,
    foreignKeyId: number
  ): Observable<ClientGLMappingItem[]> {
    const url = `${this.api}/get/client/gl/mapping/${mappingAssignmentId}/type/${generalLedgerTypeId}/key/${foreignKeyId}`;
    let params = new HttpParams();

    return this.http.get<ClientGLMappingItem[]>(url, { params: params });
  }

  getClientGeneralLedgerAccounts(): Observable<GeneralLedgerAccount[]> {
    const url = `${this.api}/get/client/gl/accounts`;
    let params = new HttpParams();

    return this.http.get<GeneralLedgerAccount[]>(url, { params: params });
  }

  getClientGeneralLedgerClassGroups(): Observable<ClientGLClassGroup[]> {
    const url = `${this.api}/get/client/gl/class-group`;
    let params = new HttpParams();

    return this.http.get<ClientGLClassGroup[]>(url, { params: params });
  }

  saveClientGLControl(control: ClientGLControl) {
    let url = `${this.api}/update/client/gl/control`;
    return this.http.post<ClientGLControl>(url, control);
  }

  saveClientGLSettings(settings: ClientGLSettings) {
    let url = `${this.api}/update/client/gl/settings`;
    return this.http.post<ClientGLSettings>(url, settings);
  }
  saveClientGLAssignments(assignments: ClientGLAssignmentCustom) {
    let url = `${this.api}/update/client/gl/assignments`;
    return this.http.post<ClientGLAssignmentCustom>(url, assignments);
  }
  saveClientGLCustomClass(customClass: ClientGLCustomClass) {
    let url = `${this.api}/update/client/gl/custom-class`;
    return this.http.post<ClientGLCustomClass>(url, customClass);
  }

  saveClientGLMappingItems(
    mappingItems: ClientGLMappingItem[],
    defaultAccount: number
  ) {
    if (defaultAccount == null) defaultAccount = 0;
    let url = `${this.api}/update/client/gl/mappings/${defaultAccount}`;
    return this.http.post<ClientGLMappingItem[]>(url, mappingItems);
  }
}
