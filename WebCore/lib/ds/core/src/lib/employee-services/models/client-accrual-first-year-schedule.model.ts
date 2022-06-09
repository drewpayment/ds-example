import { Client } from "@ds/admin/client-statistics/shared/models/client";
import { Moment } from "moment";
import { IClientAccrual } from "./client-accrual.model";

export interface ClientAccrualFirstYearSchedule {
    clientAccrualProratedScheduleId: number;
    clientId: number;
    clientAccrualId: number;
    scheduleFrom: Date | Moment | string;
    scheduleTo: Date | Moment | string;
    awardAfterDaysValue: number;
    awardOnDate: Date | Moment | string;
    awardValue:number;
    modified: Date | Moment | string;
    modifiedBy: number;
    referenceType?: number;
    client?: Client[];
    clientAccrual?: IClientAccrual[];
}
