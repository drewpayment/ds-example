import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FinalizeCompletionStatusCheck } from '../shared';
import { Observable } from 'rxjs';
import { AccountService } from '@ds/core/account.service';


@Injectable()
export class OnboardingService {

    constructor(private http: HttpClient) {}

    getFinalizeStatus(employeeId: number): Observable<FinalizeCompletionStatusCheck> {
        return this.http.get<FinalizeCompletionStatusCheck>(`api/employees/${employeeId}/onboarding/finalize-check`);
    }

}
