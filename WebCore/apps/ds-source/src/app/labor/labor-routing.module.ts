import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GeofenceStepperComponent } from './geofence-management/geofence-management-stepper/geofence-management-stepper.component';
import { GeofenceAdminComponent } from './geofence-management/geofence-management-admin/geofence-management-admin.component';

export const laborRoutes: Routes = [{
  path: 'geofence', children: [
    { path: 'admin', component: GeofenceAdminComponent },
    { path: 'stepper', component: GeofenceStepperComponent }
  ]
}
];

@NgModule({
  imports: [RouterModule.forChild(laborRoutes)],
  exports: [RouterModule]
})
export class LaborRoutingModule { }
