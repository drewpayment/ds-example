import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EmployeeNavigatorEmpRequiredFields } from '@models';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class IntegrationsService {

  constructor(private http: HttpClient) {}

  getInvalidEmployeeNavigatorEmployees(): Observable<EmployeeNavigatorEmpRequiredFields[]> {
    const url = `api/integrations/employee-navigator/employees/invalid`;
    return this.http.get<EmployeeNavigatorEmpRequiredFields[]>(url);
  }

}
