import * as angular from 'angular';
import { downgradeComponent } from '@angular/upgrade/static';
import { DsContactAutocompleteComponent } from './ds-contact-autocomplete/ds-contact-autocomplete.component';

export module DsAutocompleteDowngradeAjsModule {
    const MODULE_NAME = "ds.autocomplete.downgrade";

    const MODULE_DEPENDENCIES = [
    ];

    export const AjsModule = angular.module(MODULE_NAME, MODULE_DEPENDENCIES);

    AjsModule
        .directive('dsContactAutocomplete', downgradeComponent({ component: DsContactAutocompleteComponent}));
}