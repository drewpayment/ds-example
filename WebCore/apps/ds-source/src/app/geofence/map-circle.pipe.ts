import { Pipe, PipeTransform } from '@angular/core';
import { MapCircleType } from './map-circle-type.enum';
import { MapCircle } from './geofence-map/map-circle';

@Pipe({
    name: 'mapCircle'
})
export class MapCirclePipe implements PipeTransform {

    transform = mapCircle;

}

export function mapCircle(mapCircleType: MapCircleType): MapCircle {
    let circle: any = null;

    if (mapCircleType === MapCircleType.StaticDefaultCircle) {
        circle = {
            fillColor: '#FFB627',
            fillOpacity: 0.0,
            circleDraggable: false,
            editable: false,
            strokeWeight: 2,
            strokeOpacity: 1,
            strokeColor: '#FC621A',
        };
    }

    if (mapCircleType === MapCircleType.StaticSelectedCircle) {
        circle = {
            fillColor: '#FFB627',
            fillOpacity: 0.25,
            circleDraggable: false,
            editable: false,
            strokeWeight: 4,
            strokeOpacity: 1,
            strokeColor: '#FC621A',
        };
    }

    if (mapCircleType === MapCircleType.EditableDefaultCircle) {
        circle = {
            fillColor: '#FFB627',
            fillOpacity: 0.0,
            circleDraggable: true,
            editable: true,
            strokeWeight: 2,
            strokeOpacity: 1,
            strokeColor: '#FC621A',
        };
    }

    if (mapCircleType === MapCircleType.EditableSelectedCircle) {
        circle = {
            fillColor: '#FFB627',
            fillOpacity: 0.25,
            circleDraggable: true,
            editable: true,
            strokeWeight: 4,
            strokeOpacity: 1,
            strokeColor: '#FC621A',
        };
    }

    return circle;
}
