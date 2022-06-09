import { ClockEmployeePunchDto } from '@ds/core/employee-services/models/clock-employee-punch-dto';
import { IClockEmployeeExceptionHistory } from '@ajs/labor/models/client-exception-detail.model';
import { Geofence } from 'apps/ds-source/src/app/models';

export interface IEmployeePunchMapInfo {
    punchData: ClockEmployeePunchDto[];
    exceptionsInfo: IClockEmployeeExceptionHistory[];
    geofenceData: Geofence[];
}

export interface EmployeePunchData {
    name: string;
    punchIdList: number[];
}

export interface IEmployeePunchMapData {
    employeeId: string;
    clientId: string;
    employeePunchData: EmployeePunchData;

}
