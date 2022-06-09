import { downgradeComponent, downgradeInjectable } from "@angular/upgrade/static";
import { OutletComponent } from "../src/app/outlet.component";
import { VendorMaintenanceComponent } from "../../../lib/ds/payroll/src/lib/vendor-maintenance/vendor-maintenance.component";
import { CheckStockTriggerComponent } from '../../../lib/ds/payroll/src/lib/check-stock/check-stock-trigger/check-stock-trigger.component';
import { EmployeeNotesComponent } from '../../../lib/ds/core/src/lib/employees/employee-notes/employee-notes/employee-notes.component';
import { ClientBankInfoSetupFormComponent, ClientBankRelateFormComponent } from '../../../lib/ds/core/src/lib/clients/client-bank-info';
import { BankSetupFormComponent } from '../../../lib/ds/core/src/lib/banks/bank-setup';
import { ReportRunnerComponent } from '@ds/reports/change-history';
import { TerminateEmployeeModalComponent } from '@ajs/employee/terminate-employee';
import { TerminateEmployeeModalTriggerComponent } from '@ajs/employee/terminate-employee/terminate-employee-modal/terminate-employee-modal-trigger.component';
import { WorkNumberModalTriggerComponent } from '@ds/core/popup/equifax/work-number/work-number-trigger.component';
import { EmployeeExitInterviewRequestComponent } from '../src/app/employee/employee-exit-interview-request/employee-exit-interview-request.component';
import { ClientStatisticsComponent } from '@ds/admin/client-statistics/client-statistics/client-statistics.component';
import { EmployeeApiService } from '@ds/core/employees/shared/employee-api.service';
import { AdvanceEnrollmentReportTriggerComponent } from '@ds/benefits/enrollments/advance-enrollment-report';
import { CopyPlansTriggerComponent } from '@ds/benefits/plans/copy-plans/copy-plans-trigger/copy-plans-trigger.component';
import { SignUpComponent } from '@ds/applicants/newuser/signup.component';
import { RecalcPointsDialogComponent, RecalcPointsTriggerComponent, UpdateBalanceDialogComponent, UpdateBalanceTriggerComponent } from '@ds/labor/automated-points';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { MenuWrapperToggleComponent } from '@ds/core/users/beta-features/menu-wrapper-toggle/menu-wrapper-toggle.component';

// import { EmployeeEEOCExportComponent } from '../src/app/employee/employee-eeoc/employee-eeoc-export.component';
import { EmployeeEEOCExportComponent } from 'apps/ds-source/src/app/employee/employee-eeoc/employee-eeoc-export.component';
import { EEOCLocationsTriggerComponent } from '../src/app/employee/employee-eeoc/eeoc-locations-modal/eeoc-locations-trigger.component';
import { AngularMaterialDateboxComponent } from '@ds/core/ui/angularMaterialDatebox/angular-material-datebox/angular-material-datebox.component';
import { LoadingMessageComponent } from '@ds/core/ui/loading-message/loading-message.component';
import { EmployeeTimecardComponent } from '@ds/core/employees/employee-timecard/employee-timecard.component';
import { AvatarComponent } from "@ds/core/ui/avatar/avatar.component";
declare var angular:ng.IAngularStatic;


export module DowngradeNgxModule {
    export const MODULE_NAME = 'DowngradeNgxModule';
    export const MODULE_DEPENDENCIES = [];

    export const AjsModule = angular.module(MODULE_NAME, MODULE_DEPENDENCIES);

    AjsModule
        .directive('ajsLoadingMessage',
            downgradeComponent({ component: LoadingMessageComponent }) as ng.IDirectiveFactory)
        .directive('dsTerminateEmployee',
            downgradeComponent({ component: TerminateEmployeeModalComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsVendorMaintenance',
            downgradeComponent({ component: VendorMaintenanceComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsEmployeeEeocExport',
            downgradeComponent({ component: EmployeeEEOCExportComponent}) as ng.IDirectiveFactory
        )
        .directive(
            'dsAngularMaterialDatebox',
            downgradeComponent({ component: AngularMaterialDateboxComponent}) as ng.IDirectiveFactory
        )
        .directive(
            'dsEeocLocationsTrigger',
            downgradeComponent({ component: EEOCLocationsTriggerComponent}) as ng.IDirectiveFactory
        )
        .directive(
            'dsReportRunner',
            downgradeComponent({ component: ReportRunnerComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsReportRunner',
            downgradeComponent({ component: ReportRunnerComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsCheckStockTrigger',
            downgradeComponent({ component: CheckStockTriggerComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsRecalcPoints',
        downgradeComponent({component: RecalcPointsDialogComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsRecalcPointsTrigger',
        downgradeComponent({component: RecalcPointsTriggerComponent}) as ng.IDirectiveFactory
        )
        .directive(
            'dsUpdateBalance',
            downgradeComponent({component: UpdateBalanceDialogComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsUpdateBalanceTrigger',
            downgradeComponent({component: UpdateBalanceTriggerComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsTerminateEmployeeTrigger',
            downgradeComponent({ component: TerminateEmployeeModalTriggerComponent }) as angular.IDirectiveFactory
        )
        .directive(
            'dsClientBankInfoSetupForm',
            downgradeComponent({ component: ClientBankInfoSetupFormComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsBankSetupForm',
            downgradeComponent({ component: BankSetupFormComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsClientBankRelateForm',
            downgradeComponent({ component: ClientBankRelateFormComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsClientStatistics',
            downgradeComponent({ component: ClientStatisticsComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsCopyPlansTrigger',
            downgradeComponent({ component: CopyPlansTriggerComponent }) as ng.IDirectiveFactory
        )
	    .directive(
            'dsEmployeeNotes',
            downgradeComponent({ component: EmployeeNotesComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsEmployeeExitInterviewRequest',
            downgradeComponent({ component: EmployeeExitInterviewRequestComponent }) as angular.IDirectiveFactory
        )
        .directive(
            'dsWorkNumberTrigger',
            downgradeComponent({ component: WorkNumberModalTriggerComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsAdvanceEnrollmentReportTrigger',
            downgradeComponent({ component: AdvanceEnrollmentReportTriggerComponent }) as ng.IDirectiveFactory
        )
		    .directive(
            'dsMenuWrapperToggle',
            downgradeComponent({ component: MenuWrapperToggleComponent }) as ng.IDirectiveFactory
        )
        .factory(
            EmployeeApiService.SERVICE_NAME,
            downgradeInjectable(EmployeeApiService) as any
        ).factory(
            DsStorageService.SERVICE_NAME,
            downgradeInjectable(DsStorageService) as any
        ).directive(
            'dsSignup',
            downgradeComponent({ component: SignUpComponent }) as ng.IDirectiveFactory
        ).directive(
          'dsAvatar',
          downgradeComponent({ component: AvatarComponent }) as ng.IDirectiveFactory
        );
}
