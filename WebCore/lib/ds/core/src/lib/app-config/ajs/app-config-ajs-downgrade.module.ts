import * as angular from "angular";
import { downgradeComponent } from "@angular/upgrade/static";
import { AppResourceManagerComponent } from "../app-resource-manager/app-resource-manager.component";
import { MenuBuilderComponent } from '@ds/core/app-config/menu-builder/menu-builder.component';

export module DsAppConfigDowngradeModule {
    export const MODULE_NAME = 'ds.app-config.downgrade';
    export const AjsModule = angular.module(MODULE_NAME, []);

    AjsModule
        .directive(
            'dsAppResourceManager',
            downgradeComponent({ component: AppResourceManagerComponent }))
        .directive(
            'dsMenuBuilder',
            downgradeComponent({ component: MenuBuilderComponent }))
}