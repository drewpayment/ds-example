import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ScoreModelSettings } from "../evaluations/shared/score-model-settings.model";
import { take, concatMap, map, publishReplay, refCount } from "rxjs/operators";
import { HttpClient } from "@angular/common/http";
import { AccountService } from "@ds/core/account.service";

/**
 * A service for Performance reviews that can be used anywhere.  This service should only have service dependencies that are in the core module.
 */
@Injectable({
    providedIn: 'root'
})
export class SharedPerformanceReviewsService {
    private readonly api = 'api/performance';
    private ScoringSettings: {[id: number]: Observable<{data: ScoreModelSettings}>};

constructor(private http: HttpClient, private account: AccountService) {
    this.ScoringSettings = {};
 }

    getScoringSettings(reviewId: number, reloadData: boolean = false): Observable<{data: ScoreModelSettings}> {
        return this.ScoringSettings[reviewId] == null || reloadData ? this.ScoringSettings[reviewId] = this.account.getUserInfo().pipe(
                take(1),
            concatMap(userInfo => this.http.get<{ data: ScoreModelSettings }>(this.api + `/clients/${userInfo.lastClientId || userInfo.clientId}/review/${reviewId}/is-scoring-enabled`).pipe(
                map(x => {
                    if(x.data == null){
                        x.data = <ScoreModelSettings>{};
                    }
                    return x;
                })
            )),
            publishReplay(),
            refCount()
        ) : this.ScoringSettings[reviewId];
    }

}