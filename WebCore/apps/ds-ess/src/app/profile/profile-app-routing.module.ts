import { NgModule } from '@angular/core';
import { Routes, RouterModule, UrlSerializer } from '@angular/router';
import { ProfileOutletComponent } from './profile-outlet/profile-outlet.component';
import { HeaderComponent } from '@ds/core/ui/menu-wrapper-header/header/header.component';
import { MenuWrapperHeaderModule } from '@ds/core/ui/menu-wrapper-header/menu-wrapper-header.module';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { LowerCaseUrlSerializer } from '@ds/core/utilities';
import { OnboardingGuard } from '../onboarding.guard';

export const ProfileRoutes: Routes = [
    {
        path: 'profile',
        children: [
            { path: '', component: ProfileOutletComponent },
            { path: '', component: HeaderComponent, outlet: 'header'},
            {
                path: '',
                component: SidebarComponent,
                outlet: 'sidebar'
            }
        ],
        canActivate: [OnboardingGuard],
    },
];

@NgModule({
    imports: [
        MenuWrapperHeaderModule,
        RouterModule.forChild(ProfileRoutes),
    ],
    providers: [
        {
            provide: UrlSerializer,
            useClass: LowerCaseUrlSerializer
        }
    ]
})
export class ProfileAppRoutingModule {}
