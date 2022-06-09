import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MapMarkerPipe } from './map-marker.pipe';
import { GeofenceMapComponent } from './geofence-map/geofence-map.component';
import { AgmCoreModule, GoogleMapsAPIWrapper } from '@agm/core';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { MapCirclePipe } from './map-circle.pipe';

@NgModule({
    imports: [
        CommonModule,
        MaterialModule,
        DsCardModule,
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyDyggiB6RzKzKIWdK-v-PajBngC3vl4_P0',
            apiVersion: 'weekly',
            libraries: ['drawing', 'places']
        }),
    ],
    declarations: [
        MapMarkerPipe,
        GeofenceMapComponent,
        MapCirclePipe
    ],
    exports: [
        MapMarkerPipe,
        GeofenceMapComponent,
        MapCirclePipe,
    ]
})
export class GeofenceModule { }
