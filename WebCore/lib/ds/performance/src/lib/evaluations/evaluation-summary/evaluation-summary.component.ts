import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { switchMap, tap, mapTo } from 'rxjs/operators';
import { ActiveEvaluationService } from '../shared/active-evaluation.service';
import { UserInfo, MOMENT_FORMATS } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { Observable, zip, of, iif } from 'rxjs';
import { IReview } from '@ds/performance/reviews';
import { EvaluationSummaryDialogService } from '../evaluation-summary-dialog/evaluation-summary-dialog.service';
import { IEvaluationWithStatusInfo } from '../shared/evaluation-status-info.model';
import * as _ from "lodash";
import { Maybe } from '@ds/core/shared/Maybe';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { OneTimeEarning } from '@ds/performance/competencies/shared/one-time-earning.model';
import { FormGroup } from '@angular/forms';

@Component({
    selector: 'ds-evaluation-summary',
    templateUrl: './evaluation-summary.component.html',
    styleUrls: ['./evaluation-summary.component.scss']
})
export class EvaluationSummaryComponent implements OnInit {

    isLoading$: Observable<boolean>;
    user:UserInfo;
    review:IReview;
    evaluationDetail:IEvaluationWithStatusInfo;
    myForm: FormGroup = new FormGroup({});
    submitted: boolean = false;
    onetimeEarning: OneTimeEarning;

    private _overallScore: number;

    set overallScore(val: number) {
        this._overallScore = val;
    }

    get overallScore(): number {
        return this._overallScore;
    }

    returnRouterLink:any[] = ['performance', 'evaluations', 'reviews'];

    constructor(
        private router:Router,
        private evalStore:ActiveEvaluationService,
        private accountService:AccountService,
        private evalSummaryDialog: EvaluationSummaryDialogService) {

    }

    ngOnInit() {
        this.returnRouterLink = this.evalStore.returnUrl;

        const user$ = this.accountService.getUserInfo();
        const eval$ = this.evalStore.evaluationDetail$;
        const review$ = this.evalStore.review$;

        const initSummary = (isLoading: boolean) => zip(user$, eval$, review$,
            (user, detail, review) => ({ user, detail, review }))
            .pipe(
                tap(res => {
                    this.user = res.user;
                    this.evaluationDetail = res.detail;
                    this.review = res.review;
                    if (this.evaluationDetail
                        && this.evaluationDetail.meritIncreaseInfo != null
                        && this.review.payrollRequestEffectiveDate != null
                    ) {
                        this.evaluationDetail.meritIncreaseInfo.payrollRequestEffectiveDate = this.review.payrollRequestEffectiveDate;
                        // Added to avoid expressiion errors
                        this.onetimeEarning = this.evaluationDetail.meritIncreaseInfo.oneTimeEarning;
                    }
                }),
                mapTo(isLoading)
            );

        const doNothing = (isLoading: boolean) => of(isLoading);

        this.isLoading$ = this.evalStore.isLoadingDetail$.pipe(
            switchMap(isLoading => iif(() => isLoading == true, doNothing(isLoading) , initSummary(isLoading) ))
            );
    }

    private getOneTimeEarningFromEval(detail: IEvaluationWithStatusInfo): Maybe<OneTimeEarning> {
        return new Maybe(detail)
        .map(x => x.meritIncreaseInfo)
        .map(x => x.oneTimeEarning);
    }

    private convertOneTimeEarningDateToString(detail: IEvaluationWithStatusInfo): void {
        const oneTimeEarning = this.getOneTimeEarningFromEval(detail);


        const formattedEffectiveDate = oneTimeEarning.map(x => x.mayBeIncludedInPayroll)
        .map(x => convertToMoment(x))
        .map(x => x.format(MOMENT_FORMATS.API));

        oneTimeEarning.map(x => x.mayBeIncludedInPayroll = formattedEffectiveDate.value());
    }

    private setEmployeeOnBonus(employeeId: number, detail: IEvaluationWithStatusInfo): void {
        const oneTimeEarning =  this.getOneTimeEarningFromEval(detail);
        oneTimeEarning.map(x => {
            x.employeeId = employeeId;
        });

    }

    submitEvaluation() {
        this.submitted = true;
        if(this.myForm.invalid){
            return;
        }
        if(Object.keys(new Maybe(this.evaluationDetail).map(x => x.meritIncreaseInfo).map(x => x.oneTimeEarning).valueOr({})).length == 0){
            new Maybe(this.evaluationDetail).map(x => x.meritIncreaseInfo).map(x => {
                x.oneTimeEarning = null;
            });
        } else {
            this.convertOneTimeEarningDateToString(this.evaluationDetail);
            this.setEmployeeOnBonus(this.review.reviewedEmployeeId, this.evaluationDetail);
        }

        this.evalSummaryDialog.open(
            this.evaluationDetail,
            this.review,
            this.overallScore
        )
            .afterClosed()
            .subscribe(result => {
                if (result == null) return;
                if (this.review.meritIncreases == null) this.review.meritIncreases = [];

                result.evaluation.meritIncreaseInfo.currentPayInfo
                .forEach(x => {
                    var indexOfFoundMerit = 0;
                    var existingIncrease = this.review.meritIncreases.find((y, index) => {
                        indexOfFoundMerit = index;
                        return y.employeeClientRateId == x.employeeClientRateId
                    });
                    if (existingIncrease != null && x.selected == false) {
                        this.review.meritIncreases.splice(indexOfFoundMerit, 1);
                    } else if (existingIncrease != null) {
                        existingIncrease.proposedTotal = x.proposedTotal;
                        existingIncrease.selected = x.selected;
                        existingIncrease.increaseAmount = x.increaseAmount;
                        existingIncrease.comments = x.comments;
                        existingIncrease.currentAmount = x.currentAmount;
                    }
                });

                this.returnLink();
            });
    }

    returnLink() {
        let url: string = this.returnRouterLink[0];
        if (url.indexOf('aspx') !== -1) {
            location.href = url;
        } else {
            this.router.navigate(this.returnRouterLink);
        }
    }

}
