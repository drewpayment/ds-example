import { downgradeComponent } from '@angular/upgrade/static';
import * as angular from 'angular';
import { NgModule } from "@angular/core";
import { DsTooltipComponent } from '../ds-tooltip.component';


@NgModule({
    imports: [
        
    ],
    entryComponents: [
        
    ]
})
export class DsTooltipDowngradeModule {
    constructor() {}
}

export module DsTooltipAjsModule {
    export const AjsModule = angular
        .module('ds.ui.tooltip', [])
        .directive(
            'dsTooltip',
            downgradeComponent({ component: DsTooltipComponent }) as angular.IDirectiveFactory
        );
}