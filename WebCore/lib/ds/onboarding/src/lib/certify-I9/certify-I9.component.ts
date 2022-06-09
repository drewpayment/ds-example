import { Component, OnInit, Input, Output, Inject, ViewChild } from '@angular/core';
import { OnboardingEmployeeService } from '../shared/onboarding-employee.service';
import { AccountService } from '@ds/core/account.service';
import { DOCUMENT } from '@angular/common';
import { Observable, from, iif, of, forkJoin, merge  } from 'rxjs';
import { map, tap, switchMap } from 'rxjs/operators';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { date } from '@ajs/applicantTracking/application/inputComponents';
import { HttpErrorResponse } from '@angular/common/http';
import { IClientData, IEmployeeOnboardingData, II9DocumentData, ICountryData, IStateData } from "@ajs/onboarding/shared/models";
import { OnboardingEmployee, DashboardState, IDashboardState } from 'apps/ds-company/ajs/models';
import { Client } from '@ds/core/employee-services/models';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { ICountry, IState } from "@ajs/applicantTracking/shared/models";
import { CountryStateService } from "@ajs/location/country-state/country-state.svc";
import { changeDrawerHeightOnOpen, matDrawerAfterHeightChange } from '@ds/core/ui/animations/drawer-auto-height-animation';

@Component({
  selector: 'ds-certify-i9',
  templateUrl: './certify-I9.component.html',
  styleUrls: ['./certify-I9.component.scss'],
  animations: [
    changeDrawerHeightOnOpen,
    matDrawerAfterHeightChange
  ]
})
export class CertifyI9Component implements OnInit {
    clientId: number;
    companyRootUrl: string;
    isLinkedToOnboarding: boolean;

    displayValue = "card";
    sortByValue = "employeeName";
    sortByOrder = "arrow_upward";
    dashboardState: DashboardState;    

    hasError = false;
    showBreadCrumb = false;
    onboardingUrl = ""; 
    searchWord:string = "";

    isOnboardingCompleted: boolean;
    relatedClients: Array<Client> = [];
    searchClientId: number;
    relatedClientId: number;
    dashboardTitle: string;

    isLoading: boolean = true;
    showFiles: boolean = false;
    allEmployees:Array<IEmployeeOnboardingData>;
    employees:Array<IEmployeeOnboardingData>;
    activeEmployee:IEmployeeOnboardingData = null;
    activeEmployeeId:number = 0;

    userinfo: UserInfo;
    searchText: string;
    displayedCount: number = 0;
    totalCount: number = 0;

    titles: Array<II9DocumentData> = [];
    countries: Array<ICountryData> = [];
    states: Array<IStateData> = [];

    constructor(private accountService: AccountService,
        private onboardingEmployeeService: OnboardingEmployeeService, 
        private countryStateService: CountryStateService,
        private store: DsStorageService,
        @Inject(DOCUMENT) private document: Document) {
        
    }
    ngOnInit() {
        this.isLoading = true;
        this.showFiles = false;
        this.searchText = '';
        this.allEmployees = [];
        this.activeEmployee = null;
        this.activeEmployeeId = 0;

        this.clientId = parseInt( (<HTMLInputElement>document.querySelector("input[id$='hidClientId']")).value );
        this.companyRootUrl =  (<HTMLInputElement>document.querySelector("input[id$='hidCompanyRootUrl']")).value ;
        this.isLinkedToOnboarding = !!(<HTMLInputElement>document.querySelector("input[id$='hidLinkedToOnboarding']")).value ;

        this.isLoading = true;
        this.onboardingUrl = this.companyRootUrl + "manage/onboarding/dashboard";

        this.dashboardState = new DashboardState(this.store);
        this.dashboardState.bindStorageState();

        // Sync with the dashboard options stored
        this.isOnboardingCompleted = this.dashboardState.isOnboardingCompleted;
        this.searchText         = this.dashboardState.searchText ? this.dashboardState.searchText : '';
        this.searchWord         = this.dashboardState.searchText ? this.dashboardState.searchText : '';
        this.searchClientId     = this.dashboardState.searchClientId ? this.dashboardState.searchClientId : 0;
        this.relatedClientId    = this.dashboardState.searchClientId;
        this.displayValue       = this.dashboardState.displayValue ? this.dashboardState.displayValue : 'card';
        this.sortByValue        = this.dashboardState.sortByValue ? this.dashboardState.sortByValue : 'employeeName';
        this.sortByOrder        = this.dashboardState.sortByOrder ? this.dashboardState.sortByOrder : 'arrow_upward';
        this.dashboardTitle     = this.dashboardState.isOnboardingCompleted ? 'Closed Records' : 'Current Records';

        this.checkCurrentUser().pipe(
            switchMap(userInfo => {
                return forkJoin(
                    this.onboardingEmployeeService.getRelatedClientsList(userInfo.lastClientId || userInfo.clientId),
                    this.onboardingEmployeeService.getActiveOnboardingEmployeeList(userInfo.lastClientId || userInfo.clientId),
                    this.onboardingEmployeeService.getI9DocumentList(),
                    from(<PromiseLike<Array<ICountry>>> this.countryStateService.getCountryList(false)),
                    from(<PromiseLike<Array<IState>>> this.countryStateService.getStatesForUSA()) )      
                }),
            tap( x => {
                this.relatedClients = x[0].sort((a,b) => a.clientCode.localeCompare(b.clientCode));
                this.allEmployees   = x[1].filter(this.defaultFilter);
                this.titles         = x[2].sort((a,b) => {
                    if(a.name.indexOf('.')>0 && a.name.indexOf('.')<5 && b.name.indexOf('.')>0 && b.name.indexOf('.')<5 ) 
                        return (parseInt( a.name.split('.')[0] ) < parseInt( b.name.split('.')[0] ) ? -1 : 1);
                    else return a.name.localeCompare(b.name) ;
                });
                this.countries      = x[3].sort((a,b) => a.name.localeCompare(b.name));
                this.states         = x[4].sort((a,b) => a.name.localeCompare(b.name));
                
                this.applyFilterParams();
                this.isLoading = false; 

                
            })
        ).subscribe();

        this.accountService.canPerformActions('Onboarding.OnboardingAdministrator').subscribe( canPerform => {
            if( this.isLinkedToOnboarding || (canPerform === true) )
                this.showBreadCrumb = true;
        }, (error: HttpErrorResponse) => {
            this.showBreadCrumb = false;
        });
    }

    checkCurrentUser(): Observable<UserInfo>{
        return iif(() => this.userinfo == null, 
            this.accountService.getUserInfo().pipe(tap(u => {
                this.userinfo = u;
            })),
            of(this.userinfo));
    }

    applyFilter(srhTxt:string){
        this.searchText = srhTxt;
        this.applyFilterParams();
    }

    updateRelatedClient = (event) => {
        const clientId = event.target.value;
        this.searchClientId = (clientId == "0") ? null : parseInt( clientId ) ;
        this.applyFilterParams();
    }

    defaultFilter = (item:IEmployeeOnboardingData) => {
        return (item.isI9Required && item.employeeSignoff && !item.isI9AdminComplete);
    }

    employeeFilter = (item:IEmployeeOnboardingData) => {
        var flag = true;

        if (!(item.isI9Required && item.employeeSignoff && !item.isI9AdminComplete))    flag = false;

        if (this.searchClientId) {
            if (this.clientId != this.searchClientId) flag = false;
        }

        if (this.searchText) {
            if (item.employeeName.toLowerCase().indexOf(this.searchText.toLowerCase()) == -1)  flag = false;
        }
        return flag;
    }

    applyFilterParams():void
    {
        let k = [];
        this.totalCount = 0;
        if(this.allEmployees && this.allEmployees.length > 0)
        {
            this.totalCount = this.allEmployees.length;

            k = this.allEmployees.filter( this.employeeFilter );
            k = k.sort( (a,b) => a.employeeName.localeCompare(b.employeeName) );
        }

        this.employees = k;
        this.displayedCount = k.length;
        this.clearDrawer();
    }

    toggleActiveEmployeeId(employeeId: number) {
        if (this.activeEmployeeId == employeeId) {
          this.clearDrawer();
        } else {
          this.activeEmployeeId = employeeId;
          this.activeEmployee = this.employees.find(x => {
            return (x.employeeId == this.activeEmployeeId);
          });
          this.onboardingEmployeeService.setActiveEmployee(this.activeEmployee);
        }    
    }
    
    clearDrawer() {
        this.activeEmployeeId = 0;
        this.activeEmployee = null;
    }

    i9StatusChanged(eventName){
        if(eventName == 'certified'){
            this.activeEmployee.isI9AdminComplete = true;
            var inx = this.allEmployees.findIndex(x => {
                return (x.employeeId == this.activeEmployeeId);
              });
            this.allEmployees.splice(inx,1);
            this.applyFilterParams();
        }
        this.clearDrawer();
    }
}