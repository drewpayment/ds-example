import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { IEvaluationSummaryDialogData } from './evaluation-summary-dialog-data.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IEvaluationDetail } from '../shared/evaluation-detail.model';
import { IReview } from '@ds/performance/reviews';
import { NgForm } from '@angular/forms';
import { ISignature } from '@ds/core/signatures';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import * as moment from "moment";
import { DsApiCommonProvider } from '@ajs/core/api/ds-api-common.provider';
import { EvaluationsApiService } from '../shared/evaluations-api.service';
import { HttpErrorResponse } from '@angular/common/http';
import { EvaluationRoleType } from '../shared/evaluation-role-type.enum';
import { ScriptLoaderService } from '@ajs/google-charts/script-loader.service';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { from, forkJoin, of, iif, merge, throwError, Observable } from 'rxjs';
import { EvaluationGroupDisplay } from '../shared/evaluation-group-display.model';
import { IChartSettings } from '../shared/chart-settings.model';
import { BaseChartDirective } from 'ng2-charts';
import { ActiveEvaluationService } from '../shared/active-evaluation.service';
import { IEvaluationSummaryDialogResult } from './evaluation-summary-dialog-result.model';
import { IEvaluationWithStatusInfo } from '../shared/evaluation-status-info.model';
import { CompetencyEvalItem } from '../shared/competency-eval-item';
import { concatMap, map, tap, catchError, filter, share } from 'rxjs/operators';
import { ReviewProfilesApiService } from '@ds/performance/review-profiles/review-profiles-api.service';
import * as _ from "lodash";
import { UserInfo } from '@ds/core/shared';
import { PerformanceManagerService } from '@ds/performance/performance-manager/performance-manager.service';
import { IContact, IContactSearchResult } from '@ds/core/contacts';
import { Maybe } from '@ds/core/shared/Maybe';
import { ApprovalStatus } from '../shared/approval-status.enum';
import { IncreaseType } from '@ds/performance/competencies/shared/increase-type';
import { RecommendedBonus } from '../shared/recommended-bonus';
import { MeritIncreaseService } from '@ds/performance/performance-manager/shared/merit-increase.service';
import { EmployeeApiService } from '@ds/core/employees/shared/employee-api.service';
import { AccountService } from '@ds/core/account.service';
import { ApprovalProcessHistoryAction } from '../shared/approval-process-history-action.enum';
import { ApprovalProcessStatus } from '../shared/approval-process-status.enum';

@Component({
    selector: 'ds-evaluation-summary-dialog',
    templateUrl: './evaluation-summary-dialog.component.html',
    styleUrls: ['./evaluation-summary-dialog.component.scss']
})
export class EvaluationSummaryDialogComponent implements OnInit {

    evaluation: IEvaluationWithStatusInfo;
    review: IReview;
    @ViewChild("passApproval", { static: false }) passApproval: ElementRef<HTMLDivElement>;
    signature: ISignature;
    successfullySubmitted = false;
    competencyItems: CompetencyEvalItem[] = [];
    competencyGroups: string[] = [];
    oneTimeEarningRecommendation$: Observable<any>;

    returnUrl;
    isPayrollRequestsAvailable$: Observable<boolean>;
    shouldShowChart: boolean = false;
    score: number;
    supervisors: IContact[] = [];
    user: UserInfo;
    private _hasSelectedRateOrBonus: boolean;
    returnToSender = false;
    nextStepSelector : number;
    get hasSelectedRateOrBonus() {
        return this._hasSelectedRateOrBonus == null ?
            this._hasSelectedRateOrBonus = this.evaluation.meritIncreaseInfo.currentPayInfo.some(merit => merit.selected === true) ||
            (this.evaluation.meritIncreaseInfo.oneTimeEarning != null && !this.isEmptyObject(this.evaluation.meritIncreaseInfo.oneTimeEarning) && this.evaluation.meritIncreaseInfo.oneTimeEarning.approvalStatusID !== ApprovalStatus.Rejected)  : this._hasSelectedRateOrBonus;
    }

    get hasSelectedMeritIncrease(){
        return this.evaluation.meritIncreaseInfo.currentPayInfo.some(merit => merit.selected === true);
    }

    get IncreaseType(){
        return IncreaseType;
    }

    get ApprovalStatus(){
        return ApprovalStatus;
    }

    calculateRecommendation(recommendation: RecommendedBonus): number {
        return recommendation.targetIncreaseAmount * (recommendation.complete / recommendation.total)
      }

    constructor(
        private dialogRef: MatDialogRef<EvaluationSummaryDialogComponent, IEvaluationSummaryDialogResult>,
        @Inject(MAT_DIALOG_DATA) public data:IEvaluationSummaryDialogData,
        private msgSvc: DsMsgService,
        private evalSvc: EvaluationsApiService,
        private perfSvc: PerformanceReviewsService,
        private evalStore:ActiveEvaluationService,
        private reviewProfService: ReviewProfilesApiService,
        private acctSvc: AccountService,
        private perfManagerSvc: PerformanceManagerService,
        private meritIncreaseSvc: MeritIncreaseService,
        private employeeSvc: EmployeeApiService
    ) { }

    ngOnInit() {
        this.returnUrl = this.evalStore.returnUrl || '/performance/employees/reviews';
        this.evaluation = this.data.evaluation;
        this.review = this.data.review;
        this.score = this.data.score;
        this.perfSvc.getScoringSettings(this.review.reviewId).pipe(tap(x => {
            const scoringEnabled = x.data.isScoringEnabled;
            if (scoringEnabled && !(this.evaluation.role == EvaluationRoleType.Self)) {
                this.shouldShowChart = true;
            }
        })).subscribe();

        this.oneTimeEarningRecommendation$ = this.meritIncreaseSvc.CalculateRecommendedBonus(this.review.reviewId, this.review.reviewedEmployeeId).pipe(
            map(x => {
                const recommendation = this.calculateRecommendation(x);

                return {
                    recommendation: recommendation,
                    type: x.targetIncreaseType,
                    total: x.total
                }
            }),
            share()
        )

        this.isPayrollRequestsAvailable$ =
            this.acctSvc.canPerformActions('Performance.ReadReviewProfile').pipe(concatMap(canAccess => {
                return iif(() => this.review.reviewProfileId == null || canAccess !== true,
                of(false),
                this.reviewProfService.getReviewProfileSetup(this.review.reviewProfileId).pipe(map(result => result.includePayrollRequests === true))
            ).pipe(catchError(x => {
                this.msgSvc.showErrorMsg("Failed to retrieve review profile settings.");
                return of(false);
            }));
        }));

        if (this.evaluation.isApprovalProcess) {
            this.acctSvc.getUserInfo().subscribe((user) => {
                this.user = user;

                if (this.evaluation.evaluatedByUserId == this.user.userId) {
                    this.signature = this.evaluation.signature || <ISignature>{};
                } else {
                    if (this.evaluation.signatures != null && this.evaluation.signatures.length) {
                        let preSignature = this.evaluation.signatures.find(x => { return x.modifiedBy == this.user.userId });

                        if (preSignature) this.signature = preSignature;
                        else this.signature = <ISignature>{};
                    } else {
                        this.signature = <ISignature>{};
                    }
                }
                let employeeId = (this.evaluation.reviewedEmployeeContact) ? this.evaluation.reviewedEmployeeContact.employeeId : 0;
                this.employeeSvc.getSupervisorsForEmployee((employeeId != null ? employeeId : 0), true, true).subscribe((data: any[]) => {
                    this.supervisors = data.filter(x => { return (x.userId != this.user.userId) });
                    const len = this.evaluation.approvalProcessHistory.length;
                    this.evaluation.currentAssignedUserId = null;
                    if (len > 1) {
                        const rHistory = this.evaluation.approvalProcessHistory.filter(x => x.action === ApprovalProcessHistoryAction.Reassigned); // reassigned evaluator
                        let origEditor = false;

                        if (rHistory.length) {
                            if (this.user.userId == rHistory[rHistory.length - 1].toUserId) origEditor = true;
                        } else {
                            if (this.user.userId == this.evaluation.approvalProcessHistory[0].toUserId) origEditor = true;
                        }

                        if (!origEditor) this.returnToSender = this.needsRevision();

                        if (this.returnToSender) {
                            this.nextStepSelector = 1;
                            this.evaluation.approvalProcessAction = ApprovalProcessHistoryAction.Rejected;
                            const rHistory = this.evaluation.approvalProcessHistory.filter(x => x.action === ApprovalProcessHistoryAction.Reassigned); // reassigned evaluator
                            if (rHistory.length > 0) {
                                let mostRecentHistory = rHistory[rHistory.length - 1];
                                this.supervisors = this.supervisors.filter(x => x.userId == mostRecentHistory.toUserId);
                            } else {
                                let supervisorId = this.evaluation.approvalProcessHistory[0].toUserId;
                                this.supervisors = this.supervisors.filter(x => x.userId == supervisorId);
                            }
                            this.evaluation.currentAssignedUserId = this.supervisors[0].userId;
                        } else {
                            const history = this.evaluation.approvalProcessHistory[this.evaluation.approvalProcessHistory.length - 1];
                            this.evaluation.currentAssignedUserId = null;
                            if (history.action === ApprovalProcessHistoryAction.Rejected) {
                                this.supervisors = this.supervisors.filter(x => { return (x.userId == history.fromUserId) });
                                this.evaluation.currentAssignedUserId = this.supervisors[0].userId;
                            }
                            // let supervisorId = this.evaluation.approvalProcessHistory[0].toUserId;
                            // this.supervisors = this.supervisors.filter(x => x.userId == supervisorId);
                        }
                    }
                });
            });
        } else {
            this.signature = this.evaluation.signature || <ISignature>{};
        }

        this.evalStore.refreshCompetencyItems(this.evaluation, this.competencyGroups, this.competencyItems, this.evaluation.ratings);
    }

    isEmptyObject(val: any): boolean {
        return new Maybe(val).map(x => Object.keys(x)).map(x => x.length === 0).valueOr(true);
    }

    needsRevision() : boolean {
        let itemSet = this.evaluation.competencyEvaluations.find(x => (x.approvalProcessStatusId === ApprovalProcessStatus.Rejected));

        if (itemSet) return true;
        else {
            let itemSet2 = this.evaluation.goalEvaluations.find(x => (x.approvalProcessStatusId === ApprovalProcessStatus.Rejected));
            if (itemSet2) return true;
            else {
                let itemSet3 = this.evaluation.feedbackResponses.find(x => (x.approvalProcessStatusId === ApprovalProcessStatus.Rejected));
                if (itemSet3) return true;
            }
        }

        return false;
    }

    readonly findRecommendation = this.evalStore.findRecommendedIncrease;

    get hasCompetencies() {
        return this.evaluation.competencyEvaluations && this.evaluation.competencyEvaluations.length;
    }
    get hasGoals() {
        return this.evaluation.goalEvaluations && this.evaluation.goalEvaluations.length;
    }
    get hasFeedback() {
        return this.evaluation.feedbackResponses && this.evaluation.feedbackResponses.length;
    }

    get isSelfEval() {
        return this.evaluation.role === EvaluationRoleType.Self;
    }


    //TODO: This is copied from the review-list component ... consolidate
    getEvaluationTypeName() {
        return this.evalStore.getEvaluationTypeName(this.evaluation);
    }

    getRatingLabel(ratingValue:number){
        let rating = _.find(this.evaluation.ratings, r => r.rating === ratingValue);
        return rating.label;
    }

    cancel() {
        this.dialogRef.close(null);
    }

    submitEvaluation(form: NgForm) {
        if (form.invalid){
            this.passApproval.nativeElement.scrollIntoView();
            return;
        }

        this.msgSvc.loading(true);

        if (this.evaluation.isApprovalProcess && this.nextStepSelector == 2) {
            this.evaluation.approvalProcessAction = ApprovalProcessHistoryAction.Finalized;
            this.evaluation.completedDate = moment().format(DsApiCommonProvider.TimeFormat.DATETIME);
            this.evaluation.isViewableByEmployee = ((!!this.evaluation.isViewableByEmployee) || this.isSelfEval);
        } else if (!this.evaluation.isApprovalProcess) {
            this.evaluation.completedDate = moment().format(DsApiCommonProvider.TimeFormat.DATETIME);
            this.evaluation.isViewableByEmployee = ((!!this.evaluation.isViewableByEmployee) || this.isSelfEval);
        }
        this.signature.signatureDate = moment().format(DsApiCommonProvider.TimeFormat.DATETIME);
        this.evaluation.signature = this.signature;
        const hasMerit = this.evaluation.hasSummaryData;

        const onSubmitEvalSuccess = (result: IEvaluationWithStatusInfo) => iif(() => hasMerit,
            // TODO put meritincrease and evaluation saves into one transaction


            this.evalSvc.submitMeritIncrease(result).pipe(concatMap(() => of(result))), of(result));

        const submitEvalStrategy$ = iif(() => !this.evaluation.isApprovalProcess,
                                        this.evalSvc.submitEvaluation(this.evaluation),
                                        this.evalSvc.submitEvaluationThroughApprovalProcess(this.evaluation));
        submitEvalStrategy$.pipe(
            concatMap(data => {
                this.evaluation = data;
                return onSubmitEvalSuccess(data);
            }),
            tap(result => {
                this.dialogRef.close({
                    evaluation: result
                });
            }),
            catchError((error: HttpErrorResponse) => {
                this.msgSvc.showWebApiException(error.error);
                return throwError(error);
            }))
            .subscribe();
    }

    closeModal(): void {
        this.dialogRef.close({
            evaluation: this.evaluation
        });
    }
    filterItemsOfType(group) {
        return this.evalStore.filterItemsOfType(group, this.competencyItems);
    }

}
