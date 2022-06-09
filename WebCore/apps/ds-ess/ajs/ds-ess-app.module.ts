import * as angular from 'angular';

import './vendor';
import { DsClientEssOptionsModule } from '@ajs/client/ess-options/client-essOption.module';
import { DsImageModalModule } from '@ajs/core/ds-resource/image-modal/image-modal.module';
import { DsEssBenefitsModule } from './benefits/ds-ess-benefits.module';
import { DsEssUiModule } from './ui/ds-ess-ui.module';
import { DsEssCommonModule } from './common/ds-ess-common.module';
import { DsEssLeaveModule } from './leave/ds-ess-leave.module';
import { DsEssProfileModule } from './profile/ds-ess-profile.module';
import { DsEssOnboardingModule } from './onboarding/ds-ess-onboarding.module';
import { DowngradeNgxModule } from '../ajs/downgrade.module.ajs';
import { DsEssAppAjsModuleConfig } from "./ds-ess-app.config";
import { DsEssAppAjsModuleRun } from "./ds-ess-app.run";
import { DsGoogleCharts } from "@ajs/google-charts/google-charts.module";
import { DsLaborCompanyModule } from "@ajs/labor/company/ds-labor-company.module";

/**
 * @ngdoc module
 * @module ds.ess.app
 *
 * @description
 * Primary ESS angular application module in which all required dependencies will be built.
 */
export module DsEssAppAjsModule {
    export const AjsModule = angular.module('ds.ess.app', [
        'presence',
        'ui.bootstrap',
        'ui.router',
        DsEssUiModule.AjsModule.name,
        DsEssCommonModule.AjsModule.name,
        DsEssProfileModule.AjsModule.name,
        DsEssBenefitsModule.AjsModule.name,
        DsEssOnboardingModule.AjsModule.name,
        DsEssLeaveModule.AjsModule.name,
        DsClientEssOptionsModule.AjsModule.name,
        DsImageModalModule.AjsModule.name,
        DsLaborCompanyModule.AjsModule.name,
        DsGoogleCharts.AjsModule.name,
        DowngradeNgxModule.AjsModule.name
    ]);
    
    AjsModule
        .config(DsEssAppAjsModuleConfig)
        .run(DsEssAppAjsModuleRun);
}
