import * as angular from "angular";
import { TimeOffPolicy } from "../shared/time-off-policy.model";
import { IUserInfo } from "@ajs/user";
import { DsTimeOffService } from "@ajs/leave/time-off/time-off.service";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { DsPopupService } from "@ajs/ui/popup/ds-popup.service";
import { DsUtilityServiceProvider } from "@ajs/core/ds-utility/ds-utility.service";
import { AccountService } from "@ajs/core/account/account.service";

declare var _:any;

export class TimeOffManagerInstance {
    static $q: ng.IQService;
    static $timeoff: DsTimeOffService;
    static $state: DsStateService;
    static $popup: DsPopupService;
    static $util: DsUtilityServiceProvider;
    static $account: AccountService;

    $loadingState = {
        $promise: <ng.IPromise<TimeOffManagerInstance>>null,
        $isLoading: true
    };
    policies: TimeOffPolicy[] = [];
    eventStatusTypes = [];
    timeOffAwardTypes = [];
    timeOffUnitTypes = [];
    historicalReports = [];
    hasRequestAccess = false;

    constructor(private user: IUserInfo) {
        this.initializeManager();
    }
    
    raw = TimeOffManagerInstance.$util.stripNonValueProperties;

    // --------------------------------------------------------------------------------------
    // - TimeOffManager METHOD DEFINITIONS
    // --------------------------------------------------------------------------------------
    getUser() {
        return this.user;
    };
    canRequestTimeOff() {
        return this.hasRequestAccess;  
    };

    /**
     * @ngdoc method
     * @name TimeOffManager#getActivePolicy
     *
     * @description
     * Gets the active policy from the manager. Or undefined if no active policy is set.
     */
    getActivePolicy() {
        var active;
        if (TimeOffManagerInstance.$state.router.params.policyName) {
            angular.forEach(this.policies, function (policy) {
                if (policy.getUrlRouteId() === TimeOffManagerInstance.$state.router.params.policyName)
                    active = policy;
            });
        }

        return active;
    }

    /**
     * @ngdoc method
     * @name TimeOffManager#getPolicyIndex
     *
     * @description
     * Returns the index of the specified policy in the collection of available policies for the user.
     */
    getPolicyIndex(lookupPolicy) {
        return _.indexOf(this.policies, lookupPolicy);
    }

    /**
     * @ngdoc method
     * @name TimeOffManager#getEventStatusTypeById
     *
     * @description
     * Returns the object representation of the status type with the given ID.
     */
    getEventStatusTypeById(statusId) {
        var sType;
        angular.forEach(this.eventStatusTypes, function (t) {
                if(t.eventStatusTypeId === statusId) 
                    sType = t;
        });
        return sType;
    }

    /**
     * @ngdoc method
     * @name TimeOffManager#getTimeOffUnitTypeById
     *
     * @description
     * Returns the object representation of the unit type with the given ID.
     */
    getTimeOffUnitTypeById(unitTypeId) {
        var uType;
        angular.forEach(this.timeOffUnitTypes, function (t) {
                if(t.timeOffUnitTypeId === unitTypeId) 
                    uType = t;
        });
        return uType;
    }

    /**
     * @ngdoc method
     * @name TimeOffManager#hasPolicies
     *
     * @description
     * True if the manager contains any policies for the current user.
     */
    hasPolicies() {
        return this.policies.length > 0;
    }

    /**
     * @ngdoc method
     * @name TimeOffManager#hasMultiplePolicies
     *
     * @description
     * True if the manager contains more than one policy for the current user.
     */
    hasMultiplePolicies() {
        return this.policies.length > 1;
    }

    /**
     * @ngdoc method
     * @name TimeOffManager#addTimeOffEvent
     *
     * @description
     * Attempts to add an activity event to the specified policy.
     */
    addTimeOffEvent(policy) {
        return this.editTimeOffEvent({ policy: policy });
    }

    /**
     * @ngdoc method
     * @name TimeOffManager#editTimeOffEvent
     *
     * @description
     * Attempts to edit an activity event to the specified policy.  Currently opens a popup window to the 
     * legacy RequestTimeOff.aspx page in which the time-off request can be modified and saved.
     *
     * OPTIONS
     * -------
     * {
     *    event: (Optional) The ds.ess.leave:TimeOffEvent to edit.
     *    policy: (Optional) The ds.ess.leave:TimeOffPolicy to add/edit an event for.
     * }
     *
     * @returns 
     * Returns a promise which is resolved once the popup is closed and data is reloaded in the current 
     * time-off manager.
     */
    editTimeOffEvent(options) {
        var popupUrl = 'Legacy/RequestTimeOffPopup.aspx?',
            deferred = TimeOffManagerInstance.$q.defer();

        if(this.canRequestTimeOff()) {
            if(options.event) {
                popupUrl += 'strRequestTimeOffID=' + options.event.requestTimeOffId;
            } 
            else {
                popupUrl += 'strRequestTimeOffID=0';       
            }

            if(options.policy) {
                popupUrl += '&ClientAccrualID=' + options.policy.policyId;
            }

            popupUrl += '&strEmployeeID=' + this.getUser().employeeId;

            var popup = TimeOffManagerInstance.$popup.open(popupUrl, '',  { height: 625, width: 820 });
            return popup.closed();

        } else {
            deferred.reject('You do not have permission to request time off.');
        }

        return deferred.promise;
    }
    
    /**
     * @ngdoc method
     * @name TimeOffManager#equals
     *
     * @description
     * Checks if the current manager has the same data as a second manager instance. Compares values only.
     */
    equals(manager: TimeOffManagerInstance) {
        var equal = angular.equals(this.raw(), manager.raw()) && this.policies.length === manager.policies.length;

        // check if all policies are equal
        if(equal) {
            for(var i = 0; i < this.policies.length; i++) {
                if(!this.policies[i].equals(manager.policies[i]))
                    return false;
            }
        }

        return equal;
    }

    
    // --------------------------------------------------------------------------------------
    // - PRIVATE HELPERS
    // --------------------------------------------------------------------------------------

    /**
     * @private
     * @name initializeManager
     *
     * @description
     * Loads the specified manager with the current time off data and settings.
     */
    private initializeManager() {
        var manager = this;
        var promises = [];

        promises.push(loadPermissions());
        promises.push(loadPolicies());
        promises.push(loadHistoricalReports());
        promises.push(loadEventStatusTypes());
        promises.push(loadTimeOffUnitTypes());
        promises.push(loadTimeOffAwardTypes());

        this.$loadingState.$isLoading = true;

        this.$loadingState.$promise = TimeOffManagerInstance.$q.all(promises).then(() => {
            this.$loadingState.$isLoading = false;
            this.$loadingState.$promise = null;
            return manager;
        });

        return this.$loadingState;

        function loadPermissions() {
            return TimeOffManagerInstance.$account.canPerformActions('LeaveManagement.TimeOffRequest')
            .then(function () { manager.hasRequestAccess = true; })
            .catch(function () { manager.hasRequestAccess = false; });
        }

        /** 
         * @private
         * @description
         * Loads the time-off policies for the current user from the API.
         */
        function loadPolicies() {
            return TimeOffManagerInstance.$timeoff
                .getTimeOffPolicyActivity(manager.user)
                .then(function (policyData) {
                    manager.policies = extendPolicies(policyData);
                });
        }

        /** 
         * @private
         * @description
         * Loads the time-off historical report types.
         */
        function loadHistoricalReports() {
            return TimeOffManagerInstance.$timeoff
                .getHistoricalReports()
                .then(function (reports) {
                    manager.historicalReports = reports;
                });
        } 

        /** 
         * @private
         * @description
         * Loads the time-off unit types (eg: Hours, Days).
         */
        function loadTimeOffUnitTypes() {
            return TimeOffManagerInstance.$timeoff
                .getTimeOffUnitTypes()
                .then(function (typeData) {
                    manager.timeOffUnitTypes = typeData;
                });
        } 

        /** 
         * @private
         * @description
         * Loads the time-off award types (eg: Award, Expiration, Request, etc).
         */
        function loadTimeOffAwardTypes() {
            return TimeOffManagerInstance.$timeoff
                .getTimeOffAwardTypes()
                .then(function (typeData) {
                    manager.timeOffAwardTypes = typeData;
                });
        }
        
        /** 
         * @private
         * @description
         * Loads the time-off event status types (eg: Pending, Approved, Rejected).
         */
        function loadEventStatusTypes() {
            return TimeOffManagerInstance.$timeoff
                .getTimeOffEventStatusTypes()
                .then(function (typeData) {
                    manager.eventStatusTypes = typeData;
                });
        }  

        /** 
         * @private
         * @description
         * Objectifies the policy data returned from the API by adding helper methods and extensions.
         */
        function extendPolicies(apiPolicyData) {
            var policies: TimeOffPolicy[] = [];

            angular.forEach(apiPolicyData, function (policy) {
                policies.push(new TimeOffPolicy(policy, manager));
            });

            return policies;
        }
    }
}

/**
 * Registers the TimeOffManagerInstance class with angular to be properly
 * injected and wired up with supporting angular dependencies.
 */
export class TimeOffManagerInstanceService {
    static SERVICE_NAME = "TimeOffManagerInstance";

    static $factory() {
        let factory = ($q, timeOffSvc, state, dsPopup, util, account, Policy) => {
            TimeOffManagerInstance.$q       = $q;
            TimeOffManagerInstance.$timeoff = timeOffSvc;
            TimeOffManagerInstance.$state   = state;
            TimeOffManagerInstance.$popup   = dsPopup;
            TimeOffManagerInstance.$util    = util;
            TimeOffManagerInstance.$account = account;

            //Policy = noop ... intentional. Need to inject, don't need to do anything with it
            
            return TimeOffManagerInstance;
        };
        factory.$inject = [
            '$q',
            'DsTimeOffService',
            'DsState',
            'DsPopup',
            'DsUtility',
            'AccountService',
            'TimeOffPolicy'
        ];
        return factory;
    }
}

/**
 * @ngdoc class
 * @name ds.ess.leave:TimeOffManagerService
 *
 * @description
 * Service used to manage time-off data.
 */
export class TimeOffManagerService {
    static readonly SERVICE_NAME = 'TimeOffManagerService';
    static readonly $inject = ['$rootScope', 'TimeOffManagerInstance'];
    static readonly TIMEOFF_MANAGER_UPDATED = 'TimeOffManagerUpdated';

    manager;

    constructor(private $rootScope: ng.IRootScopeService, TimeOffManagerInstance: TimeOffManagerInstance) {
        
    }

    manageUserTimeOff(user: IUserInfo) {
        return new TimeOffManagerInstance(user).$loadingState.$promise.then((resolvedManager) => {
            // check if the new manager has different policy info than the current one
            var isUpdated = angular.isDefined(this.manager) && !this.manager.equals(resolvedManager);
            
            this.manager = resolvedManager;

            // if the policy info has changed let everyone know 
            if(isUpdated)
                this.$rootScope.$broadcast(TimeOffManagerService.TIMEOFF_MANAGER_UPDATED, this.manager);

            return this.manager;
        });
    }
}