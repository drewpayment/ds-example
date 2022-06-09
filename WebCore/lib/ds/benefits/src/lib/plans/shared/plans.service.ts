import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { IPlanAdminSummary, IPlan, ICopyPlanConfig  } from './plans.model'

@Injectable({
    providedIn: 'root'
})
export class PlansService {

    static readonly PLANS_API_BASE = "api/benefits/plans";

    constructor(private httpClient: HttpClient) {

    }

    getPlanAdminSummary(clientId: number) {
        return this.httpClient.get<IPlanAdminSummary[]>(`${PlansService.PLANS_API_BASE}/${clientId}`);
    }

    getPlanAdminSummaryDatesAndPlan(clientId: number) {
        return this.httpClient.get<IPlanAdminSummary[]>(`${PlansService.PLANS_API_BASE}/${clientId}/datesandplan`);
    }

    copyPlans(input: ICopyPlanConfig): Observable<ICopyPlanConfig> {
        return this.httpClient.post<ICopyPlanConfig>(`${PlansService.PLANS_API_BASE}/copy-plans`, input);
    }
}
