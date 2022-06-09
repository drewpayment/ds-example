import { Pipe, PipeTransform } from '@angular/core';
import { MapMarkerType } from './map-marker-type.enum';

@Pipe({
    name: 'mapMarker'
})
export class MapMarkerPipe implements PipeTransform {
    // add memoization

    transform = mapMarker;

}

export function mapMarker(mapMarkerType: MapMarkerType): any {
    let marker: any = null;

    if (mapMarkerType === MapMarkerType.GeofenceLocation) {
        marker = {
            path: 'M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 ' +
                '9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z',
            fillColor: '#FC621A',
            fillOpacity: 1,
            strokeColor: '#FC621A',
            anchor: { x: 12, y: 24 },
        };
    }

    if (mapMarkerType === MapMarkerType.GoodLocation) {
        marker = {
            path: 'M12 2c3.86 0 7 3.14 7 7 0 5.25-7 13-7 13S5 14.25 5 9c0-3.86 ' +
                '3.14-7 7-7zm-1.53 12L17 7.41 15.6 6l-5.13 5.18L8.4 9.09 7 10.5l3.47 3.5z',
            fillColor: '#5CB85C',
            fillOpacity: 1,
            strokeColor: '#5CB85C',
            anchor: { x: 12, y: 24 },
        };
    }

    if (mapMarkerType === MapMarkerType.BadLocation) {
        marker = {
            path: 'M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z',
            fillColor: '#D9534F',
            fillOpacity: 1,
            strokeColor: '#D9534F',
            anchor: { x: 12, y: 24 },
        };
    }

    return marker;
}
