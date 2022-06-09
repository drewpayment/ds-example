import { NgModule, Injector } from '@angular/core';
import {
  TimeOffComponent,
  TimeOffSummaryDirective,
} from './time-off.component';
import { RouterModule, Route, UrlSerializer } from '@angular/router';
import { HeaderComponent } from '@ds/core/ui/menu-wrapper-header/header/header.component';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material';
import { HttpClientModule } from '@angular/common/http';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { CoreModule } from '@ds/core/core.module';
import { LowerCaseUrlSerializer } from '@ds/core/utilities';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { TimeOffUnitPipe } from './time-off-unit.pipe';
import { TimeOffActivityComponent } from './activity/time-off-activity.component';
import { BetaFeaturesModule } from '@ds/core/users/beta-features/beta-features.module';
import { MenuWrapperHeaderModule } from '@ds/core/ui/menu-wrapper-header/menu-wrapper-header.module';
import { SidebarComponent } from '../sidebar/sidebar.component';

const routes: Route[] = [
  {
    path: 'timeoff/:policyName/activity',
    children: [
      {
        path: '',
        component: HeaderComponent,
        outlet: 'header',
      },
      {
        path: '',
        component: TimeOffActivityComponent,
      },
      {
        path: '',
        component: SidebarComponent,
        outlet: 'sidebar',
      },
    ],
  },
  {
    path: 'timeoff',
    children: [
      {
        path: '',
        component: HeaderComponent,
        outlet: 'header',
      },
      {
        path: '',
        component: TimeOffComponent,
      },
      {
        path: '',
        component: SidebarComponent,
        outlet: 'sidebar',
      },
    ],
  },
];

@NgModule({
  declarations: [
    TimeOffSummaryDirective,
    TimeOffComponent,
    TimeOffUnitPipe,
    TimeOffActivityComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    MaterialModule,
    CoreModule,
    LoadingMessageModule,
    DsCardModule,
    MenuWrapperHeaderModule,
    BetaFeaturesModule,

    // routing
    RouterModule.forChild(routes),
  ],
  exports: [TimeOffComponent],
  providers: [
    {
      provide: '$scope',
      useFactory: (i: any) => i.get('$rootScope'),
      deps: ['$injector'],
    },
    {
      provide: UrlSerializer,
      useClass: LowerCaseUrlSerializer,
    },
  ],
})
export class TimeOffModule {}
