import { NgModule } from '@angular/core';
import { RouterModule, Route } from '@angular/router';
import { BenefitsOutletComponent } from './benefits-outlet.component';
import { HeaderComponent } from '@ds/core/ui/menu-wrapper-header/header/header.component';
import { EssNgxBenefitsInfoReviewComponent, EssNgxBenefitsEnrollmentComponent, EssNgxBenefitsPlansComponent,
    EssNgxBenefitsSummaryComponent, EssNgxBenefitsConfirmationComponent, EssNgxBenefitsHomeComponent } from './benefits-ajs-upgrades';
import { AjsEssUpgradesModule } from '../ajs-upgrades.module';
import { APP_CONFIG, AppConfig } from '@ds/core/app-config/app-config';

const routes: Route[] = [
    {
        path: 'benefits',
        
        children: [
            {
                path: 'info',
                component: EssNgxBenefitsInfoReviewComponent,
            },
            {
                path: 'enrollment',
                component: EssNgxBenefitsEnrollmentComponent,
            },
            {
                path: 'plans',
                component: EssNgxBenefitsPlansComponent,
            },
            {
                path: 'summary',
                component: EssNgxBenefitsSummaryComponent,
            },
            {
                path: 'confirmation',
                component: EssNgxBenefitsConfirmationComponent,
            },
            { 
                path: '', component: HeaderComponent, 
                outlet: 'header' 
            },
            {
                path: '',
                component: EssNgxBenefitsHomeComponent,
            },
        ]
    }
];

@NgModule({
    declarations: [
        BenefitsOutletComponent
    ],
    imports: [
        AjsEssUpgradesModule,
        RouterModule.forChild(routes)
    ],
    exports: [
        RouterModule
    ],
    entryComponents: [],
    providers: [
        {
            provide: APP_CONFIG,
            useFactory: (config: AppConfig) => config,
            deps: [ AppConfig ]
        }
    ]
})
export class BenefitsEssModule {
    constructor() {}
}
