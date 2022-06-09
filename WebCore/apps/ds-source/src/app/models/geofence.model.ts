import { date } from '@ajs/applicantTracking/application/inputComponents';
import { MapMarker } from '../geofence/geofence-map/map-marker';
import { MapCircle } from '../geofence/geofence-map/map-circle';

export interface Geofence {
    clockClientGeofenceID: number;
    clientID: number;
    lat: number;
    lng: number;
    radius: number;
    name: string;
    address: string;
    isArchived?: boolean;
    modified?: Date | string;
    isHidden?: boolean;
}
export interface GeofenceUI extends Geofence {
    isSelected: boolean;
    mapCircle: any;
    mapMarker: any;
}
