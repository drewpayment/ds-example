import { Component, OnInit, Inject, Output, EventEmitter } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IReviewSummaryDialogData } from './review-summary-dialog-data.model';
import { IReviewStatus } from '../shared/review-status.model';
import { IEvaluation, EvaluationRoleType } from '@ds/performance/evaluations';
import { IEvaluationStatus } from '@ds/performance/performance-manager/shared/evaluation-status.model';
import { EvaluationStatusType } from '../shared/evaluation-status-type.enum';
import { UserInfo, UserType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { shareReplay, catchError } from 'rxjs/operators';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';
import { IEmployeeSearchResult } from '@ajs/employee/search/shared/models';
import { IReviewSummaryDialogResult } from './review-summary-dialog-result.model';
import { throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { EvaluationsApiService } from '@ds/performance/evaluations/shared/evaluations-api.service';
import { IEvaluationSelectedData } from '@ds/performance/reviews/review-list/review-list.component';
import { IReview } from '@ds/performance/reviews';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { PrintEvaluationArgs, PRINT_EVALUATION_KEY } from '@ds/performance/shared/print-evaluation-args';
import { PrintableEvaluationComponent } from '@ds/performance/evaluations/printable-evaluation/printable-evaluation.component';
import * as moment from 'moment';
import { Route, Router, ActivatedRoute } from '@angular/router';
import { PerformanceManagerService } from '../performance-manager.service';

@Component({
    selector: 'ds-review-summary-dialog',
    templateUrl: './review-summary-dialog.component.html',
    styleUrls: ['./review-summary-dialog.component.scss']
})
export class ReviewSummaryDialogComponent implements OnInit {

    @Output()
    evaluationSelected = new EventEmitter<IEvaluationSelectedData>();
    reviewStatus: IReviewStatus;
    evaluationStatuses: EvaluationStatus[] = [];
    currentUser: UserInfo;
    canEditOwnReview = false;
    EvaluationRoleType = EvaluationRoleType;
    isUsersReview = false;


    get review(): IReview {
        return this.reviewStatus ? this.reviewStatus.review : null;
    }

    get employee(): IEmployeeSearchResult {
        return this.reviewStatus ? this.reviewStatus.employee : null;
    }

    get canEdit() {
        return (!this.isOwnSelf || this.canEditOwnReview) || (<any>(this.currentUser || {})).userTypeId === UserType.systemAdmin;
    }

    get isOwnSelf() {
        return !this.currentUser || !this.employee || this.employee.employeeId === this.currentUser.employeeId;
    }

    constructor(private msgSvc: DsMsgService,
        private dialogRef: MatDialogRef<ReviewSummaryDialogComponent, IReviewSummaryDialogResult>,
        @Inject(MAT_DIALOG_DATA)
        private dialogData: IReviewSummaryDialogData,
        private accountSvc: AccountService,
        private evalSvc: EvaluationsApiService,
        private store: DsStorageService,
        private manager:PerformanceManagerService
    ) {}

    ngOnInit() {
        this.reviewStatus = this.dialogData.reviewStatus;
        this.accountSvc.getUserInfo()
            .subscribe(u => {
                this.currentUser = u;
                if (this.currentUser.userTypeId == UserType.companyAdmin || this.currentUser.userTypeId == UserType.systemAdmin
                    || this.currentUser.userTypeId == UserType.supervisor) {
                    if (this.currentUser.userEmployeeId == null || this.currentUser.userEmployeeId == undefined)
                        this.isUsersReview = this.currentUser.employeeId == this.review.reviewedEmployeeId;
                    else
                        this.isUsersReview = this.currentUser.userEmployeeId == this.review.reviewedEmployeeId;
                } else {
                    this.isUsersReview = this.currentUser.employeeId == this.review.reviewedEmployeeId;
                }
            });

        this.accountSvc.canPerformActions(PERFORMANCE_ACTIONS.Performance.AdministrateOwnPerformanceSetup)
            .subscribe(result => {
                this.canEditOwnReview = (result === true);
            });


        this.initEvaluations();
    }

    initEvaluations() {
        this.evaluationStatuses = [];
        if (this.reviewStatus.review.evaluations) {
            this.reviewStatus.review.evaluations.forEach(e => {
                const status = this.reviewStatus.evaluationStatuses.find(s => s.evaluationId === e.evaluationId);
                this.evaluationStatuses.push(new EvaluationStatus(e, status));
            });
        }
    }

    printReview(evaluation: IEvaluation, review: IReview, printForEmp?: boolean) {
        PrintableEvaluationComponent.printEval(evaluation, review, this.accountSvc, this.store, printForEmp);
    }

    canViewEval(evaluation: IEvaluation, review: IReview) {
        const isUsersEval = evaluation.evaluatedByUserId === this.currentUser.userId;
        const isEmployeeViewable = evaluation.isViewableByEmployee;
        const isAdminViewable = !!evaluation.completedDate && this.canEdit;
        const isSysAdmin = this.currentUser.userTypeId === UserType.systemAdmin;
        const isCurrentAssignedUser = this.currentUser.userId === evaluation.currentAssignedUserId;

        return isUsersEval || isEmployeeViewable || isAdminViewable || isSysAdmin || isCurrentAssignedUser;
    }

    evalSelected(evaluation: IEvaluation, review: IReview): void {
        if (evaluation.evaluatedByUserId == this.currentUser.userId) {
            this.evaluationSelected.emit({
                evaluation: evaluation,
                review: review
            });
        }
        if (this.dialogRef != null) {
            this.dialogRef.close({
                reviewStatus: <IReviewStatus>{}
            });
        }
    }

    releaseEvalToEmployee(evaluation: EvaluationStatus, review: IReview, isReleased: boolean) {
        // can't update release status of Self eval
        if (evaluation.evaluation.role === EvaluationRoleType.Self)
            return;

        this.msgSvc.loading(true);

        const update = isReleased ?
            this.evalSvc.releaseEvalution(evaluation.evaluation.evaluationId) :
            this.evalSvc.revokeEvalution(evaluation.evaluation.evaluationId);

        update
            .pipe(catchError((err: HttpErrorResponse, caught) => {
                this.msgSvc.showWebApiException(err.error);
                return throwError('Error saving evaluation');
            }))
            .subscribe(e => {
                evaluation.evaluation.isViewableByEmployee = isReleased;
                this.msgSvc.setTemporaryMessage('Evaluation updated successfully.');
            });
    }
    viewEvaluation(evaluation: IEvaluation, review?: IReview) {
        if (!this.canViewEval(evaluation, review))
            return;

        this.dialogRef.close({
            selectedEvalToView: evaluation
        });
    }
    cancel() {
        this.dialogRef.close(null);
    }

    transfer(e: EvaluationStatus) {
        if (!this.canViewEval(e.evaluation,  this.dialogData.reviewStatus.review))
            return;

        let transfer = "EmployeePerformance.aspx" + "?Submenu=employee&eid=" + this.dialogData.reviewStatus.review.reviewedEmployeeId
        transfer = transfer + "#performance/employees/reviews/" + "?evid=" + e.evaluation.evaluationId + "&rid=" + this.dialogData.reviewStatus.review.reviewId;
        const ed = JSON.stringify(this.manager.filterOptionsFormGroup.controls.EndDate.value);
        const sd = JSON.stringify(this.manager.filterOptionsFormGroup.controls.StartDate.value);
        const rtid = JSON.stringify(this.manager.filterOptionsFormGroup.controls.Review.value.reviewTemplateId);
        const returnValues = "?ed=" + ed + "&sd=" + sd + "&rtid=" + rtid + "&rid=" + this.dialogData.reviewStatus.review.reviewId;
        transfer = transfer + "&returnUrl=" + encodeURIComponent("PerformanceReviews.aspx?submenu=performance#/performance/manage/status" + returnValues);
        location.href = transfer;
    }
}


class EvaluationStatus {
    evaluation: IEvaluation;
    status: IEvaluationStatus;
    hoverEval = false;

    get evaluationTypeName() {
        switch (this.evaluation.role) {
            case EvaluationRoleType.Manager:
                return 'Supervisor Evaluation';
            case EvaluationRoleType.Peer:
                return 'Peer Evaluation';
            case EvaluationRoleType.Self:
                return 'Self-evaluation';
        }
    }

    get evalStatusText() {
        switch (this.status.status) {
            case EvaluationStatusType.SetupIncomplete:
                return 'Not Assigned';
            case EvaluationStatusType.ToDo:
                return 'Ready';
            case EvaluationStatusType.InProgress:
                return 'In Progress';
            case EvaluationStatusType.PastDue:
                return 'Overdue';
            case EvaluationStatusType.Submitted:
                return 'Submitted';
            case EvaluationStatusType.NeedsApproval:
                return moment().isAfter(this.evaluation.dueDate) ? 'Approval Overdue' : 'Needs Approval';
            case EvaluationStatusType.Approved:
                return 'Approved';
        }
    }

    get statusColor() {
        switch (this.status.status) {
            case EvaluationStatusType.SetupIncomplete:
                return 'danger';
            case EvaluationStatusType.ToDo:
                return 'gray';
            case EvaluationStatusType.InProgress:
                return 'warning';
            case EvaluationStatusType.PastDue:
                return 'danger';
            case EvaluationStatusType.Submitted:
                return 'info';
            case EvaluationStatusType.NeedsApproval:
                return  moment().isAfter(this.evaluation.dueDate) ? 'danger' :'warning';
            case EvaluationStatusType.Approved:
                return 'success';
        }
    }

    get hasEvaluatedBy() {
        return !!this.evaluation.evaluatedByContact;
    }

    constructor(evaluation: IEvaluation, status: IEvaluationStatus) {
        this.evaluation = evaluation;
        this.status = status;
    }
}
