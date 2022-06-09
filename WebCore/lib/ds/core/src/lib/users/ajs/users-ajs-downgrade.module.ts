import * as angular from "angular";
import { downgradeComponent } from "@angular/upgrade/static";
import { AccessRuleManagerComponent } from "../access-rule-manager/access-rule-manager.component";

export module DsUserDowngradeModule {
    export const MODULE_NAME = 'ds.user.downgrade';
    export const AjsModule = angular.module(MODULE_NAME, []);

    AjsModule
        .directive(
            'dsAccessRuleManager',
            downgradeComponent({ component: AccessRuleManagerComponent }))
}