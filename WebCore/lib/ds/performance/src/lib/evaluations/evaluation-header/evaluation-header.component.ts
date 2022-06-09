import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActiveEvaluationService } from '../shared/active-evaluation.service';
import {  Subscription, combineLatest, merge } from 'rxjs';
import { IReview } from '@ds/performance/reviews/shared/review.model';
import { IEvaluationDetail } from '../shared/evaluation-detail.model';
import { IEvaluationWithStatusInfo } from '../shared/evaluation-status-info.model';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { switchMap, tap, map, skip } from 'rxjs/operators';
import { UserInfo } from '@ds/core/shared';
import { AccountService, IActionNotAllowedRejection } from '@ds/core/account.service';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';
import { AttachmentsService } from '@ds/performance/attachments/shared/attachments.service';
import { DsEmployeeAttachmentModalService } from '@ajs/employee/attachments/addattachment-modal.service';
import { ActivatedRoute } from '@angular/router';
import { ReviewsService } from '@ds/performance/reviews/shared/reviews.service';
import { Router } from '@angular/router';

@Component({
    selector: 'ds-evaluation-header',
    templateUrl: './evaluation-header.component.html',
    styleUrls: ['./evaluation-header.component.scss']
})
export class EvaluationHeaderComponent implements OnInit, OnDestroy {

    private _sub: Subscription[] = [];

    @Input()
    headerColor = "secondary";
    @Input()
    isEss:boolean = false;

    review?: IReview;
    returnRouterLink: any[];
    evaluation: IEvaluationWithStatusInfo;
    isLoading: boolean = true;
    canVisitSummary: boolean = false;
    canViewAttachments = false;
    canEditAttachments = false;
    user : UserInfo;
    constructor(
        private accountSvc : AccountService,
        private evalStore: ActiveEvaluationService,
        private perfService:PerformanceReviewsService,
        private attachmentSvc: AttachmentsService,
	    public modalSvc: DsEmployeeAttachmentModalService,
        private reviewSvc: ReviewsService,
        private route: ActivatedRoute,
        private router:Router
    ){
    }

    ngOnInit() {
        if(this.isEss) this.evalStore.returnUrl = ['/performance/reviews'];
        this.returnRouterLink = this.evalStore.returnUrl;

        var evaluationId = +this.route.snapshot.paramMap.get("evaluationId");
        const getReview$ = this.reviewSvc.getReviewsByEvaluationId(evaluationId).pipe(tap(rev => {
            let selectedEval = rev.evaluations.find(x=>x.evaluationId == evaluationId);
            this.evalStore.setActiveEvaluation(selectedEval, rev);
            }));

        this.accountSvc.canPerformActions(
            [
                PERFORMANCE_ACTIONS.Attachment.AddEditAttachments,
                PERFORMANCE_ACTIONS.Attachment.AllEmployeesViewOnly
            ]).subscribe(x=> {
                var notAllowed = x as IActionNotAllowedRejection;

                if (notAllowed.actionsNotAllowed && notAllowed.actionsNotAllowed.length ) {

                    if (notAllowed.actionsNotAllowed.indexOf("Attachment.AllEmployeesViewOnly") < 0)
                        this.canViewAttachments = true;

                    if (notAllowed.actionsNotAllowed.indexOf("Attachment.AddEditAttachments") < 0)
                        this.canEditAttachments = true;

                } else {
                    this.canViewAttachments = true;
                    this.canEditAttachments = true;
                }
        });

        this.attachmentSvc.user$.pipe(skip(1)).subscribe((u : UserInfo) => {
            this.user = u;
        });

        this.attachmentSvc.fetchFakeResolver();


        const loadDetail$ =  merge(
            this.evalStore.isLoadingDetail$.pipe(tap(isLoading => this.isLoading = isLoading)),
            this.evalStore.review$.pipe(tap(review => this.review = review)),
            this.evalStore.evaluationDetail$.pipe(tap(detail => this.evaluation = detail))
        );

        this._sub.push(getReview$.pipe(switchMap(x => loadDetail$)).subscribe());
        this._sub.push(this.evalStore.onCanShowSummaryUpdate(next => this.canVisitSummary = next.isFormValidAndComplete && next.isPayrollRequestEnabled));
    }

    ngOnDestroy(): void {
        if (this._sub) {
            this._sub.forEach(val => val.unsubscribe());
        }
    }

    getEvaluationTypeName() {
        return this.evalStore.getEvaluationTypeName(this.evaluation);
    }


    returnLink() {
        let url: string =  this.returnRouterLink[0];
        if (url.indexOf('aspx') !== -1) {
            location.href = url;
        } else {
            this.router.navigate(this.returnRouterLink);
        }
    }

    addAttachment() {
        const currentAttachment = {
            resourceId: 0,
            clientId: this.user.selectedClientId(),
            employeeId: this.user.selectedEmployeeId(),
            name: '',
            folderId: this.attachmentSvc.getFolderId(),
            sourceType: 1,
            source: null,
            isViewableByEmployee: true,
            isNew: true,
            isCompanyAttachment: false
        };

        this.modalSvc.open(currentAttachment, null, this.attachmentSvc.employeeFolders$.value, false).result.then((result) => {

            this.attachmentSvc.setEmployeeFolderList(result.savedFolderList);

        });

      }
}
