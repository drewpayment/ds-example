import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/internal/Observable";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import {
  ITask,
  IRemark,
  ViewTask,
  ViewRemark,
  CompletionStatusType,
} from "@ds/core/shared";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import {
  catchError,
  map,
  tap,
  concatMap,
} from "rxjs/operators";
import {
  IGoal,
  ICompanyGoal,
  GoalListView,
  DetermineGoalCompletionStatus,
  IGoalRemoveState,
} from "./goal.model";
import { AccountService } from "@ds/core/account.service";
import {
  IContact,
  ContactsProfileImageLoader,
  IContactSearchOptions,
  ContactSearchOptions,
  ContactProfileImageLoader,
} from "@ds/core/contacts";
import { Subject, BehaviorSubject, from, throwError, of } from "rxjs";
import * as moment from "moment";
import { DsConfirmService } from "@ajs/ui/confirm/ds-confirm.service";

@Injectable({
  providedIn: "root",
})
export class GoalService {
  private readonly api = `api/performance/goals`;
  private _companyGoals: GoalListView[] = [];
  companyGoals$: Subject<IGoal[]> = new Subject<IGoal[]>();
  hasGoals$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  /** communicates with goal-detail component to lock/unlock UI elements during editing */
  private _tasks: ViewTask[];
  private _remarks: ViewRemark[];
  goalDetailEditMode: Subject<boolean> = new Subject<boolean>();
  tasks: Subject<ViewTask[]> = new Subject<ViewTask[]>();
  remarks: Subject<ViewRemark[]> = new Subject<ViewRemark[]>();
  private _closeSidenav: Subject<boolean> = new Subject<boolean>();
  isSidenavOpen: Observable<boolean>;
  private _goalOwners = new Subject<IContact[]>();
  get goalOwners$(): Observable<IContact[]> {
    return this._goalOwners.asObservable();
  }

  constructor(
    private http: HttpClient,
    private account: AccountService,
    private msg: DsMsgService
  ) {
    this.tasks.subscribe((next) => (this._tasks = next));
    this.remarks.subscribe((next) => (this._remarks = next));
    this.isSidenavOpen = this._closeSidenav.asObservable();
  }

  /**
   * Exposes the ability to close the MatSidenav from external components.
   */
  closeSidenav(): void {
    this._tasks = [];
    this._remarks = [];
    this.tasks.next(this._tasks);
    this.remarks.next(this._remarks);
    this._closeSidenav.next(true);
  }

  /**
   * This is a helper utility that sets all UI elements on the goal detail component back
   * to their original state, hiding text fields, etc. We use this when the sidenav is closing, because
   * during the sidenav closing, the goal-detail component isn't actually destroyed and the state
   * is held. However, when the user closes the sidenav we want to simulate the action that they've
   * destroyed the component and that they will be resetting things and starting with a new one.
   */
  disableGoalDetailComponentEditItems(): void {
    if (this._tasks != null) {
      this._tasks.forEach((t, i, a) => {
        if (t.taskId == null) a.splice(i, 1);
        else a[i].editItem = false;
      });
      this.tasks.next(this._tasks);
    }

    if (this._remarks != null) {
      this._remarks.forEach((r, i, a) => {
        if (r.remarkId == null) a.splice(i, 1);
        else a[i].editItem = false;
      });
      this.remarks.next(this._remarks);
    }

    this.goalDetailEditMode.next(false);
  }

  getGoalsByEmployee(
    clientId: number,
    employeeId: number
  ): Observable<IGoal[]> {
    const url = `${this.api}/clients/${clientId}/employees/${employeeId}`;
    return this.http.get<IGoal[]>(url).pipe(
      map((goals) => {
        goals.forEach((g, i, a) => {
          g.completionStatus = DetermineGoalCompletionStatus(
            g.startDate,
            g.dueDate,
            g.completionDate,
            g.tasks,
            g.isArchived
          );
          a[i] = g as GoalListView;
          ContactProfileImageLoader(g.goalOwner);
        });

        return goals;
      }),
      catchError(this.httpError("getGoalsByEmployee", <IGoal[]>[]))
    );
  }

  saveEmployeeGoal(
    dto: IGoal,
    clientId: number,
    employeeId: number
  ): Observable<IGoal> {
    const url =
      dto.goalId > 0
        ? `api/performance/clients/${clientId}/employees/${employeeId}/goals/${dto.goalId}`
        : `api/performance/clients/${clientId}/employees/${employeeId}/goals`;
    return this.http.post<IGoal>(url, dto).pipe(
      catchError(this.httpError("saveEmployeeGoal", <IGoal>{})),
      map((g) => {
        ContactProfileImageLoader(g.goalOwner);
        return g;
      })
    );
  }
  saveClientSubGoal(
    goal: IGoal,
    clientId: number,
    employeeId: number
  ): Observable<GoalListView> {
    const newGoal: boolean = goal != null ? goal.goalId < 1 : true;
    const url = !newGoal
      ? `api/performance/clients/${clientId}/employees/${employeeId}/goals/${goal.goalId}`
      : `api/performance/clients/${clientId}/employees/${employeeId}/goals`;

    return this.http.post<GoalListView>(url, goal).pipe(
      catchError(this.httpError("saveClientSubGoal")),
      tap((subgoal: GoalListView) => {
        this.msg.sending(false);

        this._companyGoals.forEach((cg, index) => {
          if (cg.goalId == subgoal.parentId) {
            if(cg.subGoals)
            {
              const existingIndex = this._companyGoals[index].subGoals.findIndex(x => x.goalId === subgoal.goalId);
              if (existingIndex > -1) {
                this._companyGoals[index].subGoals[existingIndex] = subgoal;
              } else {
                this._companyGoals[index].subGoals = [...this._companyGoals[index].subGoals, subgoal];
              }
            } else{
              this._companyGoals[index].subGoals = [];
              this._companyGoals[index].subGoals.push(subgoal);
            }

          }
        });

        // if (newGoal) {
        //   this._companyGoals.forEach((cg, index) => {
        //     /** Find the correct company goal and make sure it has an array of subgoals then push our result to it. */
        //     if (cg.goalId === subgoal.parentId) {
        //       if (this._companyGoals[index].subGoals == null)
        //         this._companyGoals[index].subGoals = [];
        //       this._companyGoals[index].subGoals.push(subgoal);
        //       this.createNewSubGoalsArray(this._companyGoals[index]);
        //     }
        //   });
        // } else {
        //   /**
        //    * Loop through each company goal to find the one that the subgoal is child of.
        //    */
        //   this._companyGoals.forEach((cg, i) => {
        //     if (cg.goalId === subgoal.parentId) {
        //       cg.subGoals.forEach((sg, ii) => {
        //         /** Find the appropriate subgoal that was updated */
        //         if (sg.goalId === subgoal.goalId) {
        //           this._companyGoals[i].subGoals[ii] = subgoal;
        //           this.createNewSubGoalsArray(this._companyGoals[i]);
        //         }
        //       });
        //     }
        //   });
        // }

        // this.setCompanyGoalsView();
        this.companyGoals$.next(this._companyGoals);
        this.msg.setTemporarySuccessMessage("Successfully saved your goal.");
      })
    );
  }

  /**
   * Assigns a new array to the subgoals property of the provided goal.  This is done so that the material
   * table on the company goals list component reflects any changes we make.
   * @param goal The goal containing the array of subgoals to update
   */
  private createNewSubGoalsArray(goal: GoalListView): void {
    const result: IGoal[] = [];
    goal.subGoals.forEach((x) => result.push(x));
    goal.subGoals = result;
  }

  saveClientGoal(dto: IGoal, clientId: number): void {
    const newGoal: boolean = dto != null ? dto.goalId < 1 : true;
    const url =
      dto.goalId > 0
        ? `api/performance/clients/${clientId}/goals/${dto.goalId}`
        : `api/performance/clients/${clientId}/goals`;
    this.http
      .post<IGoal>(url, dto)
      .pipe(catchError(this.httpError("saveClientGoal")))
      .subscribe((goal: GoalListView) => {
        if (goal) {
          this.msg.sending(false);
          if (newGoal) {
            this._companyGoals.push(goal);
          } else {
            this._companyGoals.forEach((cg: GoalListView, i: number) => {
              if (cg.goalId === goal.goalId) {
                this._companyGoals[i] = goal;
              }
            });
          }

          this.setCompanyGoalsView();
          this.companyGoals$.next(this._companyGoals);
          this.msg.setTemporarySuccessMessage(
            "Successfully saved your company goal."
          );
        }
      });
  }

  /**
   * only used to get company goals
   */
  getGoalsByClient(clientId: number): void {
    const url = `${this.api}/clients/${clientId}`;
    this.http
      .get<ICompanyGoal[]>(url)
      .pipe(catchError(this.httpError("getGoalsByClient", <ICompanyGoal[]>[])))
      .subscribe((goals: GoalListView[]) => {
        this.setCompanyGoalsView(goals);
        this.calculateCompanyGoalsCompletionStatus(goals);
        this.companyGoals$.next(this._companyGoals);
      });
  }

  setHasGoals(hasSome: boolean) {
    this.hasGoals$.next(hasSome);
  }

  /**
   * Loops through either the private company goals or takes a new list of goals and figures out if kanban view
   * should be enabled on each goal. Then sets the result to our private company goals list.
   *
   * @param goals A list of goals in GoalListView type which is a IGoal extended with property to tell UI
   * whether it should show the kanban view or not.
   */
  private setCompanyGoalsView(goals: GoalListView[] = null): void {
    goals = goals || this._companyGoals;
    goals.forEach((g: GoalListView, i: number) => {
      let hasSubGoals = g.subGoals != null && g.subGoals.length > 0;
      goals[i].showKanBanView =
        goals[i].showKanBanView == null ? true : goals[i].showKanBanView;

      //profile images
      ContactProfileImageLoader(g.goalOwner);
      if (hasSubGoals)
        g.subGoals.forEach((sub) => ContactProfileImageLoader(sub.goalOwner));
    });
    this._companyGoals = goals;
  }

  private calculateCompanyGoalsCompletionStatus(
    companyGoals: GoalListView[]
  ): void {
    companyGoals = companyGoals || this._companyGoals;
    companyGoals.forEach((cg, ci, ca) => {
      if (cg.subGoals == null) return;

      cg.subGoals.forEach((g, i, a) => {
        /**
         * If completion status is already set to done or the goal has a completion date
         * upon returning from API, this means that the status is in a determinate state which
         * would not be altered by time elapsing and we should skip this goal.
         */
        if (
          g.completionStatus === CompletionStatusType.Done ||
          g.completionDate != null
        )
          return;

        const today = moment();
        const startDate = moment(g.startDate);
        const dueDate = g.dueDate != null ? moment(g.dueDate) : null;

        /** check if today is before/after our goal's start date and set appropriate completion status */
        if (today.isBefore(startDate, "day")) {
          a[i].completionStatus = CompletionStatusType.NotStarted;
        } else {
          a[i].completionStatus = CompletionStatusType.InProgress;
        }

        /** check if today is after the due date and if so, set the goal overdue */
        if (dueDate != null && today.isAfter(dueDate, "day")) {
          a[i].completionStatus = CompletionStatusType.Overdue;
        }

        ca[ci].subGoals[i] = a[i];
      });
    });
  }

  // getGoalCompletionStatus(g:GoalListView):GoalListView {
  //     g.completionStatus =  DetermineGoalCompletionStatus(g.startDate, g.dueDate, g.completionDate, g.tasks);
  //     return g;
  //     // if(g.completionDate != null) {
  //     //     g.completionStatus = CompletionStatusType.Done;
  //     //     return g;
  //     // }

  //     // const today = moment();
  //     // const startDate = moment(g.startDate);
  //     // const dueDate = g.dueDate != null ? moment(g.dueDate) : null;

  //     // if(today.isBefore(startDate, 'day')) {
  //     //     g.completionStatus = CompletionStatusType.NotStarted;
  //     //     return g;
  //     // }

  //     // if(dueDate != null && today.isAfter(dueDate, 'day')) {
  //     //     g.completionStatus = CompletionStatusType.Overdue;
  //     //     return g;
  //     // }

  //     // g.completionStatus = CompletionStatusType.InProgress;
  //     // return g;
  // }

  saveGoalTask(dto: ITask, clientId: number): Observable<ITask> {
    const url =
      dto.taskId == null
        ? `api/clients/${clientId}/tasks`
        : `api/clients/${clientId}/tasks/${dto.taskId}`;
    return this.http
      .post<ITask>(url, dto)
      .pipe(catchError(this.httpError("saveGoalTask", <ITask>{})));
  }

  deleteGoalTask(clientId: number, taskId: number): Observable<boolean> {
    const url = `api/clients/${clientId}/tasks/${taskId}`;
    return this.http
      .delete<boolean>(url)
      .pipe(catchError(this.httpError("deleteGoalTask", false)));
  }

  saveGoalRemark(
    dto: IRemark,
    clientId: number,
    goalId: number
  ): Observable<IRemark> {
    const url =
      dto.remarkId == null
        ? `api/clients/${clientId}/goals/${goalId}/remarks`
        : `api/clients/${clientId}/goals/${goalId}/remarks/${dto.remarkId}`;
    return this.http
      .post<IRemark>(url, dto)
      .pipe(catchError(this.httpError("saveGoalRemark", <IRemark>{})));
  }

  getRemarksByGoal(clientId: number, goalId: number): Observable<IRemark[]> {
    const url = `api/clients/${clientId}/goals/${goalId}/remarks`;
    return this.http
      .get<IRemark[]>(url)
      .pipe(catchError(this.httpError("getRemarksByGoal", <IRemark[]>[])));
  }

  updateGoalCompletionDate(dto: IGoal, clientId: number): Observable<IGoal> {
    const url = `api/performance/clients/${clientId}/goals/${dto.goalId}/completion-date`;
    return this.http
      .post<IGoal>(url, dto)
      .pipe(catchError(this.httpError("updateGoalCompletionDate", <IGoal>{})));
  }

  deleteGoalRemark(
    clientId: number,
    goalId: number,
    remarkId: number
  ): Observable<boolean> {
    const url = `api/performance/clients/${clientId}/goals/${goalId}/remarks/${remarkId}`;
    return this.http
      .delete<boolean>(url)
      .pipe(catchError(this.httpError("deleteGoalRemark", false)));
  }

  getGoalOwnerList(clientId: number, options: IContactSearchOptions = null) {
    const url = `api/performance/clients/${clientId}/goals/owners`;
    let params = new ContactSearchOptions(options).toHttpParams();
    return this.http
      .get<IContact[]>(url, { params: params })
      .pipe(
        map(ContactsProfileImageLoader),
        catchError(this.httpError("getGoalOwnerList", [])),
        tap(res => this._goalOwners.next(res)),
      );
  }

  deleteEmployeeGoal(
    goal: IGoal,
    clientId: number,
    employeeId: number,
    isHardDelete: boolean
  ): Observable<void> {
    const url = `api/performance/clients/${clientId}/employees/${employeeId}/task/${goal.taskId}/delete/${isHardDelete}`;
    return this.http.delete<void>(url).pipe(
      tap((x) => {
        this.getGoalsByClient(clientId);
      }, catchError(this.httpError("deleteEmployeeGoal", [])))
    );
  }

  deleteCompanyGoal(parentId: number, idsToKeep: number[], clientId: number) {
    var dto = {
      parentId: parentId,
      keepIds: idsToKeep,
    };
    const url = `${this.api}/clients/${clientId}/deleteGoals`;
    let options = {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      }),
      body: JSON.stringify(dto),
    };
    this.http
      .delete(url, options)
      .pipe(catchError(this.httpError("deleteCompanyGoal", false)))
      .subscribe((value) => {
        if (value !== false) {
          this.removeGoalFromCompanyGoal(parentId);
        }
      });
  }

  removeGoalFromCompanyGoal(parentId: number) {
    this._companyGoals = this._companyGoals.filter(
      (val) => val.goalId != parentId
    );
    this.companyGoals$.next(this._companyGoals);
  }

  archiveCompanyGoal(goalId: number) {
    const url = `${this.api}/archive/${goalId}`;
    return this.http.post(url, goalId).pipe(catchError(this.httpError("")));
  }
  unArchiveCompanyGoal(goalId: number) {
    const url = `${this.api}/unarchive/${goalId}`;
    return this.http.post(url, goalId).pipe(catchError(this.httpError("")));
  }

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
}

export function RemoveGoal(
  msg: DsMsgService,
  confirmService: DsConfirmService,
  goalService: GoalService,
  goalRemovalState: IGoalRemoveState,
  goal: IGoal,
  clientId: number,
  employeeId: number
): Observable<any> {
  const dontHandle = 5; // arbitrary value used to indicate that we don't want to handle this error
  return from(
    <PromiseLike<any>>confirmService.show(null, goalRemovalState.options)
  ).pipe(
    catchError((err) => throwError(dontHandle)), // the error caught at this point will be from the user cancelling the delete
    concatMap((yes) => {
      msg.sending(true);
      return goalService.deleteEmployeeGoal(
        goal,
        clientId,
        employeeId,
        goalRemovalState.isHardDelete
      );
    }),
    tap((undefinedVal) => msg.sending(false)),
    catchError((x) => {
      if (dontHandle != x) {
        msg.setTemporaryMessage(
          `Sorry, this operation failed: ${goalRemovalState.btnText}`,
          MessageTypes.error,
          6000
        );
        return throwError(x);
      }
    })
  );
}
