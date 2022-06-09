import { Component, Input, OnInit, ElementRef, ViewChildren, QueryList, SimpleChanges, Output, EventEmitter, OnDestroy, OnChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import * as _ from 'lodash';
import * as moment from 'moment';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { Moment } from 'moment';
import { MatDialog } from '@angular/material/dialog';
import { UserInfo, UserType, CompletionStatusType, IRemark, ITask, API_STRING, ViewTask, ViewRemark, CalculateTaskListProgress } from '@ds/core/shared';
import { GoalService, RemoveGoal } from '../shared/goal.service';
import { IGoal, GoalListView, DetermineGoalCompletionStatus, IRemovedGoal, IGoalRemoveState, GoalRemoveState } from '../shared/goal.model';
import { AddGoalDialogComponent } from '../add-goal-dialog/add-goal-dialog.component';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { IDsConfirmOptions } from '@ajs/ui/confirm/ds-confirm.interface';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { MatSidenav } from '@angular/material/sidenav';
import { AccountService } from '@ds/core/account.service';
import { EMPTY, forkJoin, of, race, Subject, BehaviorSubject } from 'rxjs';
import { tap, debounceTime, take, takeUntil, switchMap, catchError } from 'rxjs/operators';
import { isUndefinedOrNull } from '@util/ds-common';

@Component({
    selector: 'ds-goal-detail',
    templateUrl: './goal-detail.component.html',
    styleUrls: ['./goal-detail.component.scss']
})
export class GoalDetailComponent implements OnInit, OnDestroy, OnChanges {
    private _goal: GoalListView;
    goalForm: FormGroup;

    private _goalRemovalState$: BehaviorSubject<IGoalRemoveState> = new BehaviorSubject<IGoalRemoveState>(null);
    goalRemovalState$ = this._goalRemovalState$.asObservable();

    @Input()
    set goal(goal:GoalListView) {
        this._goal = goal;
        this._goalRemovalState$.next(new GoalRemoveState(this.goal));
    }
    get goal() {
        return this._goal;
    }

    private _assignToRestricted:boolean = false;
    @Input('restrictAssignment')
    set isAssignToRestricted(value: boolean) {
        this._assignToRestricted = coerceBooleanProperty(value);
    }
    get isAssignToRestricted(): boolean {
        return this._assignToRestricted;
    }

    private _essMode:boolean = false;
    @Input('isEss')
    get isEssMode():boolean {
        return this._essMode;
    }
    set isEssMode(value: boolean) {
        this._essMode = coerceBooleanProperty(value);
    }

    private _canAddGoals:boolean = false;
    @Input('canAddGoals')
    get canAddGoals():boolean {
        return this._canAddGoals;
    }
    set canAddGoals(value: boolean) {
        this._canAddGoals = coerceBooleanProperty(value);
    }

    private _hasSupervisorLevelAccessToEmployee:boolean = false;
    @Input('hasSupervisorLevelAccessToEmployee')
    get hasSupervisorLevelAccessToEmployee():boolean {
        return this._hasSupervisorLevelAccessToEmployee;
    }
    set hasSupervisorLevelAccessToEmployee(value: boolean) {
        this._hasSupervisorLevelAccessToEmployee = coerceBooleanProperty(value);
    }

    @Output() goalDetailEditDialogFinish = new EventEmitter<IGoal>();
    @Output() goalChange = new EventEmitter<IGoal>();
    @Output() goalRemoved = new EventEmitter<IRemovedGoal>();
    @Output() goalAdded = new EventEmitter<IGoal>();
    @Input() user: UserInfo;
    @Input() sideNavRef: MatSidenav;
    @Input() isEmployeeGoal: boolean;
    childrenTasks: ViewTask[];
    formRemarks: ViewRemark[] = [];
    disableAddTaskButton = false;
    disableAddRemarkButton = false;
    showAddRemarkField = false;
    @ViewChildren('remark') remarkElements: QueryList<ElementRef>;
    @ViewChildren('task') taskElements: QueryList<ElementRef>;
    showOverdueBadge = false;
    editMode = false;
    employeeId: number;

    private _taskListProgress: number;
    get taskListProgress(): number {
        return this._taskListProgress;
    }
    /**
     * Setting @see taskListProgress also sets @see goal.progress
     */
    set taskListProgress(value: number) {
        this._taskListProgress = value;
        this.goal.progress = value;
    }

    private intentToTriggerSaveEmployeeGoal$ = new Subject<IGoal>(); // Used to yield a dummy response for the race.
    private triggerSaveEmployeeGoal$ = new Subject<IGoal>(); // Triggers service.saveEmployeeGoal() when pushed onto.

    private destroy$ = new Subject<void>();

    constructor(
        private fb:FormBuilder,
        private service:GoalService,
        private msg:DsMsgService,
        private dialog:MatDialog,
        private confirmService:DsConfirmService,
        private acctSvc: AccountService
    ) {
        this.service.goalDetailEditMode.pipe(takeUntil(this.destroy$)).subscribe(next => this.editMode = next);
        this.service.tasks.pipe(takeUntil(this.destroy$)).subscribe(next => this.childrenTasks = next);
        this.service.remarks.pipe(takeUntil(this.destroy$)).subscribe(next => this.formRemarks = next);
        this.service.isSidenavOpen.pipe(takeUntil(this.destroy$)).subscribe(close => {
            if (close)
                this.closeDrawer();
        });

        // this.intentToTriggerSaveEmployeeGoal$ "feeds/preempts" this.triggerSaveEmployeeGoal$ observable.
        this.triggerSaveEmployeeGoal$.pipe(
            // We want to debounce these, so that we don't fire off a bunch of saveEmployeeGoal() requests
            // before the user is done making changes to the goal.
            debounceTime(500), // Wait .5sec after the last goal edit, before trying to save the goal to the backend.

            // Discard all but the most recent saveEmployeeGoal request
            // This will make it so that updateGoalFormUI only happens for the most recent goal state.
            // And the response from a previous saveEmployeeGoal request,
            // will not "overwrite" the UI while a newer saveEmployeeGoal request is pending.
            switchMap(dto => {
                this.msg.sending(true);
                const responseDto$ = this.acctSvc.PassUserInfoToRequest(userInfo => {
                    return this.service.saveEmployeeGoal(this.goal, userInfo.selectedClientId(), userInfo.selectedEmployeeId());
                }).pipe(catchError(err => EMPTY));

                // Race for a response VS further user input that we should discard this current response for.
                // In other words:  if the user fires off an event that wins the race,
                //                  discard this saveEmployeeGoal() response.
                // This works because the "race winning" event will, in turn,
                // fire off a newer saveEmployeeGoal(), and it will be off the the (same) races again!
                return race(responseDto$, this.intentToTriggerSaveEmployeeGoal$);
            }),
            takeUntil(this.destroy$),
        ).subscribe(goal => {
            if (goal) {
                this.msg.sending(false);
                this.updateGoalFormUI(goal as GoalListView);
            }
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
    }

    ngOnInit(): void {
        this.employeeId = this.isEssMode ? this.user.employeeId || this.user.lastEmployeeId : this.user.selectedEmployeeId();
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.goal) {
            this.goal = changes.goal.currentValue;

            if(this.goal.tasks != null)
                this.service.tasks.next(this.goal.tasks as ViewTask[]);

            if(this.goal.remarks != null) {
                const sorted = this.sortRemarks(this.goal.remarks).reverse();
                this.service.remarks.next(sorted as ViewRemark[]);
            }

            this.taskListProgress = CalculateTaskListProgress(this.childrenTasks);
            this.goalForm = this.createGoalForm(this.goal, this.childrenTasks, this.formRemarks);
        }
    }

    updateCompletionStatus(event: any, index: number): void {
        // Push onto subject to indicate that a new user input has occurred.
        // Used in the race in the triggerSaveEmployeeGoal$ pipe.
        this.intentToTriggerSaveEmployeeGoal$.next(null);

        const value = event.target.checked;
        this.childrenTasks[index].completionStatus = value ? CompletionStatusType.Done : CompletionStatusType.NotStarted;
        const dto = this.childrenTasks[index];

        this.service.saveGoalTask(dto, this.user.selectedClientId()).pipe(
            tap(task => {
                this.childrenTasks[index] = task;
                this.goal.tasks = this.childrenTasks;
                this.taskListProgress = CalculateTaskListProgress(this.childrenTasks);
                this.goal.completionStatus =  DetermineGoalCompletionStatus(this.goal.startDate,
                                                                            this.goal.dueDate,
                                                                            this.goal.completionDate,
                                                                            this.goal.tasks,
                                                                            this.goal.isArchived);

                if (task.completionStatus === CompletionStatusType.Done || task.completionStatus === CompletionStatusType.NotStarted) {
                    const userInfo = this.getUserInfoForRemark(this.user);
                    const completionStatus = (task.completionStatus === CompletionStatusType.Done) ? 'completed' : 'incomplete';

                    this.goal.remarks.push({
                        remarkId: null,
                        description: `${userInfo.firstName} ${userInfo.lastName} marked ${task.description} ${completionStatus}.`,
                        addedDate: moment(),
                        addedBy: userInfo.userId,
                        isSystemGenerated: true,
                        user: userInfo
                    });
                }

                this.goalForm = this.createGoalForm(this.goal, this.childrenTasks, this.formRemarks);

                this.goalChange.emit(this.goal);
                this.service.tasks.next(this.goal.tasks);
            }),
            take(1),
        ).subscribe(responseDto => this.triggerSaveEmployeeGoal$.next({} as GoalListView));
    }

    enableTaskEdit(t:ViewTask) {
        t.editItem=!t.editItem;
        this.service.goalDetailEditMode.next(true);
    }

    enableRemarkEdit(r:ViewRemark) {
        r.editItem=!r.editItem;
        this.service.goalDetailEditMode.next(true);
    }

    get tasks() {
        return this.goalForm.get('tasks') as FormArray;
    }

    getTaskControl(index: number, name: string): FormControl {
        return this.goalForm.get('tasks').get(index + '').get(name) as FormControl;
    }

    getTaskFromGroup(index: number): FormGroup {
        return this.goalForm.get('tasks').get(index + '') as FormGroup;
    }

    getRemarkControl(index: number): FormControl {
        return this.goalForm.get('remarks').get(index + '') as FormControl;
    }

    getRemarkFormArray(): FormArray {
        return this.goalForm.get('remarks') as FormArray;
    }

    updateTaskDescription(task: ViewTask, index: number, keyEvent: boolean = false): void {
        const control = this.getTaskControl(index, 'description');

        // bail out on the method if the control is invalid
        if (control.hasError('required')) return;

        this.disableAddTaskButton = !this.disableAddTaskButton;

        task.description = control.value;

        this.msg.sending(true);
        this.service.saveGoalTask(task, this.user.selectedClientId())
            .subscribe(result => {
                this.service.goalDetailEditMode.next(false);
                this.msg.setTemporarySuccessMessage('Successfully saved your task.', 5000);
                task = result;
                task.editItem = false;
                this.childrenTasks[index] = task;

                if (keyEvent) {
                    this.addTask();
                } else {
                    this.patchGoalForm(this.goalForm, this.goal, this.childrenTasks, this.formRemarks);
                }
            });
    }

    deleteTask(task:ViewTask, index:number):void {
        if(task.taskId == null || task.taskId < 1) return;

        this.msg.sending(true);
        this.service.deleteGoalTask(this.user.selectedClientId(), task.taskId)
            .subscribe(() => {
                this.msg.setTemporarySuccessMessage('Successfully deleted your task.', 5000);
                this.childrenTasks.splice(index, 1);
                this.taskListProgress = CalculateTaskListProgress(this.childrenTasks);
                this.goalForm = this.createGoalForm(this.goal, this.childrenTasks, this.formRemarks);
                this.goalChange.emit(this.goal);
            });
    }

    deleteRemark(remark: ViewRemark, index: number): void {
        this.msg.sending(true);
        this.service.deleteGoalRemark(this.user.selectedClientId(), this.goal.goalId, remark.remarkId)
            .subscribe(() => {
                this.msg.sending(false);
                _.remove(this.goal.remarks, (r: ViewRemark) => {
                    return r.remarkId === remark.remarkId;
                });
                _.remove(this.formRemarks, (r: ViewRemark) => {
                    return r.remarkId === remark.remarkId;
                });
                (<FormArray>this.goalForm.controls.remarks).controls.splice(index, 1);

                this.editMode = false;
            });
    }

    clearTask(task: ViewTask, index: number, keyEvent: boolean = false): void {
        if (keyEvent && task.taskId > 0) return;
        this.service.goalDetailEditMode.next(false);
        if (task.taskId || task.description) {
            task.editItem = !task.editItem;
            return;
        }

        this.childrenTasks.splice(index, 1);
        this.getTaskFromGroup(index).patchValue({ description: null });
        this.taskListProgress = CalculateTaskListProgress(this.childrenTasks);
        this.createTaskFormArray(this.childrenTasks);
    }

    clearRemark(index: number): void {
        const remark = this.formRemarks[index];
        this.service.goalDetailEditMode.next(false);
        if (remark.remarkId || remark.description) {
            remark.editItem = !remark.editItem;
            return;
        }

        this.formRemarks.splice(index, 1);
        this.getRemarkControl(index).patchValue(null);
        this.createRemarkFormArray(this.formRemarks);
    }

    updateExistingRemark(newValue:string, remarkId:number, index:number) {
        this.service.goalDetailEditMode.next(false);

        if(remarkId != null) this.formRemarks[index].editItem = false;

        this.formRemarks[index].description = newValue;
        const dto:ViewRemark = this.formRemarks[index];

        dto.addedDate = dto.addedDate != null ? moment(dto.addedDate).format(API_STRING) : moment().format(API_STRING);

        this.msg.sending(true);
        this.service.saveGoalRemark(dto, this.user.selectedClientId(), this.goal.goalId)
            .subscribe(result => {
                this.msg.sending(false);

                this.formRemarks[index] = result;
                this.formRemarks = this.sortRemarksInPlace(this.formRemarks).reverse();

                this.service.remarks.next(this.formRemarks);
                this.goal.remarks = this.formRemarks;
                this.goalForm.get('remarks').get(index.toString()).patchValue(result.description);
            });
    }

    // updateRemarkDescription(remark:ViewRemark, index:number):void {
    //     this.service.goalDetailEditMode.next(false);
    //     const control = this.getRemarkControl(index);
    //     let dto:ViewRemark;

    //     if(remark.remarkId != null && remark.remarkId > 0) {
    //         this.formRemarks[index].editItem = false;
    //         this.formRemarks[index].description = control.value;
    //         dto = this.formRemarks[index];
    //     } else {
    //         this.formRemarks[index].description = control.value;
    //         dto = this.formRemarks[index];
    //     }

    //     dto.addedDate = dto.addedDate != null ? moment(dto.addedDate).format(API_STRING) : moment().format(API_STRING);

    //     this.msg.sending(true);
    //     this.service.saveGoalRemark(dto, this.user.selectedClientId(), this.goal.goalId)
    //         .subscribe(result => {
    //             this.msg.sending(false);

    //             if(remark.remarkId == null || remark.remarkId < 1)
    //                 this.formRemarks[index] = result;
    //             this.formRemarks = this.sortRemarksInPlace(this.formRemarks).reverse();
    //             this.cancelRemarkForm();
    //             this.goalForm = this.createGoalForm(this.goal, this.childrenTasks, this.formRemarks);

    //             this.goal.remarks = this.formRemarks;
    //             this.goalChange.emit(this.goal);
    //         });
    // }

    prepareToEditRemark(remark: ViewRemark, index: number): void {
        this.service.goalDetailEditMode.next(true);
        remark.editItem = !remark.editItem;
        this.resetRemarksArray();
        this.formRemarks = [remark];
        (<FormArray>this.goalForm.controls.remarks)
            .push(this.fb.control(remark.description, [Validators.required]));
    }

    cancelRemarkForm(): void {
        this.service.goalDetailEditMode.next(false);
        this.removeInvalidFormRemarks();
        this.resetRemarksArray();
        this.goalForm = this.createGoalForm(this.goal, this.childrenTasks, this.formRemarks);
    }

    removeInvalidFormRemarks(): void {
        this.formRemarks.forEach((r, index) => {
            if (r.remarkId == null || r.remarkId < 1)
                this.formRemarks.splice(index, 1);
        });
    }

    resetRemarksArray(): void {
        const remarks = this.goalForm.get('remarks') as FormArray;
        let counter = remarks.length - 1;
        while (counter >= 0) {
            (<FormArray>this.goalForm.get('remarks')).removeAt(counter);
            counter--;
        }
    }

    addTask(): void {
        this.taskElements.changes.subscribe((queryList: QueryList<ElementRef>) => {
            if (queryList == null || queryList.first == null) return;
            queryList.first.nativeElement.focus();
        });
        this.service.goalDetailEditMode.next(true);

        this.childrenTasks.push({
            taskId: null,
            parentId: this.goal.taskId,
            assignedTo: null,
            description: null,
            progress: 0,
            editItem: true,
            dueDate: null,
            completedBy: null,
            completionStatus: CompletionStatusType.NotStarted,
            completionDate: null
        });

        this.taskListProgress = CalculateTaskListProgress(this.childrenTasks);
        this.goalForm = this.createGoalForm(this.goal, this.childrenTasks, this.formRemarks);
    }

    addRemark(): void {
        this.service.goalDetailEditMode.next(true);

        /** attempt to focus on the editable field */
        this.remarkElements.changes.subscribe((queryList: QueryList<ElementRef>) => {
            if (queryList == null || queryList.first == null) return;
            queryList.first.nativeElement.focus();
        });

        const userInfo = this.getUserInfoForRemark(this.user);

        this.formRemarks.push({
            remarkId: null,
            description: null,
            addedDate: moment(),
            addedBy: userInfo.userId,
            isSystemGenerated: false,
            user: userInfo,
            editItem: true
        });
        this.formRemarks = this.sortRemarksInPlace(this.formRemarks).reverse();
        this.goalForm = this.createGoalForm(this.goal, this.childrenTasks, this.formRemarks);
    }

    updateCompletionDate(event: MatDatepickerInputEvent<Moment>): void {
        // this.intentToTriggerSaveEmployeeGoal$.next(null);

        const dateAsMom = moment(event.value);
        this.goal.completionDate = dateAsMom.isValid() ? dateAsMom.format('YYYY-MM-DD') : null;
        this.goal.completionStatus = DetermineGoalCompletionStatus(this.goal.startDate,
            this.goal.dueDate, this.goal.completionDate, this.goal.tasks, this.goal.isArchived);

        // this.triggerSaveEmployeeGoal$.next({} as GoalListView);
        this.msg.sending(true);
        this.service.updateGoalCompletionDate(this.goal, this.user.selectedClientId())
          .subscribe(result => {
            this.msg.sending(false);
            this.updateGoalFormUI(result as GoalListView);
          });
    }

    closeDrawer(): void {
        this.sideNavRef.close();
    }

    showEditGoalDialog(): void {
        // Ugh. We should just remove the ability to bring up the edit dialog from the goalDetail drawer all together...
        this.closeDrawer(); // This is a pretty kludgy fix, ngl...
        // Doing ^this to prevent the user from editing the goal in the drawer
        // before we have a chance to updateGoalFormUI with the api response data from saving the EditGoalDialog.

        const dialogRef = this.dialog.open(AddGoalDialogComponent, {
            width: '500px',
            data: {
                user: this.user,
                goal: this.goal,
                requireAssignedTo: true,
                isAssignToRestricted: this.isAssignToRestricted,
                isCompanyGoal: this.goal.client != null
            }
        });

        dialogRef
        .afterClosed()
        .pipe(
            switchMap((result: IGoal|null) => {
                if (isUndefinedOrNull(result)) return EMPTY;
                this.msg.sending(true);
                return forkJoin(
                    this.acctSvc.PassUserInfoToRequest(userInfo => {
                        return this.service.saveEmployeeGoal(result, userInfo.selectedClientId(), userInfo.selectedEmployeeId())
                    }),
                    of(this.goal.isCompanyGoal),
                    of(this.goal.subGoals),
                );
            }),
            switchMap(([goal, oldIsCompanyGoal, oldSubGoals]) => {
                // this.msg.setTemporarySuccessMessage('Successfully saved your goal.', 5000);
                goal.isCompanyGoal = oldIsCompanyGoal;
                goal.employee = this.goal.employee;
                (goal as GoalListView).subGoals = oldSubGoals;
                return of(goal);
            }),
            // switchMap(goal => {
            //     return forkJoin(
            //         of(goal),
            //         this.service.getRemarksByGoal(this.user.selectedClientId(), goal.goalId),
            //     );
            // }),
            take(1)
        )
        .subscribe((goal) => {
            this.msg.setTemporarySuccessMessage('Successfully saved your goal.', 5000);
            // this.updateGoalFormUI(goal as GoalListView, remarks);
            this.updateGoalFormUI(goal as GoalListView);
            // Reopen the sidenav and GoalDetail drawer once we've updated the form with api response data.
            this.goalDetailEditDialogFinish.emit(this.goal);
            this.sideNavRef.open(); // This is a pretty kludgy fix, ngl...
        });
    }

    addSubGoal(): void {
        this.dialog.open(AddGoalDialogComponent, {
            width: '500px',
            data: {
                user: this.user,
                requireAssignedTo: true,
                isAssignToRestricted: false,
                isSubGoal: true
            }
        })
        .afterClosed()
        .subscribe((result:IGoal) => {
            if(result == null) return;
            this.msg.sending(true);
            result.parentId = this.goal.goalId;
            if(this.goal.subGoals == null)
                this.goal.subGoals = [];

            this.service.saveClientSubGoal(result, this.user.selectedClientId(), result.goalOwner.employeeId)
                .subscribe(x=>{
                    this.goal.subGoals.push(x);
                    if(result.assignedTo == this.goal.assignedTo)   this.goalAdded.next(x);
                });
        });
    }

    editSubGoal(sub: IGoal): void {
        this.dialog.open(AddGoalDialogComponent, {
            width: '500px',
            data: {
                user: this.user,
                requireAssignedTo: true,
                hideFieldAssign: false,
                isSubGoal: true,
                goal: sub
            }
        })
            .afterClosed()
            .subscribe((result: IGoal) => {
                if (result == null) return;
                this.msg.sending(true);
                result.parentId = this.goal.goalId;
                for (let i = 0; i < this.goal.subGoals.length; i++) {
                    if (this.goal.subGoals[i].goalId === result.goalId) {
                        this.goal.subGoals[i] = result;
                    }
                }
                this.service.saveClientSubGoal(result, this.user.selectedClientId(), result.goalOwner.employeeId).subscribe();
            });
    }

    private getUserInfoForRemark(user: UserInfo = this.user): UserInfo {
        return <UserInfo>{
            userId: user.userId,
            firstName: (user.userTypeId === UserType.companyAdmin) ? user.userFirstName : user.firstName,
            lastName: (user.userTypeId === UserType.companyAdmin) ? user.userLastName : user.lastName
        }
    }

    private updateGoalFormUI(goal:GoalListView, remarks:IRemark[] = null):void {
        let remarksTemp = Array.isArray(remarks) ? remarks : goal.remarks;
        if(!remarksTemp) remarksTemp = [];
        goal.remarks = this.sortRemarksInPlace(remarksTemp).reverse();

        this.goal = Object.assign(goal, {
            isOpen: this.goal.isOpen,
            goalOwner: this.goal.goalOwner
        });

        /** pushes updated goal remarks/tasks to service that other components rely on */
        this.service.remarks.next(this.goal.remarks);
        this.service.tasks.next(this.goal.tasks);

        /** emits goal change event that external components can listen for */
        this.goalChange.emit(this.goal);

        /** Update goal-detail component UI form */
        this.childrenTasks = this.goal.tasks;
        this.goalForm.controls.tasks = this.createTaskFormArray(this.childrenTasks);
        this.goalForm.controls.remarks = this.createRemarkFormArray(this.formRemarks);
    }

    private createGoalForm(goal: GoalListView, childrenTasks: ViewTask[], formRemarks: ViewRemark[]): FormGroup {
        const goalForm = this.fb.group({
            completionDate: this.fb.control(goal.completionDate || '', []),
            tasks: this.createTaskFormArray(childrenTasks),
            remarks: this.createRemarkFormArray(formRemarks)
        });
        return goalForm;
    }

    private patchGoalForm(goalForm: FormGroup, goal: GoalListView, childrenTasks: ViewTask[], formRemarks: ViewRemark[]): void {
        goalForm.patchValue({
            completionDate: goal.completionDate
        });

        this.patchTaskFormArray(goalForm, childrenTasks);
        this.patchRemarkFormArray(goalForm, formRemarks);
    }

    private createTaskFormArray(tasks: ITask[]): FormArray {
        const result: FormArray = this.fb.array([]);
        tasks.forEach(t => {
            result.push(this.fb.group({
                completionStatus: this.fb
                .control({ value: t.completionStatus === CompletionStatusType.Done, disabled: this.goal.isArchived }),
                description: this.fb.control(t.description || '', [Validators.required])
            }));
        });
        return result;
    }

    private patchTaskFormArray(goalForm: FormGroup, childrenTasks: ViewTask[]): void {
        childrenTasks.forEach((ct, i, a) => {
            (<FormArray>goalForm.controls.tasks).at(i).patchValue({
                completionStatus: ct.completionStatus,
                description: ct.description
            });
        });
    }

    private createRemarkFormArray(formRemarks: ViewRemark[]): FormArray {
        const result = this.fb.array([]);
        formRemarks.forEach(r => {
            result.push(this.fb.control(r.description || '', [Validators.required]));
        });
        return result;
    }

    private patchRemarkFormArray(goalForm: FormGroup, formRemarks: ViewRemark[]): void {
        formRemarks.forEach((r, i, a) => {
           (<FormArray>goalForm.controls.remarks).at(i).patchValue(r.description);
        });
    }

    /**
     * Sorts the remarks array in-place by remarkId.
     * @param remarks array of IRemark[], which will be sorted in-place by remarkId.
     * @returns sorted IRemark[]
     */
    private sortRemarksInPlace(remarks: IRemark[]): IRemark[] {
        // Original lodash sort method. This was a "stable-sort",
        // where objects that compared as equivalent, stayed in the same order as they were originally.
        // this.formRemarks = _.sortBy(this.formRemarks, ['addedDate']).reverse();

        // Originally this was sorted by IRemark.addedDate prop.
        // But there isn't enough resolution in the sub-second range for this prop, to sort remarks when they're created rapidly.
        // Sorting by the ID should be equivalent, and not subject to this same limitation.
        return remarks.sort((a, b) => a.remarkId - b.remarkId);
    }

    /**
     * Wraps sortRemarksInPlace, such that the input array isn't modified in-place.
     * @param remarks   array of IRemark[], which will not be sorted in-place.
     *                  Note that items in the returned array will still hold object-references to the
     *                  items from the original array (IE: this is not a deep-clone).
     * @returns sorted IRemark[]
     */
    private sortRemarks(remarks: IRemark[]): IRemark[] {
        // Array.sort() sorts in-place.
        // So it was a tossup between Array.slice() VS ...spread operator.
        return this.sortRemarksInPlace([...remarks]);
    }

    removeGoal(goal: IGoal, goalRemovalState: IGoalRemoveState): void {
        RemoveGoal(this.msg, this.confirmService, this.service, goalRemovalState,
            goal, this.user.selectedClientId(), this.employeeId)
            .subscribe(removed => {
                let i = 0;
                this.childrenTasks.forEach(x => {
                    this.getTaskFromGroup(i).disable();
                    i++;
                });

                this.getRemarkFormArray().controls.forEach(x => {
                    x.disable();
                });
                this.goalRemoved.next(<IRemovedGoal>{ goal: goal, isHardDelete: goalRemovalState.isHardDelete });
            });
    }

    enableArchiveOrDelete(goal: IGoal): boolean {
        // this.isEssMode
        return this.canAddGoals || goal.assignedTo === this.user.userId || this.hasSupervisorLevelAccessToEmployee;
    }

}
