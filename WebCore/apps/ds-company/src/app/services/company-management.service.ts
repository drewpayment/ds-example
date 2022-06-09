import { IClientDepartment } from '@ajs/client/models/client-department.model';
import { IClientShift } from '@ajs/client/models/client-shift.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ClientDivision } from '@ds/core/employee-services/models';
import { ClientDivisionAddress } from '@ds/core/employee-services/models/client-division-address.model';
import { ClientDivisionLogo } from '@ds/core/employee-services/models/client-division-logo.model';
import { ClientGLClassGroup } from '@models';
import { AccountFeature } from '@ds/core/users';
@Injectable({
  providedIn: 'root'
})
export class CompanyManagementService {

  constructor(private http: HttpClient) { }

  getCompanyDepartmentInformation(clientId:number) {
    return this.http.get(`api/Client/clients/`+clientId+`/divisions/departments`);
  }

  deleteCompanyDepartment(dto: IClientDepartment, clientId:number) {
    return this.http.post(`api/Client/clients/`+clientId+`/departments/`+dto.clientDepartmentId+`/delete`, dto);
  }

  saveCompanyDepartment(dto: IClientDepartment, clientId:number) {
    return this.http.post(`api/Client/clients/`+clientId+`/departments/`+dto.clientDepartmentId, dto);
  }

  GetCompanyShiftInformation() {
    return this.http.get(`api/Client/GetCompanyShiftInformation`);
  }

  SaveCompanyShiftInformation(dto:IClientShift) {
    return this.http.post(`api/Client/SaveCompanyShiftInformation`, dto);
  }

  DeleteCompanyShiftInformation(dto:IClientShift) {
    return this.http.post(`api/Client/DeleteCompanyShiftInformation`, dto);
  }

  GetClientGLClassGroups() {
    return this.http.get(`api/general-ledger/get/client/gl/class-group`);
  }

  SaveClientGLClassGroup(dto:ClientGLClassGroup) {
    return this.http.post(`api/general-ledger/update/client/gl/class-group`, dto);
  }

  DeleteClientGLClassGroup(dto:ClientGLClassGroup) {
    return this.http.post(`api/general-ledger/delete/client/gl/class-group`, dto);
  }
  GetCompanyDivisionInformation() {
    return this.http.get(`api/Client/GetCompanyDivisionInformation`);
  }

  SaveCompanyDivisionInformation(dto:ClientDivision){
    return this.http.post(`api/Client/SaveCompanyDivisionInformation`, dto);
  }

  DeleteCompanyDivisionInformation(dto:ClientDivision){
    return this.http.post(`api/Client/DeleteCompanyDivisionInformation`, dto);
  }

  SaveClientDivisionAddress(dto: ClientDivisionAddress){
    return this.http.post(`api/Client/SaveClientDivisionAddress`, dto);
  }

  DeleteClientDivisionAddress(dto: ClientDivisionAddress){
    return this.http.post(`api/Client/DeleteClientDivisionAddress`, dto);
  }

  SaveClientDivisionLogo(dto: ClientDivisionLogo){
    return this.http.post(`api/Client/SaveClientDivisionLogo`, dto);
  }

  DeleteClientDivisionLogo(dto: ClientDivisionLogo){
    return this.http.post(`api/Client/DeleteClientDivisionLogo`, dto);
  }

  GetEmployeeExportList(clientid:number){
    return this.http.get(`api/employees/` + clientid + `/employee-export`);
  }

  GetClientAccountFeatures(clientId:number){
    var accountFeature = AccountFeature.Expensify
    return this.http.get(`api/client-accounts/${clientId}/features/${accountFeature}`)
  }

}
