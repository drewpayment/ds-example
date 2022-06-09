import { MeritIncreaseType } from '@ds/performance/evaluations/shared/merit-increase-type.enum';

export enum BasedOn {
    Salary = 1
}

export enum Measurement {
    GoalCompletion = 1
}

export interface IOneTimeEarningSettings  {
    oneTimeEarningSettingsId:number,
    employeeId:number,
    name:string,
    increaseType:MeritIncreaseType,
    increaseAmount:number,
    basedOn:BasedOn,
    measurement:Measurement,
    displayInEss:boolean,
    isArchived:boolean,
}