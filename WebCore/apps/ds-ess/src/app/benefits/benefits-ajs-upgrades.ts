import { Directive, ElementRef, Injector, Component, OnInit } from '@angular/core';
import { UpgradeComponent } from '@angular/upgrade/static';
import { EssBenefitsHomeState } from '../../../ajs/benefits/home/home.state';
import { EssBenefitsInfoState } from '../../../ajs/benefits/info-review/info-review.state';
import { EssBenefitsEnrollmentState } from '../../../ajs/benefits/enrollment/enrollment.state';
import { EssBenefitsPlansState } from '../../../ajs/benefits/plans/plans.state';
import { EssBenefitsSummaryState } from '../../../ajs/benefits/summary/summary.state';
import { EssBenefitsConfirmationState } from '../../../ajs/benefits/confirmation/confirmation.state';
import { EssOnboardingContactInfoState } from 'apps/ds-ess/ajs/onboarding/contact-info/contact.state';
import { Router } from '@angular/router';
import { DsBenefitsNavigationService, EnrollmentStep } from "@ajs/benefits/benefits-navigation.service";

function navigateComponent(router: Router, step: EnrollmentStep){
    switch(step) {
        case EnrollmentStep.Home        : router.navigate(['/benefits']); break;
        case EnrollmentStep.Info        : router.navigate(['/benefits/info']); break;
        case EnrollmentStep.Plans       : router.navigate(['/benefits/plans']); break;
        case EnrollmentStep.Enrollment  : router.navigate(['/benefits/enrollment']); break;
        case EnrollmentStep.Summary     : router.navigate(['/benefits/summary']); break;
        case EnrollmentStep.Confirmation : router.navigate(['/benefits/confirmation']); break;
    }
}

export function DsBenefitsNavigationServiceProviderFactory($injector:any) {
    return $injector.get(DsBenefitsNavigationService.SERVICE_NAME);    
}
export const EssNgxBenefitsNavigationProvider = {
    provide: DsBenefitsNavigationService,
    useFactory: DsBenefitsNavigationServiceProviderFactory,
    deps: ['$injector']
};

@Directive({ selector: '[essNgxBenefitsHome]' })
export class EssNgxBenefitsHomeDirective extends UpgradeComponent {
    constructor(elRef: ElementRef, injector: Injector) {
        super(EssBenefitsHomeState.COMPONENT_NAME, elRef, injector);
    }
}

@Component({
    selector: 'ess-ngx-benefits-home',
    template: `<div essNgxBenefitsHome></div>`
})
export class EssNgxBenefitsHomeComponent implements OnInit {
    constructor(private router: Router, private nav: DsBenefitsNavigationService) {}

    ngOnInit() {
        this.nav.hookToRedirection((step) => {
            navigateComponent(this.router,step);
        });
    }
}

@Directive({selector: '[essNgxBenefitsInfoReview]'})
export class EssNgxBenefitsInfoReviewDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssBenefitsInfoState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-benefits-info',
    template: `<div essNgxBenefitsInfoReview></div>`,
})
export class EssNgxBenefitsInfoReviewComponent implements OnInit {
    constructor(private router: Router, private nav: DsBenefitsNavigationService) {}

    ngOnInit() {
        this.nav.hookToRedirection((step) => {
            navigateComponent(this.router,step);
        });
    }
}

@Directive({selector: '[essNgxBenefitsEnrollment]'})
export class EssNgxBenefitsEnrollmentDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssBenefitsEnrollmentState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-benefits-enrollment',
    template: `<div essNgxBenefitsEnrollment></div>`
})
export class EssNgxBenefitsEnrollmentComponent implements OnInit {
    constructor(private router: Router, private nav: DsBenefitsNavigationService) {}

    ngOnInit() {
        this.nav.hookToRedirection((step) => {
            navigateComponent(this.router,step);
        });
    }
}

@Directive({selector: '[essNgxBenefitsPlans]'})
export class EssNgxBenefitsPlansDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssBenefitsPlansState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-benefits-plans',
    template: `<div essNgxBenefitsPlans></div>`
})
export class EssNgxBenefitsPlansComponent implements OnInit {
    constructor(private router: Router, private nav: DsBenefitsNavigationService) {}

    ngOnInit() {
        this.nav.hookToRedirection((step) => {
            navigateComponent(this.router,step);
        });
    }
}

@Directive({selector: '[essNgxBenefitsSummary]'})
export class EssNgxBenefitsSummaryDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssBenefitsSummaryState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-benefits-summary',
    template: `<div essNgxBenefitsSummary></div>`
})
export class EssNgxBenefitsSummaryComponent implements OnInit {
    constructor(private router: Router, private nav: DsBenefitsNavigationService) {}

    ngOnInit() {
        this.nav.hookToRedirection((step) => {
            navigateComponent(this.router,step);
        });
    }
}

@Directive({selector: '[essNgxBenefitsConfirmation]'})
export class EssNgxBenefitsConfirmationDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssBenefitsConfirmationState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-benefits-confirmation',
    template: `<div essNgxBenefitsConfirmation></div>`
})
export class EssNgxBenefitsConfirmationComponent implements OnInit {
    constructor(private router: Router, private nav: DsBenefitsNavigationService) {}

    ngOnInit() {
        this.nav.hookToRedirection((step) => {
            navigateComponent(this.router,step);
        });
    }
}
