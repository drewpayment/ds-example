import { Component, OnInit, Output, EventEmitter, Input, OnDestroy, Inject, ViewChild, ElementRef } from '@angular/core';
import { ReviewEditDialogService } from '../review-edit-dialog/review-edit-dialog.service';
import { ReviewsService } from '../shared/reviews.service';
import { IReview } from '../shared/review.model';
import { UserInfo, UserType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { catchError, shareReplay, exhaustMap, concatMap, tap, filter, takeUntil, switchMap } from 'rxjs/operators';
import { HttpErrorResponse, HttpClient } from '@angular/common/http';
import { throwError, from, Subject, fromEvent, Observable, iif, of } from 'rxjs';
import * as moment from "moment";
import * as _ from 'lodash';
import { DsApiCommonProvider } from '@ajs/core/api/ds-api-common.provider';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';
import { ReviewMeetingDialogService } from '../review-meeting-dialog/review-meeting-dialog.service';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { IDsConfirmOptions } from '@ajs/ui/confirm/ds-confirm.interface';
import { IEmployeeSearchResult } from '@ajs/employee/search/shared/models';
import { IEvaluation, EvaluationRoleType } from '@ds/performance/evaluations';
import { IContactWithClient } from '@ds/core/contacts/shared/contact.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { PASaveHandler, PASaveHandlerToken } from '@ds/core/shared/shared-api-fn';
import { EvaluationsApiService } from '@ds/performance/evaluations/shared/evaluations-api.service';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { PrintEvaluationArgs, PRINT_EVALUATION_KEY } from '@ds/performance/shared/print-evaluation-args';
import { PrintableEvaluationComponent } from '@ds/performance/evaluations/printable-evaluation/printable-evaluation.component';
import { ActivatedRoute } from '@angular/router';
import { ActiveEvaluationService } from '@ds/performance/evaluations/shared/active-evaluation.service';
import { IEditEmployeeNote, INoteSource } from '@ds/core/employees/shared/employee-notes-api.model';
import { AddNoteModalComponent } from '@ds/core/employees/employee-notes/employee-notes/modals/add-note-modal.component';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface IEvaluationSelectedData {
    evaluation: IEvaluation;
    review?: IReview;
}

export interface EvaluationView extends IEvaluation {
    hoverEval: boolean
}

enum ReviewListMode {
    Admin,
    Employee
}

@Component({
    selector: 'ds-review-list',
    templateUrl: './review-list.component.html',
    styleUrls: ['./review-list.component.scss'],
    //providers: [AddNoteModalComponent, AddNoteTriggerComponent, AssignTagsModalComponent],
})
export class ReviewListComponent implements OnInit, OnDestroy {
    ngOnDestroy(): void {
        this.unsubscriber.next();
    }


    private _reviewedEmployeeContact: { contact: IContactWithClient, empData: IEmployeeSearchResult };;
    private _mode = ReviewListMode.Employee;
    private _isArchiveView = false;
    private _reviews: IReview[];
    private _viewableReviews: IReview[];
    private _hasRoleToReopenEval = false;
    private editReviewSub: Subject<IReview> = new Subject();
    private unsubscriber = new Subject();
    evalParam : number;
    reviewParam : number;
    returnParam : string;
    defaultToEval = false;

    @Output()
    evaluationSelected = new EventEmitter<IEvaluationSelectedData>();

    @Input()
    set employee(val: { contact: IContactWithClient, empData: IEmployeeSearchResult }) {
        this._reviewedEmployeeContact = val;
        this.reloadReviewList();
    }
    get employee() {
        return this._reviewedEmployeeContact;
    }

    /**
     * Determines if the list should render in employee or admin-view mode.
     * Admin mode allows adding/editing of reviews and configuration of evaluations.
     * Employee mode provides a much reduced functionality.  Only allowing the employee
     * to fillout their self-eval and view other evals that have been released to them.
     */
    @Input()
    set mode(val: string) {
        this._mode = val.toLowerCase() === "admin" ? ReviewListMode.Admin : ReviewListMode.Employee;
    }

    @Input()
    set archive(val: boolean) {
        this._isArchiveView = val;
        this.updateViewableReviews();
    }
    get archive() {
        return this._isArchiveView;
    }
    get mode() {
        return this._mode.toString();
    }

    get isEmployeeViewMode() {
        return this._mode === ReviewListMode.Employee;
    }

    get canEdit() {
        return this._mode === ReviewListMode.Admin && (!this.isOwnSelf || this.canEditOwnReview) || (<any>(this.currentUser || {})).userTypeId === UserType.systemAdmin;
    }

    get isOwnSelf() {
        return !this.currentUser || !this.employee || this.employee.contact.employeeId === this.currentUser.employeeId;
    }

    canReopen(review: IReview) {
        return this.hasRoleToReopenEval || review.reviewOwnerUserId === this.currentUser.userId;
    }

    get hasReviews() {
        return this.isLoading || (this.reviews && this.reviews.length);
    }

    hasEvaluations(review: IReview) {
        return review.evaluations && review.evaluations.length > 0;
    }

    set reviews(val: IReview[]) {
        this._reviews = val;
        this.updateViewableReviews();
    }
    get reviews() {
        return this._viewableReviews;
    }

    get EvaluationRoleType() {
        return EvaluationRoleType;
    }

    isLoading = true;
    currentUser: UserInfo;
    canEditOwnReview = false;
    UserTypeRef = UserType;
    isUsersReview = false;
    get hasRoleToReopenEval() {
        return this._hasRoleToReopenEval;
    }

    constructor(
        private reviewSvc: ReviewsService,
        private reviewDialogSvc: ReviewEditDialogService,
        private reviewMeetingDialogSvc: ReviewMeetingDialogService,
        private msgSvc: DsMsgService,
        private confirmSvc: DsConfirmService,
        private accountSvc: AccountService,
        private evalSvc: EvaluationsApiService,
        @Inject(PASaveHandlerToken) private factory: PASaveHandler,
        private store: DsStorageService,
        private activeEvalSvc: ActiveEvaluationService,
        private route: ActivatedRoute,
        public dialog: MatDialog
    ) { }

    ngOnInit() {
        this.openReviewDialog();
        this.reviews = [],
            this.msgSvc.loading(true);
        this.accountSvc.getUserInfo()
            .subscribe(u => {
                this.currentUser = u;
                this._hasRoleToReopenEval = this.currentUser.userTypeId === UserType.systemAdmin || this.currentUser.userTypeId === UserType.companyAdmin;
            });

        this.accountSvc.canPerformActions(PERFORMANCE_ACTIONS.Performance.AdministrateOwnPerformanceSetup)
            .subscribe(result => {
                this.canEditOwnReview = (result === true);
            });

        this.route.queryParams.subscribe(params => {
            this.evalParam = (params['evid'] ? +params['evid'] : 0);
            this.reviewParam = (params['rid'] ? +params['rid'] : 0);
            this.returnParam = (params['returnUrl'] ? decodeURIComponent(params['returnUrl']) : "");
            if (this.evalParam && this.reviewParam) this.defaultToEval = true;
        });

        this.reloadReviewList();
    }

    openNoteModal(review: IReview): void {
        this.dialog.open(AddNoteModalComponent, {
            width: '500px',
            data: <IEditEmployeeNote> {
                remarkId: null,
                reviewId: review.reviewId,
                description: "",
                noteSourceId: 5, /*Performance*/
                addedBy: this.currentUser.userId,
                employeeId: review.reviewedEmployeeId
            }
        }).afterClosed().pipe()
        .subscribe(n => {
            if (n)
                this.msgSvc.setTemporaryMessage("Note added successfully.");
        });
    }

    addReview() {
        this.editReviewSub.next();
    }

    editReview(review: IReview) {
        this.editReviewSub.next(review);
    }

    openReviewDialog() {
        this.editReviewSub.pipe(concatMap(review => {
            return this.reviewDialogSvc
                .open(review, this._reviewedEmployeeContact.contact, this.currentUser)
                .afterClosed();
        }),
            filter(result => null != result),
            this.factory(
                (x) => this.reviewSvc.saveReview(x.review).pipe(tap(result => this.handleReviewSave(result))),
                'Save review',
                false),
            takeUntil(this.unsubscriber)).subscribe();
    }

    printReview(evaluation: IEvaluation, review: IReview, printForEmp?: boolean) {
        PrintableEvaluationComponent.printEval(evaluation, review, this.accountSvc, this.store, printForEmp);
    }

    isReviewClosed(review: IReview) {
        return review.reviewCompletedDate;
    }

    reOpenReview(review: IReview) {
        review.reviewCompletedDate = null;
        const supervisorEval = (review.evaluations || []).find(x => x.role === EvaluationRoleType.Manager);
        if(supervisorEval != null){
            supervisorEval.isViewableByEmployee = false;
        }
        this.saveReview(review);
    }

    evalSelected(evaluation: IEvaluation, review: IReview): void {
                if(review.meritIncreases != null){
                    review.meritIncreases = [];
                }
        if (evaluation.evaluatedByUserId == this.currentUser.userId) {
            this.evaluationSelected.emit({
                evaluation: evaluation,
                review: review
            });
        }
    }

    releaseEvalToEmployee(evaluation: IEvaluation, review: IReview, isReleased: boolean) {
        //can't update release status of Self eval
        if (evaluation.role === EvaluationRoleType.Self)
            return;

        this.msgSvc.loading(true);

        let update = isReleased ?
            this.evalSvc.releaseEvalution(evaluation.evaluationId) :
            this.evalSvc.revokeEvalution(evaluation.evaluationId);

        update
            .pipe(catchError((err: HttpErrorResponse, caught) => {
                this.msgSvc.showWebApiException(err.error);
                return throwError("Error saving evaluation");
            }))
            .subscribe(e => {
                evaluation.isViewableByEmployee = isReleased;
                this.msgSvc.setTemporaryMessage("Evaluation updated successfully.");
            });
    }

    handleReviewSave(review: IReview) {
        if (review) {
            this.reloadReviewList();
        }
    }

    checkCurrentUser(): Observable<UserInfo>{
        return iif(() => this.currentUser == null,
            this.accountSvc.getUserInfo().pipe(tap(u => {
                this.currentUser = u;
                this._hasRoleToReopenEval = this.currentUser.userTypeId === UserType.systemAdmin || this.currentUser.userTypeId === UserType.companyAdmin;
            })),
            of(this.currentUser));
    }

    reloadReviewList = () => {
        if (this._reviewedEmployeeContact && this._reviewedEmployeeContact.contact.employeeId) {
            this.checkCurrentUser().pipe(switchMap( userInfo =>
            this.reviewSvc
                .getEmployeeReviews(this._reviewedEmployeeContact.contact.employeeId)
                .pipe( tap(reviews => {
                    this.reviews = reviews;
                    if(this.reviews.length>0){
                        if(this.currentUser.userTypeId==UserType.companyAdmin || this.currentUser.userTypeId==UserType.systemAdmin
                            || this.currentUser.userTypeId == UserType.supervisor)
                        {
                            if(this.currentUser.userEmployeeId == null || this.currentUser.userEmployeeId == undefined)
                                this.isUsersReview = this.currentUser.employeeId == this._reviewedEmployeeContact.contact.employeeId
                            else
                                this.isUsersReview = this.currentUser.userEmployeeId == this._reviewedEmployeeContact.contact.employeeId
                        }else{
                            this.isUsersReview = this.currentUser.employeeId == this._reviewedEmployeeContact.contact.employeeId
                        }
                    }

                    this.reviews.forEach(review => {
                        if(!review.evaluations || !review.evaluations.length) return;

                        review.evaluations.forEach(ev => {
                            (<EvaluationView>ev).hoverEval = false;
                        });

                        review.evaluations.sort((e1, e2) => {
                            return e1.role > e2.role ? 1 : -1;
                        })
                    });

                    if (this.defaultToEval) {
                        let review = this.reviews.find((rev: IReview) => { return rev.reviewId == this.reviewParam; });
                        let evaluation = review.evaluations.find((e: IEvaluation) => { return e.evaluationId == this.evalParam; });
                        this.activeEvalSvc.returnUrl = [this.returnParam];
                        this.viewEvaluation(evaluation, review);
                    }

                    //isLoading is only true on initial component load
                    if (this.isLoading)
                        this.msgSvc.loading(false);

                    this.isLoading = false;
                })))).subscribe();
        }
    }

    getEvaluationTypeName(evaluation: IEvaluation) {
        switch (evaluation.role) {
            case EvaluationRoleType.Manager:
                return "Supervisor Evaluation";
            case EvaluationRoleType.Peer:
                return "Peer Evaluation";
            case EvaluationRoleType.Self:
                return "Self-evaluation";
        }
    }

    private isReviewedEmployeeIdCurrentUserEmployeeId(review: IReview, currentUser: UserInfo = this.currentUser) {
        return review.reviewedEmployeeId === currentUser.userEmployeeId || review.reviewedEmployeeId === currentUser.employeeId;
    }

    canOpenEval(evaluation: IEvaluation, review: IReview) {
        let isReviwedEmployee = review.reviewedEmployeeId == this.currentUser.employeeId;
        let isUsersEval = evaluation.currentAssignedUserId === this.currentUser.userId;
        let isEmployeeViewable = evaluation.isViewableByEmployee && this.isReviewedEmployeeIdCurrentUserEmployeeId(review);
        let isSupervisorViewable = !!evaluation.completedDate && (this.currentUser.userTypeId === UserType.supervisor) && !isReviwedEmployee;
        let isAdminViewable = !!evaluation.completedDate && this.canEdit;
        const isSysAdmin = this.currentUser.userTypeId === UserType.systemAdmin

        return isUsersEval || isEmployeeViewable || isSupervisorViewable || isAdminViewable || isSysAdmin;
    }

    canViewEval(evaluation: IEvaluation, review: IReview) {
        let isReviwedEmployee = review.reviewedEmployeeId == this.currentUser.employeeId;
        let isUsersEval = evaluation.currentAssignedUserId === this.currentUser.userId;
        let isEmployeeViewable = evaluation.isViewableByEmployee && this.isReviewedEmployeeIdCurrentUserEmployeeId(review);
        //let isSupervisorViewable = !!evaluation.completedDate && (this.currentUser.userTypeId === UserType.supervisor);
        let isSupervisorViewable =this.currentUser.userTypeId === UserType.supervisor && !isReviwedEmployee;
        let isAdminViewable = !!evaluation.completedDate && this.canEdit;
        const isSysAdmin = this.currentUser.userTypeId === UserType.systemAdmin;
        const isCompanyAdmin = this.currentUser.userTypeId === UserType.companyAdmin;

        return isUsersEval || isEmployeeViewable || isSupervisorViewable || isAdminViewable || isSysAdmin || isCompanyAdmin;
    }

    viewEvaluation(evaluation: IEvaluation, review?: IReview) {
        if (!this.canOpenEval(evaluation, review))
            return;

        this.evaluationSelected.emit({
            evaluation: evaluation,
            review: review
        });
    }

    editMeeting(review: IReview) {
        let dialog = this.reviewMeetingDialogSvc.open(review);
        dialog.afterClosed().subscribe(result => {
            if (result)
                this.reloadReviewList();
        });
    }

    unscheduleMeeting(review: IReview) {
        let opts: IDsConfirmOptions = {
            actionButtonText: "Unschedule",
            closeButtonText: "Cancel",
            bodyText: "Unschedule review meeting?"
        };

        this.confirmSvc.showModal(null, opts).then(x => {
            review.meetings = [];
            review.isReviewMeetingRequired = true;
            this.reviewSvc.saveReview(review)
                .subscribe(
                    x => this.reloadReviewList(),
                    x => this.reloadReviewList()
                )
        })
    }

    hasMeeting(review: IReview) {
        return review.meetings && review.meetings.length;
    }
    private saveReview(review: IReview) {
        this.msgSvc.loading(true);

        this.reviewSvc
            .saveReview(review)
            .pipe(catchError((err: HttpErrorResponse, caught) => {
                this.msgSvc.showWebApiException(err.error);
                return throwError("Error saving review");
            }))
            .subscribe(review => {
                this.msgSvc.setTemporaryMessage("Review updated successfully.");
                this.updateViewableReviews();
            });
    }

    private updateViewableReviews() {
        if (this._reviews)
            this._viewableReviews = _.filter(this._reviews, r => !!r.reviewCompletedDate === this._isArchiveView);
        else
            this._viewableReviews = null;
    }

    profileImageExists(evaluation:IEvaluation):boolean{
        if(evaluation.evaluatedByContact && evaluation.evaluatedByContact.profileImage){
            return true;
        }
        return false;
    }
}
