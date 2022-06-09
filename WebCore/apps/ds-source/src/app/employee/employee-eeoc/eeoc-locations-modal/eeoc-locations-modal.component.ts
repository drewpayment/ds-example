import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormControl, Validators, ValidatorFn, ValidationErrors } from '@angular/forms';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { LocationApiService } from '@ds/core/location/shared/location-api.service';
import { throwError } from 'rxjs';
import { EmployeeEEOCApiService } from '@ds/core/employees/shared/employee-eeoc-api.service';
import { IClientRelationData } from '@ajs/onboarding/shared/models';
import { catchError } from 'rxjs/operators';
import { IEEOCLocationDataPerMultiClient, IEEOCUnitAddress } from '@ajs/job-profiles/shared/models/eeoc-location-data-per-client.interface';
import { ICountry, IState } from '@ajs/applicantTracking/shared/models';
import { ICounty } from '@ds/core/location';
import { zipCodeValidator } from '../../shared/zip-code.validator';

@Component({
    selector: 'ds-eeoc-locations-modal',
    templateUrl: './eeoc-locations-modal.component.html',
    styleUrls: ['./eeoc-locations-modal.component.scss']
})
export class EEOCLocationsModalComponent implements OnInit {
    constructor(
        private dialogRef: MatDialogRef<EEOCLocationsModalComponent>,
        @Inject(MAT_DIALOG_DATA) private data: any,
        private msg: DsMsgService,
        private locationsService: LocationApiService,
        private eeocApiService: EmployeeEEOCApiService
    ) { }
    locationFormGroup: FormGroup;
    clientRelation: IClientRelationData;
    clientId: number;
    eeocLocations: IEEOCLocationDataPerMultiClient[];
    nullLocation = <IEEOCLocationDataPerMultiClient>{};
    countries: ICountry[];
    states: IState[];
    nullState = null;
    counties: ICounty[];
    selectedCounty: ICounty;
    nullCounty = null;
    showStates = false;
    showCounties = false;
    passedLocation: IEEOCLocationDataPerMultiClient;
    option: string;
    fromVLPage: boolean;

    ngOnInit() {
        // potential additions
        this.passedLocation = this.data.passedLocation;
        this.option = this.data.passedLocation == null ? 'Add' : 'Edit';

        this.states = this.data.states;
        this.showStates = this.states.length > 0;
        this.counties = this.data.counties;
        this.countries = this.data.countries;
        this.showCounties = this.counties.length > 0;
        this.clientRelation = this.data.clientRelation;
        this.eeocLocations = this.data.eeocLocations;
        this.fromVLPage = this.data.fromVLPage;
        this.clientId = this.data.clientId;

        this.initializeLocationForm();
        this.locationListener();
        if (!!this.passedLocation) {
            const passedLoc = this.eeocLocations.find((loc) => loc.eeocLocationId === this.passedLocation.eeocLocationId);
            this.locationFormGroup.get('location').setValue(passedLoc);
        } else this.locationFormGroup.get('location').setValue(this.nullLocation);
    }

    initializeLocationForm(): void {
        this.locationFormGroup = new FormGroup({
            client: new FormControl(null),
            location: new FormControl(null, Validators.required),
            name: new FormControl('', Validators.required),
            unitNumber: new FormControl(null),
            address1: new FormControl(null, Validators.required),
            address2: new FormControl(''),
            country: new FormControl(),
            city: new FormControl('', Validators.required),
            state: new FormControl(null, Validators.required),
            zip: new FormControl(null),
            county: new FormControl(null, Validators.required),
            isActive: new FormControl(true)
        }, [this.nameCityValidator(), zipCodeValidator()]);


        // add required Validator to client control if this is from the VL page
        if (this.fromVLPage && this.clientRelation.clients.length > 1) this.locationFormGroup.controls['client'].setValidators(Validators.required);
    }

    private nameCityValidator(): ValidatorFn{
        return (fg: FormGroup): ValidationErrors | null => {
            return fg.controls['name'].value == fg.controls['city'].value ? {'nameErr':true} : null
        }
    }

    locationListener(): void {
        this.locationFormGroup.get('location').valueChanges.subscribe((value) => {
            if (Object.keys(value).length === 0 && value.constructor === Object) {
                this.locationFormGroup.patchValue({
                    name: '',
                    zip: '',
                    client: null,
                    isActive: '',
                    unitNumber: '',
                    address1: '',
                    address2: '',
                    city: '',
                    state: this.states.find(s => s.stateId === this.locationsService.defaults.states.michigan),
                    country: this.countries.find(c => c.countryId === this.locationsService.defaults.countries.usa),
                    county: this.nullCounty
                });

            } else {
                const tmpUnitAddr: IEEOCUnitAddress = {
                    addressId: value.unitAddress ? value.unitAddress.addressId : null,
                    addressLine1: value.unitAddress ? value.unitAddress.addressLine1 : null,
                    addressLine2: value.unitAddress ? value.unitAddress.addressLine2 : null,
                    city: value.unitAddress ? value.unitAddress.city : null,
                    countryId: value.unitAddress ? value.unitAddress.countryId : this.locationsService.defaults.countries.usa,
                    countyId: value.unitAddress ? value.unitAddress.countyId : null,
                    stateId: value.unitAddress ? value.unitAddress.stateId : this.locationsService.defaults.states.michigan,
                    zipCode: value.unitAddress ? value.unitAddress.zipCode : null,
                };

                this.locationFormGroup.patchValue({
                    name: value.eeocLocationDescription,
                    zip: tmpUnitAddr.zipCode,
                    client: value.clientId,
                    isActive: value.isActive,
                    unitNumber: value.unitNumber,
                    address1: tmpUnitAddr.addressLine1,
                    address2: tmpUnitAddr.addressLine2,
                    city: tmpUnitAddr.city,
                    country: this.countries.find((ct) => ct.countryId === tmpUnitAddr.countryId),
                    county: this.counties.find((cs) => cs.countyId === tmpUnitAddr.countyId),
                    state: this.states.find((s) => s.stateId === tmpUnitAddr.stateId),
                    unitAddress: tmpUnitAddr,
                    unitAddressId: tmpUnitAddr.addressId
                });
                this.updateStates();
            }
        });
    }

    updateStates(): void {
        const value = this.locationFormGroup.get('country').value;
        const selectedLocation = this.locationFormGroup.value;
        this.locationsService.getStatesByCountry(value.countryId)
            .subscribe(states => {
                this.states = states;
                this.showStates = this.states.length > 0;
                if (value.countryId === this.locationsService.defaults.countries.usa) {
                    if (!!selectedLocation.location.unitAddress && !!selectedLocation.location.unitAddress.stateId) {
                        this.locationFormGroup.patchValue({
                            country: this.countries.find(c => c.countryId === value.countryId),
                            state: this.states.find(s => s.stateId === selectedLocation.location.unitAddress.stateId)
                        });
                    } else {
                        this.locationFormGroup.patchValue({
                            country: this.countries.find(c => c.countryId === value.countryId),
                            state: this.states.find(s => s.stateId === this.locationsService.defaults.states.michigan)
                        });
                    }
                } else {
                    this.locationFormGroup.patchValue({
                        country: this.countries.find(c => c.countryId === value.countryId),
                        state: this.nullState
                    });
                }
                this.updateCounty();
            });
    }

    updateCounty(): void {
        const value = this.locationFormGroup.get('state').value;

        if (value === this.nullState || (Object.keys(value).length === 0 && value.constructor === Object)) {
            this.counties = <ICounty[]>{};
            this.showCounties = this.counties.length > 0;
            this.selectedCounty = this.nullCounty;
            this.locationFormGroup.patchValue({
                county: this.nullCounty
            });
        } else {
            this.locationsService.getCountiesByState(value.stateId).subscribe((counties) => {
                this.counties = counties;
                this.showCounties = this.counties.length > 0;
                const selectedLocation = this.locationFormGroup.value;
                if (!!selectedLocation && counties != null && counties.length > 0 && selectedLocation.location.unitAddress != null
                    && (selectedLocation.location.unitAddress.countyId != null)) {
                    this.locationFormGroup.patchValue({
                        county: this.counties.find(ct => ct.countyId === selectedLocation.location.unitAddress.countyId)
                    });
                } else {
                    this.locationFormGroup.patchValue({
                        county: this.nullCounty
                    });
                }
            });
        }
    }

    saveForm(): void {
        this.locationFormGroup.markAllAsTouched();
        if (this.locationFormGroup.invalid && this.locationFormGroup.controls['isActive'].value == true) return;

        const selectedLocation = this.locationFormGroup.value;
        this.locationFormGroup.value.Location = "Test";

        let eeocLocationId = null;
        let unitAddressId = null;
        if (!(Object.keys(selectedLocation).length === 0 && selectedLocation.constructor === Object)) {
            eeocLocationId = selectedLocation.location.eeocLocationId;
            unitAddressId = selectedLocation.location.unitAddress == null ? null : selectedLocation.location.unitAddress.addressId;
        }

        const tmpLocation: IEEOCLocationDataPerMultiClient = {
            clientId: selectedLocation.client,
            eeocLocationId: eeocLocationId,
            eeocLocationDescription: selectedLocation.name,
            unitNumber: selectedLocation.unitNumber,
            unitAddress: {
                addressLine1: selectedLocation.address1,
                addressLine2: selectedLocation.address2,
                city: selectedLocation.city,
                countryId: selectedLocation.country.countryId,
                countyId: selectedLocation.county ? selectedLocation.county.countyId : null,
                stateId: selectedLocation.state.stateId,
                zipCode: selectedLocation.zip,
                addressId: unitAddressId
            },
            isActive: selectedLocation.isActive,
            isHeadquarters: selectedLocation.isHeadquarters
        };

        // if from employee page, set client to be the users current client
        if (!this.fromVLPage || this.clientRelation.clients.length == 1) tmpLocation.clientId = this.clientId;
        this.eeocApiService.saveEEOCLocation(tmpLocation).pipe(catchError((e) => {
            this.msg.showWebApiException(e);
            return throwError(e.statusText);
        })).subscribe(() => {
            this.msg.setTemporarySuccessMessage('Your Location has been saved successfully.', 5000);
            this.close();
        });
    }

    close(): void {
        this.dialogRef.close(null);
    }
}
