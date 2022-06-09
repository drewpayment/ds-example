import { Component, OnInit } from '@angular/core';
import { Geofence, GeofenceUI } from '../../../models';
import { GoogleMapsAPIWrapper } from '@agm/core';
import { Observable, BehaviorSubject, of, forkJoin } from 'rxjs';
import { FormGroup, FormBuilder, FormControl, Validators, ValidatorFn, ValidationErrors } from '@angular/forms';
import { switchMap, startWith, map, catchError, tap } from 'rxjs/operators';
import { GeofenceService } from '../../services/geofence.service';
import { AccountService } from '@ds/core/account.service';
import { ClientService } from '@ds/core/clients/shared';
import { IClientData } from '@ajs/onboarding/shared/models';
import { GeofenceManagementService } from '../geofence-management.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import * as moment from 'moment';
export declare const google: any;

const lblTag = 'Geofence #';

@Component({
    selector: 'ds-geofence-management-map',
    templateUrl: './geofence-management-map.component.html',
    styleUrls: ['./geofence-management-map.component.scss'],
    providers: [
        { provide: GoogleMapsAPIWrapper, useClass: GoogleMapsAPIWrapper },
    ]
})
export class GeofenceManagementComponent implements OnInit {
    zoom = 15;
    mapStyle: any;
    geofences$ = new BehaviorSubject<GeofenceUI[]>(null);
    formInit$: Observable<any>;
    geofences: Observable<GeofenceUI[]> = this.geofences$.asObservable();
    geofenceList: GeofenceUI[];
    mapInstance: any;
    isInDrawingMode = false;
    dm: any;
    defaultCircleOptions: any;
    selectedCircleOptions: any;
    clientId: number;
    isInitialLoad = true;
    updateGeofences: FormGroup;
    geofenceCount = 0;
    mapMarkerIcon: any;
    geocoder: any;
    selectedGeofenceUI: GeofenceUI;
    filterGeofencesControl = new FormControl();
    filteredGeofences: Observable<any>;
    clientInfo: IClientData;
    location: any;
    isLoading: boolean;
    isSaving: boolean;
    hasChanges: boolean;
    optedIn = false;
    lastModified: moment.Moment | null;
    formSubmitted: boolean;

    constructor(
        private fb: FormBuilder,
        private gService: GeofenceService,
        private account: AccountService,
        private clientService: ClientService,
        private service: GeofenceManagementService,
        private msg: DsMsgService,
    ) {
        this.geofences = this.geofences$.asObservable();
        this.geofenceList = [];

        this.defaultCircleOptions = {
            fillColor: '#FFB627',
            fillOpacity: 0.0,
            circleDraggable: false,
            editable: false,
            strokeWeight: 2,
            strokeOpacity: 1,
            strokeColor: '#FC621A',
        };

        this.selectedCircleOptions = {
            fillColor: '#FFB627',
            fillOpacity: 0.25,
            circleDraggable: true,
            editable: true,
            strokeWeight: 4,
            strokeOpacity: 1,
            strokeColor: '#FC621A',
        };

        this.mapMarkerIcon = {
            path: 'M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7z' +
                'm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z',
            fillColor: '#FC621A',
            fillOpacity: 1,
            strokeColor: '#FC621A',
            anchor: { x: 12, y: 24 },
        };

        this.mapStyle = [
            {
                'featureType': 'poi',
                'stylers': [
                    {
                        'visibility': 'off'
                    }
                ]
            }
        ];

        this.selectedGeofenceUI = null;
        this.isLoading = true;
        this.isSaving = false;
        this.hasChanges = false;
    }

    ngOnInit() {
        this.createForm();

        this.formInit$ = this.initializeListener();

        this.filteredGeofences = this.filterGeofencesControl.valueChanges
            .pipe(
                startWith(''),
                map(value => this._filterGeofences(value))
            );
    }

    initializeListener(): Observable<any> {
        return this.account.getUserInfo().pipe(
            switchMap(user => {
                this.clientId = user.clientId;

                return this.clientService.getClientById(this.clientId);
            }),
            switchMap(client => {
                this.clientInfo = client;

                return this.service.geofenceOptIn;
            }),
            map((optedIn: boolean) => {
                this.optedIn = optedIn;

                return true;
            }),
            catchError(() => {
                this.msg.setTemporaryMessage('Something went wrong.', this.msg.messageTypes.error);

                return of([]);
            })
        );
    }

    // MAP AND UI

    mapReady(agmMap: any): void {
        this.mapInstance = agmMap;
        this.geocoder = new google.maps.Geocoder;

        this.createMapDrawingManager();
        this.createShowAllClientGeofencesMapControl();
        this.createAddressSearchMapControl();
        this.createToggleDrawingModeMapControl();
        this.createToggleMapTypeMapControl();
        this.createEditClientGeofenceForm();
        this.loadClientGeofenceList();
    }

    private getLocation(): void {
        this.geocoder.geocode({ 'address': this.clientInfo.addressLine1 + ' ,' + this.clientInfo.postalCode },
            (results, status) => {
                if (status === 'OK') { // change
                    this.location = {
                        lat: results[0].geometry.location.lat(),
                        lng: results[0].geometry.location.lng(),
                    };
                } else {
                    this.location = {
                        lat: 42.9634,
                        lng: -85.6681,
                    };
                    this.zoom = 8;
                }

                this.mapInstance.setCenter({ lat: this.location.lat, lng: this.location.lng });
                this.mapInstance.setZoom(this.zoom);
            }
        );
    }

    private createMapDrawingManager(): void {
        const drawingOptions = {
            drawingControl: false,
            drawingControlOptions: {
                drawingModes: []
            },
            circleOptions: this.selectedCircleOptions,
            drawingMode: null,
        };

        // Add drawing manager to map with drawingMode defaulted to n;;
        this.dm = new google.maps.drawing.DrawingManager(drawingOptions);
        this.dm.setMap(this.mapInstance);

        google.maps.event.addListener(this.dm, 'circlecomplete', (circle) => {

            let nums = this.geofenceList.filter(x=>x.name.indexOf(lblTag) == 0).map(x=> this.numeric(x.name.substring(lblTag.length)) );
            let newNum = nums.length > 0 ? (Math.max(...nums) + 1) : 1;
            
            // Add new circle to geofence list
            const newGeofence: GeofenceUI = {
                clockClientGeofenceID: 0,
                clientID: this.clientId,
                lat: circle.center.lat(),
                lng: circle.center.lng(),
                radius: this.metersToFeet(circle.radius),
                name: `${lblTag}${newNum}`,
                address: '',
                isSelected: false,
                mapCircle: circle,
                mapMarker: new google.maps.Marker({
                    position: {
                        lat: circle.center.lat(),
                        lng: circle.center.lng(),
                    },
                    icon: this.mapMarkerIcon,
                    map: this.mapInstance,
                }),
            };

            this.createNewClientGeofence(newGeofence);
        });
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

            const isNewGeofence = this.geofenceList.findIndex(x => x.lat === lat && x.lng === lng) === -1;

            if (isNewGeofence) {
                let nums = this.geofenceList.filter(x=>x.name.indexOf(lblTag) == 0).map(x=> this.numeric(x.name.substring(lblTag.length)) );
                let newNum = nums.length > 0 ? (Math.max(...nums) + 1) : 1;
            
                const newGeofence: GeofenceUI = {
                    clockClientGeofenceID: 0,
                    clientID: this.clientId,
                    lat: lat,
                    lng: lng,
                    radius: 150,
                    name: `${lblTag}${newNum}`,
                    address: address,
                    isSelected: false,
                    mapCircle: new google.maps.Circle({
                        center: new google.maps.LatLng({ lat: lat, lng: lng }),
                        radius: this.feetToMeters(150),
                        map: this.mapInstance,
                        fillColor: '#FFB627',
                        fillOpacity: 0.0,
                        circleDraggable: false,
                        editable: false,
                        strokeWeight: 2,
                        strokeOpacity: 1,
                        strokeColor: '#FC621A',
                    }),
                    mapMarker: new google.maps.Marker({
                        position: {
                            lat: lat,
                            lng: lng,
                        },
                        icon: this.mapMarkerIcon,
                        map: this.mapInstance,
                    }),
                };

                this.createNewClientGeofence(newGeofence);
            }

            // Update map center and search value
            this.mapInstance.panTo(place.geometry.location);
            $(addressSearchInput).val('');
        });
    }

    private numeric(str:string){
        if(!str) return 0;
        if(Number(str) === NaN || !Number(str)) return 0;
        else return Number(str);
    }

    private createEditClientGeofenceForm(): void {

        // PLACES AUTOCOMPLETE
        const editGeofenceForm = document.getElementById('editGeofence');

        // Push search input into map controls and move to map view
        this.mapInstance.controls[google.maps.ControlPosition.BOTTOM_RIGHT].push(editGeofenceForm);
    }

    private createShowAllClientGeofencesMapControl(): void {
        const showAllGeofencesButton = document.getElementById('showAllGeofences');

        // Push button into map controls and move to map view
        this.mapInstance.controls[google.maps.ControlPosition.TOP_RIGHT].push(showAllGeofencesButton);
    }

    private createToggleDrawingModeMapControl(): void {
        const toggleDrawingModeButton = document.getElementById('toggleDrawingMode');

        // Push button into map controls and move to map view
        this.mapInstance.controls[google.maps.ControlPosition.TOP_LEFT].push(toggleDrawingModeButton);
    }

    private createToggleMapTypeMapControl(): void {
        const toggleMapTypeButton = document.getElementById('toggleMapType');

        // Push button into map controls and move to map view
        this.mapInstance.controls[google.maps.ControlPosition.BOTTOM_LEFT].push(toggleMapTypeButton);
    }

    toggleMapDrawingMode(): void {
        if (this.isInDrawingMode) this.exitMapDrawingMode();
        else this.enterMapDrawingMode();
    }

    private enterMapDrawingMode(): void {
        // Set all circles to unselected
        this.clearAllClientGeofenceSelections();

        this.isInDrawingMode = true;
        this.dm.setDrawingMode(google.maps.drawing.OverlayType.CIRCLE);
        // setMap is required for the drawing mode changes to take affect
        this.dm.setMap(this.mapInstance);
    }

    private exitMapDrawingMode(): void {
        this.isInDrawingMode = false;
        // Set all circles to unselected
        this.clearAllClientGeofenceSelections();
        this.dm.setDrawingMode(null);
        // setMap is required for the drawing mode changes to take affect
        this.dm.setMap(this.mapInstance);
    }

    toggleMapType(mapType: string): void {
        if (mapType.toLowerCase() === 'hybrid') this.mapInstance.setMapTypeId(google.maps.MapTypeId.HYBRID);
        else this.mapInstance.setMapTypeId(google.maps.MapTypeId.ROADMAP);

    }

    // This will update our circle styling and edit options for all circles.
    // Activates the passed in circle and deactivates others.
    // Passing null deactivates all.
    selectClientGeofenceById(id: number): void {
        this.deleteHiddenClientGeofences();

        this.selectedGeofenceUI = this.geofenceList.find(f => f.clockClientGeofenceID === id);
        const selectedCircle = this.selectedGeofenceUI.mapCircle;
        const mapCenterCalc = google.maps.geometry.spherical.computeOffset(
            selectedCircle.center, selectedCircle.radius, 90
        );

        this.hasChanges = false;
        this.isSaving = false;

        this.geofenceList.forEach(f => {
            if (f.clockClientGeofenceID === id) f.mapCircle.setOptions(this.selectedCircleOptions);
            else f.mapCircle.setOptions(this.defaultCircleOptions);
        });

        this.geofenceList.forEach(f => {
            f.isSelected = (f.clockClientGeofenceID === id);
        });

        this.mapInstance.fitBounds(selectedCircle.getBounds());
        this.mapInstance.panTo(mapCenterCalc);
        this.mapInstance.setZoom(this.mapInstance.getZoom() - 1);

        this.patchForm(this.selectedGeofenceUI);
    }

    clearAllClientGeofenceSelections(): void {
        this.selectedGeofenceUI = null;
        this.geofenceList.forEach(f => {
            f.mapCircle.setOptions(this.defaultCircleOptions);
            f.isSelected = false;
        });
    }

    private createNewClientGeofence(geofence: GeofenceUI): void {
        // Exit drawing manager after geofence is added
        this.exitMapDrawingMode();
        this.isLoading = true;

        // Reverse geocode lat,lng to get address, which will be the new geofences default name
        this.geocoder.geocode({ 'location': { lat: geofence.lat, lng: geofence.lng } }, (results, status) => {
            if (!geofence.address || geofence.address == '') {
                if (status === google.maps.GeocoderStatus.OK && results[0] != null) {
                    geofence.address = results[0].formatted_address;
                } else {
                    geofence.address = 'No Address Found';
                }
            }

            const dto = this.prepareModel(geofence);

            this.gService.addClientGeofence(dto).subscribe((res) => {
                const newGeoFence: GeofenceUI = {
                    clockClientGeofenceID: res.clockClientGeofenceID,
                    clientID: res.clientID,
                    lat: res.lat,
                    lng: res.lng,
                    radius: this.metersToFeet(res.radius),
                    name: res.name,
                    address: res.address,
                    isSelected: false,
                    modified: res.modified,
                    mapCircle: geofence.mapCircle,
                    mapMarker: geofence.mapMarker
                };

                this.createClientGeofenceUI(newGeoFence);
                this.selectClientGeofenceById(res.clockClientGeofenceID);
                this.isLoading = false;
                this.updateLastModified();
            }, (err) => {
                this.isLoading = false;
                this.handleErrorMessage(err);
            });
        });
    }

    private createClientGeofenceUI(geofence: GeofenceUI): void {
        this.geofenceList.push(geofence);
        this.addGoogleMapCircleListeners(geofence);
        this.updateGeofenceList();
        this.clearGeofenceFilter();
    }

    private updateClientGeofenceUI(id: number, circle: any): void {

        this.geofenceList.forEach(f => {
            if (f.clockClientGeofenceID === id) {
                f.lat = circle.center.lat();
                f.lng = circle.center.lng();
                f.radius = this.metersToFeet(circle.radius);
                f.mapCircle = circle;
                f.mapMarker.setPosition({
                    lat: circle.center.lat(),
                    lng: circle.center.lng(),
                });
            }
        });

        circle.setMap(this.mapInstance);
        this.updateGeofenceList();
    }

    showAllClientGeofences(): void {
        const bounds = new google.maps.LatLngBounds();
        this.clearAllClientGeofenceSelections();
        if (this.geofenceList.length === 0) return;

        this.geofenceList.forEach(f => {
            bounds.union(f.mapCircle.getBounds());
        });

        this.mapInstance.fitBounds(bounds);
    }

    private addGoogleMapCircleListeners(geofence: GeofenceUI) {
        // Listener that will be used to make circle editable on click
        google.maps.event.addListener(geofence.mapCircle, 'click', () => {
            this.selectClientGeofenceById(geofence.clockClientGeofenceID);
        });

        google.maps.event.addListener(geofence.mapCircle, 'center_changed', () => {
            this.isLoading = true;

            const lat = geofence.mapCircle.center.lat();
            const lng = geofence.mapCircle.center.lng();

            this.geocoder.geocode({ 'location': { lat: lat, lng: lng } }, (results, status) => {
                this.updateClientGeofenceUI(geofence.clockClientGeofenceID, geofence.mapCircle);
                this.updateClientGeofence();
            });
        });

        google.maps.event.addListener(geofence.mapCircle, 'radius_changed', () => {
            this.isLoading = true;
            this.updateGeofences.get('radius').setValue(this.metersToFeet(geofence.mapCircle.radius));
            this.updateClientGeofenceUI(geofence.clockClientGeofenceID, geofence.mapCircle);
            this.updateClientGeofence();
        });

        // Do not pan to each geofence if this is the initial load, only when adding a new geofence.
        if (!this.isInitialLoad) {
            this.mapInstance.panTo(geofence.mapCircle.center);
            this.mapInstance.fitBounds(geofence.mapCircle.getBounds());
        }
    }

    updateMapCircleRadius(): void {
        const value = this.updateGeofences.get('radius').value;
        if(value && Number(value) > 0){
            this.selectedGeofenceUI.mapCircle.radius = value;
            this.selectedGeofenceUI.mapCircle.setOptions({ radius: this.feetToMeters(value) });
        }
        this.updateClientGeofence();
        
    }

    clearGeofenceFilter(): void {
        this.filterGeofencesControl.setValue(null);
    }

    // API SERVICE CALLS

    private loadClientGeofenceList(): void {
        this.isLoading = false;
        // Retrieve user information for clientId only
        this.gService.getClientGeofenceList(this.clientId).subscribe((res) => {
            if (res != null) {
                res.forEach(f => {
                    const newGeofence = {
                        clockClientGeofenceID: f.clockClientGeofenceID,
                        clientID: f.clientID,
                        name: f.name,
                        address: f.address,
                        lat: f.lat,
                        lng: f.lng,
                        radius: this.metersToFeet(f.radius),
                        isSelected: false,
                        modified: f.modified,
                        mapCircle: new google.maps.Circle({
                            center: new google.maps.LatLng({ lat: f.lat, lng: f.lng }),
                            radius: f.radius,
                            map: this.mapInstance,
                            fillColor: '#FFB627',
                            fillOpacity: 0.0,
                            circleDraggable: false,
                            editable: false,
                            strokeWeight: 2,
                            strokeOpacity: 1,
                            strokeColor: '#FC621A',
                        }),
                        mapMarker: new google.maps.Marker({
                            position: {
                                lat: f.lat,
                                lng: f.lng,
                            },
                            icon: this.mapMarkerIcon,
                            map: this.mapInstance,
                        }),
                    } as GeofenceUI;

                    this.createClientGeofenceUI(newGeofence);
                });
                // After loading the list, change map bounds to fit all geofences.
                this.isInitialLoad = false;
                this.filterGeofencesControl.setValue(null);

                if (this.geofenceList.length > 0) {
                    this.showAllClientGeofences();
                } else {
                    this.getLocation();
                }
            }

            this.isLoading = false;
        }, (err) => {
            this.isLoading = false;
            this.handleErrorMessage(err);
        });
    }

    updateClientGeofence(): void {
        this.formSubmitted = true;
        this.updateGeofences.updateValueAndValidity();
        let nameCtrl = this.updateGeofences.get("name");
        if(!nameCtrl.hasError('required')){
            let existing = this.geofenceList.filter(a => a.clockClientGeofenceID != this.selectedGeofenceUI.clockClientGeofenceID)
                                .find(x=>x.name == this.updateGeofences.value.name);
            if(existing!=null)  nameCtrl.setErrors({'duplicate': true});
            else                nameCtrl.setErrors(null);
        }
        if(!this.updateGeofences.valid) {
            this.selectedGeofenceUI.isHidden = true;
            return;
        } else {
            this.selectedGeofenceUI.isHidden = false;
        }
        
        this.isLoading = true;
        this.isSaving = true;
        Object.assign(this.selectedGeofenceUI, this.updateGeofences.value);
        const dto = this.prepareModel(this.selectedGeofenceUI);

        this.gService.updateClientGeofence(dto).subscribe((res) => {
            this.geofenceList.forEach(x => {
                if (x.clockClientGeofenceID === res.clockClientGeofenceID) {
                    x.clockClientGeofenceID = res.clockClientGeofenceID;
                    x.clientID = res.clientID;
                    x.lat = res.lat;
                    x.lng = res.lng;
                    x.radius = this.metersToFeet(res.radius);
                    x.name = res.name;
                    x.address = res.address;
                    x.isArchived = res.isArchived;
                    x.modified = res.modified;
                }
            });

            this.updateGeofenceList();
            this.isLoading = false;
            this.isSaving = false;
            this.hasChanges = true;
        }, (err) => {
            this.isLoading = false;
            this.hasChanges = false;
            this.isSaving = false;
            this.handleErrorMessage(err);
        });
    }

    deleteHiddenClientGeofences(): void {
        if(this.geofenceList.filter(f => f.isHidden).length > 0){
            let arr$ = [];
            
            for(var i=this.geofenceList.length-1;i>=0;i--){
                if(this.geofenceList[i].isHidden){
                    this.geofenceList[i].isArchived = true;
                    arr$.push( this.gService.updateClientGeofence( this.prepareModel(this.geofenceList[i])) );

                    const mapCircle = this.geofenceList[i].mapCircle;
                    const mapMarker = this.geofenceList[i].mapMarker;

                    if(mapCircle)   mapCircle.setMap(null);
                    if(mapMarker)   mapMarker.setMap(null);
                    this.geofenceList.splice(i, 1);
                }
            }

            forkJoin(...arr$).subscribe(x => {
            }, (err) => {
                this.handleErrorMessage(err);
            });
        }
    }

    deleteClientGeofence(): void {
        this.isLoading = true;
        // Make the api call to set the data record IsArchived value to true
        const dto = this.prepareModel(this.selectedGeofenceUI);
        dto.isArchived = true;

        this.gService.updateClientGeofence(dto).subscribe((res) => {
            const geofence = this.geofenceList.find(f => f.clockClientGeofenceID === this.selectedGeofenceUI.clockClientGeofenceID);
            const mapCircle = geofence.mapCircle;
            const mapMarker = geofence.mapMarker;
            const index = this.geofenceList.findIndex(f => f.clockClientGeofenceID === res.clockClientGeofenceID);

            mapCircle.setMap(null);
            mapMarker.setMap(null);
            this.geofenceList.splice(index, 1);
            this.updateGeofenceList();
            this.clearAllClientGeofenceSelections();

            this.isLoading = false;
            this.updateLastModified();
        }, (err) => {
            this.isLoading = false;
            this.handleErrorMessage(err);
        });
    }

    // UTILITY AND HELPERS

    private createForm(): void {
        this.updateGeofences = this.fb.group({
            name: this.fb.control('name', Validators.required),
            address: this.fb.control('address', Validators.required),
            radius: this.fb.control('radius', Validators.compose([Validators.required, Validators.pattern(/^[1-9]{1}\d*$/)])),
        });
    }

    private prepareModel(geofence: Geofence): Geofence {
        return {
            clockClientGeofenceID: geofence.clockClientGeofenceID,
            clientID: geofence.clientID,
            lat: geofence.lat,
            lng: geofence.lng,
            radius: this.feetToMeters(geofence.radius),
            name: geofence.name,
            address: geofence.address,
            isArchived: geofence.isArchived,
        };
    }

    private patchForm(geofence: Geofence) {
        this.updateGeofences.patchValue({
            name: geofence.name || '',
            address: geofence.address || '',
            lat: geofence.lat || '',
            lng: geofence.lng || '',
            radius: geofence.radius || '',
        });
    }

    private feetToMeters(value: number): number {
        return value / 3.28084;
    }

    private metersToFeet(value: number): number {
        return Math.round(value * 3.28084);
    }

    private updateGeofenceList(): void {
        this.geofenceCount = this.geofenceList.length;
        this.geofenceList.sort((a, b) => 0 - (a.name > b.name ? -1 : 1));
        this.geofences$.next(this.geofenceList);
        this.filterGeofencesControl.updateValueAndValidity();
        this.updateLastModified();
    }

    private _filterGeofences(value: string): GeofenceUI[] {
        const filterValue = value ? value.toLowerCase() : '';
        return this.geofenceList.filter(geofence =>
            geofence.name.toLowerCase().includes(filterValue) ||
            geofence.address.toLowerCase().includes(filterValue)
        );
    }

    private handleErrorMessage(err: any): void {
        if (err.error.errors && err.error.errors.length > 0) {
            this.msg.setTemporaryMessage(err.error.errors[0].msg, this.msg.messageTypes.error);
        } else this.msg.setTemporaryMessage(err.error.message, this.msg.messageTypes.error);
    }

    private updateLastModified() {
        this.gService.getClientLastModified(this.clientId).subscribe((res) => {
            this.lastModified = res;
        });
    }
}
