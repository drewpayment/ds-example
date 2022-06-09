import * as angular from "angular";
import { NgxDemoRootComponent } from "./demo.component";

export module NgxMigrationDemoAjsModule {
    const MODULE_NAME = "ngx.demo";

    export const AjsModule = angular.module(MODULE_NAME, []);

    AjsModule
        .component(NgxDemoRootComponent.SELECTOR, NgxDemoRootComponent.CONFIG);
}