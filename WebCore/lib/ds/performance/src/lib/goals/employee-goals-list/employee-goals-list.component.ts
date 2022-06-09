import { Component, OnInit, ViewChild, SimpleChanges, ElementRef, Input } from "@angular/core";
import { GoalService } from "../shared/goal.service";
import { Observable, of, BehaviorSubject, fromEvent, Subject, iif } from "rxjs";
import { UserInfo, CompletionStatusType, UserType } from "@ds/core/shared";

import * as _ from 'lodash';
import * as moment from 'moment';
import { MatSidenav } from "@angular/material/sidenav";
import { MatDialog } from "@angular/material/dialog";
import { ActivatedRoute } from "@angular/router";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { IGoal, GoalListView, DetermineGoalCompletionStatus, IRemovedGoal } from "../shared/goal.model";
import { AccountService } from "@ds/core/account.service";
import { AddGoalDialogComponent } from "../add-goal-dialog/add-goal-dialog.component";
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { coerceBooleanProperty } from "@angular/cdk/coercion";
import { GoalPriority } from "../shared/goal.priority";
import { PERFORMANCE_ACTIONS } from "@ds/performance/shared/performance-actions";
import { EmployeeApiService } from "@ds/core/employees";
import { map, switchMap, tap } from "rxjs/operators";
import { changeDrawerHeightOnOpen, matDrawerAfterHeightChange } from "@ds/core/ui/animations/drawer-auto-height-animation";

@Component({
  selector: 'ds-employee-goals-list',
  templateUrl: './employee-goals-list.component.html',
  styleUrls: ['./employee-goals-list.component.scss'],
  animations: [
    changeDrawerHeightOnOpen,
    matDrawerAfterHeightChange
  ]
})
export class EmployeeGoalsListComponent implements OnInit {
    goalCompletePercent:number = 0;
    drawerIsOpen:boolean;
    user:UserInfo;

    goalPriorityRef = GoalPriority;

    /** Internal goals list for reference. */
    private _goals:GoalListView[];
    private _goals$: Subject<GoalListView[]> = new Subject<GoalListView[]>();
    displayGoals:GoalListView[];
    viewArchiveTxt = ['View Archive', 'Active Goals'];
    goalsCount:number = 0;
    selectedGoal:GoalListView;
    @ViewChild('goalDetail', { static: false }) goalDetail:MatSidenav;
    isEmployeeGoals:boolean;
    isLoading = true;
    private _assignToRestricted:boolean = false;
    @Input('restrictAssignment')
    get isAssignToRestricted():boolean {
        return this._assignToRestricted;
    }
    set isAssignToRestricted(value:boolean) {
        this._assignToRestricted = coerceBooleanProperty(value);
    }
    private _essMode:boolean = false;
    @Input('isEss')
    get isEssMode():boolean {
        return this._essMode;
    }
    set isEssMode(value:boolean) {
        this._essMode = coerceBooleanProperty(value);
    }

    canAddGoals: boolean = false;

    /** the currently scoped employee id for the component */
    employeeId:number;
    /** Represents whether the current user is a supervisor for the currently scoped employeeId. */
    isSupervisorForEmployee: boolean = false;
    /**
     * Represents whether the current user has _at least_ supervisor level for the currently scoped employeeId.
     * In this context, UserType.systemAdmin and UserType.companyAdmin are considered to to have supervisor level access.
     */
    hasSupervisorLevelAccessToEmployee: boolean = false;
    private _showActiveGoals:boolean = true;
    get showActiveGoals():boolean {
        return this._showActiveGoals;
    }
    set showActiveGoals(value:boolean) {
        this._showActiveGoals = coerceBooleanProperty(value);
    }

    private _hasGoals:boolean;
    get hasGoals():boolean {
        return this._hasGoals;
    }
    set hasGoals(value:boolean) {
        this._hasGoals = value == null ?
            this.isLoading || (this._goals != null && this._goals.length > 0)
            : coerceBooleanProperty(value);
    }

    editingSideNavItems:boolean = false;

    // Make this enum accessible within the template
    CompletionStatusType = CompletionStatusType;

    constructor(
        private msg:DsMsgService,
        private service:GoalService,
        private account:AccountService,
        private dialog:MatDialog,
        private route:ActivatedRoute,
        private confirmService: DsConfirmService,
        private employeeApiSvc: EmployeeApiService
    ) {
        this.service.goalDetailEditMode.subscribe(next => this.editingSideNavItems = next);
        this._goals$.subscribe(goals => {
            this.hasGoals = goals.length > 0;
            if (this.selectedGoal) {
                const selectedGoal = goals.find(goal => goal.goalId === this.selectedGoal.goalId);
                if (selectedGoal) {
                  this.showGoalDetail(selectedGoal);
                }
            }

            this.displayGoals = goals;
            this.displayGoals = this.sortGoals(this.displayGoals);
            this.setCompletionStatusAndGoalPercentage();

        });
    }

    ngOnInit() {
        if(this.route.data){
            if(this.route.data["value"].isAssignToRestricted){
                this.isAssignToRestricted = this.route.data["value"].isAssignToRestricted;
            }
            if(this.route.data["value"].isEssMode){
                this.isEssMode = this.route.data["value"].isEssMode;
            }
        }
        this.account.getUserInfo().pipe(
          tap(user => {
            this.user = user;
            this.employeeId = this.isEssMode ? this.user.employeeId || this.user.lastEmployeeId : this.user.selectedEmployeeId();
            //this.employeeId = this.isAssignToRestricted ? this.user.employeeId || this.user.lastEmployeeId : this.user.selectedEmployeeId();
          }),
          // If current user is an SA or CA, we don't need to check for permissions to access the employee.
          switchMap(user => iif(
            () => user.userTypeId === UserType.systemAdmin || user.userTypeId === UserType.companyAdmin,
            of(true),
            this.employeeApiSvc.getSupervisorsForEmployee(this.employeeId).pipe(
              map(supervisorsForEmployee => Array.isArray(supervisorsForEmployee) && supervisorsForEmployee.some(contact => contact.userId === this.user.userId)),
              tap(isSupervisorForEmployee => this.isSupervisorForEmployee = isSupervisorForEmployee)
            )
          )),
          tap(hasSupervisorLevelAccessToEmployee => this.hasSupervisorLevelAccessToEmployee = hasSupervisorLevelAccessToEmployee)
        ).subscribe(_ => {
            this.initEmployeeGoals();
        });

        this.account.canPerformActions(PERFORMANCE_ACTIONS.GoalTracking.WriteGoals).subscribe((x: boolean) => {
            this.canAddGoals = x === true;
        });
    }
    trackByFn(index) {
        return index;
    }
    private initEmployeeGoals():void {
        this.service.getGoalsByEmployee(this.user.selectedClientId(), this.employeeId)
            .subscribe((goals:GoalListView[]) => {
                //console.log(JSON.stringify(goals));
                if(goals == null) return;
                this.afterInitPageRender(goals);
            });
    }

    private afterInitPageRender(goals:GoalListView[]):void {
        this._goals = goals;
        this._goals$.next(this.getActive());
        this.setCompletionStatusAndGoalPercentage();
        this.isLoading = false;
    }

    /**
     * Sorts provided list of goals by due date and then stores them in @var _goals.
     */
    private sortGoals(goals: GoalListView[]): GoalListView[] {

        return _.sortBy(goals, (g: GoalListView) => {
            let completionStatus = 2;

            if (g.completionStatus === CompletionStatusType.Overdue) {
                completionStatus = 0;
            }

            if (g.completionStatus === CompletionStatusType.InProgress) {
                completionStatus = 1;
            }

            let priority = 3;

            if (g.priority === GoalPriority.High) {
                priority = 0;
            }

            if (g.priority === GoalPriority.Medium) {
                priority = 1;
            }

            if (g.priority === GoalPriority.Low) {
                priority = 2;
            }

            return [completionStatus, priority, g.startDate != null ? moment(g.startDate).unix() : null];
        });
    }

    private setCompletionStatusAndGoalPercentage():void {
        let completedList:number = 0;
        let totalGoals = this.getActive().length;
        this.goalsCount = totalGoals;

        if(this.goalsCount == 0) {
            this.goalCompletePercent = 0;
            return;
        }

        this._goals.forEach((g, i, a) => {
            if(g.isArchived) return;
            g.completionStatus = DetermineGoalCompletionStatus(g.startDate, g.dueDate, g.completionDate, g.tasks, g.isArchived);
            a[i] = g;

            if(a[i].completionStatus == CompletionStatusType.Done)
                completedList++;
        });

        this.goalCompletePercent = Math.round(completedList / totalGoals * 100) ;
    }

    showGoalDetail(goal:GoalListView):void {
        this._goals.forEach((g:GoalListView) => {
            if (goal && g.goalId === goal.goalId) {
                g.isOpen = true;
            } else {
                g.isOpen = false;
            }
        });

        this.selectedGoal = goal;
        this.drawerIsOpen = true;
    }

    showAddGoalDialog():void {
        if(this.drawerIsOpen) {
            this.drawerIsOpen = false;
            this.selectedGoal = null;
            this.service.closeSidenav();
        }

        this.dialog.open(AddGoalDialogComponent, {
            width: '500px',
            disableClose: true,
            data: {
                user: this.user,
                requireAssignedTo: true,
                isAssignToRestricted: this.isAssignToRestricted,
                hideFieldAssign: true
            }
        })
        .afterClosed()
        .subscribe((result:IGoal) => {
            if(result == null) return;

            this.msg.sending(true);
            this.service.saveEmployeeGoal(result, this.user.selectedClientId(), this.employeeId)
                .subscribe((goal:GoalListView) => {
                    this.msg.setTemporarySuccessMessage('Successfully saved your goal.', 5000);
                    this.updateInternalGoals(goal, result.goalId < 1);
                    this.setCompletionStatusAndGoalPercentage();

                    this._goals$.next(this.getActive());
                });
        });
    }

    drawerClosing():void {
        this.drawerIsOpen = false;
        this._goals.forEach((g:GoalListView) => {
            g.isOpen = false;
        });

        this.service.disableGoalDetailComponentEditItems();
    }

    drawerOpening():void {
        // console.log('OPENING!');
    }

    /**
     * Kludge to force the sidenav selection to match what is shown in the drawer itself.
     * (Used after closing goalDetailDetail Edit Dialog via the dialog's save.)
     */
    goalDetailEditDialogFinish(goal: GoalListView) {
        if (goal) {
            this.updateGoalsOnDetailChange(goal);
            const selectedGoalToBe = this._goals.find(g => g.goalId === goal.goalId);
            this.showGoalDetail(selectedGoalToBe);
        }
    }

    updateGoalsOnDetailChange(goal:GoalListView):void {
        this.updateInternalGoals(goal);
	    this.setCompletionStatusAndGoalPercentage();
        this._goals$.next(this.getActive());
    }

    removeGoal(removedGoalDto: IRemovedGoal): void {
        for(let i = 0; i < this._goals.length; i++) {
            if(this._goals[i].goalId == removedGoalDto.goal.goalId){
                if(removedGoalDto.isHardDelete){
                    this._goals.splice(i, 1);
                } else {
                    this._goals[i] = removedGoalDto.goal as GoalListView;
                    this._goals[i].isArchived = true;
                }

            }
        }

        this._goals$.next(this.getActive());
        this.drawerIsOpen = false;
    }

    addNewGoal(goal:GoalListView): void {
        this._goals.push(goal);
        this._goals$.next(this.getActive());
    }

    /**
     * Updates our list of datastore goals with our newly updated goal. Does not update
     * our stream of goals in our observable, because we want to do additional work in
     * updateGoalsOnDetail() to set the proper completion status.
     *
     * @param pendingGoal
     * @param isNew
     */
    updateInternalGoals(pendingGoal:GoalListView, isNew:boolean = false):void {
        if(!isNew) {
            for(let i = 0; i < this._goals.length; i++) {
                const d = this._goals[i];
                if(d.goalId != pendingGoal.goalId) continue;
                this._goals[i] = pendingGoal;
            }
        } else {
            this._goals.push(pendingGoal);
        }
    }

    private getArchived(): GoalListView[] {
        return this._goals.filter(val => val.isArchived);
    }

    private getActive(): GoalListView[] {
        return this.sortGoals(this._goals.filter(val => !val.isArchived));
    }

    swapDisplay(): void {
        this._showActiveGoals = !this._showActiveGoals;
        if(!this._showActiveGoals) {
            this._goals$.next(this.getArchived());
            this.viewArchiveTxt = ['View Active', 'Archived Goals'];
        } else {
            this._goals$.next(this.getActive());
            this.viewArchiveTxt = ['View Archive', 'Active Goals'];
        }
        this.drawerIsOpen = false;
    }

    mapCompletionStatusToColorClass(completionStatus: CompletionStatusType) {
        switch (completionStatus) {
            case CompletionStatusType.NotStarted:
                return 'success';
            case CompletionStatusType.InProgress:
                return 'warning';
            case CompletionStatusType.Overdue:
                return 'danger';
            case CompletionStatusType.Done:
            default:
                return 'info';
        }
    }
}
