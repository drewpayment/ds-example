import { Directive, ElementRef, Injector, Component, Injectable, OnInit, Inject } from '@angular/core';
import { UpgradeComponent } from '@angular/upgrade/static';
import { EssOnboardingHeaderComponent } from '../../../ajs/common/header/headerOnboarding.controller';
import { WorkflowSidebarComponent } from '../../../ajs/common/main-sidebar/workflow-sidebar.controller';
import { EssOnboardingHomeState } from '../../../ajs/onboarding/home/home.state';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, ActivatedRoute } from '@angular/router';
import { WorkflowService } from '../../../ajs/ui/workflow/workflow.service';
import { Observable } from 'rxjs';
import { DsEssOnboardingModule } from '../../../ajs/onboarding/ds-ess-onboarding.module';
import { EssOnboardingContactInfoState } from '../../../ajs/onboarding/contact-info/contact.state';
import { EssOnboardingDependentsState } from '../../../ajs/onboarding/basic-info/dependents.state';
import { EssOnboardingEeocState } from '../../../ajs/onboarding/basic-info/eeoc.state';
import { EssOnboardingElectronicConsentState } from '../../../ajs/onboarding/basic-info/electronic-consents.state';
import { EssOnboardingEmergencyContactState } from '../../../ajs/onboarding/basic-info/emergency-contact.state';
import { EssOnboardingOtherInfoState } from '../../../ajs/onboarding/basic-info/other-info.state';
import { EssOnboardingPaymentPreferenceState } from '../../../ajs/onboarding/basic-info/payment-preference.state';
import { EssOnboardingCompanyInfoState } from '../../../ajs/onboarding/company-info/company-info.state';
import { EssOnboardingDocumentState } from '../../../ajs/onboarding/custom-pages/document.state';
import { EssOnboardingLinkState } from '../../../ajs/onboarding/custom-pages/link.state';
import { EssOnboardingVideoState } from '../../../ajs/onboarding/custom-pages/video.state';
import { EssOnboardingFinalizeState } from '../../../ajs/onboarding/finalize/finalize.state';
import { EssOnboardingDefaultState } from '../../../ajs/onboarding/home/home.state.default';
import { EssOnboardingI9State } from '../../../ajs/onboarding/i9/i9.state';
import { EssOnboardingW4AssistState } from '../../../ajs/onboarding/w4/assist.state';
import { EssOnboardingW4FederalState } from '../../../ajs/onboarding/w4/federal.state';
import { EssOnboardingW4StateState } from '../../../ajs/onboarding/w4/state.state';
import { EssOnboardingEmployeeBioState } from '../../../ajs/onboarding/basic-info/employee-bio.state';
import * as angular from 'angular';

@Directive({selector: '[essNgxOnboardingHeader]'})
export class EssNgxOnboardingHeaderDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingHeaderComponent.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-header',
    template: `<div essNgxOnboardingHeader></div>`
})
export class EssNgxOnboardingHeaderComponent {}

@Directive({selector: '[essNgxOnboardingSidebar]'})
export class EssNgxOnboardingSidebarDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(WorkflowSidebarComponent.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-sidebar',
    template: `<div essNgxOnboardingSidebar></div>`
})
export class EssNgxOnboardingSidebarComponent {}

@Directive({selector: '[essNgxOnboardingHome]'})
export class EssNgxOnboardingHomeDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingHomeState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-home',
    template: `<div essNgxOnboardingHome></div>`
})
export class EssNgxOnboardingHomeComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Injectable()
export class WorkflowResolver implements Resolve<Promise<any>> {

    constructor(private workflowService: WorkflowService) {}
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
        return new Observable(ob => {
            this.workflowService.getUserWorkflowPromise()
                .then(workflow => {
                    ob.next(workflow);
                    ob.complete();
                })
                .catch(err => {
                    ob.error(err);
                    ob.complete();
                });
        });
    }

}

@Directive({selector: '[essNgxOnboardingContactInfo]'})
export class EssNgxOnboardingContactInfoDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingContactInfoState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-contact-info',
    template: `<div essNgxOnboardingContactInfo></div>`
})
export class EssNgxOnboardingContactInfoComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingDependents]'})
export class EssNgxOnboardingDependentsDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingDependentsState.COMPONENTS_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-dependents',
    template: `<div essNgxOnboardingDependents></div>`
})
export class EssNgxOnboardingDependentsComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingEeoc]'})
export class EssNgxOnboardingEeocDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingEeocState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-eeoc',
    template: `<div essNgxOnboardingEeoc></div>`
})
export class EssNgxOnboardingEeocComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingElectronicConsents]'})
export class EssNgxOnboardingElectronicConsentsDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingElectronicConsentState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-electronic-consents',
    template: `<div essNgxOnboardingElectronicConsents></div>`
})
export class EssNgxOnboardingElectronicConsentsComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingEmergencyContact]'})
export class EssNgxOnboardingEmergencyContactDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingEmergencyContactState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-emergency-contact',
    template: `<div essNgxOnboardingEmergencyContact></div>`
})
export class EssNgxOnboardingEmergencyContactComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingOtherInfo]'})
export class EssNgxOnboardingOtherInfoDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingOtherInfoState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-other-info',
    template: `<div essNgxOnboardingOtherInfo></div>`
})
export class EssNgxOnboardingOtherInfoComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingPaymentPreference]'})
export class EssNgxOnboardingPaymentPreferenceDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingPaymentPreferenceState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-payment-preference',
    template: `<div essNgxOnboardingPaymentPreference></div>`
})
export class EssNgxOnboardingPaymentPreferenceComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingCompanyInfo]'})
export class EssNgxOnboardingCompanyInfoDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingCompanyInfoState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-company-info',
    template: `<div essNgxOnboardingCompanyInfo></div>`
})
export class EssNgxOnboardingCompanyInfoComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingDocument]'})
export class EssNgxOnboardingDocumentDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingDocumentState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-document',
    template: `<div essNgxOnboardingDocument></div>`
})
export class EssNgxOnboardingDocumentComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingLink]'})
export class EssNgxOnboardingLinkDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingLinkState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-link',
    template: `<div essNgxOnboardingLink></div>`
})
export class EssNgxOnboardingLinkComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingVideo]'})
export class EssNgxOnboardingVideoDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingVideoState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-video',
    template: `<div essNgxOnboardingVideo></div>`
})
export class EssNgxOnboardingVideoComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingFinalize]'})
export class EssNgxOnboardingFinalizeDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingFinalizeState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-finalize',
    template: `<div essNgxOnboardingFinalize></div>`
})
export class EssNgxOnboardingFinalizeComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingDefault]'})
export class EssNgxOnboardingDefaultDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingDefaultState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-default',
    template: `<div essNgxOnboardingDefault></div>`
})
export class EssNgxOnboardingDefaultComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingI9]'})
export class EssNgxOnboardingI9Directive extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingI9State.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-i9',
    template: `<div essNgxOnboardingI9></div>`
})
export class EssNgxOnboardingI9Component {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingW4Assist]'})
export class EssNgxOnboardingW4AssistDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingW4AssistState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-w4-assist',
    template: `<div essNgxOnboardingW4Assist></div>`
})
export class EssNgxOnboardingW4AssistComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingW4Federal]'})
export class EssNgxOnboardingW4FederalDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingW4FederalState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-w4-federal',
    template: `<div essNgxOnboardingW4Federal></div>`
})
export class EssNgxOnboardingW4FederalComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingW4State]'})
export class EssNgxOnboardingW4StateDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingW4StateState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-w4-state',
    template: `<div essNgxOnboardingW4State></div>`
})
export class EssNgxOnboardingW4StateComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}

@Directive({selector: '[essNgxOnboardingEmployeeBio]'})
export class EssNgxOnboardingEmployeeBioDirective extends UpgradeComponent {
    constructor(el: ElementRef, injector: Injector) {
        super(EssOnboardingEmployeeBioState.COMPONENT_NAME, el, injector);
    }
}

@Component({
    selector: 'ess-ngx-onboarding-employee-bio',
    template: `<div essNgxOnboardingEmployeeBio></div>`
})
export class EssNgxOnboardingEmployeeBioComponent {
    constructor(route: ActivatedRoute) {
        const app = angular.module(DsEssOnboardingModule.AjsModule.name);
        app.factory('myWorkflow', function() {
            return route.snapshot.data.workflow;
        });
    }
}
