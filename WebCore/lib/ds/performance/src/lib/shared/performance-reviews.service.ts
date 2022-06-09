import { Injectable } from "@angular/core";
import { Subject, ReplaySubject, Observable, of, throwError } from "rxjs";
import {
  catchError,
  defaultIfEmpty,
  tap,
  map,
  concat,
  switchMap,
  concatMap,
  share,
  finalize,
  take,
  publishReplay,
  refCount,
} from "rxjs/operators";
import { IReviewRating } from "../ratings";
import { HttpClient, HttpParams } from "@angular/common/http";
import { AccountService } from "@ds/core/account.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import {
  ICompetency,
  ICompetencyGroup,
  ICompetencyModel,
  ICompetencyModelBasic,
} from "../competencies";
import { ICompetencyModelUpdate } from "@ds/performance/competencies/shared/competency-model-update.model";
import { EmployeePerformanceConfiguration } from "../competencies/shared/employee-performance-configuration.model";
import { ScoreMethodType } from "../ratings/shared/score-method-type.model";
import { ScoreModel } from "../ratings/shared/score-model.model";
import { EvaluationGroupDisplay } from "../evaluations/shared/evaluation-group-display.model";
import { userInfo } from "os";
import { UserInfo } from "@ds/core/shared";
import { checkboxComponent } from "@ajs/applicantTracking/application/inputComponents";
import { IScoreGroup } from "../evaluations/shared/score-group.model";
import { IMeritIncreaseCurrentPayAndRates } from "../evaluations/shared/merit-increase-current-pay-and-rates.model";
import { IReviewMeritIncrease } from "../evaluations/shared/merit-increase.model";
import { ScoreModelSettings } from "../evaluations/shared/score-model-settings.model";
import { Maybe } from "@ds/core/shared/Maybe";

@Injectable({
  providedIn: "root",
})
export class PerformanceReviewsService {
  private readonly api = "api/performance";
  performanceReviewRatings: Subject<IReviewRating[]> = new ReplaySubject(1);
  user: UserInfo;
  private ScoringSettings: {
    [id: number]: Observable<{ data: ScoreModelSettings }>;
  };

  constructor(
    private http: HttpClient,
    private account: AccountService,
    private msg: DsMsgService
  ) {
    this.ScoringSettings = {};
  }

  getPerformanceReviewRatings(clientId: number): Observable<IReviewRating[]> {
    return this.http
      .get<IReviewRating[]>(this.api + "/clients/" + clientId + "/ratings")
      .pipe(
        catchError(
          this.account.handleError(
            "getPerformanceReviewRatings",
            <IReviewRating[]>[]
          )
        )
      );
  }

  savePerformanceReviewRatings(
    clientId: number,
    data: IReviewRating | IReviewRating[],
    performanceReviewRatingId?: number
  ): Observable<IReviewRating[]> {
    let url =
      performanceReviewRatingId == null
        ? this.api + "/clients/" + clientId + "/ratings"
        : this.api +
          "/clients/" +
          clientId +
          "/ratings/" +
          performanceReviewRatingId;
    data = !(data instanceof Array) ? [data] : data;
    return this.http
      .post<IReviewRating[]>(url, data)
      .pipe(
        catchError(
          this.httpError("savePerformanceReviewRatings", <IReviewRating[]>[])
        )
      );
  }

  savePerformanceScoring(dto: ScoreModel): Observable<ScoreModel> {
    return this.http
      .post<ScoreModel>(this.api + "/employee-performance", dto)
      .pipe(
        catchError(
          this.httpError("savePerformanceReviewScoring", <ScoreModel>{})
        )
      );
  }

  /**
   * Get competencies by client id.
   *
   * @param clientId
   */
  getPerformanceCompetencies(
    clientId: number,
    isArchived: boolean = false
  ): Observable<ICompetency[]> {
    let params = new HttpParams();
    params = params.append("isArchived", isArchived.toString());

    const url = this.api + "/clients/" + clientId + "/competencies";
    return this.http
      .get<ICompetency[]>(url, { params: params })
      .pipe(
        catchError(
          this.httpError("getPerformanceCompetencies", <ICompetency[]>[])
        )
      );
  }

  getPerformanceCompetenciesForCurrentClient(
    isArchived: boolean = false
  ): Observable<ICompetency[]> {
    return this.account
      .getUserInfo()
      .pipe(
        concatMap((x) =>
          this.getPerformanceCompetencies(
            x.clientId || x.lastClientId,
            isArchived
          )
        )
      );
  }

  getCoreCompetencies(clientId: number = null) {
    let params = new HttpParams();
    params = params.set("isCore", "true");

    if (clientId) params = params.set("clientId", clientId.toString());

    return this.http
      .get<ICompetency[]>(`${this.api}/competencies`, { params: params })
      .pipe(
        catchError(
          this.httpError("getPerformanceCompetencies", <ICompetency[]>[])
        )
      );
  }

  getCompetenciesByEmployee(
    clientId: number,
    employeeId: number
  ): Observable<ICompetency[]> {
    const url = `${this.api}/clients/${clientId}/employees/${employeeId}/competencies`;
    return this.http
      .get<ICompetency[]>(url)
      .pipe(catchError(this.httpError("getCompetenciesByEmployee", [])));
  }

  getOneTimeEarningSettings(
    clientId: number,
    employeeId: number
  ): Observable<any> {
    const url = `${this.api}/clients/${clientId}/employees/${employeeId}/onetime-earning-settings`;
    return this.http
      .get<any>(url)
      .pipe(catchError(this.httpError("getOneTimeEarningSettings", [])));
  }

  /**
   * Get default competencies
   */
  getDefaultCompetencies(clientId: number): Observable<ICompetency[]> {
    const url = this.api + "/clients/" + clientId + "/default-competencies";
    return this.http
      .get<ICompetency[]>(url)
      .pipe(
        catchError(this.httpError("getDefaultCompetencies", <ICompetency[]>[]))
      );
  }

  /**
   * Save a new, or update an existing, performance competency entity.
   *
   * @param clientId
   * @param dto
   */
  savePerformanceCompetency(
    clientId: number,
    dto: ICompetency
  ): Observable<ICompetency> {
    let url =
      dto.competencyId == null
        ? this.api + "/clients/" + clientId + "/competencies"
        : this.api +
          "/clients/" +
          clientId +
          "/competencies/" +
          dto.competencyId;
    return this.http
      .post<ICompetency>(url, dto)
      .pipe(catchError(this.httpError("saveCompetencyGroup", <ICompetency>{})));
  }

  duplicateDefaultCompetencies(
    clientId: number,
    competencyIdList: number[]
  ): Observable<ICompetency[]> {
    const url = `${this.api}/clients/${clientId}/competencies/duplicates`;
    return this.http
      .post<ICompetency[]>(url, competencyIdList)
      .pipe(
        catchError(
          this.httpError("duplicateDefaultCompetencies", <ICompetency[]>[])
        )
      );
  }

  deletePerformanceCompetency(
    clientId: number,
    competencyId: number
  ): Observable<boolean> {
    let url = `${this.api}/clients/${clientId}/competencies/${competencyId}`;
    return this.http
      .delete<boolean>(url)
      .pipe(catchError(this.httpError("deletePerformanceCompetency", false)));
  }

  getCompetencyGroups(clientId: number): Observable<ICompetencyGroup[]> {
    let url = this.api + "/clients/" + clientId + "/competency-groups";
    return this.http
      .get<ICompetencyGroup[]>(url)
      .pipe(
        catchError(
          this.httpError("getCompetencyGroups", <ICompetencyGroup[]>[])
        )
      );
  }

  createCompetencyGroup(
    clientId: number,
    dto: ICompetencyGroup
  ): Observable<ICompetencyGroup> {
    let url = this.api + "/clients/" + clientId + "/competency-groups";
    return this.http
      .post<ICompetencyGroup>(url, dto)
      .pipe(
        catchError(
          this.httpError("createCompetencyGroup", <ICompetencyGroup>{})
        )
      );
  }

  getCompetencyModelsForCurrentClient(): Observable<ICompetencyModel[]> {
    return this.account
      .getUserInfo()
      .pipe(
        concatMap((x) =>
          this.http
            .get<ICompetencyModel[]>(
              this.api +
                "/clients/" +
                (x.lastClientId || x.clientId) +
                "/competency-models"
            )
            .pipe(
              catchError(
                this.httpError("getCompetencyModels", <ICompetencyModel[]>{})
              )
            )
        )
      );
  }

  getBasicCompetencyModelInfoForClient(): Observable<ICompetencyModelBasic[]> {
    return this.http
      .get<ICompetencyModelBasic[]>(`${this.api}/competency-models/basic`)
      .pipe(
        catchError(
          this.httpError(
            "getBasicCompetencyModelInfoForClient",
            <ICompetencyModelBasic[]>{}
          )
        )
      );
  }

  createCompetencyModelForCurrentClient(
    model: ICompetencyModel
  ): Observable<ICompetencyModel> {
    return this.account.getUserInfo().pipe(
      concatMap((userInfo) => {
        model.clientId = userInfo.lastClientId || userInfo.clientId;
        return this.http.post<ICompetencyModel>(
          this.api + "/competency-models",
          model
        );
      }),
      catchError((error) => {
        this.msg.setTemporaryMessage(
          "Sorry, this operation failed: 'Save Competency Model'",
          MessageTypes.error,
          6000
        );
        return throwError(error);
      })
    );
  }

  updateCompetencyModelForCurrentClient(
    model: ICompetencyModelUpdate
  ): Observable<ICompetencyModel> {
    return this.account.getUserInfo().pipe(
      concatMap((userInfo) => {
        model.clientId = userInfo.lastClientId || userInfo.clientId;
        return this.http.put<ICompetencyModel>(
          this.api + "/competency-models",
          model
        );
      }),
      catchError((error) => {
        this.msg.setTemporaryMessage(
          "Sorry, this operation failed: 'Save Competency Model'",
          MessageTypes.error,
          6000
        );
        return throwError(error);
      })
    );
  }

  deleteCompetencyModelForCurrentClient(
    modelId: number
  ): Observable<ICompetencyModel> {
    return this.account.getUserInfo().pipe(
      concatMap((userInfo) => {
        return this.http.delete<ICompetencyModel>(
          this.api +
            `/competency-models/client/${
              userInfo.lastClientId || userInfo.clientId
            }/competency-model-id/${modelId}`
        );
      })
    );
  }

  /**
   * Assigns a competency model to the current employee.
   * @param compModelId The id of the CompetencyModel the current employee should be assigned to.
   *
   */
  assignCompetencyModelToEmployee(
    dto: EmployeePerformanceConfiguration
  ): Observable<void> {
    return this.account.getUserInfo().pipe(
      concatMap((userInfo) => {
        dto.clientId = userInfo.lastClientId || userInfo.clientId;
        return this.http
          .put<void>(
            this.api + `/competency-models/one-time-earning-settings`,
            dto
          )
          .pipe(catchError((e) => throwError(e)));
      }),
      catchError((e) => throwError(e)),
      take(1)
    );
  }

  assignCompetencyModelToEmployeeByEmployeeId(
    employeeId: number,
    compModelId?: number
  ): Observable<void> {
    return this.account.getUserInfo().pipe(
      concatMap((userInfo) => {
        var dto = {
          employeeId: employeeId,
          competencyModelId: compModelId,
          clientId: userInfo.lastClientId || userInfo.clientId,
        };
        return this.http
          .put<void>(
            this.api + `/competency-models/assign-competency-model-to-employee`,
            dto
          )
          .pipe(catchError((e) => throwError(e)));
      }),
      catchError((e) => throwError(e)),
      take(1)
    );
  }

  getEmployeePerformanceConfiguration(): Observable<EmployeePerformanceConfiguration> {
    return this.account.getUserInfo().pipe(
      concatMap((userInfo) => {
        return this.http
          .get<EmployeePerformanceConfiguration>(
            this.api +
              `/employee-performance-configuration/client/${
                userInfo.lastClientId || userInfo.clientId
              }/employee/${userInfo.lastEmployeeId}`
          )
          .pipe(
            catchError(
              this.httpError(
                "getCompetencyModels",
                <EmployeePerformanceConfiguration>{}
              )
            )
          );
      })
    );
  }

  getEmployeePerformanceSettings(): Observable<boolean> {
    return this.account.getUserInfo().pipe(
      concatMap((userInfo) => {
        return this.http
          .get<boolean>(
            this.api +
              `/employee-performance-settings/client/${
                userInfo.lastClientId || userInfo.clientId
              }/employee/${userInfo.lastEmployeeId}`
          )
          .pipe(
            catchError(
              this.httpError("getEmployeePerformanceSettings", <boolean>{})
            )
          );
      })
    );
  }

  isScoringEnabledForReviewTemplate(
    reviewTemplateId: number
  ): Observable<{ data: boolean }> {
    return this.http.get<{ data: boolean }>(
      this.api +
        `/reviewTemplate/${reviewTemplateId}/is-scoring-enabled-for-review-template`
    );
  }

  getScoreMethodTypes(): Observable<ScoreMethodType> {
    return this.http.get<ScoreMethodType>(this.api + "/score-method-type");
  }

  getScoreModelForCurrentClient(): Observable<{ data: ScoreModel }> {
    return this.account.getUserInfo().pipe(
      take(1),
      concatMap((userInfo) => {
        return this.http.get<{ data: ScoreModel }>(
          this.api +
            `/score-model/client/${userInfo.lastClientId || userInfo.clientId}`
        );
      })
    );
  }

  getScore(clientId: number, reviewId: number): Observable<{ data: number }> {
    const params = new HttpParams()
      .set("clientId", clientId.toString())
      .set("reviewId", reviewId.toString());
    return this.http.get<{ data: number }>(`${this.api}/overall-score`, {
      params: params,
    });
  }

  /**
   * @param evaluationId The evaluation we want to calculate the score for
   * @param includeCommentsAndIds Include the comments and ids for each goal/competency
   * evaluation in the returned value @see IWeightedScoreItem.commentsAndIds
   */
  calculateEvalScore(
    evaluationId: number,
    saveResult: boolean,
    includeCommentsAndIds?: boolean
  ): Observable<IScoreGroup> {
    return this.http.get<IScoreGroup>(
      `api/performance/evaluations/${evaluationId}/score/include-comments/${
        includeCommentsAndIds == null ? false : includeCommentsAndIds
      }/save-result/${saveResult}`
    ).pipe(map(results => {
      (results.items as IScoreGroup[]).forEach(evalGroup => {
        if (!evalGroup.items)
          evalGroup.items = [];
      });
      return results;
    }));
  }

  getScoringSettings(
    reviewId: number,
    reloadData: boolean = false
  ): Observable<{ data: ScoreModelSettings }> {
    return this.ScoringSettings[reviewId] == null || reloadData
      ? (this.ScoringSettings[reviewId] = this.account.getUserInfo().pipe(
          concatMap((userInfo) =>
            this.http
              .get<{ data: ScoreModelSettings }>(
                this.api +
                  `/clients/${
                    userInfo.lastClientId || userInfo.clientId
                  }/review/${reviewId}/is-scoring-enabled`
              )
              .pipe(
                map((x) => {
                  if (x.data == null) {
                    x.data = <ScoreModelSettings>{};
                  }
                  return x;
                })
              )
          ),
          publishReplay(),
          refCount()
        ))
      : this.ScoringSettings[reviewId];
  }

  getMeritIncreasesForReviews(
    reviewIds: number[]
  ): Observable<{ data: IReviewMeritIncrease[] }> {
    const index = new Maybe(reviewIds)
      .map((x) => x.join(""))
      .valueOr("default");
    if (this.getMeritIncreasesForReviewsCache[index] != null) {
      return this.getMeritIncreasesForReviewsCache[index];
    }
    return (this.getMeritIncreasesForReviewsCache[index] = this.account
      .PassUserInfoToRequest((userInfo) =>
        this.http.post<{ data: IReviewMeritIncrease[] }>(
          this.api +
            `/reviews/merit-increase/${
              userInfo.lastClientId || userInfo.clientId
            }`,
          reviewIds
        )
      )
      .pipe(
        catchError(this.httpError("getMeritIncreases", { data: null })),
        share()
      ));
  }

  private getMeritIncreasesForReviewsCache: {
    [id: string]: Observable<{ data: IReviewMeritIncrease[] }>;
  } = {};

  private httpError<T>(operation = "operation", result?: T) {
    return (error: any): Observable<T> => {
      let errorMsg =
        error.error.errors != null && error.error.errors.length
          ? error.error.errors[0].msg
          : error.message;

      this.account.log(error, `${operation} failed: ${errorMsg}`);

      // TODO: better job of transforming error for user consumption
      this.msg.setTemporaryMessage(
        `Sorry, this operation failed: ${errorMsg}`,
        MessageTypes.error,
        6000
      );

      // let app continue by return empty result
      return of(result as T);
    };
  }

  isPayrollRequestEnabled(reviewId: number): Observable<{ data: boolean }> {
    return this.http.get<{ data: boolean }>(
      this.api + `/review/${reviewId}/is-payroll-request-enabled`
    );
  }
}
