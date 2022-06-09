import * as angular from "angular";
import { DsCoreModule } from "@ajs/core/ds-core.module";
import { DsUiModule } from "@ajs/ui/ds-ui.module";
import { DsEssUiModule } from "../ui/ds-ess-ui.module";
import { CountryAndStateControllerBase } from "./base/country-state.controller";
import { EssHeaderController } from './header/header.controller';
import { DowngradeNgxModule } from '../downgrade.module.ajs';

/**
 * @ngdoc module
 * @module ds.ess.common
 */
export module DsEssCommonModule {
    export const AjsModule = angular.module('ds.ess.common',
    [
        DsCoreModule.AjsModule.name,
        DsUiModule.AjsModule.name,
        DsEssUiModule.AjsModule.name,
        DowngradeNgxModule.AjsModule.name,
    ]);

    AjsModule.controller(CountryAndStateControllerBase.CONTROLLER_NAME, CountryAndStateControllerBase)
        .component('essDefaultHeader', {
            controller: EssHeaderController,
            template: require('./header/header.html'),
            bindings: {
                'isOnboarding': '<'
            },
            controllerAs: '$ctrl'
        });
}
