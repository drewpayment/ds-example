import {
  Component,
  OnInit,
  Inject,
  ViewChildren,
  ElementRef,
  QueryList,
  AfterViewInit,
  ViewChild,
} from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormArray,
  FormControl,
  NgModel,
} from "@angular/forms";

import * as _ from "lodash";
import * as moment from "moment";
import {
  UserInfo,
  ITask,
  CompletionStatusType,
  API_STRING,
  UserType,
} from "@ds/core/shared";
import {
  IGoal,
  ICompanyGoal,
  DetermineGoalCompletionStatus,
} from "../shared/goal.model";
import { IContact } from "@ds/core/contacts";
import { AccountService } from "@ds/core/account.service";
import { GoalService } from "../shared/goal.service";
import { Observable, Subject } from "rxjs";
import { startWith, concatMap, debounceTime, skip } from "rxjs/operators";
import { EmployeeService } from "apps/ds-source/src/app/employee/shared/employee.service";
import { IOneTimeEarningSettings } from "@ds/performance/goals/";
import { Maybe } from "@ds/core/shared/Maybe";

interface DialogData {
  user: UserInfo;
  goal: IGoal;
  isCompanyGoal: boolean;
  isSubGoal: boolean;
  goalOwnerList: IContact[];
  requireAssignedTo: boolean;
  isAssignToRestricted?: boolean;
}

interface ViewTask extends ITask {
  editItem?: boolean;
}

@Component({
  selector: "ds-add-goal-dialog",
  templateUrl: "./add-goal-dialog.component.html",
  styleUrls: ["./add-goal-dialog.component.scss"],
})
export class AddGoalDialogComponent implements OnInit, AfterViewInit {
  user: UserInfo;
  goal: IGoal;
  goalOwners: IContact[];
  selectedGoalOwner: IContact;
  f: FormGroup;
  formSubmitted: boolean;
  viewTasks: ViewTask[];
  disableAddButton: boolean = false;
  isCompanyGoal: boolean;
  requireAssignedTo: boolean;
  hideAssignTo: boolean = false;
  isAssignToRestricted?: boolean;
  isNewEssGoal: boolean = false;
  priority: number;
  isSubGoal: boolean;
  goalTitle: string;
  goalOwnerCtrl: FormControl = new FormControl();
  goalOwnerInputCtrl = new FormControl();
  private _editMode: boolean;
  onetimeEarningChecked: boolean;

  @ViewChildren("task") taskElements: QueryList<ElementRef>;
  @ViewChild("ctrl", { static: false }) ctrl: NgModel;

  showCompanyAlignment: boolean = false;
  companyGoals: IGoal[];
  oneTimeEarningSettings: IOneTimeEarningSettings[];
  selectedOneTimeEarningId: number;
  companyGoals$: Subject<IGoal[]> = new Subject<IGoal[]>();
  filteredCompanyGoals: IGoal[];
  companyGoalCtrl = new FormControl();
  /** TPR-215 - Using this instead of reactive form control.  The reactive form control isn't working with mat-select @see https://github.com/angular/material2/issues/10214 */
  companyGoal: IGoal;

  constructor(
    public dialogRef: MatDialogRef<AddGoalDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private fb: FormBuilder,
    private accountService: AccountService,
    private goalService: GoalService,
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    this.isCompanyGoal =
      this.data.isCompanyGoal == null ? false : this.data.isCompanyGoal;
    this.requireAssignedTo = this.data.requireAssignedTo;
    this.isAssignToRestricted =
      this.data.isAssignToRestricted == null
        ? false
        : this.data.isAssignToRestricted;
    this.isSubGoal = this.data.isSubGoal == null ? false : this.data.isSubGoal;
    this.goalOwners = this.data.goalOwnerList || [];
    this.user = this.data.user;

    if (this.data.isAssignToRestricted != false) {
      var isAligned = false;
      if (this.data.goal && this.data.goal.parentId > 0) {
        isAligned = true;
      }
      if (
        (this.user.userTypeId == UserType.systemAdmin ||
          this.user.userTypeId == UserType.companyAdmin) &&
        isAligned
      ) {
        this.hideAssignTo = false;
      } else {
        this.hideAssignTo = this.data.isAssignToRestricted;
      }
    }

    if (this.data.goal) {
      this.priority = this.data.goal.priority;
    } else {
      this.priority = 0;
    }

    if (!this.user)
      this.accountService.getUserInfo().subscribe((u) => {
        this.user = u;
        this.initializePage();
      });
    else this.initializePage();
  }

  private initializePage() {
    this.resolveGoal();

    /** if we're adding an employee goal, we load client's company goals to allow user to optionally attach */
    if (
      !this.isCompanyGoal ||
      (this.data.goal != null && this.data.goal.parentId > 0)
    ) {
      this.goalService.companyGoals$.subscribe((goals) => {
        this.filterCompanyGoalsUserDoesNotOwn(goals as ICompanyGoal[]);
        this.filterArchivedGoals(goals as ICompanyGoal[]);
        this.companyGoals$.next(this.companyGoals);

        if (this.goal.parentId > 0) {
          this.showCompanyAlignment = true;

          this.companyGoal = this.companyGoals.find(
            (g) => g.goalId == this.goal.parentId
          );
          if (this.companyGoal) this.companyGoalCtrl.setValue(this.companyGoal);
        }
      });

      this.selectedOneTimeEarningId = this.goal.oneTimeEarningSettings
        ? this.goal.oneTimeEarningSettings.oneTimeEarningSettingsId
        : 0;
      var employeeId = this.user.selectedEmployeeId();
      if (employeeId > 0) {
        this.employeeService
          .getAvailableOneTimeEarningSettingsList(employeeId)
          .subscribe((data) => {
            this.oneTimeEarningSettings = data;
          });
      }
    }

    /** set up our forms with validation */
    if (this.requireAssignedTo && !this.isSubGoal)
      this.goalOwnerCtrl.setValidators(Validators.required);
    this.createForm();

    /**
     * We need to figure out the goalowner based on the type of goal we're interacting with.
     * The dialog is designed to accept a list of goal owners, because some implementations come from
     * external components that will already have a goal owner list. However, the dialog doesn't require
     * this list and can call the API to get the list.
     */
    if (this.goalOwners.length) {
      this.setGoalOwner();
    } else if (!this.goalOwners.length) {
      this.goalService.getGoalsByClient(this.user.selectedClientId());

      if (this.requireAssignedTo || !this.goalOwners.length)
        this.goalService
          .getGoalOwnerList(this.user.selectedClientId(), {
            excludeTimeClockOnly: true,
            haveActiveEmployee: true,
            ifSupervisorGetSubords: false,
          })
          .subscribe((ownerList) => {
            this.goalOwners = ownerList;
            this.setGoalOwner();
          });
    } else {
      this.setGoalOwner();
    }
  }

  setOneTimeId(id: number, checked: boolean) {
    if (checked) {
      this.selectedOneTimeEarningId = id;
    } else {
      this.selectedOneTimeEarningId = null;
    }
  }

  /**
   * When adding a goal as a supervisor or employee from ESS, this gets called to filter out any company
   * goals that the end user should not be accessing.
   */
  private filterCompanyGoalsUserDoesNotOwn(companyGoals: ICompanyGoal[]): void {
    /** if user is a ca or sa, they should be able  */
    if (
      !this.isAssignToRestricted ||
      this.user.userTypeId < UserType.supervisor
    ) {
      this.companyGoals = companyGoals;
      return;
    }

    let resultGoals: IGoal[] = [];

    /** if this is an ess goal that we're editing, we only show them the goal that it is attached to. */
    if (!this.isNewEssGoal && this.goal.parentId != null)
      resultGoals.push(
        companyGoals.find((cg) => cg.goalId == this.goal.parentId)
      );

    const ownerId = this.goal.assignedTo;

    for (let i = 0; i < companyGoals.length; i++) {
      let g = companyGoals[i];

      if (
        g.assignedTo == ownerId &&
        resultGoals.find((rg) => rg.goalId == g.goalId) == null
      )
        resultGoals.push(g);
    }

    this.companyGoals = resultGoals;
  }

  ngAfterViewInit() {
    this.goalOwnerInputCtrl.valueChanges
      .pipe(
        skip(1),
        startWith(""),
        debounceTime(500),
        concatMap((search: string): Observable<IContact[]> => {
          return this.goalService.getGoalOwnerList(
            this.user.selectedClientId(),
            {
              page: 1,
              pageSize: 50,
              searchText: search,
              excludeTimeClockOnly: true,
              haveActiveEmployee: true,
              ifSupervisorGetSubords: false,
            }
          );
        })
      )
      .subscribe((next) => {
        this.goalOwners=next ;
        if(this.isSubGoal){
          this.goalOwners = this.goalOwners.filter(x=> x.employeeId!=null);
        }
      });
      
    this.goalOwnerCtrl.valueChanges
    .subscribe((contact) => {
      this.goalOwnerChange(contact);
    });
  }

  // handleSelectedChip(event) {
  //     console.dir(event);
  // }

  displayFn(contact: IContact): string | undefined {
    return contact != null
      ? `${contact.firstName} ${contact.lastName}`
      : undefined;
  }

  private resolveGoal(): void {
    this.isNewEssGoal =
      _.clone(this.data.goal == null) && this.isAssignToRestricted;
    this.goal = _.cloneDeep(this.data.goal) || this.createEmptyGoal();
    if (!this.goal.priority) {
      this.goal.priority = 0;
    }
    this.viewTasks = this.goal.tasks as ViewTask[];

    this.goalTitle =
      this.goal != null && this.goal.goalId > 0
        ? this.isCompanyGoal
          ? "Edit Company Goal"
          : "Edit Goal"
        : this.isCompanyGoal
        ? "Add Company Goal"
        : "Add Goal";

    this.onetimeEarningChecked = new Maybe(this.goal)
      .map((x) => x.oneTimeEarningSettings)
      .map((x) => !!x)
      .valueOr(false);

    this._editMode = this.goal != null && this.goal.goalId > 0;
  }

  getMaxStart() {
    return this._editMode ? null : this.f.value.dueDate;
  }

  getMinDue() {
    return this._editMode ? null : this.f.value.startDate;
  }

  getMinCompletion() {
    return this._editMode ? null : this.f.value.startDate;
  }

  saveGoal(): void {
    this.formSubmitted = true;
    this.f.updateValueAndValidity();
    if (this.f.invalid) return;
    if (this.disableAddButton) this.updateAllTasks();
    if (this.showCompanyAlignment && this.ctrl != null && this.ctrl.invalid)
      return;
    const dto = this.prepareModel();
    this.dialogRef.close(dto);
  }

  updateAllTasks() {
    this.viewTasks.forEach((task, index) => {
      this.updateTaskDescription(index);
    });
  }
  
  onNoClick(): void {
    this.dialogRef.close();
  }

  getTaskFormArray(): FormArray {
    return this.f.get("tasks") as FormArray;
  }

  updateTaskCompletion(event: any, index): void {
    /** Set the completion status on our task accordingly */
    this.viewTasks[index].completionStatus = event.target.checked
      ? CompletionStatusType.Done
      : CompletionStatusType.NotStarted;
  }

  updateTaskDescription(index: number, keyEvent: boolean = false): void {
    const invalid = this.getTaskFormError("description", "required", index);
    if (invalid) return;
    this.disableAddButton = false;
    this.viewTasks[index].description = this.f
      .get("tasks")
      .get(index + "").value.description;
    this.viewTasks[index].editItem = false;

    if (keyEvent) this.dialogAddTask();
  }

  clearTask(index: number, keyEvent: boolean = false): void {
    if (keyEvent && this.viewTasks[index].taskId > 0) return;
    this.disableAddButton = false;
    (<FormArray>this.f.controls.tasks).removeAt(index);
    this.viewTasks.splice(index, 1);
  }

  editTask(task: ViewTask): void {
    task.editItem = true;
    this.disableAddButton = true;
  }

  dialogAddTask(): void {
    this.taskElements.changes.subscribe((queryList: QueryList<ElementRef>) => {
      if (queryList == null || queryList.first == null) return;
      queryList.first.nativeElement.focus();
    });
    this.disableAddButton = true;

    this.viewTasks.push(this.createEmptyTask());
    (<FormArray>this.f.controls.tasks).push(
      this.fb.group({
        completionStatus: this.fb.control(""),
        description: this.fb.control("", [Validators.required]),
      })
    );
  }

  getTaskFormError(field: string, errorCode: string, index: number): boolean {
    const control = this.f
      .get("tasks")
      .get(index + "")
      .get(field);
    return control.hasError(errorCode) && (control.touched || this.f.pending);
  }

  getFormControlError(field: string, errorCode: string): boolean {
    const control = this.f.get(field);
    return (
      control.hasError(errorCode) && (control.touched || this.formSubmitted)
    );
  }

  goalOwnerChange(goalOwner: IContact, isOwnEvent:boolean = true) {
    if (goalOwner) {
      this.f.get("assignedTo").setValue(goalOwner.userId);
    } else {
      // If the goalowner object is undefined,
      // that means the input was cleared and we need to reset our assignedto formcontrol accordingly
      this.f.get("assignedTo").reset();
    }

    this.f.updateValueAndValidity();
    this.goal.goalOwner = goalOwner;
    this.selectedGoalOwner = goalOwner;
    if(!isOwnEvent)
    this.goalOwnerCtrl.setValue(goalOwner);
  }

  alignCompanyGoalCheckboxHandler(event: any) {
    this.showCompanyAlignment = event.target.checked;

    if (this.showCompanyAlignment) {
      this.companyGoalCtrl.setValidators(Validators.required);
    } else {
      this.companyGoalCtrl = new FormControl();
      this.goal.parentId = null;
    }
  }

  private setGoalOwner() {
    let assignedTo = this.goal.assignedTo;

    if (
      assignedTo == null &&
      (this.user.userTypeId == UserType.systemAdmin ||
        this.user.userTypeId == UserType.companyAdmin ||
        this.user.userTypeId == UserType.supervisor) &&
      !this.isCompanyGoal &&
      !this.isSubGoal
    ) {
      if (this.user.selectedEmployeeId() > 0)
        this.selectedGoalOwner = _.find(this.goalOwners, {
          employeeId: this.user.selectedEmployeeId(),
        });
    } else if (
      assignedTo == null &&
      (this.user.userTypeId == UserType.employee ||
        this.user.userTypeId == UserType.applicant) &&
      !this.isCompanyGoal &&
      !this.isSubGoal
    ) {
      this.selectedGoalOwner = this.goalOwners.find(x => x.userId == this.user.userId);
    } else {
      this.selectedGoalOwner = this.goal.goalOwner;
    }

    this.goalOwnerChange(this.selectedGoalOwner, false);
  }

  private createEmptyGoal(): IGoal {
    return {
      goalId: null,
      taskId: null,
      title: null,
      description: null,
      includeReview: true,
      assignedTo: null,
      completedBy: null,
      completionDate: null,
      completionStatus: null,
      dueDate: null,
      parentId: null,
      progress: null,
      startDate: null,
      priority: 0,
      remarks: [],
      tasks: [],
    };
  }

  private createEmptyTask(): ViewTask {
    return {
      taskId: null,
      parentId: null,
      progress: null,
      assignedTo: null,
      completedBy: null,
      completionDate: null,
      completionStatus: null,
      description: null,
      dueDate: null,
      editItem: true,
    };
  }

  private createForm(): void {
    /**
     * The form's assignedTo field is dynamic based on where the dialog is opening from or who is opening it.
     * If it is a subgoal to a company goal, an assignedTo is required, but it isn't prepopulated. However, if it
     * is an employee goal they're creating and doing some from the ESS site, we set it to the goal's assignedTo value
     * or we set it to the logged in user's id.
     */
    let assignedToFormControl =
      this.requireAssignedTo &&
      this.isSubGoal &&
      !this.isAssignToRestricted &&
      !this.isCompanyGoal
        ? this.fb.control(this.goal.assignedTo, [Validators.required])
        : this.isAssignToRestricted
        ? this.fb.control(this.goal.assignedTo || this.user.userId)
        : this.fb.control(this.goal.assignedTo);

    this.f = this.fb.group({
      title: this.fb.control(this.goal.title || "", [Validators.required]),
      assignedTo: assignedToFormControl,
      priority: this.fb.control(this.goal.priority.toString() || "0"),
      description: this.fb.control(this.goal.description || ""),
      startDate: this.fb.control(this.goal.startDate || moment(), [
        Validators.required,
      ]),
      dueDate: this.fb.control(
        this.goal.dueDate == "Invalid date" ? "" : this.goal.dueDate || ""
      ),
      completionDate: this.fb.control(
        this.goal.completionDate == "Invalid date"
          ? ""
          : this.goal.completionDate || ""
      ),
      includeReview: this.fb.control(
        this.goal.includeReview == null ? true : this.goal.includeReview
      ),
      tasks: this.createTaskFormArray(),
    });
  }

  private createTaskFormArray(): FormArray {
    var result: FormArray = this.fb.array([]);
    this.viewTasks.forEach((t) => {
      result.push(
        this.fb.group({
          completionStatus: this.fb.control(
            t.completionStatus == CompletionStatusType.Done
          ),
          description: this.fb.control(t.description || "", [
            Validators.required,
          ]),
        })
      );
    });
    return result;
  }

  private prepareModel(): IGoal {
    let tasks: ITask[] = [];
    let parentId: number = null;
    let isAlignedToCompanyGoal: boolean = false;
    let alignedCompanyGoalName: string = "";

    const isAligned = this.showCompanyAlignment;
    if (isAligned) {
      const alignedGoal: IGoal = this.companyGoal;
      if (alignedGoal != null) {
        parentId = alignedGoal.goalId;
        isAlignedToCompanyGoal = true;
        alignedCompanyGoalName = alignedGoal.title;
      }
    } else {
      parentId = this.goal.parentId;
      isAlignedToCompanyGoal = this.goal.isAlignedToCompanyGoal;
      alignedCompanyGoalName = this.goal.alignedCompanyGoalName;
    }

    this.viewTasks.forEach((t) => {
      tasks.push({
        taskId: t.taskId,
        parentId: t.parentId,
        assignedTo: t.assignedTo || this.f.value.assignedTo,
        description: t.description,
        dueDate: moment(t.dueDate).format(API_STRING),
        progress: t.progress,
        completedBy: t.completedBy,
        completionDate: moment(t.completionDate).format(API_STRING),
        completionStatus: t.completionStatus,
      });
    });
    var k = null;
    if (this.oneTimeEarningSettings && this.selectedOneTimeEarningId) {
      k = this.oneTimeEarningSettings.find(
        (x) => x.oneTimeEarningSettingsId == this.selectedOneTimeEarningId
      );
      this.goal.oneTimeEarningSettings = k;
    }
    return {
      goalId: this.goal != null ? this.goal.taskId || this.goal.goalId : null,
      taskId: this.goal.taskId,
      isAlignedToCompanyGoal: isAlignedToCompanyGoal,
      alignedCompanyGoalName: alignedCompanyGoalName,
      parentId: parentId,
      assignedTo: this.f.value.assignedTo,
      goalOwner: this.goal.goalOwner,
      title: this.f.value.title,
      description: this.f.value.description,
      includeReview: this.f.value.includeReview,
      priority: this.f.value.priority,
      startDate: moment(this.f.value.startDate).format(API_STRING),
      dueDate: moment(this.f.value.dueDate).format(API_STRING),
      progress: this.goal.progress,
      completedBy: this.goal.completedBy,
      completionDate: moment(this.f.value.completionDate).format(API_STRING),
      completionStatus: DetermineGoalCompletionStatus(
        this.f.value.startDate,
        this.f.value.dueDate,
        this.f.value.completionDate,
        this.viewTasks,
        false
      ),
      tasks: this.viewTasks,
      oneTimeEarningSettings: k,
    };
  }

  private determineCompletionStatus(): CompletionStatusType {
    const today = moment();
    let returnType: CompletionStatusType = CompletionStatusType.NotStarted;
    if (today.isSameOrAfter(moment(this.f.value.startDate), "days"))
      returnType = CompletionStatusType.InProgress;
    if (
      this.f.value.dueDate != null &&
      today.isAfter(moment(this.f.value.dueDate), "days")
    )
      returnType = CompletionStatusType.Overdue;
    if (
      this.f.value.completionDate != null &&
      moment(this.f.value.completionDate).isValid()
    )
      returnType = CompletionStatusType.Done;
    return returnType;
  }

  private filterArchivedGoals(companyGoals: ICompanyGoal[]): void {
    let activeGoals: IGoal[] = [];
    for (let i = 0; i < companyGoals.length; i++) {
        let g = companyGoals[i];

        if ( g.isArchived != true ) activeGoals.push(g);
    }
    this.companyGoals = activeGoals;
  }
}
