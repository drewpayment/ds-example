import { Moment } from 'moment';
import { PunchOptionType } from '../enums';
import { ClockEmployeePayPeriodEndedDto } from '@ajs/labor/punch/api';
import { EmployeeClockConfiguration } from '.';

export interface CheckPunchTypeResultDto {
    /// <summary>
    /// Time of last punch, if applicable
    /// </summary>
    lastPunchTime?: Moment|Date|string;

    /// <summary>
    /// true if the user has no punches for the current day of shift (depends on their shift setup)
    /// </summary>
    isFirstPunchOfDay: boolean;

    /// <summary>
    /// true is the user will be punching out with there next / current punch
    /// </summary>
    isOutPunch: boolean;

    /// <summary>
    /// property tht indicates if the punch type selection control should be disabled
    /// </summary>
    shouldDisablePunchTypeSelection: boolean;

    /// <summary>
    /// property that indicates whether the cost center selection control should be hidden.
    /// </summary>
    shouldHideCostCenter: boolean;

    /// <summary>
    /// property that indicates whether the department selection controls should be hidden.
    /// </summary>
    shouldHideDepartment: boolean;

    /// <summary>
    /// property that indicates whether the job costing selection controls should be hidden.
    /// </summary>
    shouldHideJobCosting: boolean;

    /// <summary>
    /// property that indicates whether employee note input controls should be hidden.
    /// </summary>
    shouldHideEmployeeNotes: boolean;

    /// <summary>
    /// Property that indicate whether a cost center must be selected.  Based on
    /// the <see cref="ClockCostCenterRequirementType"/> company option.
    /// </summary>
    isCostCenterSelectionRequired: boolean;

    /// <summary>
    /// if not null, indicates the cost center used in the last punch.  If using a
    /// cost center selection control, this should be the default selection.
    /// </summary>
    costCenterId?: number;

    /// <summary>
    /// Cost center of previous punch
    /// </summary>
    lastOutCostCenterId?: number;

    /// <summary>
    /// indicates the lunch cost center id, if applicable
    /// </summary>
    lunchCostCenterId?: number;

    /// <summary>
    /// department id from last punch
    /// </summary>
    departmentId?: number;

    /// <summary>
    /// division id from last punch
    /// </summary>
    divisionId?: number;

    /// <summary>
    /// Id for the type of punch, normal versus lunch, etc
    /// </summary>
    punchTypeId?: number;

    /// <summary>
    /// What type of punch (e.g. Normal, Input Hours, etc)
    /// </summary>
    punchOption?: PunchOptionType;

    /// <summary>
    /// If input punches are allowed.  This overrides the <see cref="PunchOption"/> setting.
    /// </summary>
    allowInputPunches: boolean;

    /// <summary>
    /// The home cost center for the employee. If applicable.
    /// </summary>
    homeCostCenterId?: number;

    /// <summary>
    /// The home department for the employee. If applicable.
    /// </summary>
    homeDepartmentId?: number;

    /// <summary>
    /// id for first job costing assignment selected on previous punch
    /// </summary>
    clientJobCostingAssignmentId1?: number;

    /// <summary>
    /// id for second job costing assignment selected on previous punch
    /// </summary>
    clientJobCostingAssignmentId2?: number;

    /// <summary>
    /// id for third job costing assignment selected on previous punch
    /// </summary>
    clientJobCostingAssignmentId3?: number;

    /// <summary>
    /// id for fourth job costing assignment selected on previous punch
    /// </summary>
    clientJobCostingAssignmentId4?: number;

    /// <summary>
    /// id for fifth job costing assignment selected on previous punch
    /// </summary>
    clientJobCostingAssignmentId5?: number;

    /// <summary>
    /// id for sixth job costing assignment selected on previous punch
    /// </summary>
    clientJobCostingAssignmentId6?: number;

    /// <summary>
    /// The DTO that represents the period ending information for the employee.
    /// </summary>
    payPeriodEnded: ClockEmployeePayPeriodEndedDto;

    /// <summary>
    /// IP Address punch is requested from.
    /// </summary>
    ipAddress: string;

    /// <summary>
    /// Indication if the user can punch from their current IP. (see <see cref="IpAddress"/>).
    /// If null, IP check was not performed.
    /// </summary>
    canPunchFromIp?: boolean;

    /// <summary>
    /// Indication if the user has mobile punching option on the assigned punch rules.
    /// </summary>
    hasMobilePunching: boolean;

    /// <summary>
    /// Additional details about the employee and their clock setup.
    /// </summary>
    employeeClockConfiguration: EmployeeClockConfiguration;
}
