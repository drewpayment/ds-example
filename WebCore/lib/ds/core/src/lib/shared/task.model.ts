import { CompletionStatusType } from "./completion-status.enum";
import { Moment } from "moment";
import { IContact } from '../contacts';

export interface ITask {
    taskId:number,
    parentId:number|null,
    description:string,
    progress:number|null,
    completionStatus:CompletionStatusType|null,
    completionDate:Date|string|Moment,
    completedBy:number|null,
    /**
     * Represents the userId the task is assigned to.
     */
    assignedTo:number|null,
    dueDate:Date|string|Moment,
    goalOwner?:IContact
}

/**
 * Used to control UI elements in a *ngFor on the goal-detail component.
 */
export interface ViewTask extends ITask {
    editItem?:boolean
}

/**
 * Calculates progress percentage for given viewTaskList.
 * @param viewTaskList basis for the calculation.
 */
export function CalculateTaskListProgress(viewTaskList: ITask[]): number {
    /** if there are no tasks, we simply set progress to zero and short circuit the method */
    if (!Array.isArray(viewTaskList) || viewTaskList.length === 0) return 0;

    const completedTasksCount = viewTaskList.reduce((accumulator, currentValue) => {
        return accumulator + Number(currentValue.completionStatus === CompletionStatusType.Done);
    }, 0);

    return (completedTasksCount / viewTaskList.length) * 100;
}
