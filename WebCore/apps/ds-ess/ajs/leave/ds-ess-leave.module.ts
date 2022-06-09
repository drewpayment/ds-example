import * as angular from "angular";
import { DsCoreModule } from "@ajs/core/ds-core.module";
import { DsUiModule } from "@ajs/ui/ds-ui.module";
import { DsLeaveModule } from "@ajs/leave/ds-leave.module";
import { TimeOffEventService } from "./time-off/services/event.service";
import { TimeOffPolicyService } from "./time-off/services/policy.service";
import { TimeOffManagerInstanceService, TimeOffManagerService } from "./time-off/services/manager.service";
import { EssLeaveTimeoffState } from "./time-off/header.state";
import { TimeOffHeaderController } from "./time-off/header.component";
import { TimeOffSummaryController } from "./time-off/summary.component";
import { EssLeaveTimeOffActivityState } from "./time-off/activity.state";
import { TimeoffActivityController } from "./time-off/activity.component";

/**
 * @ngdoc module
 * @module ds.ess.leave
 *
 * @description
 * ESS Leave Management module. Contains angular components related to Dominion ESS Leave Management.
 */
export module DsEssLeaveModule {
    export const MODULE_NAME = 'ds.ess.leave';
    export const AjsModule = angular.module('ds.ess.leave',
        [
            DsCoreModule.AjsModule.name,
            DsUiModule.AjsModule.name,
            DsLeaveModule.AjsModule.name,
            'angularMoment'
        ]
    );

    AjsModule.factory(TimeOffEventService.SERVICE_NAME, TimeOffEventService.$factory());
    AjsModule.factory(TimeOffPolicyService.SERVICE_NAME, TimeOffPolicyService.$factory());
    AjsModule.factory(TimeOffManagerInstanceService.SERVICE_NAME, TimeOffManagerInstanceService.$factory());
    AjsModule.service(TimeOffManagerService.SERVICE_NAME, TimeOffManagerService);

    AjsModule.component(TimeOffHeaderController.COMPONENT_NAME, TimeOffHeaderController.COMPONENT_OPTIONS);
    AjsModule.component(TimeOffSummaryController.COMPONENT_NAME, TimeOffSummaryController.COMPONENT_OPTIONS);
    AjsModule.component(TimeoffActivityController.COMPONENT_NAME, TimeoffActivityController.COMPONENT_OPTIONS);
}
