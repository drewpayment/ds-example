
import { Directive, ElementRef, Injector, Component, NgModule, forwardRef, Input } from '@angular/core';
import { UpgradeComponent } from '@angular/upgrade/static';
import { EssNgxBenefitsHomeDirective, EssNgxBenefitsHomeComponent, EssNgxBenefitsInfoReviewDirective,
    EssNgxBenefitsInfoReviewComponent, EssNgxBenefitsEnrollmentDirective, EssNgxBenefitsEnrollmentComponent,
    EssNgxBenefitsPlansDirective, EssNgxBenefitsPlansComponent, EssNgxBenefitsSummaryDirective,
    EssNgxBenefitsSummaryComponent, EssNgxBenefitsConfirmationDirective,
    EssNgxBenefitsConfirmationComponent, EssNgxBenefitsNavigationProvider } from './benefits/benefits-ajs-upgrades';
import { EssNgxOnboardingHeaderDirective, EssNgxOnboardingHeaderComponent,
    EssNgxOnboardingSidebarDirective, EssNgxOnboardingSidebarComponent, EssNgxOnboardingHomeDirective, EssNgxOnboardingHomeComponent, EssNgxOnboardingContactInfoDirective, EssNgxOnboardingContactInfoComponent, EssNgxOnboardingDependentsDirective, EssNgxOnboardingDependentsComponent, EssNgxOnboardingEeocDirective, EssNgxOnboardingEeocComponent, EssNgxOnboardingElectronicConsentsDirective, EssNgxOnboardingElectronicConsentsComponent, EssNgxOnboardingEmergencyContactDirective, EssNgxOnboardingEmergencyContactComponent, EssNgxOnboardingOtherInfoDirective, EssNgxOnboardingOtherInfoComponent, EssNgxOnboardingPaymentPreferenceDirective, EssNgxOnboardingPaymentPreferenceComponent, EssNgxOnboardingCompanyInfoDirective, EssNgxOnboardingCompanyInfoComponent, EssNgxOnboardingDocumentDirective, EssNgxOnboardingDocumentComponent, EssNgxOnboardingLinkDirective, EssNgxOnboardingLinkComponent, EssNgxOnboardingVideoDirective, EssNgxOnboardingVideoComponent, EssNgxOnboardingFinalizeDirective, EssNgxOnboardingFinalizeComponent, EssNgxOnboardingDefaultDirective, EssNgxOnboardingDefaultComponent, EssNgxOnboardingI9Directive, EssNgxOnboardingI9Component, EssNgxOnboardingW4AssistDirective, EssNgxOnboardingW4AssistComponent, EssNgxOnboardingW4FederalComponent, EssNgxOnboardingW4FederalDirective, EssNgxOnboardingW4StateDirective, EssNgxOnboardingW4StateComponent, EssNgxOnboardingEmployeeBioDirective, EssNgxOnboardingEmployeeBioComponent } from './onboarding/onboarding-ajs-upgrades';

// tslint:disable-next-line: directive-selector
@Directive({ selector: '[ngxEssHeader]' })
export class NgxEssHeaderDirective extends UpgradeComponent {
    @Input() isOnboarding!: boolean;
    constructor(elementRef: ElementRef, injector: Injector) {
        super('essDefaultHeader', elementRef, injector);
    }
}

@Component({
    selector: 'ngx-ess-header',
    template: `<div ngxEssHeader></div>`
})
export class NgxEssHeaderComponent {}

@Directive({ selector: '[ngxEssMsg]' })
export class NgxEssMsgDirective extends UpgradeComponent {
    constructor(elementRef: ElementRef, injector: Injector) {
        super('essMsgController', elementRef, injector);
    }
}

@Component({
    selector: 'ngx-ess-msg',
    template: `<div ngxEssMsg></div>`
})
export class NgxEssMsgComponent {}


@NgModule({
    declarations: [
        NgxEssHeaderDirective,
        NgxEssHeaderComponent,
        NgxEssMsgDirective,
        NgxEssMsgComponent,
        EssNgxBenefitsHomeDirective,
        EssNgxBenefitsHomeComponent,
        EssNgxBenefitsInfoReviewDirective,
        EssNgxBenefitsInfoReviewComponent,
        EssNgxBenefitsEnrollmentDirective,
        EssNgxBenefitsEnrollmentComponent,
        EssNgxBenefitsPlansDirective,
        EssNgxBenefitsPlansComponent,
        EssNgxBenefitsSummaryDirective,
        EssNgxBenefitsSummaryComponent,
        EssNgxBenefitsConfirmationDirective,
        EssNgxBenefitsConfirmationComponent,
        EssNgxOnboardingHeaderDirective,
        EssNgxOnboardingHeaderComponent,
        EssNgxOnboardingSidebarDirective,
        EssNgxOnboardingSidebarComponent,
        EssNgxOnboardingHomeDirective,
        EssNgxOnboardingHomeComponent,
        EssNgxOnboardingContactInfoDirective,
        EssNgxOnboardingContactInfoComponent,
        EssNgxOnboardingDependentsDirective,
        EssNgxOnboardingDependentsComponent,
        EssNgxOnboardingEeocDirective,
        EssNgxOnboardingEeocComponent,
        EssNgxOnboardingElectronicConsentsDirective,
        EssNgxOnboardingElectronicConsentsComponent,
        EssNgxOnboardingEmergencyContactDirective,
        EssNgxOnboardingEmergencyContactComponent,
        EssNgxOnboardingOtherInfoDirective,
        EssNgxOnboardingOtherInfoComponent,
        EssNgxOnboardingPaymentPreferenceDirective,
        EssNgxOnboardingPaymentPreferenceComponent,
        EssNgxOnboardingCompanyInfoDirective,
        EssNgxOnboardingCompanyInfoComponent,
        EssNgxOnboardingDocumentDirective,
        EssNgxOnboardingDocumentComponent,
        EssNgxOnboardingLinkDirective,
        EssNgxOnboardingLinkComponent,
        EssNgxOnboardingVideoDirective,
        EssNgxOnboardingVideoComponent,
        EssNgxOnboardingFinalizeDirective,
        EssNgxOnboardingFinalizeComponent,
        EssNgxOnboardingDefaultDirective,
        EssNgxOnboardingDefaultComponent,
        EssNgxOnboardingI9Directive,
        EssNgxOnboardingI9Component,
        EssNgxOnboardingW4AssistDirective,
        EssNgxOnboardingW4AssistComponent,
        EssNgxOnboardingW4FederalDirective,
        EssNgxOnboardingW4FederalComponent,
        EssNgxOnboardingW4StateDirective,
        EssNgxOnboardingW4StateComponent,
        EssNgxOnboardingEmployeeBioDirective,
        EssNgxOnboardingEmployeeBioComponent,
    ],
    imports: [],
    exports: [
        NgxEssHeaderDirective,
        NgxEssHeaderComponent,
        NgxEssMsgDirective,
        NgxEssMsgComponent,
        EssNgxBenefitsHomeDirective,
        EssNgxBenefitsHomeComponent,
        EssNgxBenefitsInfoReviewDirective,
        EssNgxBenefitsInfoReviewComponent,
        EssNgxBenefitsEnrollmentDirective,
        EssNgxBenefitsEnrollmentComponent,
        EssNgxBenefitsPlansDirective,
        EssNgxBenefitsPlansComponent,
        EssNgxBenefitsSummaryDirective,
        EssNgxBenefitsSummaryComponent,
        EssNgxBenefitsConfirmationDirective,
        EssNgxBenefitsConfirmationComponent,
        EssNgxOnboardingHeaderDirective,
        EssNgxOnboardingHeaderComponent,
        EssNgxOnboardingSidebarDirective,
        EssNgxOnboardingSidebarComponent,
        EssNgxOnboardingHomeDirective,
        EssNgxOnboardingHomeComponent,
        EssNgxOnboardingContactInfoDirective,
        EssNgxOnboardingContactInfoComponent,
        EssNgxOnboardingDependentsDirective,
        EssNgxOnboardingDependentsComponent,
        EssNgxOnboardingEeocDirective,
        EssNgxOnboardingEeocComponent,
        EssNgxOnboardingElectronicConsentsDirective,
        EssNgxOnboardingElectronicConsentsComponent,
        EssNgxOnboardingEmergencyContactDirective,
        EssNgxOnboardingEmergencyContactComponent,
        EssNgxOnboardingOtherInfoDirective,
        EssNgxOnboardingOtherInfoComponent,
        EssNgxOnboardingPaymentPreferenceDirective,
        EssNgxOnboardingPaymentPreferenceComponent,
        EssNgxOnboardingCompanyInfoDirective,
        EssNgxOnboardingCompanyInfoComponent,
        EssNgxOnboardingDocumentDirective,
        EssNgxOnboardingDocumentComponent,
        EssNgxOnboardingLinkDirective,
        EssNgxOnboardingLinkComponent,
        EssNgxOnboardingVideoDirective,
        EssNgxOnboardingVideoComponent,
        EssNgxOnboardingFinalizeDirective,
        EssNgxOnboardingFinalizeComponent,
        EssNgxOnboardingDefaultDirective,
        EssNgxOnboardingDefaultComponent,
        EssNgxOnboardingI9Directive,
        EssNgxOnboardingI9Component,
        EssNgxOnboardingW4AssistDirective,
        EssNgxOnboardingW4AssistComponent,
        EssNgxOnboardingW4FederalDirective,
        EssNgxOnboardingW4FederalComponent,
        EssNgxOnboardingW4StateDirective,
        EssNgxOnboardingW4StateComponent,
        EssNgxOnboardingEmployeeBioDirective,
        EssNgxOnboardingEmployeeBioComponent,
    ],
    providers: [
        EssNgxBenefitsNavigationProvider,
    ]
})
export class AjsEssUpgradesModule {}
