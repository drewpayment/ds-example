import { Component, OnInit, ViewChild } from '@angular/core';
import { IEEOCRaceEthnicCategoryData } from '@ajs/job-profiles/shared/models/eeoc-race-ethnic-categories-data.interface';
import { IEEOCJobCategoryData } from '@ajs/job-profiles/shared/models/eeoc-job-category-data.interface';
import { IEEOCLocationDataPerMultiClient } from '@ajs/job-profiles/shared/models/eeoc-location-data-per-client.interface';
import { IEEOCEmployeeInfo } from '@ds/core/employees/shared/employee-eeoc.model';
import { EmployeeEEOCApiService } from '@ds/core/employees/shared/employee-eeoc-api.service';
import { IClientData } from '@ajs/onboarding/shared/models';
import { Observable, of, combineLatest } from 'rxjs';
import { FormGroup, FormControl } from '@angular/forms';
import { isUndefinedOrNullOrEmptyString, isUndefinedOrNull } from '@util/ds-common';
import { isNullOrUndefined } from 'util';
import { EmpEeocService } from '../emp-eeoc.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { filter, switchMap, catchError, map, tap } from 'rxjs/operators';
import { LocationFilterPipe } from './location-filter-pipe';
import { truncate } from 'fs';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';

@Component({
    selector: 'ds-eeoc-validate-employees',
    templateUrl: './eeoc-validate-employees.component.html',
    styleUrls: ['./eeoc-validate-employees.component.scss']
})
export class EEOCValidateEmployeesComponent implements OnInit {
    thirdFormGroup: FormGroup;
    raceDropdownList: IEEOCRaceEthnicCategoryData[];
    jobCategoriesDropdownList: IEEOCJobCategoryData[];
    activeLocationsByClientId: IEEOCLocationDataPerMultiClient[];
    incompleteRecordsCount = 0;
    showComplete = true;
    showIncomplete = true;
    formInit: Observable<any>;
    isLoading = true;
    continueButtonClicked = false;
    genderDropdownList = [
        {
            designation: 'Male',
            value: 'M'
        },
        {
            designation: 'Female',
            value: 'F'
        }
    ];
    clientIds: number[];
    payrollIds: number[];
    displayedColumns: string[] = ['avi', 'name', 'number', 'clientCode', 'gender', 'race', 'location', 'jobCategory'];
    dataSource: MatTableDataSource<IEEOCEmployeeInfo> = new MatTableDataSource([]);
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    clients: IClientData[];

    constructor(
        private eeocService: EmployeeEEOCApiService,
        private service: EmpEeocService,
        private msg: DsMsgService,
        private locationsFilterPipe: LocationFilterPipe
    ) { }

    ngOnInit() {
        this.thirdFormGroup = new FormGroup({
            testControl: new FormControl('')
        });

        this.initializeListener();
    }

    private initializeListener(): void {
        const eeocJobCategories$ = this.eeocService.getEeocJobCategoriesList();
        const eeocRaceCategories$ = this.eeocService.getEeocRaceEthnicCategoriesList();

        this.formInit = combineLatest(this.service.clientList, this.service.locationsChanged).pipe(
        // this.formInit = this.service.clientList.pipe(
            filter((res) => !!res[0]),//don't continue if the clients(res[0]) given is null
            map(result => {
                this.clients = result[0];
                this.clientIds = [];
                this.clients.forEach((client) => this.clientIds.push(client.clientId));
            }),
            switchMap(_ => this.service.payrollList),
            tap((payrollList) => this.payrollIds = payrollList),
            switchMap(_ => eeocJobCategories$),
            tap((eeocJobCategories) => {
                // remove the first option 'None' as the user cannot select this anyway
                eeocJobCategories.splice(0, 1);
                this.jobCategoriesDropdownList = eeocJobCategories;
            }),
            switchMap(_ => eeocRaceCategories$),
            tap((eeocRaceCategories) => {
                // remove the first option 'prefer not to answer' as the user cannot select this anyway
                eeocRaceCategories.splice(0, 1);
                this.raceDropdownList = eeocRaceCategories;
            }),
            switchMap(_ => this.eeocService.getEeocLocationsListMultipleClients(this.clientIds, false, true).pipe(
                catchError(err => {
                    this.msg.setTemporaryMessage(err.error.message, this.msg.messageTypes.error);
                    return of([]);
                }),
            )),
            map((locationData: IEEOCLocationDataPerMultiClient[]) => this.activeLocationsByClientId = locationData),
            switchMap(_ => this.eeocService.getEeocEmployeesToValidate(this.payrollIds).pipe(
                catchError(() => of([])),
            )),
            map((eeocEmployeeInfo: IEEOCEmployeeInfo[]) => {
                if (!isNullOrUndefined(eeocEmployeeInfo)) {
                    // create new filter predicate to take only filters of 'valid' and 'invalid', and return
                    // corresponding data based on whether the row is valid or invalid
                    this.dataSource.filterPredicate = (data: any, filter: string) => {
                        // if the valid filter is given, return true for all data that is valid
                        if (filter.toLowerCase() === 'complete') return !data.isMissingEeocInfo;
                        // if the invalid filter is given, return true for all data that is invalid
                        else if (filter.toLowerCase() === 'incomplete') return data.isMissingEeocInfo;
                        // if filter is none, show no data
                        else if (filter.toLowerCase() === 'none') return false;
                    };

                    if (eeocEmployeeInfo.length > 0) this.isLoading = false;
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.data = eeocEmployeeInfo;
                    this.validateDataSource();
                    const tmpValidChk: boolean = (this.incompleteRecordsCount === 0);
                    this.service.eeocFormGroup.get('validateEmployees').setValue(tmpValidChk || null);
                }
                return true;
            }),
            catchError(err => {
                this.msg.setTemporaryMessage(err.error.message, this.msg.messageTypes.error);
                return of(false);
            }),
        );
    }

    getEmployees(): void {
        this.eeocService.getEeocEmployeesToValidate(this.payrollIds).subscribe(eeocEmployeeInfo => {
            if (!isNullOrUndefined(eeocEmployeeInfo)) {
                this.dataSource.data = eeocEmployeeInfo;
                this.validateDataSource();
                const tmpValidChk: boolean = (this.incompleteRecordsCount === 0);
                this.service.eeocFormGroup.get('validateEmployees').setValue(tmpValidChk || null);
            }
        });
    }

    validateDataSource(): void {
        // reset count variables
        this.incompleteRecordsCount = 0;
        // check each data row to see if any data is missing that should be filled in,
        if (!isNullOrUndefined(this.dataSource.data)) {
            this.dataSource.data.forEach(elem => {
                // if nothing is given or something other than M or F or is set to prefer not to answer
                // or maybe if the one given is not in the available list of options they can select
                // or set to None
                elem.isMissingEeocInfo = (
                                            isUndefinedOrNullOrEmptyString(elem.gender)         // no gender specified
                                            || (elem.gender !== 'M' && elem.gender !== 'F')     // gender is not 'M' or 'F'
                                            || (isUndefinedOrNull(elem.race)                    // no race specified
                                            || elem.race === 0)                                 // specified race is invalid
                                            || (isUndefinedOrNull(elem.locationId))             // no location specified
                                            || this.checkHasLocationNotInActiveLocations(elem)  // specified location is not active
                                            || (isUndefinedOrNull(elem.jobCategory)             // no job category specified
                                            || elem.jobCategory === 0)                          // job category is invalid
                                         );

                if (elem.isMissingEeocInfo)
                    this.incompleteRecordsCount++;
            });
            this.filterByCompletionStatus();
            const tmpValidChk: boolean = (this.incompleteRecordsCount === 0);
            this.service.eeocFormGroup.get('validateEmployees').setValue(tmpValidChk || null);
        }
    }

    filterByCompletionStatus(): void {
        this.dataSource.filter = '';
        if (this.showIncomplete && !this.showComplete) this.dataSource.filter = 'incomplete';
        else if (!this.showIncomplete && !this.showComplete) this.dataSource.filter = 'none';
        else if (!this.showIncomplete && this.showComplete) this.dataSource.filter = 'complete';
    }

    saveDropdownChange(row: IEEOCEmployeeInfo, event: any, dropdownCalled: string): void {
        // row passed from template to save that data, this is called onChange of select elements
        const adjustedRow = row;
        // create the object the backend recieves from out of the row variable
        if (dropdownCalled.toLowerCase() === 'gender') adjustedRow.gender = event.target.value;
        else if (dropdownCalled.toLowerCase() === 'race') adjustedRow.race = event.target.value;
        else if (dropdownCalled.toLowerCase() === 'location') adjustedRow.locationId = event.target.value;
        else if (dropdownCalled === 'jobCategory') adjustedRow.jobCategory = event.target.value;

        // refreshed employee data and puts data through ValidateDataSource
        this.eeocService.saveEmployeeEeocInfo(adjustedRow).subscribe(() => this.validateDataSource(),
            err => {
                this.msg.setTemporaryMessage(err.error.message, this.msg.messageTypes.error);
                this.getEmployees();
                this.validateDataSource();
            });
    }

    checkHasLocationNotInActiveLocations(row: IEEOCEmployeeInfo){
        //return true if location not found in array of available locations, false if it is found
        return this.locationsFilterPipe.transform(this.activeLocationsByClientId, row).findIndex(x => x.eeocLocationId == row.locationId) == -1
    }

    continueClicked(){
        if(this.incompleteRecordsCount > 0){
            this.continueButtonClicked = true;
            return false;
        }
    }
}
