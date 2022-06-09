import { MapMarkerType } from '../map-marker-type.enum';

export class MapMarker {
    position: LatLngLiteral;
    icon: MapMarkerType;
    relatedId?: number;
    clickable?: boolean;
}

export interface LatLngLiteral {
  lat: any;
  lng: any;
}


