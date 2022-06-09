import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LaborRoutingModule } from './labor-routing.module';
import {
    AutomatedPointsModule, RecalcPointsTriggerComponent, RecalcPointsDialogComponent,
    UpdateBalanceDialogComponent, UpdateBalanceTriggerComponent
} from '@ds/labor/automated-points';
import { GeofenceManagementComponent } from './geofence-management/geofence-management-map/geofence-management-map.component';
import { MaterialModule } from '@ds/core/ui/material';
import { AgmCoreModule, GoogleMapsAPIWrapper } from '@agm/core';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { GeofenceStepperComponent } from './geofence-management/geofence-management-stepper/geofence-management-stepper.component';
import { GeofenceEmployeesComponent } from './geofence-management/geofence-management-employees/geofence-management-employees.component';
import { OverlayModule } from '@angular/cdk/overlay';
import {
    GeofenceTimePolicyComponent
} from './geofence-management/geofence-management-time-policies/geofence-management-time-policies.component';
import {
    GeofenceAcceptTermsComponent
} from './geofence-management/geofence-management-accept-terms/geofence-management-accept-terms.component';
import {
    GeofenceGetStartedComponent
} from './geofence-management/geofence-management-get-started/geofence-management-get-started.component';
import { GeofenceAdminComponent } from './geofence-management/geofence-management-admin/geofence-management-admin.component';
import { TwoDecimalRoundPipe } from './geofence-management/geofence-management-map/two-decimal-rounding.pipe';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';

@NgModule({
    declarations: [
        GeofenceStepperComponent,
        GeofenceTimePolicyComponent,
        GeofenceEmployeesComponent,
        GeofenceAcceptTermsComponent,
        GeofenceGetStartedComponent,
        GeofenceAdminComponent,
        GeofenceManagementComponent,
        TwoDecimalRoundPipe,
    ],
    imports: [
        // Angular
        CommonModule,
        FormsModule,
        ReactiveFormsModule,

        // DS
        MaterialModule,
        DsCardModule,
        AutomatedPointsModule,
        LoadingMessageModule,
        AvatarModule,
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyDyggiB6RzKzKIWdK-v-PajBngC3vl4_P0',
            apiVersion:  'weekly',
            libraries: ['drawing', 'places']
        }),

        // Routing
        LaborRoutingModule,
        OverlayModule,
    ],
    entryComponents: [
        RecalcPointsTriggerComponent,
        RecalcPointsDialogComponent,
        UpdateBalanceDialogComponent,
        UpdateBalanceTriggerComponent
    ],
    exports: [
        GeofenceAdminComponent,
        GeofenceManagementComponent,
    ],
    providers: [
        { provide: GoogleMapsAPIWrapper, useClass: GoogleMapsAPIWrapper }
    ]
})
export class LaborAppModule { }
