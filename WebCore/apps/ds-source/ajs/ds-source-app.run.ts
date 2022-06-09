import { DsTimeoutService } from "@ajs/core/timeout/timeout.service";

/**
 * The run() method is called when the angular app is first initialized.  Therefore, we can setup any application-wide
 * logic here, such as app-wide route change management and authentication checks.
 */
export class DsSourceAppModuleRun {
    static $inject = ['$window'];

    constructor(private $window: ng.IWindowService) {
        this.$onInit();
    }

    $onInit(){
    }

    static factory() {
        return [...DsSourceAppModuleRun.$inject, (win) => new DsSourceAppModuleRun(win)];
    }
}
