import { Component, OnInit, Input } from '@angular/core';
import { CompletionStatusType, UserInfo } from '@ds/core/shared';
import * as moment from 'moment';
import { IEmployeeData } from '@ajs/employee/add-employee/shared/models';
import { GoalService } from '@ds/performance/goals/shared/goal.service';
import { AccountService } from '@ds/core/account.service';
import { MatDialog } from '@angular/material/dialog';
import { AddGoalDialogComponent } from '@ds/performance/goals/add-goal-dialog/add-goal-dialog.component';
import { Subject, Observable, EMPTY } from 'rxjs';
import { DeleteGoalDialogComponent } from '../delete-goal-dialog/delete-goal-dialog.component';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import * as _ from 'lodash';
import { IContact } from '@ds/core/contacts';
import { GoalListView, IGoal } from '..';
import { UserType } from '@ds/core/shared';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';
import { switchMap, take, tap } from 'rxjs/operators';

@Component({
    selector: 'ds-company-goals-list',
    templateUrl: './company-goals-list.component.html',
    styleUrls: ['./company-goals-list.component.scss']
})
export class CompanyGoalsListComponent implements OnInit {

    user: UserInfo;
    displayColumns: string[] = ['goalName', 'description', 'status', 'start', 'due', 'complete', 'goalOwner', 'meta'];
    employees: IEmployeeData[];
    goals$: Subject<GoalListView[]> = new Subject<GoalListView[]>();
    goals: Observable<GoalListView[]>;
    private _goals: GoalListView[];
    goalOwners: IContact[];
    private _isEssMode = false;
    viewArchive = false;
    canAddGoals: boolean = false;
    canAddEmployeeGoals: boolean = false;

    // Make this enum accessible within the template
    // See: https://marco.dev/enums-angular
    CompletionStatusType = CompletionStatusType;

    @Input()
    get isEssMode(): boolean {
        return this._isEssMode;
    }
    set isEssMode(value: boolean) {
        this._isEssMode = coerceBooleanProperty(value);
    }

    constructor(
        private accountService: AccountService,
        private service: GoalService,
        private dialog: MatDialog,
        private msg: DsMsgService
    ) {
        this.goals = this.goals$.asObservable();
        this.service.companyGoals$
            .subscribe((goals: GoalListView[]) => {

                for (let i = 0; i < goals.length; i++) {
                    goals[i] = this.loadGoalOwner(goals[i]);

                    /** Load our goal owners for our subgoals if there are subgoals */
                    if (goals[i].subGoals == null || !goals[i].subGoals.length) continue;
                    goals[i].subGoals=goals[i].subGoals.filter(x => x.isArchived == false);
                    for (let j = 0; j < goals[i].subGoals.length; j++) {
                        goals[i].subGoals[j] = this.loadGoalOwner(goals[i].subGoals[j]);
                    }
                }

                this._goals = goals;
                this.filterArchived();
            });
    }

    ngOnInit() {
        const contactSearchOptions = {
            excludeTimeClockOnly: true,
            haveActiveEmployee: true,
            ifSupervisorGetSubords: true
        };

        this.accountService.getUserInfo().pipe(
            tap(user => this.user = user),
            tap(user => this.service.getGoalsByClient(user.selectedClientId())),
            switchMap(user => this.service.getGoalOwnerList(user.selectedClientId(), contactSearchOptions)),
            take(1)
        ).subscribe(goalOwnerList => {
            this.goalOwners = goalOwnerList;
        });

        this.accountService.canPerformActions(PERFORMANCE_ACTIONS.GoalTracking.WriteGoals).subscribe((x: boolean) => {
            this.canAddGoals = x === true;
        });

        //this.accountService.canPerformActions("EmployeeGoal.Write").subscribe((x: boolean) => {
          //  this.canAddEmployeeGoals = x === true;
        //});
    }

    isGoalCompleted(goal: IGoal): boolean {
        // const status = DetermineGoalCompletionStatus(goal.startDate, goal.dueDate, goal.completionDate, goal.tasks, goal.isArchived);
        const status = goal.completionStatus;
        return status === CompletionStatusType.Done;
    }

    canEditGoal(goal: IGoal): boolean {
        return this.canAddGoals || goal.assignedTo === this.user.userId;
    }

    private loadGoalOwner(goal: GoalListView | IGoal): GoalListView {
        if (!goal)
            return;
        (<GoalListView>goal).goalOwner = _.find(this.goalOwners, { 'userId': goal.assignedTo });
        return goal as GoalListView;
    }

    getGoalStatus(goal: IGoal): string {
        if (goal.completionStatus == CompletionStatusType.NotStarted) {
            return 'Not Started';
        } else if (goal.completionStatus == CompletionStatusType.InProgress) {
            return 'In Progress';
        } else if (goal.completionStatus == CompletionStatusType.Done) {
            return 'Completed';
        } else if (goal.completionStatus == CompletionStatusType.Overdue) {
            return 'Overdue';
        } else if (goal.completionStatus == null && moment().isSameOrBefore(moment(goal.startDate), 'days')) {
            return 'Not Started';
        } else if (goal.completionStatus == null && moment().isAfter(moment(goal.startDate), 'days')) {
            return 'In Progress';
        } else {
            return null;
        }
    }

    getCompletionAccuracyMessage(goal: GoalListView): string {
        const completed = moment(goal.completionDate);
        const due = moment(goal.dueDate);

        let duration: number;
        let message: string;
        let daysLabel: string;

        if (completed.isBefore(due, 'day')) {
            duration = due.diff(completed, 'days');
            daysLabel = duration > 1 ? 'Days' : 'Day';
            message = `Completed ${duration} ${daysLabel} Ahead of Schuedule`;
        } else if (completed.isAfter(due, 'day')) {
            duration = completed.diff(due, 'days');
            daysLabel = duration > 1 ? 'Days' : 'Day';
            message = `Completed ${duration} ${daysLabel} Behind Schedule`;
        } else {
            message = `Completed on Time`;
        }

        return message;
    }

    switchView(destinationView: string, goal: GoalListView): void {
        if (destinationView == 'list') {
            goal.showKanBanView = false;
        } else {
            goal.showKanBanView = true;
        }

        this.filterArchived();
    }

    editCompanyGoal(goal: IGoal): void {
        this.dialog.open(AddGoalDialogComponent, {
            width: '500px',
            data: {
                user: this.user,
                goal: goal,
                isCompanyGoal: true,
                goalOwnerList: this.goalOwners
            }
        })
        .afterClosed()
        .subscribe((result: GoalListView) => {
            if (result == null) return;
            this.msg.sending(true);

            /** let's get the subgoals and send them along if it has any */
            if (result.goalId > 0) {
                const goal: GoalListView = _.find(this._goals, {goalId: result.goalId});
                if (goal.subGoals != null)
                    result.subGoals = goal.subGoals;
            }

            this.service.saveClientGoal(result, this.user.selectedClientId());
        });
    }

    editSubGoal(goal: IGoal): void {
      this.dialog.open(AddGoalDialogComponent, {
          width: '500px',
          data: {
              user: this.user,
              goal: goal,
              requireAssignedTo: true,
              isSubGoal: true,
              goalOwnerList: this.goalOwners
          }
      })
      .afterClosed()
      .pipe(
          switchMap((result: IGoal) => {
              this.msg.sending(true);
              if (result) {
                  const user = this.findUserInGoalOwnersByGoalAssignee(result);
                  const employeeId = (user) ? user.employeeId : 0;
                  return this.service.saveClientSubGoal(result, this.user.selectedClientId(), employeeId);
              } else {
                  return EMPTY;
              }
          }),
          take(1),
      ).subscribe(result => {
          // Nothing...
          // goalService.saveClientSubGoal() mutates things behind the scenes.
      });
  }


    deleteGoal(goal: GoalListView) {
        this.dialog.open(DeleteGoalDialogComponent, {
            width: '500px',
            data: {
                user: this.user,
                goal: goal
            }
        }).afterClosed()
        .subscribe((result) => {
            if (result == null) return;
            this.service.deleteCompanyGoal(result['parentId'], result['idsToKeep'], this.user.selectedClientId());
        });
    }

    addSubGoal(goal: IGoal): void {
        this.dialog.open(AddGoalDialogComponent, {
            width: '500px',
            disableClose: true,
            data: {
                user: this.user,
                goalOwnerList: this.goalOwners,
                requireAssignedTo: true,
                isSubGoal: true
            }
        })
        .afterClosed()
        .pipe(switchMap((result: IGoal) => {
          if (result == null) return EMPTY;
          this.msg.sending(true);
          result.parentId = goal.goalId;
          const employeeId = this.goalOwners.find(x => x.userId === result.assignedTo).employeeId;
          return this.service.saveClientSubGoal(result, this.user.selectedClientId(), employeeId);
        }))
        .subscribe();
    }

    changeArchiveStatus(goal: IGoal): void {
        goal.isArchived = !goal.isArchived;
        if (!goal.isArchived) {
            this.service.unArchiveCompanyGoal(goal.goalId).subscribe(() =>
                this.setArchiveMessageAndDisplayedGoals(),
                err => this.msg.showWebApiException(err)
            );
        } else {
            this.service.archiveCompanyGoal(goal.goalId).subscribe(() =>
                this.setArchiveMessageAndDisplayedGoals(),
                err => this.msg.showWebApiException(err)
            );
        }
    }
    setArchiveMessageAndDisplayedGoals() {
        if (this.viewArchive) {
            this.msg.setTemporarySuccessMessage('Goal unarchived successfully.');
        } else {
            this.msg.setTemporarySuccessMessage('Goal archived successfully.');
        }
        this.filterArchived();
    }

    /**
     * Calculates the current progress of the overall goal based on the completion status of the sub goals.
     *
     * @param goal GoalListView
     */
    private calculateProgress(goal: GoalListView): number {

        /** if the goal is undefined or there are no subgoals, we simply return 0 */
        if (goal == null || goal.subGoals == null) return 0;

        /** company goal admins have the ability to set a goal completed regardless of the completion status of subgoals */
        if (goal.completionStatus === CompletionStatusType.Done)
            return 100;

        let totalSubGoals = 0;
        let totalCompleted = 0;

        totalSubGoals += goal.subGoals.length;
        goal.subGoals.forEach((sg: IGoal, ii: number) => {
            if (sg.completionStatus === CompletionStatusType.Done)
                totalCompleted++;
        });

        return Math.round((totalCompleted / totalSubGoals) * 100);
    }

    swapDisplay() {
        this.viewArchive = !this.viewArchive;
        this.filterArchived();
    }
    filterArchived() {
        this.goals$.next(this._goals.filter(val => val.isArchived == this.viewArchive));
        this.service.setHasGoals(this._goals.filter(val => val.isArchived == this.viewArchive).length > 0);
    }

    private findUserInGoalOwnersByGoalAssignee(goal: IGoal): IContact | null {
      return (goal) ? this.goalOwners.find(x => x.userId === goal.assignedTo) : null;
    }
}
