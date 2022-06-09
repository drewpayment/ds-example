import * as angular from "angular";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import { STATES } from "../../shared/state-router-info";

export class DsNavigationService {
    static readonly SERVICE_NAME = 'DsNavigationService';
    static readonly $inject = [
        '$location',
        '$cacheFactory',
        DsMsgService.SERVICE_NAME
    ];

    cache = this.$cacheFactory('dsNavigationServiceCache');

    constructor(private $location: ng.ILocationService, private $cacheFactory: ng.ICacheFactoryService, private msg: DsMsgService) {
    }

    /**
     * @ngdoc method
     * @name #gotoRoute
     * @param routeName - the route key / path to navigate to. A starting slash will automatically be added if it is not provided.
     * @param cacheObject - object to cache during the route change. Object is keyed against the given route.
     *
     * @description
     * Perform a route change based on the routeName parameter.
     */
    gotoRoute(routeName, cacheObject?) {
        // ensure route name will match the format of route URLs in the route table
        // 1. prepend route w/ forward slash if not already present
        routeName = routeName.charAt(0) === '/' ? routeName : '/' + routeName;
        // 2. remove trailing forward slash if present
        routeName = routeName.charAt(routeName.length - 1) === '/' ? routeName.slice(0, -1) : routeName;

        this.cache.put(routeName, cacheObject);

        this.$location.path(routeName);
    };

    /**
     * @ngdoc method
     * @name #gotoProfile
     *
     * @description
     * We have a lot of pages that need to reroute to the profile page.
     * This will make that happen (convenience).
     */
    gotoProfile(cacheObject?) {
        this.gotoRoute('profile', cacheObject);
    };

    /**
     * @ngdoc method
     * @name #gotoProfileThenShowSuccessMessage
     *
     * @description
     * We have a lot of pages that need to reroute to the profile page and show a success message (generic).
     * This will make that happen (convenience).
     */
    gotoProfileThenShowSuccessMessage(cacheObject?) {
        this.gotoRouteThenShowSuccessMessage(
            'profile',
            cacheObject);
    };

    /**
     * @ngdoc method
     * @name #gotoRouteThenShowSuccessMessage
     * @param routeName - the route key with NO starting slash (it's automatically prepended)
     *
     * @description
     * We have a lot of pages that need to reroute then show a success message (generic).
     * This will make that happen (convenience).
     */
    gotoRouteThenShowSuccessMessage(routeName, cacheObject) {
        this.gotoRouteThenShowMessage(
            routeName,
            window.MESSAGE_SUCCESSFUL_FORM_SUBMISSION_ESS,
            MessageTypes.success,
            cacheObject);
    };

    /**
    * @ngdoc method
    * @name #gotoRouteThenShowMessage
    * @param routeName - the route key with NO starting slash (it's automatically prepended)
    * @param msg - the message to show after route desination is reached.
    * @param msgType - the message type
    *
    * @description
    * This can be used to specify a message to show after routing the user to the route specified by the 'routeName' parameter.
    */
    gotoRouteThenShowMessage(routeName, message, msgType, cacheObject?) {
        this.msg.setOnRouteChangeSuccessMessage(
            message,
            msgType);

        this.gotoRoute(routeName, cacheObject);
    };

    /**
     * @ngdoc method
     * @name getCacheForCurrentRoute
     *
     * @description
     * Attempts to get the cached object for the current route.  If found, the object is copied, removed from cache and returned.
     */
    getCacheForCurrentRoute() {
        var returnObject,
            key = this.$location.path(),
            cachedObject = this.cache.get(key);

        if (cachedObject) {
            returnObject = angular.copy(cachedObject);
            this.cache.remove(key);
        }

        return returnObject;
    };

}
