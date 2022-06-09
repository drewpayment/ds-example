import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IEmployeeSearchFilter } from '@ajs/employee/search/shared/models';
import { DashboardSession } from '@ds/analytics/models/dashboard-session.model';
import { DashboardFilterOption } from '@ds/analytics/models';

@Injectable({
  providedIn: 'root'
})
export class DashboardApiService {

    private searchFilters: DashboardFilterOption[];

  constructor(private http: HttpClient) { }

  GetDashboardConfig(id){
    return this.http.get(`api/dashboard/GetDashboardConfig/${id}`)
  }

  GetDashboardList(){
    return this.http.get(`api/dashboard/GetDashboardConfig/all`)
  }

  GetDashboardWidgets(id){
    return this.http.get(`api/dashboard/GetDashboardConfig/object/${id}`)
  }

  GetFilters(){
    return this.http.get<IEmployeeSearchFilter[]>(`api/employees/search/options`)
  }

  GetEmployeeIdsFromFilters(filters){
    return this.http.post(`api/employees/employeeIds/filter`, filters)
  }

  GetDashboardSession(){
    return this.http.get<DashboardSession>(`api/dashboard/GetDashboardSession`)
  }

  SaveDashboardSession(sessionData){
    return this.http.post<DashboardSession>(`api/dashboard/SaveDashboardSession`, { DashboardId: sessionData.DashboardId, FilterData: sessionData.FilterData })
  }

  setSearchFilters(filters: DashboardFilterOption[]) {
      if (!filters || !filters.length) return;
      this.searchFilters = filters;
  }

  getSearchFilters(): DashboardFilterOption[] {
      return this.searchFilters;
  }
}
