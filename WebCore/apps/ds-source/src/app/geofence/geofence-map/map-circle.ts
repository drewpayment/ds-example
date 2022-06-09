import { LatLngLiteral } from './map-marker';


export class MapCircle {
    center: LatLngLiteral;
    radius: number;
    relatedId?: number;
    description?: string;
    strokeColor?: string;
    strokeOpacity?: number;
    strokeWeight?: number;
    fillColor?: string;
    fillOpacity?: number;
    circleDraggable?: boolean;
    editable?: boolean;
    clickable?: boolean;
}
