import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { windowProvider } from '@ds/core/core.tokens';
import { CoreModule } from '@ds/core/core.module';
import { OnboardingOutletComponent } from './onboarding-outlet.component';
import { Route, RouterModule, UrlSerializer } from '@angular/router';
import { LowerCaseUrlSerializer } from '@ds/core/utilities';
import { AjsEssUpgradesModule } from '../ajs-upgrades.module';
import { EssNgxOnboardingSidebarComponent, EssNgxOnboardingHomeComponent, WorkflowResolver, EssNgxOnboardingContactInfoComponent, EssNgxOnboardingDependentsComponent, EssNgxOnboardingEeocComponent, EssNgxOnboardingElectronicConsentsComponent, EssNgxOnboardingEmergencyContactComponent, EssNgxOnboardingPaymentPreferenceComponent, EssNgxOnboardingCompanyInfoComponent, EssNgxOnboardingDocumentComponent, EssNgxOnboardingLinkComponent, EssNgxOnboardingVideoComponent, EssNgxOnboardingFinalizeComponent, EssNgxOnboardingDefaultComponent, EssNgxOnboardingI9Component, EssNgxOnboardingW4AssistComponent, EssNgxOnboardingW4FederalComponent, EssNgxOnboardingW4StateComponent, EssNgxOnboardingEmployeeBioComponent, EssNgxOnboardingOtherInfoComponent } from './onboarding-ajs-upgrades';
import * as angular from 'angular';
import { WorkflowService } from 'apps/ds-ess/ajs/ui/workflow/workflow.service';
import { OnboardingService } from './onboarding.service';

export function workflowServiceProviderFactory(injector) {
    return injector.get(WorkflowService.SERVICE_NAME);
}

export const workflowServiceProvider = {
    provide: WorkflowService,
    useFactory: workflowServiceProviderFactory,
    deps: ['$injector']
};

export const resolve = {
    workflow: WorkflowResolver,
};

const routes: Route[] = [

    {
        path: 'onboarding',
        component: OnboardingOutletComponent,
        children: [
            {
                path: 'contactinfo',
                component: EssNgxOnboardingContactInfoComponent,
                resolve,
            },
            {
                path: 'dependents',
                component: EssNgxOnboardingDependentsComponent,
                resolve,
            },
            {
                path: 'eeoc',
                component: EssNgxOnboardingEeocComponent,
                resolve,
            },
            {
                path: 'electronicconsents',
                component: EssNgxOnboardingElectronicConsentsComponent,
                resolve,
            },
            {
                path: 'emergencycontact',
                component: EssNgxOnboardingEmergencyContactComponent,
                resolve,
            },
            {
                path: 'otherinfo',
                component: EssNgxOnboardingOtherInfoComponent,
                resolve,
            },
            {
                path: 'paymentpreference',
                component: EssNgxOnboardingPaymentPreferenceComponent,
                resolve,
            },
            {
                path: 'companyinfo',
                component: EssNgxOnboardingCompanyInfoComponent,
                resolve,
            },
            {
                path: 'document/:workflowTaskId',
                component: EssNgxOnboardingDocumentComponent,
                resolve,
            },
            {
                path: 'link/:workflowTaskId',
                component: EssNgxOnboardingLinkComponent,
                resolve,
            },
            {
                path: 'video/:workflowTaskId',
                component: EssNgxOnboardingVideoComponent,
                resolve,
            },
            {
                path: 'finalize',
                component: EssNgxOnboardingFinalizeComponent,
                resolve,
            },
            {
                path: 'default',
                component: EssNgxOnboardingDefaultComponent,
                resolve,
            },
            {
                path: 'i9',
                component: EssNgxOnboardingI9Component,
                resolve,
            },
            {
                path: 'w4',
                children: [
                    {
                        path: 'assist',
                        component: EssNgxOnboardingW4AssistComponent,
                        resolve,
                    },
                    {
                        path: 'federal',
                        component: EssNgxOnboardingW4FederalComponent,
                        resolve,
                    },
                    {
                        path: 'state',
                        component: EssNgxOnboardingW4StateComponent,
                        resolve,
                    },
                ]
            },
            {
                path: 'employeebio',
                component: EssNgxOnboardingEmployeeBioComponent,
                resolve,
            },
            {
                path: '',
                component: EssNgxOnboardingHomeComponent,
                resolve,
            }
        ]
    },
];

@NgModule({
    imports: [
        CommonModule,
        CoreModule,
        AjsEssUpgradesModule,

        RouterModule.forChild(routes),
    ],
    declarations: [
        OnboardingOutletComponent,
    ],
    exports: [
        RouterModule,
    ],
    providers: [
        windowProvider,
        // {
        //     provide: UrlSerializer,
        //     useClass: LowerCaseUrlSerializer,
        // },
        workflowServiceProvider,
        { provide: WorkflowResolver, useClass: WorkflowResolver },
        OnboardingService,
    ]
})
export class OnboardingEssModule {}
