import * as angular from "angular";
import { TimeOffEvent } from "../shared/time-off-event.model";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { DsUtilityServiceProvider } from "@ajs/core/ds-utility/ds-utility.service";

declare var _:any;

/**
 * @ngdoc class
 * @name TimeOffPolicy
 * @class
 *
 * @description
 * Represents a Time-Off policy.  Extends the basic policy information returned from the API with properties
 * and functions relevant to ESS time-off management.
 */
export class TimeOffPolicy {

    static $moment;
    static $dsState: DsStateService;
    static $dsUtil: DsUtilityServiceProvider;
    
    policyId = 0;
    policyName = '';
    unitsAvailable = 0;
    pendingUnits = 0;
    pendingRequest = 0;
    nextAwardDate = null;
    activity: TimeOffEvent[] = [];
    manager;
    startingUnits = 0;
    startingUnitsAsOf = null;
    timeOffUnitType = 1; // HOURS
    timeOffUnitTypeName: string;
    display4Decimals: boolean;
    unitsPerDay: number;

    constructor(rawPolicyData, manager) {
        angular.extend(this, rawPolicyData);
        
        this.manager = manager;
        this.activity = this.extendedActivity();

        var unitType = this.manager.getTimeOffUnitTypeById(this.timeOffUnitType);
        if(unitType)
            this.timeOffUnitTypeName = unitType.name;
    }

    raw = TimeOffPolicy.$dsUtil.stripNonValueProperties;
    
    // --------------------------------------------------------------------------------------
    // - TimeOffPolicy METHOD DEFINITIONS
    // --------------------------------------------------------------------------------------

    /**
     * @ngdoc method
     * @name TimeOffPolicy#getUrlRouteId
     * @methodOf TimeOffPolicy
     *
     * @description
     * Creates a unique URL segment for the given policy from the policy's name by replacing all special
     * characters and spaces by a '-'.
     */
    getUrlRouteId() {
        return this.policyName.replace(/[^a-z0-9]/gi, '-');
    }

    /**
     * @ngdoc method
     * @name TimeOffPolicy#viewActivity
     * @methodOf TimeOffPolicy
     *
     * @description
     * Navigates the user to the policy's "Activity" view.
     */
    viewActivity() {
        TimeOffPolicy.$dsState.router.go('ess.leave.timeoff.activity', { policyName: this.getUrlRouteId() });
    }

    /**
     * @ngdoc method
     * @name TimeOffPolicy#getPendingUnits
     * @methodOf TimeOffPolicy
     *
     * @description
     * Returns the total number of requested time off units (eg: hours/days) that are pending approval.
     */
    getPendingUnits() {
        return this.pendingUnits;
    }

    /**
     * @ngdoc method
     * @name TimeOffPolicy#getPendingRequestCount
     * @methodOf TimeOffPolicy
     *
     * @description
     * Returns the total number of policy event requests that are pending approval.
     */
    getPendingRequestCount() {
        return this.pendingRequest;
    }

    /**
     * @ngdoc method
     * @name TimeOffPolicy#hasEvents
     * @methodOf TimeOffPolicy
     *
     * @description
     * Returns an indication if the policy has any activity events.
     */
    hasEvents() {
        return this.activity.length > 0;
    }

    /**
     * @ngdoc method
     * @name TimeOffPolicy#hasEvents
     * @methodOf TimeOffPolicy
     *
     * @description
     * Returns the next award date for the policy or null if no awards are available.
     */
    getNextAwardDate() {
        return this.nextAwardDate;
    }

    /**
     * @ngdoc method
     * @name TimeOffPolicy#hasEvents
     * @methodOf TimeOffPolicy
     *
     * @description
     * Returns an indication if there is a projected award event for the policy.
     */
    hasNextAwardDate() {
        return !!this.nextAwardDate;
    }

    /**
     * @ngdoc method
     * @name TimeOffPolicy#buildReportUrl
     * @methodOf TimeOffPolicy
     *
     * @description
     * Returns a policy-specific URL for the given ds.leave:TimeOffHistoricalReport.
     */
    getReportUrl(report) {
        var from = TimeOffPolicy.$moment(this.startingUnitsAsOf).add(-1, 'days').add(-1, 'years'),
            to = TimeOffPolicy.$moment(this.startingUnitsAsOf).add(-1, 'days');

        return 'Legacy/API/GetLeaveManagementEmployeeReport.ashx' +
            '?PolicyId='+ this.policyId + 
            '&FromDate=' + from.format('MM/DD/YYYY') +
            '&ToDate=' + to.format('MM/DD/YYYY') +
            '&ReportTypeId=' + report.reportTypeId;   
    }

    /**
     * @ngdoc method
     * @name TimeOffPolicy#isHourlyPolicy
     * @methodOf TimeOffPolicy
     *
     * @description
     * True if the policy is tracked in number of hours.
     */
    isHourlyPolicy() {
        return this.timeOffUnitType === 1;
    }

    /**
     * @ngdoc method
     * @name TimeOffPolicy#isDailyPolicy
     * @methodOf TimeOffPolicy
     *
     * @description
     * True if the policy is tracked in number of days.
     */
    isDailyPolicy() {
        return this.timeOffUnitType === 2;
    }

    /**
     * @ngdoc method
     * @name TimeOffPolicy#equals
     *
     * @description
     * Checks if the current policy has the same data as a second policy. Compares values only.
     */
    equals(policy) {
        var equal = angular.equals(this.raw(), policy.raw()) && this.activity.length === policy.activity.length;

        // check if all events are equal
        if(equal) {
            for(var i = 0; i < this.activity.length; i++) {
                if(!this.activity[i].equals(policy.activity[i]))
                    return false;
            }
        }

        return equal;
    }

    // --------------------------------------------------------------------------------------
    // - PRIVATE HELPERS
    // --------------------------------------------------------------------------------------
    private extendedActivity() {
        return _.map(this.activity, (e) => {return new TimeOffEvent(e, this);});
    }
}


