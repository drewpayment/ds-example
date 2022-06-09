import * as angular from "angular";
import { DsUtilityServiceProvider } from "@ajs/core/ds-utility/ds-utility.service";
import { TimeOffStatusType } from '@ds/core/employee-services/models';
export class TimeOffEvent {
    /**
     * Instance of the filter service.  Set at runtime.
     */
    static $filter: ng.IFilterService;
    /**
     * Instance of angular-moment service. Set at runtime.
     */
    static $moment: any;
    /**
     * Instance of DsUtilityService. Set at runtime.
     */
    static dsUtilSvc: DsUtilityServiceProvider;
    static EventTypes = {
        request: { name: 'Request' },
        balance: { name: 'Balance' },
        award: { name: 'Award' },
        adjustment: { name: 'Adjustment' },
        expiration: { name: 'Expiration' }
    };
    amount = 0;
    balanceAfter = 0;
    endDate = null;
    startDate = null;
    requestDate = null;
    requestTimeOffId = null;
    timeOffStatus = null;
    timeOffAward = null;
    policy = null;
    manager = null;
    notes: any;
    constructor(rawEventData, policy) {
        policy = policy || <any>{};
        this.policy = policy;
        this.manager = policy.manager;
        angular.extend(this, rawEventData);
    }
    raw = TimeOffEvent.dsUtilSvc.stripNonValueProperties;
    // --------------------------------------------------------------------------------------
    // - TimeOffEvent METHOD DEFINITION
    // --------------------------------------------------------------------------------------
    /**
     * @ngdoc method
     * @name TimeOffEvent#isPending
     * @methodOf TimeOffEvent
     *
     * @description
     * True if the event is a request and is 'Pending' status.
     */
    isPending() {
        return this.isRequestEvent() && this.timeOffStatus === TimeOffStatusType.Pending; // PENDING
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#isApproved
     * @methodOf TimeOffEvent
     *
     * @description
     * True if the event is a request and is 'Approved' status.
     */
    isApproved() {
        return this.isRequestEvent() && this.timeOffStatus === TimeOffStatusType.Approved; // APPROVED
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#isRejected
     * @methodOf TimeOffEvent
     *
     * @description
     * True if the event is a request and is 'Rejected' status.
     */
    isRejected() {
        return this.isRequestEvent() && this.timeOffStatus === TimeOffStatusType.Rejected; // REJECTED
    }
    isCancelled() {
        return this.isRequestEvent() && this.timeOffStatus === TimeOffStatusType.Cancelled; // CANCELLED
    }

    /**
     * @ngdoc method
     * @name TimeOffEvent#isProjected
     * @methodOf TimeOffEvent
     *
     * @description
     * True if the event is an award event.
     */
    isProjected() {
        return this.isAwardEvent();
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#isAwardEvent
     * @methodOf TimeOffEvent
     *
     * @description
     * True if the event is an award event (eg: 'Award' or 'Expiration').
     */
    isAwardEvent() {
        return !!this.timeOffAward;
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#isPositiveAward
     * @methodOf TimeOffEvent
     *
     * @description
     * True if the event is an award and the amount adds to the overall policy balance.
     */
    isPositiveAward() {
        return this.isAwardEvent() && this.amount > 0;
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#isPositiveAward
     * @methodOf TimeOffEvent
     *
     * @description
     * True if the event is an award and the amount subtracts from the overall policy balance.
     */
    isNegativeAward() {
        return this.isAwardEvent() && this.amount < 0;
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#isRequestEvent
     * @methodOf TimeOffEvent
     *
     * @description
     * True if the event is an request event (ie: has a valid timeOff ID).
     */
    isRequestEvent() {
        return !!this.requestTimeOffId;
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#getEventStatusType
     * @methodOf TimeOffEvent
     *
     * @description
     * Returns the event status type object for the current event.  If the event is an award, a "fake" type
     * is returned w/ a 'Projected' status.
     */
    getEventStatusType() {
        if (this.isAwardEvent())
            return { eventStatusTypeId: null, name: 'Projected' };
        else
            return this.manager.getEventStatusTypeById(this.timeOffStatus);
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#getEventStatusType
     * @methodOf TimeOffEvent
     *
     * @description
     * Returns the event type for the current event (ie: 'Award', 'Expiration' or 'Balance').
     */
    getEventType() {
        if (this.isAwardEvent()) {
            if (this.timeOffAward === 1) {
                return TimeOffEvent.EventTypes.award;
            }
            if (this.timeOffAward === 4) {
                return TimeOffEvent.EventTypes.adjustment;
            }
            return TimeOffEvent.EventTypes.expiration;
        }
        if (this.isRequestEvent()) {
            return TimeOffEvent.EventTypes.request;
        }
        return TimeOffEvent.EventTypes.balance;
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#startMoment
     * @methodOf TimeOffEvent
     *
     * @description
     * Event start date as a MomentJS object.
     */
    startMoment() {
        return TimeOffEvent.$moment(this.startDate);
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#endMoment
     * @methodOf TimeOffEvent
     *
     * @description
     * Event end date as a MomentJS object.
     */
    endMoment() {
        return TimeOffEvent.$moment(this.endDate);
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#friendlyDuration
     * @methodOf TimeOffEvent
     *
     * @description
     * Event duration as friendly/displayable text (eg: 'January 1-5, 2015'). Format is updated based on if the event
     * crosses multiple days, months or years.
     */
    friendlyDuration() {
        var start = this.startMoment(), end = this.endMoment();
        if (start.isSame(end))
            return start.format("MMMM D, YYYY");
        if (start.isSame(end, 'month'))
            return start.format("MMMM D\u2009\u2013\u2009") + end.format("D, YYYY");
        return start.format("MMMM D\u2009\u2013\u2009") + end.format("MMMM D, YYYY");
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#friendlyAmount
     * @methodOf TimeOffEvent
     *
     * @description
     * Event's amount as friendly/displayable text.  If the event's policy is daily policy a single decimal
     * place will always be displayed.
     */
    friendlyAmount() {
        var filter = TimeOffEvent.$filter('number');
        if (this.policy.isDailyPolicy())
            return filter(this.amount, 1);
        return filter(this.amount);
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#friendlyBalance
     * @methodOf TimeOffEvent
     *
     * @description
     * Event's balance as friendly/displayable text.  If the event's policy is daily policy a single decimal
     * place will always be displayed.
     */
    friendlyBalance() {
        var filter = TimeOffEvent.$filter('number');
        if (this.policy.isDailyPolicy())
            return filter(this.balanceAfter, 1);
        return filter(this.balanceAfter);
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#equals
     *
     * @description
     * Checks if the current event has the same data as a second event. Compares values only.
     */
    equals(event) {
        return angular.equals(this.raw(), event.raw()) && angular.equals(this.notes, event.notes);
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#hasRequesterNotes
     *
     * @description
     * True if requestor notes are available.
     */
    hasRequesterNotes() {
        return this.notes && this.notes.requesterNotes;
    }
    /**
     * @ngdoc method
     * @name TimeOffEvent#hasApproverNotes
     *
     * @description
     * True if approver notes are available.
     */
    hasApproverNotes() {
        return this.notes && this.notes.approverNotes;
    }
}