import * as angular from "angular";

import "./vendor";

//dominion ajs modules
import { DsLaborPunchModule } from "@ajs/labor/punch/ds-labor-punch.module";

//app specific modules
import { DsMobileAppModuleConfig } from "./ds-mobile-app.config";
import { DsMobileAppModuleRun } from "./ds-mobile-app.run";

export module DsMobileAppAjsModule {
    const MODULE_NAME = "ds.mobile.app";

    const MODULE_DEPENDENCIES = [
        DsLaborPunchModule.AjsModule.name
    ];

    export const AjsModule = angular.module(MODULE_NAME, MODULE_DEPENDENCIES);

    AjsModule.config(DsMobileAppModuleConfig.$instance);
    AjsModule.run(DsMobileAppModuleRun.factory());

}
