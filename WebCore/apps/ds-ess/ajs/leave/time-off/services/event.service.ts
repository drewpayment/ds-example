import { TimeOffEvent } from "../shared/time-off-event.model";
import { DsUtilityServiceProvider } from "@ajs/core/ds-utility/ds-utility.service";

export class TimeOffEventService {
    static readonly SERVICE_NAME = 'TimeOffEvent';

    static $factory() {
        let factory = ($filter, $moment, util: DsUtilityServiceProvider) => {
            //assign static properties of a timeoffevent to the 
            //singleton instances from angular
            TimeOffEvent.$filter = $filter;
            TimeOffEvent.$moment = $moment;
            TimeOffEvent.dsUtilSvc = util;

            //return the TimeOffEvent class itself so it can be "new"-ed up
            //by other services
            return TimeOffEvent;
        }
        factory.$inject = ['$filter', 'moment', DsUtilityServiceProvider.SERVICE_NAME];
        return factory;
    }
} 
