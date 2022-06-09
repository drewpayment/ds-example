import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsConfigurationService } from "@ajs/core/ds-configuration/ds-configuration.service";

declare var _:any;

export function DsCompanyAppAjsRun(
    $window: ng.IWindowService,
    states: DsStateService,
    dsMsgSvc: DsMsgService,
    dsConfig: DsConfigurationService) {

    // INIT MSG SVC
    dsMsgSvc.registerStateChangeListeners();

    // PAGE TITLE
    states.updatePageTitleOnStateChange();

    // STATE PERMISSIONS
    states.registerDeniedStatePermissionAction(function (denial) {

        if (isNotGroupScheduler()){
            $window.location.href = dsConfig.getAbsoluteUrl('Legacy/Error.aspx?DominionError=access');
        }

        function isNotGroupScheduler() {
            return denial && denial.actionsNotAllowed && denial.actionsNotAllowed.some(function (action) {
                    return action === 'LaborManagement.LaborScheduleAdministrator' || action === 'LaborManagement.LaborPlanAdministrator';
            });
        }
    });
}

DsCompanyAppAjsRun.$inject = [
    '$window',
    DsStateService.SERVICE_NAME,
    DsMsgService.SERVICE_NAME,
    DsConfigurationService.SERVICE_NAME
];
