import { downgradeComponent } from '@angular/upgrade/static';
import * as angular from 'angular';
import { NgModule } from "@angular/core";
import { DsProgressComponent, DsProgressModule } from '../';


@NgModule({
    imports: [
        DsProgressModule
    ],
    entryComponents: [
        DsProgressComponent
    ]
})
export class DsProgressDowngradeModule {
    constructor() {}
}

export module DsProgressAjsModule {
    export const AjsModule = angular
        .module('ds.ui.progress', [])
        .directive(
            'dsProgress',
            downgradeComponent({ component: DsProgressComponent }) as angular.IDirectiveFactory
        );
}