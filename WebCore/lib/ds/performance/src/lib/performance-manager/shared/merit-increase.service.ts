import { Injectable } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { IMeritIncreaseView } from '@ds/performance/evaluations';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IncreaseType } from '@ds/performance/competencies/shared/increase-type';
import { RecommendedBonus } from '@ds/performance/evaluations/shared/recommended-bonus';

@Injectable({
  providedIn: 'root'
})
export class MeritIncreaseService {

  private readonly api = 'api/payroll/merit-increase/';

  constructor(
    private accntSvc: AccountService,
    private http:HttpClient) { }

  ApproveIncrease(reviewId: number, meritIncrease: IMeritIncreaseView, employeeId: number, approve: boolean): Observable<{data: any}> {
    return this.accntSvc.PassUserInfoToRequest(userInfo =>
      this.http.post<{data: any}>(`${this.api}approve-request/review/${reviewId}/employee/${employeeId}/client/${userInfo.lastClientId || userInfo.clientId}/approval/${approve}`, meritIncrease)
      );
  }

  ReopenProposal(proposalId: number, employeeId: number): Observable<{data: any}> {
    return this.accntSvc.PassUserInfoToRequest(userInfo =>
      this.http.get<{data: any}>(`${this.api}reopen-proposal/proposal/${proposalId}/employee/${employeeId}/client/${userInfo.lastClientId || userInfo.clientId}`)
      );
  }

  CalculateBonus(increaseType: IncreaseType, increaseAmount: number, employeeId: number, reviewId: number): Observable<number> {
    return this.accntSvc.PassUserInfoToRequest(userInfo =>
      this.http.get<number>(`${this.api}calculate-bonus/increase-type/${increaseType}/increase-amount/${increaseAmount}/client/${userInfo.lastClientId || userInfo.clientId}/employee/${employeeId}/review/${reviewId}`)
      );
  }

  CalculateRecommendedBonus(reviewId: number, reviewedEmployeeId: number): Observable<RecommendedBonus> {
    return this.accntSvc.PassUserInfoToRequest(userInfo =>
      this.http.get<RecommendedBonus>(`${this.api}calculate-recommnded-bonus/review/${reviewId}/client/${userInfo.selectedClientId()}/reviewed-employee/${reviewedEmployeeId}`)
      );
  }
}
