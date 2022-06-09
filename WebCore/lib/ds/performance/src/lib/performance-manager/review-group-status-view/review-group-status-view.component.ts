import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ElementRef,
  ViewChild,
} from "@angular/core";
import * as angular from "angular";
import {
  IReviewGroupStatus,
  IAssignedToEmployee,
} from "../shared/review-group-status.model";
import { IReviewStatus } from "../shared/review-status.model";
import { ReviewStatusType } from "../shared/review-status-type.enum";
import { IReview } from "@ds/performance/reviews";
import { ReviewsService } from "@ds/performance/reviews/shared/reviews.service";
import { IEvaluation, EvaluationRoleType } from "@ds/performance/evaluations";
import { EvaluationStatusType } from "../shared/evaluation-status-type.enum";
import { ReferenceDate } from "@ds/core/groups/shared/schedule-type.enum";
import { DateUnit } from "@ds/core/shared/time-unit.enum";
import { ICompetencyModel } from "@ds/performance/competencies/shared/competency-model.model";
import { IReviewRating } from "@ds/performance/ratings/shared/review-rating.model";
import { PerformanceManagerService } from "@ds/performance/performance-manager/performance-manager.service";
import { switchMap, tap } from "rxjs/operators";
import { FieldType } from "@ajs/core/shared/models/field-type";
import {
  IFeedbackSetup,
  IFeedbackResponseData,
  IReviewIdWithFeedbackResponse,
} from "@ds/performance/feedback";
import { INameVal } from "@ajs/labor/models/name-value.model";
import { PerformanceReviewsService } from "@ds/performance/shared/performance-reviews.service";
import { ScoreModel } from "@ds/performance/ratings/shared/score-model.model";
import { Maybe } from "@ds/core/shared/Maybe";

enum ViewMode {
  Kanban,
  Analytics,
}

class ReviewGroup implements IReviewGroupStatus {
  payrollRequestEffectiveDate?: string | Date;
  supervisorEvalDueDate?: string | Date;
  referenceDateTypeId: ReferenceDate;
  delayAfterReference?: number;
  delayAfterReferenceUnitTypeId?: DateUnit;
  reviewProcessDuration?: number;
  reviewProcessDurationUnitTypeId?: DateUnit;

  reviewPolicyId: number;
  reviewPolicyName: string;
  reviewTemplateId: number;
  reviewTemplateName: string;
  reviewProcessStartDate: string | Date | null;
  reviewProcessDueDate: string | Date | null;
  reviews: IReviewStatus[];
  statusGroups: StatusGroup[];
  overallEmployees: number;
  selectedEmployees: number;

  constructor(raw: IReviewGroupStatus) {
    Object.assign(this, raw);

    this.reviews = this.reviews || [];

    this.overallEmployees = this.countEmployees(this.reviews);

    this.initializeStatusGroups();
  }

  countEmployees(items: IReviewStatus[]) {
    const nonNullItems = items || [];

    const dupesGone = nonNullItems
      .map((x) => x.review.reviewedEmployeeId)
      .reduce((prev, next) => {
        prev[next] = null;
        return prev;
      }, {});

    return Object.keys(dupesGone).length;
  }

  initializeStatusGroups(
    statuses: ReviewStatusType[] = [
      ReviewStatusType.ToDo,
      ReviewStatusType.InProgress,
      ReviewStatusType.EvaluationComplete,
      ReviewStatusType.ReadyToClose,
    ]
  ) {
    this.statusGroups = [];
    this.selectedEmployees = 0;

    const apReviews = this.reviews.some((x) => {
      const apEvals = x.review.evaluations.some((y) => {
        return y.isApprovalProcess == true;
      });
      return apEvals;
    });

    if (apReviews)
      statuses = [
        ReviewStatusType.InProgress,
        ReviewStatusType.NeedsApproval,
        ReviewStatusType.Approved,
        ReviewStatusType.ReadyToClose,
      ];

    if (statuses.some((s) => s === ReviewStatusType.ToDo)) {
      let s = new StatusGroup();
      var filtered = this.reviews.filter(
        (r) => r.status == ReviewStatusType.ToDo
      );
      s.reviewStatuses = this.sortReviews(filtered);
      s.title = "To Do";
      s.statusType = ReviewStatusType.ToDo;
      this.statusGroups.push(s);
      this.selectedEmployees += this.countEmployees(s.reviewStatuses);
    }

    if (statuses.some((s) => s === ReviewStatusType.InProgress)) {
      let s = new StatusGroup();
      s.reviewStatuses = this.sortReviews(
        this.reviews.filter((r) => r.status == ReviewStatusType.InProgress)
      );
      s.title = "In Progress";
      s.statusType = ReviewStatusType.InProgress;
      this.statusGroups.push(s);
      this.selectedEmployees += this.countEmployees(s.reviewStatuses);
    }

    if (statuses.some((s) => s === ReviewStatusType.NeedsApproval)) {
      let s = new StatusGroup();
      s.reviewStatuses = this.sortReviews(
        this.reviews.filter((r) => r.status == ReviewStatusType.NeedsApproval)
      );
      s.title = "Needs Approval";
      s.statusType = ReviewStatusType.NeedsApproval;
      this.statusGroups.push(s);
      this.selectedEmployees += this.countEmployees(s.reviewStatuses);
    }

    if (statuses.some((s) => s === ReviewStatusType.Approved)) {
      let s = new StatusGroup();
      s.reviewStatuses = this.sortReviews(
        this.reviews.filter((r) => r.status == ReviewStatusType.Approved)
      );
      s.title = "Approved";
      s.statusType = ReviewStatusType.Approved;
      this.statusGroups.push(s);
      this.selectedEmployees += this.countEmployees(s.reviewStatuses);
    }

    if (statuses.some((s) => s === ReviewStatusType.EvaluationComplete)) {
      let s = new StatusGroup();
      s.reviewStatuses = this.sortReviews(
        this.reviews.filter(
          (r) => r.status == ReviewStatusType.EvaluationComplete
        )
      );
      s.title = "Evaluation Submitted";
      s.statusType = ReviewStatusType.EvaluationComplete;
      this.statusGroups.push(s);
      this.selectedEmployees += this.countEmployees(s.reviewStatuses);
    }

    if (statuses.some((s) => s === ReviewStatusType.ReadyToClose)) {
      let s = new StatusGroup();
      s.reviewStatuses = this.sortReviews(
        this.reviews.filter((r) => r.status == ReviewStatusType.ReadyToClose)
      );
      s.title = "Ready To Close";
      s.statusType = ReviewStatusType.ReadyToClose;
      this.statusGroups.push(s);
      this.selectedEmployees += this.countEmployees(s.reviewStatuses);
    }

    if (statuses.some((s) => s === ReviewStatusType.Closed)) {
      let s = new StatusGroup();
      s.reviewStatuses = this.sortReviews(
        this.reviews.filter((r) => r.status == ReviewStatusType.Closed)
      );
      s.title = "Closed";
      s.statusType = ReviewStatusType.Closed;
      this.statusGroups.push(s);
      this.selectedEmployees += this.countEmployees(s.reviewStatuses);
    }
  }
  /**
   * Sorts the provided reviews by overdue, then by first name, and then by last name.
   * @param filteredReviews The reviews to sort.
   * @returns The sorted reviews.
   */
  private sortReviews(filteredReviews: IReviewStatus[]): IReviewStatus[] {
    filteredReviews.sort((reviewA, reviewB) => {
      const overdueProp =
        (reviewB.evaluationStatuses.some(
          (e) => e.status === EvaluationStatusType.PastDue
        )
          ? 1
          : 0) -
        (reviewA.evaluationStatuses.some(
          (e) => e.status === EvaluationStatusType.PastDue
        )
          ? 1
          : 0);
      if (overdueProp === 0) {
        const firstProp = reviewA.employee.lastName
          .toLocaleLowerCase()
          .localeCompare(reviewB.employee.lastName.toLocaleLowerCase());
        if (firstProp === 0) {
          return reviewA.employee.firstName
            .toLocaleLowerCase()
            .localeCompare(reviewB.employee.firstName.toLocaleLowerCase());
        }
        return firstProp;
      }
      return overdueProp;
    });
    return filteredReviews;
  }
}

class StatusGroup {
  statusType: ReviewStatusType;
  title: string;
  reviewStatuses: IReviewStatus[];
  get reviewCount(): number {
    return this.reviewStatuses ? this.reviewStatuses.length : 0;
  }
}

@Component({
  selector: "ds-review-group-status-view",
  templateUrl: "./review-group-status-view.component.html",
  styleUrls: ["./review-group-status-view.component.scss"],
})
export class ReviewGroupStatusViewComponent implements OnInit {
  isScoringEnabled: boolean = false;

  @Input()
  set reviewGroup(group: IReviewGroupStatus) {
    this.group = group ? new ReviewGroup(group) : null;
  }

  @Input()
  set statuses(statuses: ReviewStatusType[]) {
    if (this.group) this.group.initializeStatusGroups(statuses);
  }

  @Input() scoreModel: ScoreModel;

  @Input()
  competencyModels: Array<ICompetencyModel>;
  @Input()
  allRatings: Array<IReviewRating>;
  @Input()
  allFeedbackQuestions: Array<IFeedbackSetup>;

  @Input()
  expanded: boolean;

  @Input()
  allowKanbanView = false;

  @Input()
  allowAnalyticsView = false;

  private _viewMode: ViewMode = ViewMode.Kanban;

  @Input()
  set mode(mode: "kanban" | "analytics") {
    if (this.group) {
      if (mode === "analytics") {
        this._viewMode = ViewMode.Analytics;
        this.allowAnalyticsView = true;
      } else {
        this._viewMode = ViewMode.Kanban;
        this.allowKanbanView = true;
      }
    }
  }

  @Input()
  csvDownloadLink: string;

  @Output()
  reviewChange = new EventEmitter<IReview>();

  @Output("evaluationSelect")
  evaluationSelect = new EventEmitter<{
    evaluation: IEvaluation;
    review: IReview;
  }>();

  get isStatusView() {
    return this._viewMode === ViewMode.Kanban;
  }

  get isAnalyticsView() {
    return this._viewMode === ViewMode.Analytics;
  }

  group: ReviewGroup;
  empReviews: IAssignedToEmployee[];
  selectedUser: IAssignedToEmployee;
  competencyScores: Array<any>;
  clientId: number;
  textQuestionsAndFeedBacks: Array<INameValueArry>;
  yesnoQuestionsAndFeedBacks: Array<INameValueArry>;
  multiSelectQuestionsAndFeedBacks: Array<INameValueArry>;
  feedbacksReceived: IReviewIdWithFeedbackResponse[];
  readonly reviewedEmps: { [id: number]: string } = {};

  constructor(
    private manager: PerformanceManagerService,
    private svc: PerformanceReviewsService
  ) {}

  get hasCompletedReviews() {
    return this.group.reviews.filter(
      (review) => review.review.reviewCompletedDate != null
    );
  }

  ngOnInit() {
    this.empReviews = [];

    this.svc
      .isScoringEnabledForReviewTemplate(this.group.reviewTemplateId)
      .subscribe((x) => {
        this.isScoringEnabled = x.data;
      });

    this.group.statusGroups.forEach((stGroup) => {
      stGroup.reviewStatuses.forEach((revStatus) => {
                let emp = _.filter(this.empReviews, { employeeId: revStatus.review.reviewedEmployeeContact.employeeId });
                if(emp.length==0)
                {
	                var firs = revStatus.review.reviewedEmployeeContact.firstName;
	                var las = revStatus.review.reviewedEmployeeContact.lastName;
	        	      this.empReviews.push(<IAssignedToEmployee>{ employeeId: revStatus.review.reviewedEmployeeContact.employeeId, firstName: firs, lastName: las, fullName: las + ', ' + firs });
		            }
                if(revStatus.review.reviewOwnerContact)
                {
                    let own = _.filter(this.empReviews, { employeeId: revStatus.review.reviewOwnerContact.employeeId });
                    if(own.length==0){
                        var firs = revStatus.review.reviewOwnerContact.firstName;
                        var las = revStatus.review.reviewOwnerContact.lastName;
                        this.empReviews.push(<IAssignedToEmployee>{ employeeId: revStatus.review.reviewOwnerContact.employeeId, firstName: firs, lastName: las, fullName: las + ', ' + firs });
                    }
                }
                if(revStatus.review.evaluations)
                {
                    revStatus.review.evaluations.forEach(evt =>{
                        if(evt.evaluatedByContact){
                            let sup =  _.filter(this.empReviews, { employeeId: evt.evaluatedByContact.employeeId });
                            if(sup.length==0){
                                var firs = evt.evaluatedByContact.firstName;
                                var las = evt.evaluatedByContact.lastName;
                                this.empReviews.push(<IAssignedToEmployee>{ employeeId: evt.evaluatedByContact.employeeId, firstName: firs, lastName: las, fullName: las + ', ' + firs });
                            }
                        }
                    })
                    
                }
                
                //var firs = revStatus.review.reviewedEmployeeContact.firstName;
                //var las = revStatus.review.reviewedEmployeeContact.lastName;
                //this.empReviews.push(<IAssignedToEmployee>{ employeeId: revStatus.review.reviewedEmployeeContact.employeeId, firstName: firs, lastName: las, fullName: las + ', ' + firs });
        });
      });
      
    this.empReviews = this.empReviews.sort((x, y) =>
      x.fullName > y.fullName ? 1 : -1
    );
    this.selectedUser = <IAssignedToEmployee>null;

    // Populate the cmpletency evaludations in employee level for chart
    this.competencyScores = [];

    this.group.reviews.forEach((stGroup) => {
      this.clientId = stGroup.review.clientId;
      var firs = stGroup.review.reviewedEmployeeContact.firstName;
      var las = stGroup.review.reviewedEmployeeContact.lastName;
      var empName = las + ", " + firs;
      this.reviewedEmps[stGroup.review.reviewId] = empName;

      // populate the competency scores
      var scoresAdded = false;
      if (
        !!stGroup.review.evaluations &&
        stGroup.review.evaluations.length > 0
      ) {
        stGroup.review.evaluations
          .filter((x) => x.role == EvaluationRoleType.Manager)
          .forEach((ev) => {
            // Manger evaluation takes precedence
            if (
              this.createScoreRecords(
                ev,
                stGroup.review.reviewedEmployeeId,
                empName
              )
            )
              scoresAdded = true;
          });

        if (!scoresAdded)
          stGroup.review.evaluations
            .filter((x) => x.role == EvaluationRoleType.Peer)
            .forEach((ev) => {
              // Peer is priority 2
              if (
                this.createScoreRecords(
                  ev,
                  stGroup.review.reviewedEmployeeId,
                  empName
                )
              )
                scoresAdded = true;
            });

        if (!scoresAdded)
          stGroup.review.evaluations
            .filter((x) => x.role == EvaluationRoleType.Self)
            .forEach((ev) => {
              // Self is priority 3
              if (
                this.createScoreRecords(
                  ev,
                  stGroup.review.reviewedEmployeeId,
                  empName
                )
              )
                scoresAdded = true;
            });
      }

      // No evaluation exists for the competency
      if (!scoresAdded && !!this.competencyModels) {
        if (stGroup.review.reviewedEmployeeContact.competencyModel) {
          var modelId =
            stGroup.review.reviewedEmployeeContact.competencyModel
              .competencyModelId;

          var model = this.competencyModels.find(
            (x) => x.competencyModelId == modelId
          );
          if (!!model && !!model.competencies) {
            model.competencies.forEach((y) => {
              var temp = {
                employeeId: stGroup.review.reviewedEmployeeId,
                employeeName: empName,
                competencyId: y.competencyId,
                competencyName: y.name,
                score: 0,
              };

              this.competencyScores.push(temp);
            });
          }
        }
      }

      // collect the feedbacks
      this.feedbacksReceived = [];
      stGroup.review.evaluations.forEach((ev) => {
        ev.feedbackResponses.forEach((fe) => {
          const val: IReviewIdWithFeedbackResponse = {
            feedback: <IFeedbackResponseData>fe,
            reviewId: ev.reviewId,
          };
          val.feedback.evaluationRoleType = ev.role;
          this.feedbacksReceived.push(val);
        });
      });
      var separateFeedbacks = (typ: FieldType): Array<INameValueArry> => {
        let holder: Array<INameValueArry> = [];

        this.feedbacksReceived
          .filter((x) => x.feedback.fieldType == typ)
          .sort((x, y) => x.feedback.feedbackId - y.feedback.feedbackId)
          .forEach((x) => {
            let q = holder.find(
              (y) => y.name == x.feedback.feedbackId.toString()
            );
            if (q) {
              let k: Array<IReviewIdWithFeedbackResponse> = <
                Array<IReviewIdWithFeedbackResponse>
              >(<object>q.value);
              k.push(x);
            } else {
              let k: Array<IReviewIdWithFeedbackResponse> = [];
              k.push(x);
              holder.push(<INameValueArry>{
                name: x.feedback.feedbackId.toString(),
                value: <object>k,
              });
            }
          });
        return holder;
      };
      let tqfb = separateFeedbacks(FieldType.Text);
      let ynfb = separateFeedbacks(FieldType.Boolean);
      let msfb = separateFeedbacks(FieldType.MultipleSelection);
      if (this.textQuestionsAndFeedBacks) {
        tqfb.forEach((x) => {
          let idx = this.textQuestionsAndFeedBacks.findIndex(
            (y) => y.name == x.name
          );
          if (idx > -1) {
            x.value.forEach((z) => {
              this.textQuestionsAndFeedBacks[idx].value.push(z);
            });
          } else {
            if (x.name != "") this.textQuestionsAndFeedBacks.push(x);
          }
        });
      } else {
        this.textQuestionsAndFeedBacks = tqfb;
      }
      if (this.yesnoQuestionsAndFeedBacks) {
        ynfb.forEach((x) => {
          let idx = this.yesnoQuestionsAndFeedBacks.findIndex(
            (y) => y.name == x.name
          );
          if (idx > -1) {
            x.value.forEach((z) => {
              this.yesnoQuestionsAndFeedBacks[idx].value.push(z);
            });
          } else {
            if (x.name != "") this.yesnoQuestionsAndFeedBacks.push(x);
          }
        });
      } else {
        this.yesnoQuestionsAndFeedBacks = ynfb;
      }
      if (this.multiSelectQuestionsAndFeedBacks) {
        msfb.forEach((x) => {
          let idx = this.multiSelectQuestionsAndFeedBacks.findIndex(
            (y) => y.name == x.name
          );
          if (idx > -1) {
            x.value.forEach((z) => {
              this.multiSelectQuestionsAndFeedBacks[idx].value.push(z);
            });
          } else {
            if (x.name != "") this.multiSelectQuestionsAndFeedBacks.push(x);
          }
        });
      } else {
        this.multiSelectQuestionsAndFeedBacks = msfb;
      }
    });
  }

  createScoreRecords(ev: IEvaluation, empId: number, empName: string): boolean {
    var scored = false;
    if (!!ev.competencyEvaluations && ev.competencyEvaluations.length > 0) {
      ev.competencyEvaluations
        .filter((x) => x.ratingValue != null && x.ratingValue != undefined)
        .forEach((compEv) => {
          var temp = {
            employeeId: empId,
            employeeName: empName,
            competencyId: compEv.competencyId,
            competencyName: compEv.name,
            score: compEv.ratingValue,
          };

          this.competencyScores.push(temp);
          scored = true;
        });
    }
    return scored;
  }

  reviewUpdated(review: IReview) {
    this.reviewChange.emit(review);
  }

  evaluationSelected(event: { evaluation: IEvaluation; review: IReview }) {
    this.evaluationSelect.emit(event);
  }

  findFeedback(feedbackId: string) {
    return this.allFeedbackQuestions.find(
      (x) => x.feedbackId == parseInt(feedbackId)
    );
  }

  filterAssigned() {
    // var name = this.selectedUser;
  }

  filterStatus(statusGroup: StatusGroup): any[] {
    if (this.selectedUser === null) {
      return statusGroup.reviewStatuses;
    }
    const selectedEmployeeId = this.selectedUser.employeeId;
    //var firstName = this.selectedUser.substring(this.selectedUser.indexOf(',')+1).trim();
    //var lastname = this.selectedUser.substring(0,this.selectedUser.indexOf(',')).trim();
    var reviewStatuses = []; 
    statusGroup.reviewStatuses.forEach(revStat => {
    if (selectedEmployeeId === revStat.employee.employeeId ||
        (revStat.review.reviewOwnerContact && selectedEmployeeId === revStat.review.reviewOwnerContact.employeeId)) {
 
         reviewStatuses.push(revStat);
    }
    else if(revStat.review.evaluations)
    {
        revStat.review.evaluations.forEach(evt =>{
            if(evt.evaluatedByContact && selectedEmployeeId == evt.evaluatedByContact.employeeId)
            {
                reviewStatuses.push(revStat);
            }
        })
    }
 })
    return this.sortReviews(this.sortReviews(reviewStatuses));
  }

  private sortReviews(filteredReviews: IReviewStatus[]): IReviewStatus[] {
    return filteredReviews.sort((x, y) =>
      x.review.reviewedEmployeeContact.lastName +
        x.review.reviewedEmployeeContact.firstName >
      y.review.reviewedEmployeeContact.lastName +
        y.review.reviewedEmployeeContact.firstName
        ? 1
        : -1
    );
  }
}

export interface INameValueArry {
  name: string;
  value: any[];
}
