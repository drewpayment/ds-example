import { Component, OnInit, Input } from '@angular/core';
import { EEOCLocationsModalComponent } from './eeoc-locations-modal.component';
import { ICountry, IState } from '@ajs/applicantTracking/shared/models';
import { ICounty } from '@ds/core/location';
import { LocationApiService } from '@ds/core/location/shared/location-api.service';
import { EmployeeEEOCApiService } from '@ds/core/employees/shared/employee-eeoc-api.service';
import { switchMap, tap } from 'rxjs/operators';
import { IClientRelationData, IClientData } from '@ajs/onboarding/shared/models';
import { IEEOCLocationDataPerMultiClient } from '@ajs/job-profiles/shared/models/eeoc-location-data-per-client.interface';
import { Observable } from 'rxjs';
import { EmpEeocService } from '../emp-eeoc.service';
import { AccountService } from '@ds/core/account.service';
import * as ang from 'angular';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';

@Component({
    selector: 'ds-eeoc-locations-trigger',
    templateUrl: './eeoc-locations-trigger.component.html',
})
export class EEOCLocationsTriggerComponent implements OnInit {
    clientRelation: IClientRelationData;
    eeocLocations: IEEOCLocationDataPerMultiClient[];
    countries: ICountry[];
    states: IState[];
    counties: ICounty[];
    isLoading$: Observable<any>;
    clientIdList: number[] = [];
    clientId: number;
    @Input() fromVLPage = false;
    @Input() saveSuccessClickTarget = null;

    constructor(
        private dialog: MatDialog,
        private locationsService: LocationApiService,
        private eeocApiService: EmployeeEEOCApiService,
        private empEeocService: EmpEeocService,
        private accountService: AccountService
    ) { }

    ngOnInit() {
        const countries$ = this.locationsService.getCountryList();
        const states$ = this.locationsService.getStatesByCountry(this.locationsService.defaults.countries.usa);
        const counties$ = this.locationsService.getCountiesByState(this.locationsService.defaults.states.michigan);
        const clients$ = this.eeocApiService.getClientRelatedClientIds(true);
        const userInfo$ = this.accountService.getUserInfo();

        this.isLoading$ = countries$.pipe(
            tap((countries) => this.countries = countries),
            switchMap(_ => states$),
            tap((states) => this.states = states),
            switchMap(_ => counties$),
            tap((counties) => this.counties = counties),
            switchMap(_ => userInfo$),
            tap((info) => this.clientId = info.clientId),
            switchMap(_ => clients$),
            tap((clientRelation) => {
                this.clientRelation = clientRelation;

                if (this.fromVLPage) clientRelation.clients.forEach((client: IClientData) => this.clientIdList.push(client.clientId));
                else this.clientIdList.push(this.clientId);
            }),
            switchMap(_ => this.eeocApiService.getEeocLocationsListMultipleClients(this.clientIdList, false, false)),
            tap((eeocLocations: IEEOCLocationDataPerMultiClient[]) => this.eeocLocations = eeocLocations),
        );
        this.isLoading$.subscribe();
    }

    openLocations(passedLocation: IEEOCLocationDataPerMultiClient = null): void {
        const config = new MatDialogConfig<any>();

        config.width = '800px';
        config.disableClose = true;
        config.panelClass = 'eeoc-locations';
        config.data = {
            states: this.states,
            counties: this.counties,
            clientRelation: this.clientRelation,
            eeocLocations: this.eeocLocations,
            countries: this.countries,
            passedLocation: null,
            fromVLPage: this.fromVLPage,
            clientId: this.clientId
        };

        if (!!location) config.data.passedLocation = passedLocation;

        const dialogRef = this.dialog.open(EEOCLocationsModalComponent, config);
        dialogRef.afterClosed().subscribe(() => {

            this.eeocApiService.getEeocLocationsListMultipleClients(this.clientIdList, false, false).subscribe((eeocLocations) => {
                this.eeocLocations = eeocLocations
                this.empEeocService.notifyLocationsChanged();

                //if from aspx page: make sure to click dom element hidden button that refreshes update panel
                if(this.saveSuccessClickTarget != null) ang.element(document).find('#' + this.saveSuccessClickTarget).click();
            });
        });
    }
}
