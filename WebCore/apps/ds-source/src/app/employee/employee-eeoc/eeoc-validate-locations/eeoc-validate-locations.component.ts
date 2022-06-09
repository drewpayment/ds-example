import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { EmployeeEEOCApiService } from '@ds/core/employees/shared/employee-eeoc-api.service';
import { IClientData } from '@ajs/onboarding/shared/models';
import {
    IEEOCLocationDataPerMultiClient, IEEOCUnitAddress,
    IFlattenedEEOCLocationDataPerMultiClient
} from '@ajs/job-profiles/shared/models/eeoc-location-data-per-client.interface';
import { LocationApiService } from '@ds/core/location';
import { IState, ICountry } from '@ajs/applicantTracking/shared/models';
import { EEOCLocationsTriggerComponent } from '../eeoc-locations-modal/eeoc-locations-trigger.component';
import { isUndefinedOrNullOrEmptyString } from '@util/ds-common';
import { EmpEeocService } from '../emp-eeoc.service';
import { Observable, of, combineLatest } from 'rxjs';
import { filter, switchMap, map, catchError } from 'rxjs/operators';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { isNullOrUndefined } from '../../../../../../../lib/utilties';

@Component({
    selector: 'ds-eeoc-validate-locations',
    templateUrl: './eeoc-validate-locations.component.html',
    styleUrls: ['./eeoc-validate-locations.component.scss']
})
export class EEOCValidateLocationsComponent implements OnInit {
    secondFormGroup: FormGroup;
    hasErr = false;
    isLoading = true;
    clientIds: number[];
    dataSourceLength: number;
    locations: IEEOCLocationDataPerMultiClient[];
    states: IState[];
    countries: ICountry[];
    showValid = true;
    showInvalid = true;
    invalidCount = 0;
    formInit: Observable<any>;
    clients: IClientData[];
    selectedClientIds: number[];
    clientCodesWithNoLocations: string[] = [];
    continueButtonClicked = false;
    displayedColumns:
        string[] = ['isValidLocation', 'clientCode', 'eeocLocationDescription',
            'unitNumber', 'country', 'address', 'city', 'state', 'zipCode', 'isActive', 'actions'];

    dataSource: MatTableDataSource<IFlattenedEEOCLocationDataPerMultiClient> = new MatTableDataSource([]);
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(EEOCLocationsTriggerComponent, { static: true }) modalTrigger: EEOCLocationsTriggerComponent;

    constructor(
        private eeocApiService: EmployeeEEOCApiService,
        private locationService: LocationApiService,
        private service: EmpEeocService,
        private msg: DsMsgService,

    ) { }

    ngOnInit() {
        this.secondFormGroup = new FormGroup({
            testControl: new FormControl('')
        });

        this.locationService.getStatesForUSA().subscribe(states => this.states = states);
        this.locationService.getCountryList().subscribe(countries => this.countries = countries);

        this.initializeListener();
    }

    private initializeListener(): void {
        // combineLatest will wait for all observables to emit a value before continuing down the pipe
        this.formInit = combineLatest(this.service.clientList, this.service.selectedClientIdsList, this.service.locationsChanged).pipe(
            // clientList and locationsChanged emit values onInit, we are waiting for
            // selectedClientIdsList from the first component before continuing
            // down the pipeline, locationsChanged is just here so when the locationsModal
            // emits that locations are changed, we can run through this pipeline again and update the locations data.
            filter((clients) => !!clients),
            switchMap(result => {
                this.clients = result[0]; // set clients to this.service.clientList observables last value
                // set selectedClientIds to this.service.selectedClientIdsList observables last value
                this.selectedClientIds = result[1];
                this.clientIds = [];
                this.clients.forEach(x => { // filter out clients that are not selected
                    if (this.selectedClientIds.indexOf(x.clientId) !== -1) this.clientIds.push(x.clientId);
                });
                return this.eeocApiService.getEeocLocationsListMultipleClients(this.clientIds, false, false).pipe(
                    catchError(() => {
                        this.hasErr = true;
                        this.isLoading = false;
                        return of([]);
                    }),
                );
            }),
            map((eeocLocations) => {
                this.locations = eeocLocations;
                this.validateLocations(); // check for any errors in the rows, and set isValidLocation for each row
                const tmpValidChk: boolean = ((this.invalidCount === 0)
                    && (this.clientCodesWithNoLocations.length === 0));
                this.service.eeocFormGroup.get('validateLocations').setValue(tmpValidChk || null);

                const adjustedRowArray: IFlattenedEEOCLocationDataPerMultiClient[] = [];
                this.locations.forEach(row => {
                    const tmpUnitAddr: IEEOCUnitAddress = {
                        addressId: row.unitAddress ? row.unitAddress.addressId : null,
                        addressLine1: row.unitAddress ? row.unitAddress.addressLine1 : null,
                        addressLine2: row.unitAddress ? row.unitAddress.addressLine2 : null,
                        city: row.unitAddress ? row.unitAddress.city : null,
                        countryId: row.unitAddress ? row.unitAddress.countryId : null,
                        countyId: row.unitAddress ? row.unitAddress.countyId : null,
                        stateId: row.unitAddress ? row.unitAddress.stateId : null,
                        zipCode: row.unitAddress ? row.unitAddress.zipCode : null,
                    };

                    const tmpClient = this.clients.find(x => x.clientId === row.clientId);
                    const clientCode = !!tmpClient ? tmpClient.clientCode : null;

                    const adjustedRowObj: IFlattenedEEOCLocationDataPerMultiClient = {
                        eeocLocationId: row.eeocLocationId,
                        eeocLocationDescription: row.eeocLocationDescription,
                        clientCode: clientCode,
                        clientId: row.clientId,
                        isActive: row.isActive,
                        unitNumber: row.unitNumber,
                        country: tmpUnitAddr.countryId ? this.countries
                            .find((country) => country.countryId === tmpUnitAddr.countryId).name : null,
                        address: tmpUnitAddr.addressLine1,
                        city: tmpUnitAddr.city,
                        state: tmpUnitAddr.stateId ? this.states
                            .find(x => x.stateId === tmpUnitAddr.stateId).abbreviation : null,
                        zipCode: tmpUnitAddr.zipCode,
                        isValidLocation: row.isValidLocation,
                        tooltipError: row.tooltipError
                    };
                    adjustedRowArray.push(adjustedRowObj);
                });
                this.dataSourceLength = adjustedRowArray.length;
                this.isLoading = false;


                this.dataSource.filterPredicate = (data: any, filter: string) => {
                    // if the valid filter is given, return true for all data that is valid
                    if (filter.toLowerCase() === 'valid') return data.isValidLocation;
                    // if the invalid filter is given, return true for all data that is invalid
                    else if (filter.toLowerCase() === 'invalid') return !data.isValidLocation;
                    // if filter is none, show no data
                    else if (filter.toLowerCase() === 'none') return false;
                };

                this.dataSource.paginator = this.paginator;
                this.dataSource.data = adjustedRowArray;
                return this.dataSource;
            }),
            catchError(err => {
                console.log(err);
                this.isLoading = false;
                this.msg.setTemporaryMessage(err, this.msg.messageTypes.error);
                this.hasErr = true;
                return of(false);
            })
        );
    }

    editLocation(location: IEEOCLocationDataPerMultiClient): void {
        this.modalTrigger.openLocations(location);
    }

    filterByValidityStatus(): void {
        this.dataSource.filter = '';
        if (this.showInvalid && !this.showValid) this.dataSource.filter = 'invalid';
        else if (!this.showInvalid && !this.showValid) this.dataSource.filter = 'none';
        else if (!this.showInvalid && this.showValid) this.dataSource.filter = 'valid';
    }

    validateLocations(): void {
        if (this.locations != null) {
            this.invalidCount = 0;
            this.locations.forEach(loc => {
                loc.tooltipError = '';
                loc.isValidLocation = true; // assume true, unless if fails one of the below cases and is set to false;

                if (loc.isActive) { // check validity of only active rows
                    this.locations.forEach(loc2 => { // validation compared with other rows
                        if (loc2 !== loc && loc2.isActive) { // rows are not the same
                            if (loc.eeocLocationDescription === loc2.eeocLocationDescription && (!this.hasSameAddress(loc, loc2) || loc.unitNumber !== loc2.unitNumber )) { // check for same name AND (diff address OR diff unitNumber)
                                loc.isValidLocation = false;
                                if (!loc.tooltipError.includes('Duplicate location')){ //if tooltip doesn't already have this error message
                                    loc.tooltipError += 'Duplicate location name with differing addresses or unit numbers, '; //add it
                                }
                            }
                            else if (this.hasSameAddress(loc, loc2) && (loc.eeocLocationDescription !== loc2.eeocLocationDescription || loc.unitNumber !== loc2.unitNumber)) { // check for same address AND (diff name OR diff unitNumber)
                                loc.isValidLocation = false;
                                if(!loc.tooltipError.includes('Duplicate address')){ //if tooltip doesn't already have this error message
                                    loc.tooltipError += 'Duplicate address with differing location names or unit numbers, ';//add it
                                }
                            }
                            else if (!isUndefinedOrNullOrEmptyString(loc.unitNumber) && !isUndefinedOrNullOrEmptyString(loc2.unitNumber)){ //make sure both locations first have unit numbers
                                if(loc.unitNumber === loc2.unitNumber && (loc.eeocLocationDescription !== loc2.eeocLocationDescription || !this.hasSameAddress(loc,loc2)) ){ //check for same unit number AND (diff name OR diff address)
                                    loc.isValidLocation = false;
                                    if(!loc.tooltipError.includes('Duplicate unit number')){ //if tooltip doesn't already have this error message
                                        loc.tooltipError += 'Duplicate unit number with differing location names or addresses, '; //add it
                                    }
                                }
                            }
                        }
                    });

                    if (!isNullOrUndefined(loc.unitAddress)) { // validation within the row itself
                        // description is same name as city
                        if (loc.unitAddress.city.toLowerCase() === loc.eeocLocationDescription
                            .toLowerCase()) {
                            loc.isValidLocation = false;
                            loc.tooltipError += 'Location name cannot match city, ';
                        }
                        if (isNullOrUndefined(loc.unitAddress.countryId) ||
                            isUndefinedOrNullOrEmptyString(loc.unitAddress.city) ||
                            isNullOrUndefined(loc.unitAddress.stateId) ||
                            isUndefinedOrNullOrEmptyString(loc.unitAddress.zipCode)) {
                            loc.isValidLocation = false;
                            loc.tooltipError += 'Missing location data, ';
                        }
                    } else {
                        loc.isValidLocation = false;
                        loc.tooltipError += 'Missing location data, ';
                    }
                    if (!loc.isValidLocation) this.invalidCount++;
                    // format tooltip string to remove trailing comma and space
                    loc.tooltipError = loc.tooltipError !== '' ?
                        loc.tooltipError.slice(0, loc.tooltipError.length - 2) : null;
                }
            });

            this.clientCodesWithNoLocations.length = 0;
            this.clientIds.forEach(clientId => {
                let clientHasAtLeastOneActiveLoc = false;
                this.locations.forEach(loc => { // check to see if the client has at least one active location
                    if (clientId === loc.clientId && loc.isActive) clientHasAtLeastOneActiveLoc = true;
                });
                // if they dont have any locations available
                if (!clientHasAtLeastOneActiveLoc)
                    this.clientCodesWithNoLocations.push(this.clients.find(x => x.clientId === clientId).clientCode);
            });
        }
    }

    hasSameAddress(loc1: IEEOCLocationDataPerMultiClient, loc2: IEEOCLocationDataPerMultiClient): boolean {
        if(loc1.unitAddress != null && loc2.unitAddress != null){



            if ((loc1.unitAddress.addressLine1!== loc2.unitAddress.addressLine1)
            || (loc1.unitAddress.addressLine2 ? loc1.unitAddress.addressLine2 : "" !== loc2.unitAddress.addressLine2 ? loc2.unitAddress.addressLine2 : "")
            || (loc1.unitAddress.city !== loc2.unitAddress.city)
            || (loc1.unitAddress.countryId !== loc2.unitAddress.countryId)
            || (loc1.unitAddress.countyId !== loc2.unitAddress.countyId)
            || (loc1.unitAddress.stateId !== loc2.unitAddress.stateId)
            || (loc1.unitAddress.zipCode !== loc2.unitAddress.zipCode)) {
                return false;
            }
        }
        else{
            return false;
        }
        return true;
    }

    nextClick() {
        if(this.invalidCount != 0 || this.clientCodesWithNoLocations.length > 0){
            this.continueButtonClicked = true;
            return false;
        }
    }

    backClick() {
        this.service.updateDisableYearList(false);
    }
}
