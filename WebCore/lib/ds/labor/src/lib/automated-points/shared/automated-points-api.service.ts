import { IUpdateBalanceRequest } from './update-balance-request.model';
import { HttpClient } from '@angular/common/http';
import { IRecalcPointsRequest } from './recalc-points-request.model';
import { Injectable } from '@angular/core';
import { IRecalcPointsResult } from './recalc-points-result.model';

@Injectable({
  providedIn: 'root'
})
export class AutomatedPointsApiService {

  constructor(private httpClient: HttpClient) { }

  recalculateAutomatedPoints(request: IRecalcPointsRequest){
    return this.httpClient.post<IRecalcPointsResult>('api/clock/automated-points/recalc', request);
  }

  updateBalance(request: IUpdateBalanceRequest){
    return this.httpClient.post<IRecalcPointsResult>('api/clock/automated-points/update-balance', request);
  }
}
