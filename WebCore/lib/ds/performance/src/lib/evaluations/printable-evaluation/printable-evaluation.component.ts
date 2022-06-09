import { Component, OnInit } from "@angular/core";
import { EvaluationsApiService } from "../shared/evaluations-api.service";
import { EmployeeSearchService } from "@ajs/employee/search/shared/employee-search.service";
import {
  from,
  forkJoin,
  Observable,
  throwError,
  of,
  concat,
  zip,
  iif,
} from "rxjs";
import { IEmployeeSearchResultResponseData } from "@ajs/employee/search/shared/models";
import { IEvaluationDetail } from "../shared/evaluation-detail.model";
import { ReviewsService } from "../../reviews/shared/reviews.service";
import {
  map,
  catchError,
  tap,
  share,
  concatMap,
  reduce,
  shareReplay,
  publishReplay,
  finalize,
  refCount,
} from "rxjs/operators";
import * as moment from "moment";
import { ICompetencyEvaluation } from "../shared/competency-evaluation.model";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { EvaluationRoleType } from "../shared/evaluation-role-type.enum";
import { IGoalEvaluation } from "../shared/goal-evaluation.model";
import { IFeedbackResponse } from "@ds/performance/feedback";
import { IReviewRating } from "@ds/performance/ratings";
import { PerformanceReviewsService } from "@ds/performance/shared/performance-reviews.service";
import { ScoreModel } from "@ds/performance/ratings/shared/score-model.model";
import { IEvalDetailAndSyncContent } from "../shared/eval-data-and-sync-content.model";
import { IScoreGroup } from "../shared/score-group.model";
import { UserType, UserInfo } from "@ds/core/shared";
import { AccountService } from "@ds/core/account.service";
import { ScoreModelSettings } from "../shared/score-model-settings.model";
import { ReviewProfilesApiService } from "@ds/performance/review-profiles/review-profiles-api.service";
import { IReview } from "@ds/performance/reviews/shared/review.model";
import { Maybe } from "@ds/core/shared/Maybe";
import {
  PrintEvaluationArgs,
  PRINT_EVALUATION_KEY,
} from "@ds/performance/shared/print-evaluation-args";
import { DsStorageService } from "@ds/core/storage/storage.service";
import { IEvaluation } from "../shared/evaluation.model";
import { ApprovalProcessHistoryAction } from "../shared/approval-process-history-action.enum";

const inputDateFormatString = "YYYY-MM-DDTHH:mm:ss";
const outputDateFormatString = "MM/DD/YYYY";

interface IEvaluationDetailWithSupervisor extends IEvaluationDetail {
  supervisor: string;
}

interface TitleAndPeriod {
  title: string;
  evalPeriod: string;
  instructions: string;
}

interface IScoreModelShouldShowBlankScore extends IScoreGroup {
  shouldShowBlankScore: boolean;
}
/**
 * Display an evaluation in a printer friendly format.  Always opens up in the payroll site.
 */
@Component({
  selector: "ds-printable-evaluation",
  templateUrl: "./printable-evaluation.component.html",
  styleUrls: ["./printable-evaluation.component.scss"],
})
export class PrintableEvaluationComponent implements OnInit {
  constructor(
    private evalSvc: EvaluationsApiService,
    private empService: EmployeeSearchService,
    private reviewSvc: ReviewsService,
    private msg: DsMsgService,
    private service: PerformanceReviewsService,
    private acctSvc: AccountService,
    private reviewProfSvc: ReviewProfilesApiService,
    private store: DsStorageService
  ) {}

  params: {
    employeeid: number;
    reviewid: number;
    evaluationid: number;
    /**
     * Always filter feedback if user wants to print employee view
     */
    printforemp: boolean;
  } = {
    employeeid: null,
    reviewid: null,
    evaluationid: null,
    printforemp: false,
  };

  arrayWithLengthOf5 = [1, 2, 3, 4, 5];
  data: PrintEvaluationArgs;
  actualScore: any;
  ratingDesc: string;
  employeeNotes: any;
  readonly noValString = "Not Specified";
  readonly objectTypeRef = Object;
  private titleString = ["Eval", ""];
  private myWindow: any = window;
  shouldHideFeedback: boolean;

  ratingDictionary: { [id: number]: IReviewRating } = {};
  data$: Observable<{
    detail: IEvaluationDetail;
    searchResult: IEmployeeSearchResultResponseData;
    titleAndPeriod: TitleAndPeriod;
    canShowScores: boolean;
    group: IScoreGroup[];
    overallScore: number;
    scoreModel: ScoreModel;
    showBlankOverallScore: boolean;
    ratingDesc?: string;
  }>;

  hasError = false;

  /**
   * Displays the printable evaluation to the user
   */
  static printEval(
    evaluation: IEvaluation,
    review: IReview,
    accountSvc: AccountService,
    store: DsStorageService,
    printForEmp?: boolean
  ): void {
    const nonNullPrintForEmp = !!printForEmp;
    accountSvc
      .getLegacyRootUrl()
      .pipe(
        shareReplay(1),
        tap((url) => {
          const data: PrintEvaluationArgs = {
            reviewedEmployeeId: review.reviewedEmployeeId,
            reviewId: review.reviewId,
            evaluationId: evaluation.evaluationId,
            printForEmp: nonNullPrintForEmp,
          };

          const storeResult = store.set(PRINT_EVALUATION_KEY, data);

          if (storeResult.success) {
            window.open(
              url + "/Popup.aspx" + "#/performance/print-evaluation",
              "Evaluation",
              "height=640,width=960,toolbar=no,menubar=no,scrollbars=yes,location=no,status=no"
            );
          }
        })
      )
      .subscribe();
  }

  ngOnInit() {
    const storeResult = this.store.get(PRINT_EVALUATION_KEY);
    this.hasError = storeResult.hasError;
    this.data = storeResult.data;

    const getDetail$ = this.evalSvc
      .getEvaluationDetail(this.data.evaluationId)
      .pipe(
        concatMap((detail) => {
          const safeMaybe = new Maybe(detail);
          const hasCompletedDate = safeMaybe.map(
            (x) => !x.approvalProcessHistory && x.completedDate
          );
          const isApprovalProcessFinalized = safeMaybe
            .map((x) => x.approvalProcessHistory)
            .map((x) => {
              const wasContinued =
                x.length &&
                x[x.length - 1].action ===
                  ApprovalProcessHistoryAction.Continued;
              const wasFinalized =
                x.length &&
                x[x.length - 1].action ===
                  ApprovalProcessHistoryAction.Finalized;
              const isFirstStepOfProcess = x.length === 1;
              return (wasContinued && !isFirstStepOfProcess) || wasFinalized;
            })
            .valueOr(false);
          const shouldNotSync =
            isApprovalProcessFinalized || !!hasCompletedDate.value();
          return iif(
            () => shouldNotSync,
            of(detail),
            this.evalSvc
              .getSyncContentAndDetail(this.data.evaluationId)
              .pipe(map((x) => x.data.syncContent || x.data.detail))
          );
        }),
        publishReplay(1),
        refCount()
      );

      let shouldHideFeedback$ = this.acctSvc.PassUserInfoToRequest((userInfo) =>
        getDetail$.pipe(
          map((x) => {
            const isSelfSelected = userInfo.userEmployeeId === x.reviewedEmployeeContact.employeeId;
            
            this.shouldHideFeedback = this.data.printForEmp || isSelfSelected;

            return (
              this.data.printForEmp || isSelfSelected
            );
          })
        )
      );

    const canShowScores$ = this.acctSvc.getUserInfo().pipe(
      concatMap((userInfo) =>
        this.service.getScoringSettings(this.data.reviewId).pipe(
          map((x) => this.checkSettingsAndUserType(x.data, userInfo)),
          concatMap((prevResult) => {
            return getDetail$.pipe(
              map(
                (y) =>
                  prevResult ||
                  this.isNotUsersSelfEval(y, userInfo) ||
                  this.isUsersEvalOfEmp(y, userInfo)
              )
            );
          })
        )
      )
    );

    const calcScore$ = this.service
      .calculateEvalScore(this.data.evaluationId, false, true)
      .pipe(share());

    const flattenedAndSortedScoreModels$ = calcScore$.pipe(
      map(this.flattenAndSortEvalScore as any)
    );
    const overallScore$ = calcScore$.pipe(map((x: any) => x.score));
    const showBlankOverallScore$ = flattenedAndSortedScoreModels$.pipe(
      map((x: any) => x.some((y) => y.shouldShowBlankScore === true))
    );

    const getReviewData$ = this.reviewSvc
      .getReviewsByEvaluationId(this.data.evaluationId)
      .pipe(
        map((x) => x || <IReview>{}),
        share()
      );

    const scoreData$ = concat(
      canShowScores$,
      forkJoin(
        flattenedAndSortedScoreModels$,
        overallScore$,
        showBlankOverallScore$
      )
    ).pipe(
      reduce<boolean | [IScoreGroup[], number, boolean], ScoreDataResult>(
        (acc, curr) => ({
          canShowScores: acc as unknown as boolean, // the first thing emitted is a boolean, this accumulator fn isn't called until the second thing is emitted
          group: curr[0],
          overallScore: curr[1],
          showBlankOverallScore: curr[2],
        })
      )
    );

    this.data$ = this.whenAllDataLoaded(
      forkJoin(
        this.getEvalStuff(shouldHideFeedback$, getDetail$),
        this.searchEmps(),
        this.getEmpReviews(getReviewData$),
        scoreData$,
        this.service.getScoreModelForCurrentClient()
      ).pipe(
        catchError((x) => {
          this.msg.setTemporaryMessage(
            "Failed to load evaluation data.",
            MessageTypes.error,
            6000
          );
          return throwError(x);
        })
      )
    );

    this.reviewSvc.getReviewsByReviewId(this.data.reviewId).subscribe((x) => {
      this.employeeNotes = x.employeeNotes;
    });
  }

  private readonly TraverseTreeInOrder: (tree: IScoreGroup) => IScoreGroup[] = (
    tree
  ) => {
    var result: IScoreGroup[] = [];
    if ((tree.items || []).some((x) => (<IScoreGroup>x).items != null)) {
      for (var i = 0; i < tree.items.length; i++) {
        const current = tree.items[i];
        result = result.concat(this.TraverseTreeInOrder(<IScoreGroup>current));
      }
      return result;
    } else {
      return [tree];
    }
  };

  private readonly sortScoreGroups: (groups: IScoreGroup[]) => IScoreGroup[] = (
    groups
  ) => {
    return groups.sort((a, b) => {
      const groupNameA = new Maybe(a)
        .map((x) => x.items)
        .map((x) => (x.length > 0 ? x[0] : null))
        .map((x) => x.evalInfo)
        .map((x) => x.groupName);
      const groupNameB = new Maybe(b)
        .map((x) => x.items)
        .map((x) => (x.length > 0 ? x[0] : null))
        .map((x) => x.evalInfo)
        .map((x) => x.groupName);
      const isStringEqualTo = (val: string) => (otherString: string) =>
        val.toLowerCase().localeCompare(otherString.toLowerCase()) === 0;
      if (groupNameA.map(isStringEqualTo("Competencies")).value()) {
        return -1;
      }
      if (groupNameA.map(isStringEqualTo("Goals")).value()) {
        return 1;
      }
      if (groupNameB.map(isStringEqualTo("Competencies")).value()) {
        return 1;
      }
      if (groupNameB.map(isStringEqualTo("Goals")).value()) {
        return -1;
      }
      return groupNameA.valueOr("").localeCompare(groupNameB.valueOr(""));
    });
  };

  private readonly sortEvaluationItems: (
    groups: IScoreGroup[]
  ) => IScoreGroup[] = (groups) => {
    groups.forEach((x) => {
      x.items = x.items.sort((a, b) =>
        (a.name.toLowerCase() || "").localeCompare(b.name.toLowerCase() || "")
      );
    });
    return groups;
  };

  private readonly addBlankScoreField: (
    groups: IScoreGroup[]
  ) => IScoreModelShouldShowBlankScore[] = (groups) => {
    var result: IScoreModelShouldShowBlankScore[] = [];
    for (var i = 0; i < groups.length; i++) {
      const current = groups[i] as IScoreModelShouldShowBlankScore;
      current.shouldShowBlankScore = current.items.some((x) => x.score == null);
      result.push(current);
    }

    return result;
  };

  private readonly flattenAndSortEvalScore = (tree: IScoreGroup) => {
    tree.items = tree.items.filter((x) => {
      const group = x as IScoreGroup;
      return (group.items != null && group.items.length) || group.items == null;
    });
    this.actualScore = tree;
    return this.addBlankScoreField(
      this.sortEvaluationItems(
        this.sortScoreGroups(this.TraverseTreeInOrder(tree))
      )
    );
  };

  private readonly checkSettingsAndUserType = (
    settings: ScoreModelSettings,
    userInfo: UserInfo
  ) =>
    settings.isScoringEnabled &&
    (settings.isScoreEmployeeViewable ||
      userInfo.userTypeId === UserType.companyAdmin ||
      userInfo.userTypeId === UserType.systemAdmin);

  private readonly isNotUsersSelfEval = (
    detail: IEvaluationDetail,
    userInfo: UserInfo
  ) =>
    detail.role == EvaluationRoleType.Self &&
    userInfo.userId !== detail.evaluatedByUserId &&
    userInfo.userTypeId === UserType.supervisor;

  private readonly isUsersEvalOfEmp = (
    detail: IEvaluationDetail,
    userInfo: UserInfo
  ) =>
    detail.role == EvaluationRoleType.Manager &&
    userInfo.userId === detail.evaluatedByUserId &&
    userInfo.userTypeId === UserType.supervisor;

  private getValueOrEmptyArray<T>(value: T[]) {
    if (value == null) {
      return [];
    }
    return value;
  }

  private ProcessFeedbackResponses(
    detail: IEvalDetailAndSyncContent
  ): IFeedbackResponse[] {
    const nonNullSyncContent = detail.syncContent || <IEvaluationDetail>{};
    const nonNullDetail = detail.detail || <IEvaluationDetail>{};
    if (nonNullDetail.feedbackResponses == null) {
      nonNullDetail.feedbackResponses = nonNullSyncContent.feedbackResponses;
    } else if (nonNullSyncContent.feedbackResponses == null) {
    } else {
      // since we don't want feedback sorted in the printed evaluation we need different functionality to insert vals in to the displayed array.
      // the functionality applied to goal and competency evaluations also sorts them
      let responses = {};
      var i;
      // put new vals into dictionary
      for (i = 0; i < nonNullSyncContent.feedbackResponses.length; i++) {
        console.log(nonNullSyncContent.feedbackResponses[i]);
        responses[
          nonNullSyncContent.feedbackResponses[i].feedbackId.toString() +
            nonNullSyncContent.feedbackResponses[i].responseId.toString()
        ] = nonNullSyncContent.feedbackResponses[i];
      }
      // update any existing vals
      for (i = 0; i < nonNullDetail.feedbackResponses.length; i++) {
        var existing = nonNullDetail.feedbackResponses[i];
        console.log(existing.feedbackId);
        if (
          responses[
            existing.feedbackId.toString() + existing.responseId.toString()
          ] != null &&
          !existing.hasResponse() &&
          responses[
            existing.feedbackId.toString() + existing.responseId.toString()
          ].hasResponse()
        ) {
          existing =
            responses[
              existing.feedbackId.toString() + existing.responseId.toString()
            ];
          responses[
            existing.feedbackId.toString() + existing.responseId.toString()
          ] = undefined;
        }
      }
      // append any remaining feedback to our list
      var keys = Object.keys(responses);
      var arrayToAppend = [];
      for (i = 0; i < keys.length; i++) {
        if (this.params[keys[i]] != null) {
          arrayToAppend.push(this.params[keys[i]]);
        }
      }
      nonNullDetail.feedbackResponses =
        nonNullSyncContent.feedbackResponses.concat(arrayToAppend);
    }
    return nonNullDetail.feedbackResponses;
  }

  private readonly getEvalStuff = (
    hideFeedback$: Observable<boolean>,
    getDetail$: Observable<IEvaluationDetail>
  ) => {
    return zip(getDetail$, hideFeedback$).pipe(
      concatMap((x) =>
        of(x[0]).pipe(
          map((fromServer) => {
            const nonNullDetail = fromServer || <IEvaluationDetail>{};

            const result: IEvaluationDetailWithSupervisor = {
              allowGoalWeightAssignment:
                nonNullDetail.allowGoalWeightAssignment,
              competencyEvaluations: this.getValueOrEmptyArray(
                nonNullDetail.competencyEvaluations
              ),
              goalEvaluations: this.getValueOrEmptyArray(
                nonNullDetail.goalEvaluations
              ),
              feedbackResponses: this.ProcessFeedbackResponses({
                detail: fromServer,
                syncContent: null,
              }),
              completedDate: nonNullDetail.completedDate,
              dueDate: nonNullDetail.dueDate,
              evaluatedByContact: nonNullDetail.evaluatedByContact,
              evaluatedByUserId: nonNullDetail.evaluatedByUserId,
              currentAssignedUserId: nonNullDetail.currentAssignedUserId,
              evaluationId: nonNullDetail.evaluationId,
              isViewableByEmployee: nonNullDetail.isViewableByEmployee,
              overallRatingValue: nonNullDetail.overallRatingValue,
              ratings: nonNullDetail.ratings,
              reviewId: nonNullDetail.reviewId,
              signature: nonNullDetail.signature,
              supervisor:
                new Maybe(fromServer)
                  .map((x) => x.evaluatedByContact)
                  .map((x) => x.firstName)
                  .valueOr("") +
                " " +
                new Maybe(fromServer)
                  .map((x) => x.evaluatedByContact)
                  .map((x) => x.lastName)
                  .valueOr(""),
              meritIncreaseInfo: nonNullDetail.meritIncreaseInfo,
              reviewedEmployeeContact: nonNullDetail.reviewedEmployeeContact,
              role: nonNullDetail.role,
              startDate: nonNullDetail.startDate,
              approvalProcessHistory: null,
              approvalProcessAction: nonNullDetail.approvalProcessAction,
              isApprovalProcess: nonNullDetail.isApprovalProcess,
            };
            return result;
          }),
          map((data) => {
            const result = data;
            this.updateTitle(
              0,
              (result.role == EvaluationRoleType.Self
                ? "Employee"
                : "Supervisor") + " Evaluation"
            );
            var projected = {
              noGroup: [],
              groups: {},
            };
            var i;
            var filteredGoals: IGoalEvaluation[] = [];
            for (i = 0; i < result.goalEvaluations.length; i++) {
              const evaluation = result.goalEvaluations[i];
              this.insertEval<IGoalEvaluation>(
                filteredGoals,
                evaluation,
                (a, b) =>
                  a.evaluationId == b.evaluationId && a.goalId == b.goalId,
                (val) => val.title,
                this.assignValIfNullCompEval
              );
            }
            result.completedTime =
              result.completedDate == null
                ? null
                : this.getMoment(result.completedDate).format("h:mm a");
            result.completedDate =
              result.completedDate == null
                ? null
                : this.getMoment(result.completedDate).format(
                    outputDateFormatString
                  );
            result.goalEvaluations = result.goalEvaluations.sort((a, b) => {
              return a.title.toLowerCase().localeCompare(b.title.toLowerCase());
            });

            for (i = 0; i < result.competencyEvaluations.length; i++) {
              const evaluation = result.competencyEvaluations[i];
              if (evaluation.groupName === "" || evaluation.groupName == null) {
                this.insertEval<ICompetencyEvaluation>(
                  projected.noGroup,
                  evaluation,
                  this.pkComparatorCompEval,
                  (val) => val.name,
                  this.assignValIfNullCompEval
                );
              } else {
                if (projected.groups[evaluation.groupName] == null) {
                  projected.groups[evaluation.groupName] = [];
                }
                this.insertEval<ICompetencyEvaluation>(
                  projected.groups[evaluation.groupName],
                  evaluation,
                  this.pkComparatorCompEval,
                  (val) => val.name,
                  this.assignValIfNullCompEval
                );
              }
            }

            (<any>result).projCompEvaluations = projected;
            (<any>result).filteredGoals = filteredGoals;

            if ( this.shouldHideFeedback ) {
              (<any>result).filteredFeedback = result.feedbackResponses.filter(
                (r) => r.isVisibleToEmployee
              );
            } else {
              (<any>result).filteredFeedback = result.feedbackResponses;
            }

            return result;
          }),
          tap((result) => {
            var i;
            for (i = 0; i < result.ratings.length; i++) {
              var rating = result.ratings[i];
              this.ratingDictionary[rating.rating] = rating;
            }
          })
        )
      )
    );
  };

  private readonly searchEmps = () => {
    return from(
      <PromiseLike<IEmployeeSearchResultResponseData>>(
        this.empService.searchEmployees({
          employeeId: this.data.reviewedEmployeeId,
        })
      )
    ).pipe(
      tap((x) => {
        this.updateTitle(
          1,
          " for " + x.nav.current.firstName + " " + x.nav.current.lastName
        );
      })
    );
  };

  private readonly getEmpReviews = (review$: Observable<IReview>) => {
    return review$.pipe(
      map((found) => {
        const result: TitleAndPeriod = {
          title: found.title,
          evalPeriod:
            this.getMoment(found.evaluationPeriodFromDate).format(
              outputDateFormatString
            ) +
            " - " +
            this.getMoment(found.evaluationPeriodToDate).format(
              outputDateFormatString
            ),
          instructions: found.instructions,
        };
        return result;
      })
    );
  };

  private readonly whenAllDataLoaded = (
    data: Observable<
      [
        IEvaluationDetail,
        IEmployeeSearchResultResponseData,
        TitleAndPeriod,
        ScoreDataResult,
        { data: ScoreModel }
      ]
    >
  ) => {
    return data.pipe(
      catchError((e) => {
        return throwError(e);
      }),
      map((result) => ({
        detail: result[0],
        searchResult: result[1],
        titleAndPeriod: result[2],
        canShowScores: result[3].canShowScores,
        group: result[3].group,
        overallScore: result[3].overallScore,
        scoreModel: result[4].data,
        showBlankOverallScore: result[3].showBlankOverallScore,
        ratingDesc: new Maybe(result[4])
          .map((x) => x.data)
          .map((x) => x.scoreRangeLimits)
          .map((x) => x.sort((a, b) => b.maxScore - a.maxScore))
          .map((x) => {
            if (result[3].overallScore == null || !x.length) {
              return null;
            }

            var i;
            for (i = 0; i < x.length; i++) {
              if (result[3].overallScore >= x[i].maxScore) {
                break;
              }
            }

            if (i == 0) {
              return x[i].label;
            }
            return x[i - 1].label;
          })
          .valueOr(""),
      })),
      tap(() => setTimeout(() => this.myWindow.print(), 200))
    );
  };

  private insertEval<T>(
    list: T[],
    evaluation: T,
    pkComparator: (added: T, newVal: T) => boolean,
    getStringValToCompare: (val: T) => string,
    assignValIfNull: (existing: T, current: T) => void
  ) {
    if (list.length == 0) {
      list.push(evaluation);
    } else if (list.length == 1) {
      if (
        getStringValToCompare(list[0])
          .toLowerCase()
          .localeCompare(getStringValToCompare(evaluation).toLowerCase()) < 0
      ) {
        list.push(evaluation);
      } else if (
        getStringValToCompare(list[0])
          .toLowerCase()
          .localeCompare(getStringValToCompare(evaluation).toLowerCase()) == 0
      ) {
        if (!pkComparator(list[0], evaluation)) {
          list.push(evaluation);
        }
      } else {
        list.splice(0, 0, evaluation);
      }
    } else {
      this.searchAndInsertEval(
        list,
        0,
        Math.floor((list.length - 1) / 2),
        list.length,
        evaluation,
        pkComparator,
        getStringValToCompare,
        assignValIfNull
      );
    }
  }

  private searchAndInsertEval<T>(
    list: T[],
    lower: number,
    middle: number,
    upper: number,
    evaluation: T,
    pkComparator: (added: T, newVal: T) => boolean,
    getStringValToCompare: (val: T) => string,
    assignValIfNull: (existing: T, current: T) => void
  ): void {
    const evalName = getStringValToCompare(evaluation).toLowerCase();
    const compareResultMid = getStringValToCompare(list[middle])
      .toLowerCase()
      .localeCompare(evalName);
    const compareResultNext = getStringValToCompare(list[middle + 1])
      .toLowerCase()
      .localeCompare(evalName);

    if (compareResultMid <= 0 && compareResultNext > 0) {
      if (
        !(
          pkComparator(list[middle], evaluation) ||
          pkComparator(list[middle + 1], evaluation)
        )
      ) {
        list.splice(middle + 1, 0, evaluation);
      } else if (pkComparator(list[middle], evaluation)) {
        assignValIfNull(list[middle], evaluation);
      } else if (pkComparator(list[middle + 1], evaluation)) {
        assignValIfNull(list[middle + 1], evaluation);
      }
      return;
    }

    if (compareResultMid > 0 && compareResultNext > 0) {
      if (middle == 0) {
        list.splice(0, 0, evaluation);
        return;
      }
      this.searchAndInsertEval(
        list,
        lower,
        Math.floor((lower + middle) / 2),
        middle,
        evaluation,
        pkComparator,
        getStringValToCompare,
        assignValIfNull
      );
    }

    if (compareResultMid < 0 && compareResultNext < 0) {
      if (middle + 1 == list.length - 1) {
        list.push(evaluation);
        return;
      }
      this.searchAndInsertEval(
        list,
        middle,
        Math.floor((upper + middle) / 2),
        upper,
        evaluation,
        pkComparator,
        getStringValToCompare,
        assignValIfNull
      );
    }
  }

  /**
   * Modifes the data used to set the popup title and then manually updates the title.
   * @param indexToUpdate Where in the titleString array we want to update
   * @param value The value used to update the titleString
   */
  private updateTitle(indexToUpdate: number, value: string): void {
    this.titleString[indexToUpdate] = value;
    document.head.getElementsByTagName("title").item(0).innerHTML =
      this.titleString[0] + this.titleString[1];
  }

  private pkComparatorCompEval(
    added: ICompetencyEvaluation,
    newVal: ICompetencyEvaluation
  ): boolean {
    return (
      added.evaluationId == newVal.evaluationId &&
      added.competencyId == newVal.competencyId
    );
  }

  private getMoment(date: string | Date): moment.Moment {
    return typeof date === "string"
      ? moment(date, inputDateFormatString)
      : moment(date);
  }

  private assignValIfNullCompEval(
    a: ICompetencyEvaluation | IGoalEvaluation,
    b: ICompetencyEvaluation | IGoalEvaluation
  ): void {
    if (a.ratingValue == null && b.ratingValue != null) {
      a.ratingValue = b.ratingValue;
    }
    if (a.comment == null && b.comment != null) {
      a.comment = b.comment;
    }
  }
}

interface ScoreDataResult {
  canShowScores: boolean;
  group: IScoreGroup[];
  overallScore: number;
  showBlankOverallScore: boolean;
}
