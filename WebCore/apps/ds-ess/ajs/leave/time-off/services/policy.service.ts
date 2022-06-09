import { TimeOffEvent } from "../shared/time-off-event.model";
import { TimeOffPolicy } from "../shared/time-off-policy.model";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { DsUtilityServiceProvider } from "@ajs/core/ds-utility/ds-utility.service";
import { TimeOffEventService } from "./event.service";
export class TimeOffPolicyService {
    static SERVICE_NAME = "TimeOffPolicy";
    static $factory() {
        let factory = ($moment, state, util, TimeOffEvent: TimeOffEvent) => {
            //initialize static properties of TimeOffPolicy to the 
            //injected angular singletons
            TimeOffPolicy.$moment = $moment;
            TimeOffPolicy.$dsState = state;
            TimeOffPolicy.$dsUtil = util;
            //NOTE: Don't need to do anything with TimeOffEvent
            //but needs to be injected to be sure its static 
            //properties are set as well
            return TimeOffPolicy;
        };
        factory.$inject = [
            'moment',
            DsStateService.SERVICE_NAME,
            DsUtilityServiceProvider.SERVICE_NAME,
            TimeOffEventService.SERVICE_NAME
        ];
        return factory;
    }
}