import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IEvaluationDetail } from './evaluation-detail.model';
import { ICompetencyEvaluation } from './competency-evaluation.model';
import { IGoalEvaluation } from './goal-evaluation.model';
import { IEvaluation } from './evaluation.model';
import { ContactProfileImageLoader } from '@ds/core/contacts';
import { map, take, concatMap, publishReplay, refCount } from 'rxjs/operators';
import { IFeedbackResponse, BooleanFeedbackResponse, DateFeedbackResponse, ListItemFeedbackResponse, TextItemFeedbackResponse, MultiSelectFeedbackResponse } from '@ds/performance/feedback';
import { FieldType, ViewRemark } from '@ds/core/shared';
import { throwError, Observable, BehaviorSubject } from 'rxjs';
import { AccountService } from '@ds/core/account.service';
import { IMeritIncreaseView } from './merit-increase-view.model';
import { IEvalDetailAndSyncContent } from './eval-data-and-sync-content.model';
import { createCache } from '@ds/core/shared/shared-api-fn';
import { CompetencyEvalItem } from './competency-eval-item';
import { IClientEarningDto } from '@ajs/labor/models';

@Injectable({
    providedIn: 'root'
})
export class EvaluationsApiService {
    EVALUATION_API_BASE = 'api/performance/evaluations';
    private getSyncContentAndDetailCache$: Observable<{data: IEvalDetailAndSyncContent}>
    getMeritIncreaseCache: {[reviewId: number]: Observable<IMeritIncreaseView>} = {};

    constructor(
        private http: HttpClient,
        private accountService: AccountService,
    ) { }

    getEvaluationDetail(evaluationId:number) {
        return this.http.get<IEvaluationDetail>(`${this.EVALUATION_API_BASE}/${evaluationId}/details`).pipe(this.mapEval());
    }

    getSyncContentAndDetail(evaluationId: number){
        if(this.getSyncContentAndDetailCache$ == null){
            this.getSyncContentAndDetailCache$ = createCache(() => this.http.get<{data: IEvalDetailAndSyncContent}>(`${this.EVALUATION_API_BASE}/${evaluationId}/sync-content-and-detail`).pipe(map(response => {
                response.data.detail = this.feedbackResponseMapper(response.data.detail);
                response.data.syncContent = this.feedbackResponseMapper(response.data.syncContent);
                return response;
            })));
        }
        return this.getSyncContentAndDetailCache$;
    }

    private typeFeedbackResponse = function(rawJson: IFeedbackResponse){
        let response: IFeedbackResponse;
        switch(rawJson.fieldType) {
            case FieldType.Boolean:
                response = new BooleanFeedbackResponse();
                break;
            case FieldType.Date:
                response = new DateFeedbackResponse();
                break;
            case FieldType.List:
                response = new ListItemFeedbackResponse();
                break;
            case FieldType.MultipleSelection:
                response = new MultiSelectFeedbackResponse();
                break;
            case FieldType.Text:
                response = new TextItemFeedbackResponse();
                break;
            default:
                throwError("Unknown feedback type");
        }

        return Object.assign(response, rawJson);
    }
    private mapEval<TEval extends IEvaluation>() {
        return map<TEval, TEval>(this.feedbackResponseMapper)
    }

    private readonly feedbackResponseMapper: <TEval extends IEvaluation>(evl: TEval) => TEval = (evl) => {
        if(evl != null && (<any>evl).feedbackResponses) {
            let typedResponses: IFeedbackResponse[] = [];
            (<any>evl).feedbackResponses.forEach(r => {
                typedResponses.push(this.typeFeedbackResponse(r));
            });
            (<any>evl).feedbackResponses = typedResponses;
        }

        return evl;
    }

    syncEvaluationContent(evaluationId:number) {
        return this.http.post<IEvaluationDetail>(`${this.EVALUATION_API_BASE}/${evaluationId}/sync-content`, {}).pipe(this.mapEval());
    }

    sendReminder(evaluationId: number) {
        return this.http.get<any>(`${this.EVALUATION_API_BASE}/send-reminder/evaluation/${evaluationId}`);
    }

    removeCompetencyEvaluation(evaluationId:number, competencyId:number) {
        return this.http.delete(`${this.EVALUATION_API_BASE}/${evaluationId}/competencies/${competencyId}`, {});
    }

    saveCompetencyEvaluation(dto: ICompetencyEvaluation) {
        return this.http.post<ICompetencyEvaluation>(`${this.EVALUATION_API_BASE}/${dto.evaluationId}/competencies/${dto.competencyId}`, dto);
    }

    removeGoalEvaluation(evaluationId:number, goalId:number) {
        return this.http.delete(`${this.EVALUATION_API_BASE}/${evaluationId}/goals/${goalId}`, {});
    }

    saveGoalEvaluation(dto: IGoalEvaluation) {
        return this.http.post<IGoalEvaluation>(`${this.EVALUATION_API_BASE}/${dto.evaluationId}/goals/${dto.goalId}`, dto);
    }

    saveFeedbackResponse<TResponse extends IFeedbackResponse>(evaluationId:number, response:TResponse) {
        return this.http.post<TResponse>(`${this.EVALUATION_API_BASE}/${evaluationId}/feedback/${response.feedbackId}`, response)
            .pipe(map((data:TResponse) => this.typeFeedbackResponse(data)));
    }

    submitEvaluation<TEval extends IEvaluation>(dto: TEval) {
        return this.http.post<TEval>(`${this.EVALUATION_API_BASE}/${dto.evaluationId}/submit`, dto)
            .pipe(
                map(evl => {
                    ContactProfileImageLoader(evl.evaluatedByContact);
                    return evl;
                }),
                this.mapEval()
            );
    }

    submitEvaluationThroughApprovalProcess(dto: IEvaluationDetail) {
        return this.http.post<IEvaluationDetail>(`${this.EVALUATION_API_BASE}/${dto.evaluationId}/approval/process/submit`, dto)
            .pipe(
                map(evl => {
                    ContactProfileImageLoader(evl.evaluatedByContact);
                    return evl;
                }),
                this.mapEval()
            );
    }

    reopenEvalution(evaluationId: number) {
        return this.http.post(`${this.EVALUATION_API_BASE}/${evaluationId}/reopen`, { evaluationId: evaluationId });
    }

    revokeEvalution(evaluationId: number) {
        return this.http.post(`${this.EVALUATION_API_BASE}/${evaluationId}/revoke-from-employee`,{ evaluationId: evaluationId });
    }

    releaseEvalution(evaluationId: number) {
        return this.http.post(`${this.EVALUATION_API_BASE}/${evaluationId}/release-to-employee`,{ evaluationId: evaluationId });
    }

    saveWeightedGoals(goalWeights: any[]){
        return this.http.post(`${this.EVALUATION_API_BASE}/goalweights`,goalWeights ).pipe();
    }

    submitMeritIncrease(dto: IEvaluationDetail) {
        return this.http.post(`${this.EVALUATION_API_BASE}/merit-increase/${dto.reviewId}/submit`, dto.meritIncreaseInfo);
    }

    getMeritIncrease(employeeId: number, reviewId: number, reload: boolean): Observable<IMeritIncreaseView> {
        if (this.getMeritIncreaseCache[reviewId] != null && !reload) {
            return this.getMeritIncreaseCache[reviewId];
        }
        return this.getMeritIncreaseCache[reviewId] = this.accountService.getUserInfo().pipe(
            concatMap(userInfo => this.http.get<{ data: IMeritIncreaseView }>(`${this.EVALUATION_API_BASE}/merit-increase/employee/${employeeId}/client/${userInfo.clientId}/review/${reviewId}`)),
            publishReplay(),
            refCount(),
            map(result => result.data));
    }

    saveRemark(remark: ViewRemark, evaluationId: number, typeId: number, fk: number): Observable<ViewRemark> {
        const url = `${this.EVALUATION_API_BASE}/remarks/evaluation/${evaluationId}/type/${typeId}/foreignKey/${fk}`;
        return this.http.post<ViewRemark>(url, remark);
    }

    removeRemark(remark: ViewRemark, evaluationId: number, typeId: number, fk: number): Observable<ViewRemark> {
        const url = `${this.EVALUATION_API_BASE}/remarks/${remark.remarkId}/evaluation/${evaluationId}/type/${typeId}/foreignKey/${fk}/remove`;
        return this.http.delete<ViewRemark>(url);
    }

    approveEvaluationItem(evaluationId: number, typeId: number, fk: number, approveId: number): Observable<CompetencyEvalItem | IFeedbackResponse | IGoalEvaluation> {
        let url = `${this.EVALUATION_API_BASE}/${evaluationId}/item/${fk}/type/${typeId}/approve`;
        if (approveId != null)
            url = `${this.EVALUATION_API_BASE}/${evaluationId}/item/${fk}/type/${typeId}/approve/${approveId}`;
        return this.http.put<CompetencyEvalItem | IFeedbackResponse | IGoalEvaluation>(url, null);
    }

    approveAllEvaluationItems(evaluationId: number): Observable<any> {
        let url = `${this.EVALUATION_API_BASE}/${evaluationId}/approve/all`;
        return this.http.put<any>(url, null);
    }
    /**
     * Get IClientEarningDto list by the clientId
     * 
     * @param clientId 
     */
    getClientEarnings(clientId:number):Observable<IClientEarningDto[]> {
        let url = `${this.EVALUATION_API_BASE}/clients/${clientId}/client-earnings`;
        return this.http.get<IClientEarningDto[]>( url );
    }
}
