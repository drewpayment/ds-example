import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  Inject,
} from "@angular/core";
import { IReviewStatus } from "../shared/review-status.model";
import { ReviewStatusType } from "@ds/performance/performance-manager/shared/review-status-type.enum";
import { EvaluationStatusType } from "@ds/performance/performance-manager/shared/evaluation-status-type.enum";
import { IContactWithClient } from "@ds/core/contacts";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { HttpErrorResponse } from "@angular/common/http";
import { catchError, tap, filter, switchMap } from "rxjs/operators";
import { throwError, merge, Observable } from "rxjs";
import { IEvaluation, EvaluationRoleType } from "@ds/performance/evaluations";
import { IEvaluationStatus } from "../shared/evaluation-status.model";
import { ReviewSummaryDialogService } from "../review-summary-dialog/review-summary-dialog.service";
import { AccountService } from "@ds/core/account.service";
import { UserInfo, UserType } from "@ds/core/shared";
import { PERFORMANCE_ACTIONS } from "@ds/performance/shared/performance-actions";
import {
  PASaveHandler,
  PASaveHandlerToken,
} from "@ds/core/shared/shared-api-fn";
import { IReview } from "@ds/performance/reviews/shared/review.model";
import { ReviewEditDialogService } from "@ds/performance/reviews/review-edit-dialog/review-edit-dialog.service";
import { ReviewsService } from "@ds/performance/reviews/shared/reviews.service";
import * as moment from "moment";
import { ActivatedRoute, Router } from "@angular/router";
import {
  IEditEmployeeNote,
  INoteSource,
} from "@ds/core/employees/shared/employee-notes-api.model";
import { AddNoteModalComponent } from "@ds/core/employees/employee-notes/employee-notes/modals/add-note-modal.component";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { IReviewSummaryDialogResult } from "../review-summary-dialog/review-summary-dialog-result.model";
import { ResourceApiService } from "@ds/core/resources/shared/resources-api.service";

@Component({
  selector: "ds-review-status-card",
  templateUrl: "./review-status-card.component.html",
  styleUrls: ["./review-status-card.component.scss"],
})
export class ReviewStatusCardComponent implements OnInit {
  @Input("reviewStatus")
  reviewStatus: IReviewStatus;

  @Output("reviewChange")
  reviewChange = new EventEmitter<IReview>();

  @Output("evaluationSelect")
  evaluationSelect = new EventEmitter<{
    evaluation: IEvaluation;
    review: IReview;
  }>();

  supervisorEval: IEvaluation;
  supervisorEvalStatus: IEvaluationStatus;
  employeeEval: IEvaluation;
  employeeEvalStatus: IEvaluationStatus;
  private currentUser: UserInfo;
  private canEditOwnReview = false;
  reviewIdParam: number;
  get color() {
    switch (this.reviewStatus.status) {
      case ReviewStatusType.ToDo:
        return "gray";
      case ReviewStatusType.InProgress:
        return this.hasOverdueEval ? "danger" : "warning";
      case ReviewStatusType.EvaluationComplete:
        return "success";
      case ReviewStatusType.ReadyToClose:
        return "info";
      case ReviewStatusType.NeedsApproval:
        return this.hasOverdueEval ? "danger" : "warning";
      case ReviewStatusType.Approved:
        return "success";
      case ReviewStatusType.Closed:
      default:
        return "dark";
    }
  }

  canEdit(review: IReviewStatus) {
    return (
      !(
        (this.currentUser || <UserInfo>{}).employeeId ==
        review.employee.employeeId
      ) ||
      this.canEditOwnReview ||
      (<any>(this.currentUser || {})).userTypeId === UserType.systemAdmin
    );
  }

  canClose(review: IReviewStatus) {
    return (
      (!review.employee.isActive || this.isReadyToClose) &&
      !review.review.reviewCompletedDate
    );
  }

  canReopen(review: IReviewStatus) {
    return review.review.reviewCompletedDate;
  }

  get isReadyToClose() {
    return this.reviewStatus.status === ReviewStatusType.ReadyToClose;
  }

  get hasOverdueEval() {
    return (
      this.reviewStatus.evaluationStatuses.some(
        (e) => e.status === EvaluationStatusType.PastDue
      ) ||
      (this.supervisorEval
        ? moment().isAfter(moment(this.supervisorEval.dueDate)) &&
          this.supervisorEvalStatus.status == 5
        : false)
    );
  }

  get employeeStatusText() {
    if (!this.reviewStatus.employee.isActive) return "Terminated";

    if (this.employeeEvalStatus) {
      return this.getEvalStatusText(this.employeeEvalStatus);
    }

    return "";
  }

  get supervisorStatusText() {
    return this.supervisorEvalStatus
      ? this.getEvalStatusText(this.supervisorEvalStatus)
      : "";
  }

  private reviewSummaryDialogCloseHandler = (
    result: IReviewSummaryDialogResult
  ) => {
    if (result && result.selectedEvalToView) {
      this.evaluationSelect.emit({
        evaluation: result.selectedEvalToView,
        review: this.reviewStatus.review,
      });
    }
    if (result && result.reviewStatus) {
      this.reviewChanged(this.reviewStatus);
    }
  };

  constructor(
    private reviewDialogSvc: ReviewEditDialogService,
    private msgSvc: DsMsgService,
    private reviewSvc: ReviewsService,
    private reviewSummaryDialogSvc: ReviewSummaryDialogService,
    private acctSvc: AccountService,
    private route: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog,
    @Inject(PASaveHandlerToken) private factory: PASaveHandler,
    private resourceService: ResourceApiService
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      this.reviewIdParam = params["rid"] ? +params["rid"] : 0;
    });

    this.supervisorEval = this.reviewStatus.review.evaluations.find(
      (e) => e.role === EvaluationRoleType.Manager
    );
    if (this.supervisorEval) {
      this.supervisorEvalStatus = this.reviewStatus.evaluationStatuses.find(
        (s) => s.evaluationId === this.supervisorEval.evaluationId
      );
    }

    this.employeeEval = this.reviewStatus.review.evaluations.find(
      (e) => e.role === EvaluationRoleType.Self
    );
    if (this.employeeEval) {
      this.employeeEvalStatus = this.reviewStatus.evaluationStatuses.find(
        (s) => s.evaluationId === this.employeeEval.evaluationId
      );
    }

    merge(
      this.acctSvc.getUserInfo().pipe(tap((x) => (this.currentUser = x))),
      this.acctSvc
        .canPerformActions(
          PERFORMANCE_ACTIONS.Performance.AdministrateOwnPerformanceSetup
        )
        .pipe(tap((result) => (this.canEditOwnReview = result === true)))
    ).subscribe();

    if (this.reviewStatus.review.reviewId == this.reviewIdParam) {
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: {},
      });
      this.viewSummary();
    }
  }

  editReview() {
    let eeContact: IContactWithClient = Object.assign(
      { clientId: this.reviewStatus.review.clientId },
      this.reviewStatus.review.reviewedEmployeeContact
    );
    this.reviewDialogSvc
      .open(this.reviewStatus.review, eeContact)
      .afterClosed()
      .pipe(
        filter((x) => x != null && x.review != null),
        this.factory(
          (x) =>
            this.reviewSvc
              .saveReview(x.review)
              .pipe(tap((result) => this.reviewChange.emit(result))),
          "Save review",
          false
        )
      )
      .subscribe();
  }

  viewSummary() {
    const mgrEval = this.reviewStatus.review.evaluations.find(
      (e) => e.role === EvaluationRoleType.Manager
    );

    if (mgrEval && mgrEval.evaluatedByContact && mgrEval.evaluatedByContact.employeeId) {
      this.resourceService
        .getEmployeeProfileImages(
          this.reviewStatus.review.clientId,
          mgrEval.evaluatedByContact.employeeId
        )
        .pipe(
          switchMap((image) => {
            this.reviewStatus.review.evaluations.forEach((e) => {
              if (e.role === EvaluationRoleType.Manager) {
                e.evaluatedByContact.profileImage = {
                  clientId: image.clientId,
                  clientGuid: image.clientGuid,
                  employeeGuid: image.employeeGuid,
                  employeeId: image.employeeId,
                  profileImageInfo: image.profileImageInfo,
                  extraLarge: image.extraLarge || {
                    hasImage: false,
                    url: '',
                  },
                  sasToken: image.sasToken,
                };
              }
            });
            return this.openReviewSummaryDialog();
          })
        )
        .subscribe(this.reviewSummaryDialogCloseHandler);
    } else {
      this.openReviewSummaryDialog().subscribe(
        this.reviewSummaryDialogCloseHandler
      );
    }
  }

  private openReviewSummaryDialog(): Observable<IReviewSummaryDialogResult> {
    return this.reviewSummaryDialogSvc.open(this.reviewStatus).afterClosed();
  }

  reviewChanged(reviewStat: IReviewStatus) {
    this.reviewStatus = reviewStat;
    this.reviewChange.emit(reviewStat.review);
  }

  reopenReview() {
    this.reviewStatus.review.reviewCompletedDate = null;
    this.saveReview();
  }

  openNoteModal(review: IReview): void {
    this.dialog
      .open(AddNoteModalComponent, {
        width: "500px",
        data: <IEditEmployeeNote>{
          remarkId: null,
          reviewId: review.reviewId,
          description: "",
          noteSourceId: 5 /*Performance*/,
          addedBy: this.currentUser.userId,
          employeeId: review.reviewedEmployeeId,
        },
      })
      .afterClosed()
      .pipe()
      .subscribe((n) => {
        if (n) this.msgSvc.setTemporaryMessage("Note added successfully.");
      });
  }

  private getEvalStatusText(status: IEvaluationStatus) {
    switch (status.status) {
      case EvaluationStatusType.SetupIncomplete:
        return "Not Assigned";
      case EvaluationStatusType.ToDo:
        return "Ready";
      case EvaluationStatusType.InProgress:
        return "In Progress";
      case EvaluationStatusType.PastDue:
        return "Overdue";
      case EvaluationStatusType.Submitted:
        return "Submitted";
      case EvaluationStatusType.NeedsApproval:
        return "Needs Approval";
      case EvaluationStatusType.Approved:
        return "Approved";
    }
  }

  private saveReview() {
    this.msgSvc.loading(true);

    this.reviewSvc
      .saveReview(this.reviewStatus.review)
      .pipe(
        catchError((err: HttpErrorResponse) => {
          this.msgSvc.showWebApiException(err.error);
          return throwError("Error saving review");
        })
      )
      .subscribe((review) => {
        this.reviewChange.emit(review);
      });
  }
}
