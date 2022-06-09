import { NgModule } from '@angular/core';
import { RouterModule, Route, UrlSerializer, RouteReuseStrategy, Router } from '@angular/router';
import { AjsEssUpgradesModule } from './ajs-upgrades.module';
import { LowerCaseUrlSerializer } from '@ds/core/utilities';
import { EssNgxOnboardingSidebarComponent } from './onboarding/onboarding-ajs-upgrades';
import { EssRouteReuseStrategy } from './ess-route-reuse-strategy';

const routes: Route[] = [
    {
        path: 'onboarding',
        component: EssNgxOnboardingSidebarComponent,
        outlet: 'sidebar',
    },
];

@NgModule({
    declarations: [],
    imports: [
        AjsEssUpgradesModule,

        RouterModule.forRoot(routes)
    ],
    exports: [
        RouterModule
    ],
    providers: [
        {
            provide: RouteReuseStrategy,
            useClass: EssRouteReuseStrategy,
        },
        {
            provide: UrlSerializer,
            useClass: LowerCaseUrlSerializer
        },
    ]
})
export class AppRoutingModule {
    constructor() {}
}
