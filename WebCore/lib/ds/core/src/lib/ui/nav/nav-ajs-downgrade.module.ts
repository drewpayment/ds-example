import * as angular from "angular";
import { downgradeComponent } from "@angular/upgrade/static";
import { DsNavMenuComponent } from "./ds-nav-menu/ds-nav-menu.component";
import { DsNavMainContentComponent } from "./ds-nav-main-content/ds-nav-main-content.component";
import { DsNavMainLegacyContentComponent } from "./ds-nav-main-legacy-content/ds-nav-main-legacy-content.component";
import { DsNavToolbarContentComponent } from "./ds-nav-toolbar-content/ds-nav-toolbar-content.component";
import { DsNavMenuContentComponent } from "./ds-nav-menu-content/ds-nav-menu-content.component";

export module DsUiNavAjsModule {
    export const AjsModule = angular
        .module('ds.ui.nav', [])
        .directive(
            'dsNavMenu',
            downgradeComponent({ component: DsNavMenuComponent }) as angular.IDirectiveFactory
        )
        .directive(
            'dsNavMainContent',
            downgradeComponent({ component: DsNavMainContentComponent }) as angular.IDirectiveFactory
        )
        .directive(
            'dsNavMainLegacyContent',
            downgradeComponent({ component: DsNavMainLegacyContentComponent }) as angular.IDirectiveFactory
        )
        .directive(
            'dsNavToolbarContent',
            downgradeComponent({ component: DsNavToolbarContentComponent }) as angular.IDirectiveFactory
        )
        .directive(
            'dsNavMenuContent',
            downgradeComponent({ component: DsNavMenuContentComponent }) as angular.IDirectiveFactory
        );
}