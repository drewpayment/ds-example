import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forEach } from 'angular';
import { IEmployeeBirthdate } from '../models/employee-birthdates.model';
import { Observable } from 'rxjs';
import { IEmployee } from '@ajs/core/ds-resource/models';
import { IEmployeeAnniversary } from '../models/employee-anniversary.model';

@Injectable({
  providedIn: 'root'
})
export class EventsComponentService {

  constructor(private http: HttpClient) { }

  getBirthdays(clientId, startDate, endDate, employeeIds, userId, userTypeId): Observable<IEmployeeBirthdate[]> {
      return this.http.post<IEmployeeBirthdate[]>(`api/employees/employee-birthdates`, { clientId, startDate, endDate, employeeIds, userId, userTypeId });
  }

  getAnniversaries(clientId, startDate, endDate, employeeIds, userId, userTypeId): Observable<IEmployeeAnniversary[]> {
      return this.http.post<IEmployeeAnniversary[]>(`api/employees/employee-anniversaries`, { clientId, startDate, endDate, employeeIds, userId, userTypeId });
  }
}