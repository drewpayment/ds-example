import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IClockEmployeeExceptionHistory } from '@ajs/labor/models/client-exception-detail.model';

@Injectable({
    providedIn: 'root'
})
export class ClockEmployeeExceptionsService {

    constructor(private http: HttpClient) { }

    private api = 'api/clock-employee-exception';

    getExceptionsByPunchIds(dto: number[], employeeId: number): Observable<IClockEmployeeExceptionHistory[]> {
        const url = `${this.api}/${employeeId}/exceptions`;
        return this.http.post<IClockEmployeeExceptionHistory[]>(url, dto);
    }
}
