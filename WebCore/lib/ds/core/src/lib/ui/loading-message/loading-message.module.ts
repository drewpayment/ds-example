import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingMessageComponent } from '@ds/core/ui/loading-message/loading-message.component';
import { downgradeComponent } from '@angular/upgrade/static';

@NgModule({
    declarations: [LoadingMessageComponent],
    imports: [
        CommonModule
    ],
    exports: [LoadingMessageComponent],
    entryComponents: [
        LoadingMessageComponent, // ONLY ADDED BECAUSE THIS COMPONENT IS DOWNGRADED FOR USE IN AJS
    ]
})
export class LoadingMessageModule { }

declare var angular: ng.IAngularStatic;
export module LoadingMessageDowngradeModule {
    export const MODULE_NAME = 'DowngradeLoadingMessageModule';
    export const MODULE_DEPENDENCIES = [];
    export const AjsModule = angular.module(MODULE_NAME, MODULE_DEPENDENCIES);
    AjsModule
        .directive('dsLoadingMessage',
            downgradeComponent({ component: LoadingMessageComponent }) as ng.IDirectiveFactory
        );
}