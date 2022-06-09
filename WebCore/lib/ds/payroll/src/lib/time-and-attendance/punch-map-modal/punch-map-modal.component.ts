import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { IEmployeePunchMapInfo, IEmployeePunchMapData } from '../shared/employee_punch_map_info.model';
import { ClockEmployeePunchDto } from '@ds/core/employee-services/models/clock-employee-punch-dto';
import { ClockService } from '@ds/core/employee-services/clock.service';
import { Observable, BehaviorSubject, forkJoin } from 'rxjs';
import { ClockEmployeeExceptionsService } from '../shared/clock_employee_exceptions.service';
import { switchMap, map } from 'rxjs/operators';
import { ClockExceptionEnum } from '@ajs/labor/models/client-exception-detail.model';
import { GeofenceService } from 'apps/ds-source/src/app/labor/services/geofence.service';
import { GeofenceMapComponent } from 'apps/ds-source/src/app/geofence/geofence-map/geofence-map.component';
import { MapMarkerType } from 'apps/ds-source/src/app/geofence/map-marker-type.enum';
import { MapMarker } from 'apps/ds-source/src/app/geofence/geofence-map/map-marker';
import { Geofence } from 'apps/ds-source/src/app/models';
import { MapCircle } from 'apps/ds-source/src/app/geofence/geofence-map/map-circle';
import { MapCircleType } from 'apps/ds-source/src/app/geofence/map-circle-type.enum';
import { MapCirclePipe } from 'apps/ds-source/src/app/geofence/map-circle.pipe';
import { MapMarkerPipe } from 'apps/ds-source/src/app/geofence/map-marker.pipe';
import { MapLocation } from 'apps/ds-source/src/app/geofence/map-location';
import { MapControlSettings } from 'apps/ds-source/src/app/geofence/map-control-settings';
import { MapLocationType } from 'apps/ds-source/src/app/geofence/map-location-type.enum';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';

@Component({
    selector: 'ds-punch-map-modal',
    templateUrl: './punch-map-modal.component.html',
    styleUrls: ['./punch-map-modal.component.css'],
})

export class PunchMapModalComponent implements OnInit {
    employeeName: string;
    isLoading = true;
    isChecked = false;
    punchMapInfo: IEmployeePunchMapInfo;
    displayedColumns: string[] = ['checkbox', 'day', 'date', 'time', 'status', 'detail'];
    dataSource = new BehaviorSubject<ClockEmployeePunchDto[]>(null);
    dataSource$ = this.dataSource.asObservable();
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(GeofenceMapComponent, { static: false }) geofenceMap: GeofenceMapComponent;
    formInit = new Observable<any>();
    punchMapData: ClockEmployeePunchDto[] | null;
    tmpIndex = 0;
    pagLength = 0;
    markerList = new BehaviorSubject<MapMarker[]>(null);
    markerList$ = this.markerList.asObservable();
    circleList = new BehaviorSubject<MapCircle[]>(null);
    circleList$ = this.circleList.asObservable();
    location = new BehaviorSubject<MapLocation>(null);
    location$ = this.location.asObservable();
    mapControlSettings: MapControlSettings;
    mapCirclePipe = new MapCirclePipe();
    mapMarkerPipe = new MapMarkerPipe();

    constructor(
        public dialogRef: MatDialogRef<PunchMapModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: IEmployeePunchMapData,
        private clockService: ClockService,
        private exceptionService: ClockEmployeeExceptionsService,
        private geoService: GeofenceService
    ) {
        this.mapControlSettings = {
            showCircleDrawingManager: false,
            showAddressSearch: false,
            showMapTypeSelector: true,
            showShowAll: true,
            showInfoWindow: false,
        };
    }

    ngOnInit() {
        this.setData(this.data);
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    setData(data: IEmployeePunchMapData): void {
        const punchIDList: number[] = data.employeePunchData.punchIdList;
        this.employeeName = data.employeePunchData.name;
        this.formInit = this.clockService.getPunchesByIdList(punchIDList, Number(data.employeeId)).pipe(
            switchMap((res) => {
                this.punchMapData = res;
                this.punchMapData.sort((a, b) => a.modifiedPunch < b.modifiedPunch ? -1 : a.modifiedPunch > b.modifiedPunch ? 1 : 0);

                const punchIds: number[] = [];
                this.punchMapData.forEach((punchData: ClockEmployeePunchDto) => {

                    punchIds.push(punchData.clockEmployeePunchId);
                });

                return forkJoin(
                    this.exceptionService.getExceptionsByPunchIds(
                        punchIds,
                        this.punchMapData[0] ? this.punchMapData[0].employeeId : +data.employeeId),
                    // this.geoService.getPunchesByPunchIds(
                    //     punchIds,
                    //     this.punchMapData[0] ? this.punchMapData[0].employeeId : +data.employeeId),
                    this.geoService.getClientGeofenceList(
                        this.punchMapData[0] ? this.punchMapData[0].clientId : +data.clientId),
                );
            }),
            map(res => {
                this.punchMapData.forEach(punchData => {
                    let hasMatch = false;

                    res[0].forEach(exception => {
                        if (exception.clockEmployeePunchId === punchData.clockEmployeePunchId) {
                            punchData.errorType = exception.clockExceptionTypeId;
                            if (!hasMatch &&
                                (exception.clockExceptionTypeId === ClockExceptionEnum.BadLocation
                                    || exception.clockExceptionTypeId === ClockExceptionEnum.NoLocation)) hasMatch = true;
                        }
                    });
                    if (!hasMatch && punchData.clockEmployeePunchLocationId) punchData.errorType = ClockExceptionEnum.GoodLocation;

                    if (this.isValidCoords(
                        { lat: punchData.clockEmployeePunchLocationLat, lng: punchData.clockEmployeePunchLocationLng }
                    )) {
                        punchData.isChecked = true;
                        punchData.isSelectable = true;
                    } else {
                        punchData.isChecked = false;
                        punchData.isSelectable = false;
                    }
                });

                const punchMapInfo: IEmployeePunchMapInfo = {
                    punchData: this.punchMapData ? this.punchMapData : [],
                    exceptionsInfo: res[0] ? res[0] : [],
                    geofenceData: res[1] ? res[1] : [],
                };

                this.punchMapInfo = punchMapInfo;
                this.pagLength = this.dataSource != null ? this.punchMapInfo.punchData.length : 0;
                this.paginatorClicked();
                this.updateMarkerList();
                this.updateCircleList();
                this.updateLocation();
                this.isLoading = false;

                return true;
            }),
        );
    }

    paginatorClicked(): void {
        const index = this.paginator == null ? 0 : this.paginator.pageIndex;
        const tmpPageSize = this.paginator == null ? 5 : this.paginator.pageSize;
        const tmpIndex = index * tmpPageSize;
        this.tmpIndex = tmpIndex;
        const filtered = this.punchMapInfo.punchData.slice(tmpIndex, tmpIndex + tmpPageSize);
        this.dataSource.next(filtered);
    }

    checkAll(): void {
        this.isChecked = !this.isChecked;
        this.punchMapData.forEach(punchData => {
            if (punchData.isSelectable === true) {
                punchData.isChecked = this.isChecked;
            }
        });
        this.updateMarkerList();
        this.updateLocation();
    }

    checkClicked(clockEmployeePunchId: number): void {
        this.isChecked = false;
        this.punchMapInfo.punchData.forEach(punchData => {
            if (punchData.clockEmployeePunchId === clockEmployeePunchId) {
                punchData.isChecked = !punchData.isChecked;
            }
        });

        this.updateMarkerList();
        this.updateLocation();
    }

    isValidCoords(position): boolean {
        return (
            position.lat !== null && position.lat >= -90 && position.lat <= 90 &&
            position.lng != null && position.lng >= -180 && position.lng <= 180
        );
    }

    updateMarkerList(): void {
        const tempMarkerList: MapMarker[] = [];

        this.punchMapInfo.punchData.forEach(punchData => {
            const marker = this.punchToMapMarker(punchData);
            if (this.isValidCoords(marker.position) && punchData.isChecked) {
                tempMarkerList.push(marker);
            }
        });

        this.punchMapInfo.geofenceData.forEach(geofence => {
            const marker = this.geofenceToMapMarker(geofence);
            if (this.isValidCoords(marker.position)) {
                tempMarkerList.push(marker);
            }
        });

        this.markerList.next(tempMarkerList);
    }

    updateCircleList(): void {
        const tempCircleList: MapCircle[] = [];

        this.punchMapInfo.geofenceData.forEach(geofence => {
            const circle = this.geofenceToMapCircle(geofence);

            if (this.isValidCoords(circle.center)) {
                tempCircleList.push(circle);
            }
        });

        this.circleList.next(tempCircleList);
    }

    updateLocation(): void {
        let mapLocation: MapLocation;

        if (this.circleList.value.length > 0 || this.markerList.value.length > 0) {
            mapLocation = {
                mapLocationType: MapLocationType.ShowAll,
            };
        } else {
            mapLocation = {
                mapLocationType: MapLocationType.Address,
                value: '1038 Lumina, 49428',
            };
        }

        this.location.next(mapLocation);

        if (this.geofenceMap) {
            this.geofenceMap.setLocation();
        }
    }

    punchToMapMarker(punch: ClockEmployeePunchDto): MapMarker {
        let markerType: MapMarkerType;

        if (punch.errorType === ClockExceptionEnum.BadLocation) {
            markerType = MapMarkerType.BadLocation;
        } else {
            markerType = MapMarkerType.GoodLocation;
        }

        const mapMarker: MapMarker = {
            position: { lat: punch.clockEmployeePunchLocationLat, lng: punch.clockEmployeePunchLocationLng },
            icon: this.mapMarkerPipe.transform(markerType),
            clickable: false,
        };

        return mapMarker;
    }


    geofenceToMapMarker(geofence: Geofence): MapMarker {
        const mapMarker: MapMarker = {
            position: { lat: geofence.lat, lng: geofence.lng },
            icon: this.mapMarkerPipe.transform(MapMarkerType.GeofenceLocation),
            clickable: false,
        };

        return mapMarker;
    }
    geofenceToMapCircle(geofence: Geofence): MapCircle {
        const mapCircleSettings = this.mapCirclePipe.transform(MapCircleType.StaticDefaultCircle);

        const mapCircle: MapCircle = {
            center: { lat: geofence.lat, lng: geofence.lng },
            radius: geofence.radius,
            relatedId: geofence.clockClientGeofenceID,
            description: geofence.address,
            strokeColor: mapCircleSettings.strokeColor,
            strokeOpacity: mapCircleSettings.strokeOpacity,
            strokeWeight: mapCircleSettings.strokeWeight,
            fillColor: mapCircleSettings.fillColor,
            fillOpacity: mapCircleSettings.fillOpacity,
            circleDraggable: mapCircleSettings.circleDraggable,
            editable: mapCircleSettings.editable,
            clickable: false,
        };

        return mapCircle;
    }
}
