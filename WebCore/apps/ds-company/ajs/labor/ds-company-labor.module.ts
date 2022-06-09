import * as angular from "angular";
import { DsCoreModule } from "@ajs/core/ds-core.module";
import { DsUiModule } from "@ajs/ui/ds-ui.module";
import { DsLaborModule } from "@ajs/labor/ds-labor.module";
import { CompanyLaborScheduleGroupState } from "./scheduling/group/group.state";
import { CompanyLaborState } from "./labor.state";
import { CompanyLaborScheduleState } from "./scheduling/schedule.state";
import { GroupScheduleConfigModalService } from "./scheduling/group/schedule-config-modal.service";
import { GroupScheduleShiftModalService } from "./scheduling/group/shift-modal.service";
import { GroupScheduleSubGroupModalService } from "./scheduling/group/subgroup-modal.service";
import { CompanyLaborScheduleGroupPlanState } from "./scheduling/group/plan.state";
import { GroupSchedulePrintDialogService } from "./scheduling/group/print-dialog.service";
import { ScheduleGroupOverrideModalService } from "./scheduling/group/schedule-group-override-modal.service";
import { CompanyLaborScheduleGroupScheduleState } from "./scheduling/group/schedule.state";

export module DsCompanyLaborModule {
    export const AjsModule = angular.module('ds.company.labor', 
        [
            DsCoreModule.AjsModule.name,
            DsUiModule.AjsModule.name,
            DsLaborModule.AjsModule.name
        ]
    );

    //services, etc
    AjsModule
        .service(GroupScheduleConfigModalService.SERVICE_NAME, GroupScheduleConfigModalService)
        .service(GroupScheduleShiftModalService.SERVICE_NAME, GroupScheduleShiftModalService)
        .service(GroupScheduleSubGroupModalService.SERVICE_NAME, GroupScheduleSubGroupModalService)
        .service(GroupSchedulePrintDialogService.SERVICE_NAME, GroupSchedulePrintDialogService)
        .service(ScheduleGroupOverrideModalService.SERVICE_NAME, ScheduleGroupOverrideModalService);

    //ui-router states
    AjsModule
        .config(CompanyLaborState.$config())
        .config(CompanyLaborScheduleState.$config())
        .config(CompanyLaborScheduleGroupState.$config())
        .config(CompanyLaborScheduleGroupPlanState.$config())
        .config(CompanyLaborScheduleGroupScheduleState.$config());
}
