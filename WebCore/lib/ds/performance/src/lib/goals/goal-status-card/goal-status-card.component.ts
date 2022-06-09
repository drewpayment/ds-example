import { Component, OnInit, Input } from "@angular/core";
import {
  IGoal,
  GoalListView,
  DetermineGoalCompletionStatus,
  IGoalRemoveState,
  GoalRemoveState,
} from "@ds/performance/goals/shared/goal.model";
import { IEmployeeData } from "@ajs/employee/add-employee/shared/models";
import {
  CompletionStatusType,
  UserInfo,
  CalculateTaskListProgress,
} from "@ds/core/shared";
import * as moment from "moment";
import { MatDialog } from "@angular/material/dialog";
import { AddGoalDialogComponent } from "../add-goal-dialog/add-goal-dialog.component";
import { IContact } from "@ds/core/contacts";
import { GoalService, RemoveGoal } from "../shared/goal.service";
import { AccountService } from "@ds/core/account.service";
import { coerceBooleanProperty } from "@angular/cdk/coercion";
import * as _ from "lodash";
import { BehaviorSubject, EMPTY, Subject } from "rxjs";
import { DsConfirmService } from "@ajs/ui/confirm/ds-confirm.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { switchMap, take, takeUntil, tap } from "rxjs/operators";

type CardType = "todo" | "inProgress" | "overdue" | "complete";

@Component({
  selector: "ds-goal-status-card",
  templateUrl: "./goal-status-card.component.html",
  styleUrls: ["./goal-status-card.component.scss"],
})
export class GoalStatusCardComponent implements OnInit {
  @Input() user: UserInfo;
  @Input() employee: IEmployeeData = {} as IEmployeeData;
  @Input() goal: GoalListView = {} as GoalListView;
  @Input() goalOwners: IContact[];
  progress: number;
  contextColor: string;
  mode: CardType = "todo";

  private _isEditable: boolean = false;
  @Input()
  get isEditable(): boolean {
    return this._isEditable;
  }
  set isEditable(value: boolean) {
    this._isEditable = coerceBooleanProperty(value);
  }

  private _goalRemovalState$ = new BehaviorSubject<IGoalRemoveState>(null);
  goalRemovalState$ = this._goalRemovalState$.asObservable();

  destroy$ = new Subject();

  constructor(
    private dialog: MatDialog,
    private goalService: GoalService,
    private accountService: AccountService,
    private confirmService: DsConfirmService,
    private msg: DsMsgService
  ) {}

  ngOnInit() {
    this.goalService.goalOwners$.pipe(
      takeUntil(this.destroy$),
      tap(goalOwners => this.goalOwners = goalOwners),
    )
      .subscribe(() => {
        const owner = this.goalOwners.find(x => x.employeeId == this.goal.employee.employeeId);
        if (owner) {
          this.goal.goalOwner = owner;
        }
      });

    this.setContextColor();

    if (this.user == null)
      this.accountService.getUserInfo().subscribe((user) => (this.user = user));

    this.calculateProgress();
    this._goalRemovalState$.next(new GoalRemoveState(this.goal));
  }

  getGoalStatus(goal: GoalListView): string {
    if (goal.completionStatus == CompletionStatusType.NotStarted) {
      return "Not Started";
    } else if (goal.completionStatus == CompletionStatusType.InProgress) {
      return "In Progress";
    } else if (goal.completionStatus == CompletionStatusType.Done) {
      return "Completed";
    } else if (goal.completionStatus == CompletionStatusType.Overdue) {
      return "Overdue";
    } else if (
      goal.completionStatus == null &&
      moment().isBefore(moment(goal.startDate))
    ) {
      return "Not Started";
    } else {
      return null;
    }
  }

  getCompletionAccuracyMessage(goal: GoalListView): string {
    let completed = moment(goal.completionDate);
    let due = moment(goal.dueDate);

    let duration: number;
    let message: string;
    let daysLabel: string;

    if (completed.isBefore(due, "day")) {
      duration = due.diff(completed, "days");
      daysLabel = duration > 1 ? "Days" : "Day";
      message = `Completed ${duration} ${daysLabel} Ahead of Schedule`;
    } else if (completed.isAfter(due, "day")) {
      duration = completed.diff(due, "days");
      daysLabel = duration > 1 ? "Days" : "Day";
      message = `Completed ${duration} ${daysLabel} Behind Schedule`;
    } else {
      message = `Completed on Time`;
    }

    return message;
  }

  editGoal(): void {
    this.dialog
      .open(AddGoalDialogComponent, {
        width: "500px",
        data: {
          user: this.user,
          goal: this.goal,
          requireAssignedTo: true,
          isSubGoal: true,
          goalOwnerList: this.goalOwners,
        },
      })
      .afterClosed()
      .pipe(
        switchMap((result: IGoal) => {
          if (result) {
            this.msg.sending(true);
            const user = this.findUserInGoalOwnersByGoalAssignee(result);
            const employeeId = user ? user.employeeId : 0;
            return this.goalService.saveClientSubGoal(
              result,
              this.user.selectedClientId(),
              employeeId
            );
          } else {
            return EMPTY;
          }
        }),
        take(1)
      )
      .subscribe();
  }

  calculateProgress(): void {
    /** if there are no tasks, we simply set progress to zero and short circuit the method */
    this.goal.completionStatus = DetermineGoalCompletionStatus(
      this.goal.startDate,
      this.goal.dueDate,
      this.goal.completionDate,
      this.goal.tasks,
      this.goal.isArchived
    );
    this.progress = CalculateTaskListProgress(this.goal.tasks);
    this._goalRemovalState$.next(new GoalRemoveState(this.goal));
  }

  private setContextColor(): void {
    if (this.goal.completionStatus == CompletionStatusType.NotStarted) {
      this.contextColor = "success";
      this.mode = "todo";
    } else if (this.goal.completionStatus == CompletionStatusType.InProgress) {
      this.contextColor = "warning";
      this.mode = "inProgress";
    } else if (this.goal.completionStatus == CompletionStatusType.Done) {
      this.contextColor = "info";
      this.mode = "complete";
    } else if (this.goal.completionStatus == CompletionStatusType.Overdue) {
      this.contextColor = "danger";
      this.mode = "overdue";
    } else if (
      this.goal.completionStatus == null &&
      moment().isBefore(moment(this.goal.startDate))
    ) {
      this.contextColor = "success";
      this.mode = "todo";
    }
  }

  removeGoal(goal: IGoal, goalRemovalState: IGoalRemoveState): void {
    const employeeId = this.findUserInGoalOwnersByGoalAssignee(goal).employeeId;
    // RemoveGoal dynamically selects archive or delete API endpoint based on IGoalRemoveState.isHardDelete value.
    RemoveGoal(
      this.msg,
      this.confirmService,
      this.goalService,
      goalRemovalState,
      goal,
      this.user.selectedClientId(),
      employeeId
    ).subscribe();
  }

  private findUserInGoalOwnersByGoalAssignee(goal: IGoal): IContact | null {
    return goal
      ? this.goalOwners.find((x) => x.userId === goal.assignedTo)
      : null;
  }
}
