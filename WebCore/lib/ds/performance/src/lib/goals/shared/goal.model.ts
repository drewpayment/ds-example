import { Moment } from "moment";
import { ITask, IRemark, CompletionStatusType } from "@ds/core/shared";
import { IContact } from "@ds/core/contacts";
import { GoalPriority } from "./goal.priority";
import * as moment from 'moment';
import { IOneTimeEarningSettings } from '@ds/performance/goals/shared/oneTimeEarningSettings.model';
import { IDsConfirmOptions } from "@ajs/ui/confirm/ds-confirm.interface";

export interface IGoal extends ITask {
    goalId:number,
    title:string,
    includeReview:boolean,
    description:string,
    startDate:string|Date|Moment,
    tasks?:ITask[],
    remarks?:IRemark[],
    client?:{ clientId:number },
    employee?:{ employeeId:number },
    isArchived?:boolean,
    isCompanyGoal?:boolean,
    isAlignedToCompanyGoal?:boolean,
    alignedCompanyGoalName?:string,
    goalOwner?: IContact,
    isSelectedForDelete?:boolean,
    priority?:GoalPriority,
    oneTimeEarningSettings?: IOneTimeEarningSettings,
}

export interface ICompanyGoal extends IGoal {
    subGoals:IGoal[]
}

export interface GoalListView extends ICompanyGoal {
    showKanBanView?:boolean,
    isOpen?:boolean
}

export interface IEmpGoalAligned extends IGoal {
    isAligned:boolean,
    alignedGoal?:IGoal
}

export function DetermineGoalCompletionStatus(
    startDate: string | Date | moment.Moment,
    dueDate: string | Date | moment.Moment,
    completionDate: string | Date | moment.Moment,
    taskList: ITask[],
    isArchived: boolean): CompletionStatusType {

    const startDateMom = moment(startDate);
    const dueDateMom = moment(dueDate);
    const today = moment();
    const nonNullTaskList = taskList || [];
    const allTasksAreDone = !nonNullTaskList.some(x => x.completionStatus !== CompletionStatusType.Done);

    const whenNotNullCallFn = (nullableDate: string | Date | moment.Moment, FnToCall: (date: string | Date | moment.Moment) => boolean) => nullableDate != null && nullableDate != "" && FnToCall(nullableDate);
    const throwWhenNotValid = (date: string | Date | moment.Moment) => {
        if (moment(date).isValid()) return true
        else throw `Cannot determine completion status of this goal! The provided date is invalid!`
    }

    if(isArchived === true){
        return CompletionStatusType.Done;
    }

    if (whenNotNullCallFn(completionDate, throwWhenNotValid)) {
        return CompletionStatusType.Done;
    }

    if (whenNotNullCallFn(dueDate, throwWhenNotValid) && dueDateMom.isBefore(today, 'day')) {
        return CompletionStatusType.Overdue;
    }

    if (whenNotNullCallFn(startDate, throwWhenNotValid) && startDateMom.isSameOrBefore(today, 'day')) {
        return CompletionStatusType.InProgress;
    }

    if (nonNullTaskList.length > 0 && allTasksAreDone) {
        return CompletionStatusType.InProgress;
    }

    return CompletionStatusType.NotStarted;
}

export interface IRemovedGoal {
    goal: IGoal;
    isHardDelete: boolean;
}

export interface IGoalRemoveState {
    btnText: string;
    options: IDsConfirmOptions;
    isHardDelete: boolean;
}

export class GoalRemoveState implements IGoalRemoveState {
    btnText: string;
    options: IDsConfirmOptions;
    isHardDelete: boolean;

    constructor(goal: IGoal) {
        // const status = DetermineGoalCompletionStatus(goal.startDate, goal.dueDate, goal.completionDate, goal.tasks, goal.isArchived);
        const status = goal.completionStatus;
        const isGoalCompleted = (status === CompletionStatusType.Done);
        const archiveOrDelete = isGoalCompleted ? 'Archive' : 'Delete';

        this.btnText = archiveOrDelete;
        this.options = {
            swapOkClose: true,
            closeButtonText: 'Cancel',
            actionButtonText: `Yes, ${archiveOrDelete}`,
            bodyText: `Are you sure you want to ${archiveOrDelete.toLowerCase()} this goal?`
        };
        this.isHardDelete = !isGoalCompleted;
    }
}
