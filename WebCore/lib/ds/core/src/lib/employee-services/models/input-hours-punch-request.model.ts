import { PunchOptionType } from '../enums';
import { ClockEmployeeBenefit } from '.';


export interface PunchResult {
    requestType:PunchOptionType;
    success:boolean;
    message:string;
    punchId:number|null;
}

export interface InputHoursPunchRequest {
    ipAddress?:string;
    data:ClockEmployeeBenefit;
}

export interface InputHoursPunchRequestResult extends PunchResult {
    data:ClockEmployeeBenefit;
}
