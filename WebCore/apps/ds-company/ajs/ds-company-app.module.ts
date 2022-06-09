import * as angular from 'angular';

import './vendor';

import { DsCoreModule } from '@ajs/core/ds-core.module';
import { DsUiModule } from '@ajs/ui/ds-ui.module';
import { DsLocationModule } from '@ajs/location/ds-location.mod';
import { DsSelectorModule } from '@ajs/selector/selector.mod';
import { DsImageModalModule } from '@ajs/core/ds-resource/image-modal/image-modal.module';
import { DsClientEssOptionsModule } from '@ajs/client/ess-options/client-essOption.module';
import { DsCompanyAppAjsRun } from './ds-company-app.run';
import { DsCompanyAppAjsConfig } from './ds-company-app.config';
import { DsCompanyAppDefaultState } from './ds-company-app.state';
import { DsCompanyLaborModule } from './labor/ds-company-labor.module';
import { DsCompanyResourceModule } from '@ajs/company-resource/ds-company-resource.module';
import { DsUiHelpModule } from '@ajs/ui/help/ds-ui-help.module';
import { DsCompanyDowngradeNgxModule } from './downgrade.module.ajs';
import { CompetenciesDowngradeModule } from 'lib/ds/performance/src/lib/competencies/competencies.downgrade.module';
import { HeaderController } from './header/header.controller';
import { DsEmployeeAddModule } from "@ajs/employee/add-employee/ds-employee-add.module";

export module DsCompanyAppAjsModule {
    export const AjsModule = angular.module('ds.company.app',
        [
            'ui.router',
            'ui.bootstrap',
            'presence',
            'angularMoment',
            'ngFileUpload',

            DsCompanyLaborModule.AjsModule.name,
            DsCompanyResourceModule.AjsModule.name,
            DsCoreModule.AjsModule.name,
            DsUiModule.AjsModule.name,
            DsUiHelpModule.AjsModule.name,
            DsLocationModule.AjsModule.name,
            DsSelectorModule.AjsModule.name,
            DsImageModalModule.AjsModule.name,
            DsClientEssOptionsModule.AjsModule.name,
            CompetenciesDowngradeModule.AjsModule.name,
            DsCompanyDowngradeNgxModule.AjsModule.name,
            DsEmployeeAddModule.AjsModule.name,
        ]
    );

    // components
    // AjsModule
    //     .component('dsCompanyHeader', {
    //         require: 'E',
    //         controller: HeaderController,
    //         template: require('./header/header.html'),
    //     });

    // config
    AjsModule
        .config(DsCompanyAppAjsConfig)
        .run(DsCompanyAppAjsRun);

    // services, directives, etc
    
    // ui-router states
    AjsModule
        .config(DsCompanyAppDefaultState.$ng_config())
        .config(DsCompanyAppDefaultState.$config());
}

export function bootstrap(el: HTMLElement) {
    return angular.bootstrap(el, [DsCompanyAppAjsModule.AjsModule.name]);
}
