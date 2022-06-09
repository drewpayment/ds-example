import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TimeCardAuthorizationSearchOptions } from '../shared/time-card-authorization-search-options.model';
import { Observable, BehaviorSubject } from 'rxjs';
import { EmployeePunchListCountResult } from '../shared/employee-punch-list-count-result.model';
import { coerceNumberProperty } from '@angular/cdk/coercion';


@Injectable({
    providedIn: 'root'
})
export class TimeCardAuthorizationService {
    
    private apiBase = `api/time-card-authorization`;
    
    private _totalPages = new BehaviorSubject<number>(null);
    totalPages = this._totalPages.asObservable();
    
    private _totalEmployees = new BehaviorSubject<number>(null);
    totalEmployees = this._totalEmployees.asObservable();
    
    /**
     * Updates the total pages observable
     */
    updateTotalPages(value: number) {
        if (!value) return;
        const numberValue = coerceNumberProperty(value);
        this._totalPages.next(numberValue);
    }
    
    /**
     * Updates the total employees observable
     */
    updateTotalEmployees(value: number) {
        if (!value) return;
        const numberValue = coerceNumberProperty(value);
        this._totalEmployees.next(numberValue);
    }
    
    constructor(private http: HttpClient) {}
    
    getApprovalListCount(filterOptions: TimeCardAuthorizationSearchOptions): Observable<EmployeePunchListCountResult> {
        const url = `${this.apiBase}/approval-list`;
        return this.http.post<EmployeePunchListCountResult>(url, filterOptions);
    }
    
}
