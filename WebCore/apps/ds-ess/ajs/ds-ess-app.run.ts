import { AccountService } from "@ajs/core/account/account.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { GoogleAnalyticsService } from "@ajs/core/google-analytics/google-analytics.service";
import { DsTimeoutService } from "@ajs/core/timeout/timeout.service";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { DsStyleLoaderService } from "@ajs/ui/ds-styles/ds-styles.service";

/**
 * The run() method is called when the Ess.App is first initialized.  Therefore, we can setup any application-wide
 * logic here, such as app-wide route change management and authentication checks. *
 */
export function DsEssAppAjsModuleRun(
    $rootScope: ng.IRootScopeService,
    $location: ng.ILocationService,
    $window: ng.IWindowService,
    $document: ng.IDocumentService,
    accountService: AccountService,
    analytics: GoogleAnalyticsService,
    msg: DsMsgService,
    $http: ng.IHttpService,
    $templateCache: ng.ITemplateCacheService,
    dsStates: DsStateService,
    dsStyles: DsStyleLoaderService
) {
    // ui-bootstrap template overrides
    $http
        .get('Scripts/app/ui/ui-bootstrap-overrides/modal-carousel-slide.html')
        .then(function (response) {
            $templateCache.put('template/carousel/slide.html', response.data);
        });
    $http
        .get('Scripts/app/ui/ui-bootstrap-overrides/modal-carousel.html')
        .then(function (response) {
            $templateCache.put('template/carousel/carousel.html', response.data);
        });

    //hookup google analytics to state changes
    analytics.registerGoogleAnalyticsPageViewOnStateChange();

    // MESSAGES
    msg.registerStateChangeListeners();

    // STATE PERMISSIONS
    dsStates.registerDeniedStatePermissionAction(function (denial) {
        dsStates.router.transitionTo('ess.profile.view');
    });

    // PAGE TITLE
    dsStates.updatePageTitleOnStateChange();

    // STYLESHEETS
    // dsStyles.resetToOriginalOnStateChange();
}

DsEssAppAjsModuleRun.$inject = [
    '$rootScope',
    '$location',
    '$window',
    '$document',
    AccountService.SERVICE_NAME,
    GoogleAnalyticsService.SERVICE_NAME,
    DsMsgService.SERVICE_NAME,
    '$http',
    '$templateCache',
    DsStateService.SERVICE_NAME,
    DsStyleLoaderService.SERVICE_NAME
];
