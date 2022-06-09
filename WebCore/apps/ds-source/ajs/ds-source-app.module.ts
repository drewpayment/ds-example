import * as angular from "angular";

import "./vendor";

//dominion ajs modules
import { DsCoreModule } from "@ajs/core/ds-core.module";
import { DsUiModule } from "@ajs/ui/ds-ui.module";
import { DsSelectorModule } from "@ajs/selector/selector.mod";
import { DsW2Module } from "@ajs/w2/ds-w2.mod";
import { DsLaborPunchModule } from "@ajs/labor/punch/ds-labor-punch.module";
import { DsEmployeeModule } from "@ajs/employee/ds-employee.module";
import { DsBenefitsModule } from "@ajs/benefits/ds-benefits.module";
import { DsClientCostCentersModule } from "@ajs/client/cost-centers/client-cost-center.module";
import { DsClientGroups } from "@ajs/client/groups/client-group.module";
import { DsDuplicateClientModule } from "@ajs/client/duplicateClient/client-duplicateClient.module";
import { DsClientBankInfoModule } from "@ajs/client/bank-info/client-bank-info.module";
import { DsClientEssOptionsModule } from "@ajs/client/ess-options/client-essOption.module";
import { DsClientRatesModule } from "@ajs/client/Rates/client-rate.module";
import { DsPaygradeModule } from "@ajs/paygrade/paygrade.mod";
import { DsCheckstockModule } from "@ajs/checkstock/ds-checkstock.module";
import { DsReportScheduleModule } from "@ajs/reportscheduling/ds-reportschedule.module";
import { DsDisciplinesModule } from "@ajs/disciplines/discipline-modal.module";
import { DsEmployeeSearchModule } from "@ajs/employee/search/ds-employee-search.module";
import { DsUiHelpModule } from "@ajs/ui/help/ds-ui-help.module";
import { DsLegacyWebFormsModule } from "@ajs/ui/legacy-web-forms/legacy-web-forms.module";
import { DsJobProfilesBasicModule } from "@ajs/job-profiles/basic/ds-job-profile.module";
import { DsExternalApiModule } from "@ajs/ds-external-api/ds-external-api.mod";
import { DsBenefitsEnrollmentReportModule } from "@ajs/benefits/enrollment-report/ds-benefits-enrollment-report.module";
import { DsApplicantModule } from "@ajs/applicantTracking/ds-applicant.module";
import { DsApplicantQuestionsModule } from "@ajs/applicantTracking/application/applicantquestions.module";
import { DsAppHistoryModule } from "@ajs/applicantTracking/app-history/app-history.module";
import { DsApplicationDisclaimerModule } from "@ajs/applicantTracking/disclaimer/disclaimer.module";
import { DsApplicantTrackingHomeModule } from "@ajs/applicantTracking/home/applicanttrackinghome.module";
import { DsNotificationPreferenceModule } from "@ajs/notification/preferences/notification-preference.module";

//app specific modules
import { DsSourceAppModuleConfig } from "./ds-source-app.config";
import { DsSourceAppModuleRun } from "./ds-source-app.run";
import { DsEmployeeAddModule } from "@ajs/employee/add-employee/ds-employee-add.module";
import { DsEmployeeHireModule } from "@ajs/employee/hiring/ds-employee-hire.module";
import { DsLaborCompanyModule } from "@ajs/labor/company/ds-labor-company.module";
import { DsLaborEmployeeSetupModule } from "@ajs/labor/clock-employee/labor-clockEmployee.module";
import { DsPayrollHistoryModule } from "@ajs/payroll/history/payroll-history.module";
import { DsBenefitPlanModule } from "@ajs/client/benefit-plan/client-benefit-plan.module";
import { DowngradeNgxModule } from "./downgrade.module.ajs";
import { DsNewUserModule } from "@ajs/applicantTracking/newuser/applicant-newUser.module";
import { DsClientTopicsModule } from "@ajs/client/topics/client-topic.module";
import { DsPayrollEmployeeModule } from "@ajs/payroll/employee/employee.module";
import { DsGoogleCharts } from "@ajs/google-charts/google-charts.module";
import { DsUserDowngradeModule } from "@ds/core/users";
import { DsAuthenticationModule } from '@ajs/authentication/ds-authentication.module';
import { DsUiNavAjsModule } from '@ds/core/ui/nav/nav-ajs-downgrade.module';
import { DsAppConfigDowngradeModule } from '@ds/core/app-config';

export module DsSourceAppAjsModule {
    const MODULE_NAME = "ds.source.app";

    const MODULE_DEPENDENCIES = [
        "ui.bootstrap",
        "ui.router",
        "ngFileUpload",
        DsUserDowngradeModule.AjsModule.name,
        DsAppConfigDowngradeModule.AjsModule.name,
        DsCoreModule.AjsModule.name,
        DsSelectorModule.AjsModule.name,
        DsW2Module.AjsModule.name,
        DsLaborPunchModule.AjsModule.name,
        DsBenefitsModule.AjsModule.name,
        DsClientCostCentersModule.AjsModule.name,
        DsClientGroups.AjsModule.name,
        DsDuplicateClientModule.AjsModule.name,
        DsClientBankInfoModule.AjsModule.name,
        DsClientEssOptionsModule.AjsModule.name,
        DsClientRatesModule.AjsModule.name,
        DsPaygradeModule.AjsModule.name,
        DsCheckstockModule.AjsModule.name,
        DsReportScheduleModule.AjsModule.name,
        DsDisciplinesModule.AjsModule.name,
        DsEmployeeModule.AjsModule.name,
        DsEmployeeSearchModule.AjsModule.name,
        DsUiModule.AjsModule.name,
        DsUiHelpModule.AjsModule.name,
        DsApplicantModule.AjsModule.name,
        DsApplicantQuestionsModule.AjsModule.name,
        DsAppHistoryModule.AjsModule.name,
        DsExternalApiModule.AjsModule.name,
        DsApplicationDisclaimerModule.AjsModule.name,
        DsApplicantTrackingHomeModule.AjsModule.name,
        DsLegacyWebFormsModule.AjsModule.name,
        DsJobProfilesBasicModule.AjsModule.name,
        DsBenefitsEnrollmentReportModule.AjsModule.name,
        DsEmployeeAddModule.AjsModule.name,
        DsEmployeeHireModule.AjsModule.name,
        DsLaborCompanyModule.AjsModule.name,
        DsLaborEmployeeSetupModule.AjsModule.name,
        DsPayrollHistoryModule.AjsModule.name,
        DsBenefitPlanModule.AjsModule.name,
        DowngradeNgxModule.AjsModule.name,
        DsNewUserModule.AjsModule.name,
        DsClientTopicsModule.AjsModule.name,
        DsNotificationPreferenceModule.AjsModule.name,
        DsPayrollEmployeeModule.AjsModule.name,
        DsUiNavAjsModule.AjsModule.name,
        DsGoogleCharts.AjsModule.name,
        DsAuthenticationModule.AjsModule.name
    ];

    export const AjsModule = angular.module(MODULE_NAME, MODULE_DEPENDENCIES);

    AjsModule.config(DsSourceAppModuleConfig.$instance);
    AjsModule.run(DsSourceAppModuleRun.factory());
}
