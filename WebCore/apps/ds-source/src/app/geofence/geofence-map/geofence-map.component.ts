import { Component, OnInit, Input, AfterViewInit, AfterViewChecked, EventEmitter, Output } from '@angular/core';
import { GoogleMapsAPIWrapper } from '@agm/core';
import { MapMarkerPipe } from '../map-marker.pipe';
import { MapCirclePipe } from '../map-circle.pipe';
import { MapMarkerType } from '../map-marker-type.enum';
import { MapCircleType } from '../map-circle-type.enum';
import { MapMarker } from './map-marker';
import { Observable, forkJoin } from 'rxjs';
import { MapCircle } from './map-circle';
import { MapLocation } from '../map-location';
import { MapLocationType } from '../map-location-type.enum';
import { MapControlSettings } from '../map-control-settings';

export declare const google: any;

@Component({
    selector: 'ds-geofence-map',
    templateUrl: './geofence-map.component.html',
    styleUrls: ['./geofence-map.component.scss'],
    providers: [
        { provide: GoogleMapsAPIWrapper, useClass: GoogleMapsAPIWrapper },
    ]
})
export class GeofenceMapComponent implements OnInit {
    @Input() markerListInput?: Observable<MapMarker[]>;
    @Input() circleListInput?: Observable<MapCircle[]>;
    @Input() locationInput?: Observable<MapLocation>;
    @Input() controlSettingsInput: MapControlSettings;
    @Output() newMapCircleEvent: EventEmitter<MapCircle> = new EventEmitter<MapCircle>();
    @Output() deleteSelectedCircleEvent: EventEmitter<number> = new EventEmitter<number>();
    @Input() isPunchMap: boolean;

    @Output() selectedObjectRelatedIdEvent: EventEmitter<number> = new EventEmitter<number>();
    selectedObjectRelatedId: number;
    markerList: MapMarker[] = [];
    circleList: MapCircle[] = [];
    location: MapLocation;
    geocoder: any;
    drawingManager: any;
    isInDrawingMode = false;
    googleMapMarkerList: any[] = [];
    googleMapCircleList: any[] = [];
    mapInstance: any;
    formInit = new Observable<any>();
    mapMarkerPipe = new MapMarkerPipe();
    mapCirclePipe = new MapCirclePipe();

    dominionLocation = {
        lat: 45,
        lng: -80,
    };

    styles = [{
        'featureType': 'poi',
        'stylers': [{ 'visibility': 'off' }]
    }];

    constructor() {
    }

    ngOnInit() {
    }

    // This is called from the html. It will run when agm is loaded and the map is available in the dom.
    mapReady(agmMap: any): void {
        this.mapInstance = agmMap;
        this.geocoder = new google.maps.Geocoder;

        if (this.markerListInput) {
            this.markerListInput.subscribe(res => {
                this.markerList = res ? res : [];
                this.updateMarkers();
            });
        }

        if (this.circleListInput) {
            this.circleListInput.subscribe(res => {
                this.circleList = res ? res : [];
                this.updateCircles();
            });
        }

        this.setLocation();

        if (this.controlSettingsInput) {
            if ( this.controlSettingsInput.showCircleDrawingManager ) this.createToggleDrawingModeMapControl();
            if ( this.controlSettingsInput.showAddressSearch ) this.createAddressSearchMapControl();
            if ( this.controlSettingsInput.showShowAll ) this.createShowAllMapObjectsMapControl();
            if ( this.controlSettingsInput.showMapTypeSelector ) this.createToggleMapTypeMapControl();
            if ( this.controlSettingsInput.showInfoWindow ) this.createInfowindowControl();
        }

        // if (this.googleMapMarkerList.length !== 0 || this.googleMapCircleList.length !== 0) this.showAllMapObjects();
    }

    public setLocation(): void {
        if (this.locationInput) {
            this.locationInput.subscribe(res => {
                this.location = res;
                this.updateMapLocation();
            });
        }
    }

    private updateMapLocation(): void {
        if (this.location.mapLocationType === MapLocationType.ShowAll) {
            this.showAllMapObjects();
        } else if (this.location.mapLocationType === MapLocationType.Address) {
            this.geocoder.geocode({ 'address': this.location.value },
                (results, status) => {
                    if (status === 'OK') { // change
                        this.mapInstance.setCenter({ // change
                            lat: results[0].geometry.location.lat(),
                            lng: results[0].geometry.location.lng(),
                        });
                    } else {
                        this.mapInstance.setCenter(this.dominionLocation);
                    }
                }
            );
        } else if (this.location.mapLocationType === MapLocationType.LatLngLiteral) {
            this.mapInstance.setCenter(this.location.value);
        } else {
            this.mapInstance.setCenter(this.dominionLocation);
        }
    }

    private createAddressSearchMapControl(): void {

        // PLACES AUTOCOMPLETE
        const addressSearchInput = document.getElementById('addressSearch');
        const searchType = { types: ['address'] };

        const autocomplete = new google.maps.places.Autocomplete(
            addressSearchInput,
            searchType
        );

        // Push search input into map controls and move to map view
        this.mapInstance.controls[google.maps.ControlPosition.TOP_RIGHT].push(addressSearchInput);

        // This creates a bias for predictions near to the current window focus
        this.mapInstance.addListener('bounds_changed', () => {
            autocomplete.setBounds(this.mapInstance.getBounds());
        });

        // When a prediction is selected, update map bounds and center
        autocomplete.addListener('place_changed', () => {
            const place = autocomplete.getPlace();

            // verify result
            if (!place.geometry) {
                return;
            }

            // create new geofence at search location if one does not already exist
            const lat = place.geometry.location.lat();
            const lng = place.geometry.location.lng();
            const address = place.formatted_address;

            // const isNewGeofence = this.geofenceList.findIndex(x => x.lat === lat && x.lng === lng) === -1;

            // if (isNewGeofence) {
            //     const newGeofence: GeofenceUI = {
            //         clockClientGeofenceID: 0,
            //         clientID: this.clientId,
            //         lat: lat,
            //         lng: lng,
            //         radius: 150,
            //         name: `Geofence #${this.geofenceList.length + 1}`,
            //         address: address,
            //         isSelected: false,
            //         mapCircle: new google.maps.Circle({
            //             center: new google.maps.LatLng({ lat: lat, lng: lng }),
            //             radius: this.feetToMeters(150),
            //             map: this.mapInstance,
            //             fillColor: '#FFB627',
            //             fillOpacity: 0.0,
            //             circleDraggable: false,
            //             editable: false,
            //             strokeWeight: 2,
            //             strokeOpacity: 1,
            //             strokeColor: '#FC621A',
            //         }),
            //         mapMarker: new google.maps.Marker({
            //             position: {
            //                 lat: lat,
            //                 lng: lng,
            //             },
            //             icon: this.mapMarkerPipe.transform(MapMarkerType.GeofenceLocation),
            //             map: this.mapInstance,
            //         }),
            //     };

            //     this.createNewClientGeofence(newGeofence);
            // }

            // Update map center and search value
            this.mapInstance.panTo(place.geometry.location);
            $(addressSearchInput).val('');
        });
    }

    private createShowAllMapObjectsMapControl(): void {
        const showAllMapObjectsButton = document.getElementById('showAllMapObjects');

        // Push button into map controls and move to map view
        this.mapInstance.controls[google.maps.ControlPosition.TOP_RIGHT].push(showAllMapObjectsButton);
    }

    showAllMapObjects(): void {
        const punchMarkerList = [];
        const bounds = new google.maps.LatLngBounds();
        const geofenceIconPath = this.mapMarkerPipe.transform(MapMarkerType.GeofenceLocation).path;

        this.googleMapMarkerList.forEach(marker => {
            if (marker.icon.path !== geofenceIconPath) {
                punchMarkerList.push(marker);
            }
        });

        if (punchMarkerList.length > 0 ) {
            punchMarkerList.forEach(marker => {
                bounds.extend(marker.getPosition());
            });
        } else if (this.googleMapMarkerList.length > 0 ) {
            this.googleMapMarkerList.forEach(marker => {
                bounds.extend(marker.getPosition());
            });
        } else if (this.googleMapCircleList.length > 0) {
            this.googleMapCircleList.forEach(circle => {
                bounds.union(circle.getBounds());
            });
        } else {
            return;
        }

        this.mapInstance.fitBounds(bounds);
    }

    private createToggleDrawingModeMapControl(): void {
        this.initMapDrawingManager();
        const toggleDrawingModeButton = document.getElementById('toggleDrawingMode');

        // Push button into map controls and move to map view
        this.mapInstance.controls[google.maps.ControlPosition.TOP_LEFT].push(toggleDrawingModeButton);
    }

    private initMapDrawingManager(): void {
        const drawingOptions = {
            drawingControl: false,
            drawingControlOptions: {
                drawingModes: []
            },
            circleOptions: this.mapCirclePipe.transform(MapCircleType.StaticDefaultCircle),
            drawingMode: null,
        };

        // Add drawing manager to map with drawingMode defaulted to off
        this.drawingManager = new google.maps.drawing.DrawingManager(drawingOptions);
        this.drawingManager.setMap(this.mapInstance);

        google.maps.event.addListener(this.drawingManager, 'circlecomplete', (circle) => {
            this.exitMapDrawingMode();
            const coords = { lat: circle.center.lat(), lng: circle.center.lng() };
            let address: string;
            this.geocoder.geocode({ 'location': coords }, (results, status) => {
                if (status === google.maps.GeocoderStatus.OK && results[0] != null) {
                    address = results[0].formatted_address;
                } else {
                    address = 'No Address Found';
                }

                const mapCircle: MapCircle = {
                    strokeColor: circle.strokeColor,
                    strokeOpacity: circle.strokeOpacity,
                    strokeWeight: circle.strokeWeight,
                    fillColor: circle.fillColor,
                    fillOpacity: circle.fillOpacity,
                    center: coords,
                    radius: circle.radius,
                    circleDraggable: circle.circleDraggable,
                    editable: circle.editable,
                    relatedId: 0,
                    description: address,
                };

                this.newMapCircleEvent.emit(mapCircle);
            });
        });
    }

    toggleMapDrawingMode(): void {
        if (this.isInDrawingMode) this.exitMapDrawingMode();
        else this.enterMapDrawingMode();
    }

    private enterMapDrawingMode(): void {
        // Set all circles to unselected
        // this.clearAllClientGeofenceSelections();

        this.isInDrawingMode = true;
        this.drawingManager.setDrawingMode(google.maps.drawing.OverlayType.CIRCLE);
        // setMap is required for the drawing mode changes to take affect
        this.drawingManager.setMap(this.mapInstance);
    }

    private exitMapDrawingMode(): void {
        this.isInDrawingMode = false;
        // Set all circles to unselected
        // this.clearAllClientGeofenceSelections();
        this.drawingManager.setDrawingMode(null);
        // setMap is required for the drawing mode changes to take affect
        this.drawingManager.setMap(this.mapInstance);
    }

    private createToggleMapTypeMapControl(): void {
        const toggleMapTypeButton = document.getElementById('toggleMapType');

        // Push button into map controls and move to map view
        this.mapInstance.controls[google.maps.ControlPosition.BOTTOM_LEFT].push(toggleMapTypeButton);
    }

    toggleMapType(mapType: string): void {
        if (mapType.toLowerCase() === 'hybrid') this.mapInstance.setMapTypeId(google.maps.MapTypeId.HYBRID);
        else this.mapInstance.setMapTypeId(google.maps.MapTypeId.ROADMAP);

    }

    private createInfowindowControl(): void {

        // PLACES AUTOCOMPLETE
        const infowindowForm = document.getElementById('infowindowForm');

        // Push search input into map controls and move to map view
        this.mapInstance.controls[google.maps.ControlPosition.BOTTOM_RIGHT].push(infowindowForm);
    }

    updateMarkers() {
        // Reset markers. Setting map to null deletes the marker from the map.
        this.googleMapMarkerList.forEach(marker => {
            marker.setMap(null);
        });
        this.googleMapMarkerList = [];

        // Set the google map object to the mapCircle, and assign it to this map to display it.
        this.markerList.forEach(mapMarker => {
            const googleMapMarker = new google.maps.Marker(mapMarker);
            googleMapMarker.setMap(this.mapInstance);

            // Add a listener to each marker to return the relatedId on click.
            if (mapMarker.clickable === true) {
                google.maps.event.addListener(googleMapMarker, 'click', marker => {
                    this.selectMarker(googleMapMarker.relatedId);
                });
            }

            // Keep a running list of google map marker objects (different than our list of marker objects)
            this.googleMapMarkerList.push(googleMapMarker);
        });
    }

    updateCircles() {
        // Reset circles. Setting map to null deletes the circle from the map.
        this.googleMapCircleList.forEach(circle => {
            circle.setMap(null);
        });
        this.googleMapCircleList = [];

        this.circleList.forEach(mapCircle => {
            // Set the google map object to the mapCircle, and assign it to this map to display it.
            const googleMapCircle = new google.maps.Circle(mapCircle);
            googleMapCircle.setMap(this.mapInstance);

            // Add a listener to eash circle to return the relatedId on click.
            if (mapCircle.clickable === true) {
                google.maps.event.addListener(googleMapCircle, 'click', circle => {
                    this.selectCircle(googleMapCircle.relatedId);
                });
            }

            // Keep a running list of google map circle objects (different than our list of circle objects)
            this.googleMapCircleList.push(googleMapCircle);
        });
    }

    selectMarker(relatedId: number) {
        // Deselect other circles
        this.clearSelectedMarkers();

        // Find map circle with this relatedId
        const googleMapMarker = this.googleMapMarkerList.find(marker => marker.relatedId === relatedId);

        // Pass back the relatedId of the selected object
        this.selectedObjectRelatedIdEvent.emit(googleMapMarker.relatedId);
        this.selectedObjectRelatedId = googleMapMarker.relatedId;

        // Focus map on selected object
        this.mapInstance.panTo(googleMapMarker);
        this.mapInstance.setZoom(this.mapInstance.getZoom() - 1);

        // Style objects
        this.googleMapMarkerList.forEach(marker => {
            if (marker.relatedId === googleMapMarker.relatedId) {
                marker.setOptions(this.mapMarkerPipe.transform(MapMarkerType.BadLocation));
            } else {
                marker.setOptions(this.mapMarkerPipe.transform(MapMarkerType.BadLocation));
            }
        });
    }

    clearSelectedMarkers(): void {
        // Pass back the relatedId of the selected object
        this.selectedObjectRelatedIdEvent.emit(null);
        this.selectedObjectRelatedId = null;

        // Style objects
        this.googleMapMarkerList.forEach(marker => {
            marker.setOptions(this.mapMarkerPipe.transform(MapMarkerType.BadLocation));
        });
    }

    selectCircle(relatedId: number) {
        // Deselect other circles
        this.clearSelectedCircles();

        // Find map circle with this relatedId
        const googleMapCircle = this.googleMapCircleList.find(circle => circle.relatedId === relatedId);

        // Pass back the relatedId of the selected object
        this.selectedObjectRelatedIdEvent.emit(googleMapCircle.relatedId);
        this.selectedObjectRelatedId = googleMapCircle.relatedId;

        // Focus map on selected object
        const mapCenterCalc = google.maps.geometry.spherical.computeOffset(
            googleMapCircle.center, googleMapCircle.radius, 90
        );
        this.mapInstance.fitBounds(googleMapCircle.getBounds());
        this.mapInstance.panTo(mapCenterCalc);
        this.mapInstance.setZoom(this.mapInstance.getZoom() - 1);

        // Style objects
        this.googleMapCircleList.forEach(circle => {
            if (circle.relatedId === googleMapCircle.relatedId) {
                circle.setOptions(this.mapCirclePipe.transform(MapCircleType.StaticSelectedCircle));
            } else {
                circle.setOptions(this.mapCirclePipe.transform(MapCircleType.StaticDefaultCircle));
            }
        });
    }

    clearSelectedCircles(): void {
        // Pass back the relatedId of the selected object
        this.selectedObjectRelatedIdEvent.emit(null);
        this.selectedObjectRelatedId = null;

        // Style objects
        this.googleMapCircleList.forEach(circle => {
            circle.setOptions(this.mapCirclePipe.transform(MapCircleType.StaticDefaultCircle));
        });
    }

    deleteSelectedCircle(): void {
        const googleMapCircle = this.googleMapCircleList.find(circle => circle.relatedId === this.selectedObjectRelatedId);
        googleMapCircle.setMap(null);

        this.deleteSelectedCircleEvent.emit(this.selectedObjectRelatedId);
    }
}
