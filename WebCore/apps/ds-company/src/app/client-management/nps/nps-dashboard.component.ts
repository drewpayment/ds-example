import { Component, OnInit, ViewChild } from '@angular/core';
import { tap } from 'rxjs/operators';
import { UserInfo, UserType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientSelectorService } from '@ds/core/ui/menu-wrapper-header/ng-client-selector/client-selector.service';
import { Store } from '@ngrx/store';
import { UserState } from '@ds/core/users/store/user.reducer';
import { forkJoin, Observable, Subject } from 'rxjs';
import { NpsService } from '../../services/nps.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { IEnabledNpsFilter, IOption, NpsFilterType, NpsResponse, NpsResponseFilter, NpsResponseType, NpsScore, NPS_DASHBOARD_FILTERS_KEY } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
    selector: 'ds-nps-dashboard',
    templateUrl: './nps-dashboard.component.html',
    styleUrls: ['./nps-dashboard.component.scss'],
})
export class NPSDashboardComponent implements OnInit {
    destroy$ = new Subject();

    private _user: UserInfo;
    isLoading: boolean = true;
    isFiltersOff: boolean = true;
    showFilters: boolean = false;
    scoresLoaded: boolean;
    filterForm: FormGroup;
    readonly momentFormatString = 'MM/DD/YYYY';

    get EndDate() { return this.filterForm.controls.EndDate as FormControl; }
    get StartDate() { return this.filterForm.controls.StartDate as FormControl; }
    get Organization() { return this.filterForm.controls.Organization as FormControl; }
    get ClientCode() { return this.filterForm.controls.ClientCode as FormControl; }
    get ClientStatus() { return this.filterForm.controls.ClientStatus as FormControl; }
    get ResolutionStatus() { return this.filterForm.controls.ResolutionStatus as FormControl; }
    get UserTypeEmployee() { return this.filterForm.controls.UserTypeEmployee as FormControl; }
    get UserTypeSupervisor() { return this.filterForm.controls.UserTypeSupervisor as FormControl; }
    get UserTypeCompanyAdmin() { return this.filterForm.controls.UserTypeCompanyAdmin as FormControl; }
    get ResponseTypeDetractor() { return this.filterForm.controls.ResponseTypeDetractor as FormControl; }
    get ResponseTypePassive() { return this.filterForm.controls.ResponseTypePassive as FormControl; }
    get ResponseTypePromoter() { return this.filterForm.controls.ResponseTypePromoter as FormControl; }
    get ShowResponsesWithoutFeedback() { return this.filterForm.controls.ShowResponsesWithoutFeedback as FormControl; }
    get SearchFeedback() { return this.filterForm.controls.SearchFeedback as FormControl; }

    /** Mat Table variables */
    npsResponseSource = new MatTableDataSource<NpsResponse>([]);
    npsResponseColumns = ['isResolved', 'userTypeId', 'score', 'responseDate', 'clientCode', 'employeeFirstName', 'feedback'];

    @ViewChild(MatPaginator, {static: false}) set contentForPaginator(npsResponsePaginator: MatPaginator) {
        this.npsResponseSource.paginator = npsResponsePaginator;
    }

    @ViewChild(MatSort, {static: false}) set contentForSort(npsResponseSort: MatSort) {
        this.npsResponseSource.sort = npsResponseSort;
    }

    blankScore =
    {
        userTypeId: 0,
        title: "",
        isApplicable: false,
        score: 0,
        numerator: 0,
        denominator: 0,
        percentage: 0,
        colorCode: ""
    } //prevents errors when trying to access the score properties while loading

    blocker = false;
    filters: NpsResponseFilter;

    npsScores: NpsScore[];
    npsAllScore: NpsScore = this.blankScore;
    npsCaScore: NpsScore  = this.blankScore;
    npsSupScore: NpsScore  = this.blankScore;
    npsEmpScore: NpsScore  = this.blankScore;

    clientOrganizationDropdownName: string = 'Organization';
    clientOrganizations: IOption[] = [];
    changeClientOrganization$: Observable<any>;

    clientDropdownName: string = 'ClientCode';
    clients: IOption[] = [];
    changeClient$: Observable<any>;

    clientStatuses: any[] = [
        { id: 1, name: 'Active' },
        { id: 3, name: 'InfrequentProcessing' },
        { id: 5, name: 'Parallel' },
        { id: 2, name: 'Seasonal' },
        { id: 4, name: 'Terminated' },
        { id: 6, name: 'TerminatedWithAccess' }
      ];

    resolutionStatuses: any[] = [
        { id: true, name: 'Resolved' },
        { id: false, name: 'Not Resolved' }
    ];

    enabledFilters: Array<IEnabledNpsFilter>=[];

    defaultOption: IOption = {
        filter: "",
        id: null
    };

    constructor(
        private accountService: AccountService,
        private npsService: NpsService,
        private ngxMsgSvc: NgxMessageService,
        private dialog: MatDialog,
        private route: ActivatedRoute,
        private router: Router,
        private clientSelectorSvc: ClientSelectorService,
        private store: Store<UserState>,
        public fb: FormBuilder,
        private storeService: DsStorageService,
    ) {
        this.filterForm = fb.group({
            StartDate: fb.control(null, { validators: Validators.required, updateOn: 'blur' }),
            EndDate: fb.control(null, { validators: Validators.required, updateOn: 'blur' }),
            Organization: fb.control(null),
            ClientCode: fb.control(null),
            ClientStatus: fb.control(null),
            ResolutionStatus: fb.control(null),
            UserTypeEmployee: fb.control(true),
            UserTypeSupervisor: fb.control(true),
            UserTypeCompanyAdmin: fb.control(true),
            ResponseTypeDetractor: fb.control(true),
            ResponseTypePassive: fb.control(true),
            ResponseTypePromoter: fb.control(true),
            ShowResponsesWithoutFeedback: fb.control(true),
            SearchFeedback: fb.control(null)
        });

        this.npsService.clientOrganizations$.pipe().subscribe(res => {
            this.isLoading = true;
            res.forEach( (element) => {
                this.clientOrganizations.push({id: element.clientOrganizationId, filter: element.clientOrganizationName})
            });
            this.isLoading = false;
        });

        this.npsService.clients$.pipe().subscribe(res => {
            this.isLoading = true;
            res.forEach( (element) => {
                this.clients.push({id: element.clientId, filter: `${element.clientName} (${element.clientCode} - ${element.clientStatusCode})`})
            });
            this.isLoading = false;
        });

        this.npsService.npsResponses$.pipe().subscribe(res => {
            this.isLoading = true;
            this.npsResponseSource.data = res;
            //this.setNpsResponseSourceAttr();
            this.isLoading = false;
        });

        this.npsService.npsScores$.pipe().subscribe(res => {
            this.isLoading = true;
            this.npsScores = res;
            this.npsAllScore = this.npsScores.find(x => x.userTypeId == 1)
            this.npsCaScore = this.npsScores.find(x => x.userTypeId == 2)
            this.npsEmpScore = this.npsScores.find(x => x.userTypeId == 3)
            this.npsSupScore = this.npsScores.find(x => x.userTypeId == 4)
            this.isLoading = false;
            this.scoresLoaded = true;
        });

        this.changeClientOrganization$ = this.filterForm.get('Organization').valueChanges.pipe(tap(x => this.changeClientOrganization(x)));
        this.changeClient$ = this.filterForm.get('ClientCode').valueChanges.pipe(tap(x => this.changeClient(x)));
    }

    ngOnInit() {
        this.npsScores = [];

        forkJoin(this.npsService.fetchOrganizationsList(), this.npsService.fetchClientsList()).subscribe(x=> {
            const storeResult = this.storeService.get(NPS_DASHBOARD_FILTERS_KEY);
            if (storeResult.success) {
                this.filters = storeResult.data;

                if (!this.filters.userTypes.find(x => x == UserType.employee))
                    this.filters.userTypes.push(UserType.employee);

                if (!this.filters.userTypes.find(x => x == UserType.supervisor))
                    this.filters.userTypes.push(UserType.supervisor);

                if (!this.filters.userTypes.find(x => x == UserType.companyAdmin))
                    this.filters.userTypes.push(UserType.companyAdmin);

                var startDate = new Date()
                startDate.setDate(startDate.getDate() - 90);
                var endDate = new Date();

                this.filters.hideResponsesWithoutFeedback = false;
                this.filters.searchFeedback = null;

                if (this.filters.fromDate != null)
                    this.StartDate.setValue(this.filters.fromDate);
                else
                    this.StartDate.setValue(startDate);

                if (this.filters.toDate != null)
                    this.EndDate.setValue(this.filters.toDate);
                else
                    this.EndDate.setValue(endDate);

                if (this.filters.clientOrganizationId != null && !!this.clientOrganizations && this.clientOrganizations.length > 0 ) {
                    this.Organization.setValue({id: this.filters.clientOrganizationId, filter: this.clientOrganizations.find(x=>x.id == this.filters.clientOrganizationId).filter});
                }

                if (this.filters.clientId != null && !!this.clients && this.clients.length > 0 ){
                    this.ClientCode.setValue({id: this.filters.clientId, filter: this.clients.find(x=>x.id == this.filters.clientId).filter});
                }

                if (this.filters.clientStatus != null && !!this.clientStatuses && this.clientStatuses.length > 0 ){
                    this.ClientStatus.setValue(this.filters.clientStatus);
                }

                if (this.filters.isResolved != null && !!this.resolutionStatuses && this.resolutionStatuses.length > 0 ){
                    this.ResolutionStatus.setValue(this.filters.isResolved);
                }

                if (this.filters.userTypes.find(x => x == UserType.employee))
                    this.UserTypeEmployee.setValue(true);
                else
                    this.UserTypeEmployee.setValue(false);

                if (this.filters.userTypes.find(x => x == UserType.supervisor))
                    this.UserTypeSupervisor.setValue(true);
                else
                    this.UserTypeSupervisor.setValue(false);

                if (this.filters.userTypes.find(x => x == UserType.companyAdmin))
                    this.UserTypeCompanyAdmin.setValue(true);
                else
                    this.UserTypeCompanyAdmin.setValue(false);

                if (this.filters.responseTypes.find(x => x == NpsResponseType.Detractor))
                    this.ResponseTypeDetractor.setValue(true);
                else
                    this.ResponseTypeDetractor.setValue(false);

                if (this.filters.responseTypes.find(x => x == NpsResponseType.Passive))
                    this.ResponseTypePassive.setValue(true);
                else
                    this.ResponseTypePassive.setValue(false);

                if (this.filters.responseTypes.find(x => x == NpsResponseType.Promoter))
                    this.ResponseTypePromoter.setValue(true);
                else
                    this.ResponseTypePromoter.setValue(false);

                // if (this.filters.hideResponsesWithoutFeedback != null && this.filters.hideResponsesWithoutFeedback == true) {
                //     this.ShowResponsesWithoutFeedback.setValue(false);
                // }
                // else {
                     this.ShowResponsesWithoutFeedback.setValue(true);
                // }

                // if (this.filters.searchFeedback != null && this.filters.searchFeedback.trim() != '') {
                //     this.SearchFeedback.setValue(this.filters.searchFeedback);
                // }

                this.enabledFilters = this.getEnabledFilters();

                this.isFiltersOff =  ((!this.filters)  ||
                (this.filters.clientOrganizationId == null &&
                this.filters.clientId == null &&
                this.filters.clientStatus == null &&
                this.filters.isResolved == null &&
                this.filters.userTypes.length == 0 &&
                this.filters.responseTypes.length == 0)); // &&
                //(this.filters.searchFeedback == null || this.filters.searchFeedback.trim() == '');

                this.npsService.fetchNpsResponses(this.filters);
                this.npsService.fetchNpsScores(this.filters);
            }
            else {
                this.clearFilters();
                this.UserTypeEmployee.setValue(true);
                this.UserTypeSupervisor.setValue(true);
                this.UserTypeCompanyAdmin.setValue(true);
                this.setFilters();
                this.npsService.fetchNpsResponses(this.filters);
                this.npsService.fetchNpsScores(this.filters);
           }
        })
    }

    clearFilters() {
        var startDate = new Date()
        startDate.setDate(startDate.getDate() - 90);
        var endDate = new Date();

        this.StartDate.setValue(startDate);
        this.EndDate.setValue(endDate);

        if (this.filters) {
            this.filters.fromDate = startDate;
            this.filters.toDate = endDate;
        }

        this.removeNpsFilter(null);

        this.isFiltersOff =  ((!this.filters)  ||
            (this.filters.clientOrganizationId == null &&
            this.filters.clientId == null &&
            this.filters.clientStatus == null &&
            this.filters.isResolved == null &&
            this.filters.userTypes.length == 0 &&
            this.filters.responseTypes.length == 0));
    }

    preventUncheckingAllUserTypes(e: any) {
        if (!e.target.checked && (this.filters.userTypes.length == 1)) {
            e.target.checked = true;
            if (e.target.id == 'UserTypeEmployee')
                this.UserTypeEmployee.setValue(true);
            else if (e.target.id == 'UserTypeSupervisor')
                this.UserTypeSupervisor.setValue(true);
            else if (e.target.id == 'UserTypeCompanyAdmin')
                this.UserTypeCompanyAdmin.setValue(true);
        }
        else
            this.setFilters();
    }

    setFilters() {
        var userTypes: UserType[] = [];

        if (this.UserTypeEmployee.value == true)
            userTypes.push(UserType.employee);

        if (this.UserTypeSupervisor.value == true)
            userTypes.push(UserType.supervisor);

        if (this.UserTypeCompanyAdmin.value == true)
            userTypes.push(UserType.companyAdmin);

        var responseTypes: NpsResponseType[] = [];

        if (this.ResponseTypeDetractor.value)
            responseTypes.push(NpsResponseType.Detractor);

        if (this.ResponseTypePassive.value)
            responseTypes.push(NpsResponseType.Passive);

        if (this.ResponseTypePromoter.value)
            responseTypes.push(NpsResponseType.Promoter);

        this.filters = {
            fromDate: this.StartDate.value,
            toDate: this.EndDate.value,
            clientOrganizationId: this.Organization.value != null ? this.Organization.value.id : null,
            clientId: this.ClientCode.value != null ? this.ClientCode.value.id : null,
            clientStatus: this.ClientStatus.value,
            isResolved: this.ResolutionStatus.value,
            userTypes: userTypes,
            responseTypes: responseTypes,
            hideResponsesWithoutFeedback: !this.ShowResponsesWithoutFeedback.value,
            searchFeedback: this.SearchFeedback.value
        };

        this.storeService.set(NPS_DASHBOARD_FILTERS_KEY, this.filters);

        this.enabledFilters = this.getEnabledFilters();

        this.isFiltersOff =  ((!this.filters)  ||
            (this.filters.clientOrganizationId == null &&
            this.filters.clientId == null &&
            this.filters.clientStatus == null &&
            this.filters.isResolved == null &&
            this.filters.userTypes.length == 0 &&
            this.filters.responseTypes.length == 0));
    }

    private createUniqueId(){
        var dt = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = (dt + Math.random()*16)%16 | 0;
            dt = Math.floor(dt/16);
            return (c=='x' ? r :(r&0x3|0x8)).toString(16);
        });
        return uuid;
    }

    isFilterEnabled(): boolean{
        return
        this.filters.clientOrganizationId != null ||
        this.filters.clientId != null ||
        this.filters.clientStatus != null ||
        this.filters.isResolved != null ||
        this.filters.userTypes.length > 0 ||
        this.filters.responseTypes.length > 0;
    }

    getEnabledFilters():Array<IEnabledNpsFilter>{
        var filters :Array<IEnabledNpsFilter>=[];

        if (this.filters.clientOrganizationId != null && !!this.clientOrganizations && this.clientOrganizations.length > 0 ){
            filters.push({id:this.createUniqueId(), type:NpsFilterType.clientOrganizationId, value: this.filters.clientOrganizationId != null ? this.clientOrganizations.find(x=>x.id == this.filters.clientOrganizationId).filter : ""});
        }

        if (this.filters.clientId != null && !!this.clients && this.clients.length > 0 ){
            filters.push({id:this.createUniqueId(), type:NpsFilterType.clientId, value: this.filters.clientId != null ? this.clients.find(x=>x.id == this.filters.clientId).filter : ""});
        }

        if (this.filters.clientStatus != null && !!this.clientStatuses && this.clientStatuses.length > 0 ){
            filters.push({id:this.createUniqueId(), type:NpsFilterType.clientStatus, value: this.clientStatuses.find(x=>x.id == this.filters.clientStatus).name});
        }

        if (this.filters.isResolved != null && !!this.resolutionStatuses && this.resolutionStatuses.length > 0 ){
            filters.push({id:this.createUniqueId(), type:NpsFilterType.isResolved, value: (this.filters.isResolved == true) ? 'Resolved' : 'Not Resolved' });
        }

        if (this.filters.userTypes.length > 0){
            if (this.filters.userTypes.find(x => x == UserType.employee)) {
                filters.push({id:this.createUniqueId(), type:NpsFilterType.userType, value: 'Employee' });
            }

            if (this.filters.userTypes.find(x => x == UserType.supervisor)) {
                filters.push({id:this.createUniqueId(), type:NpsFilterType.userType, value: 'Supervisor' });
            }

            if (this.filters.userTypes.find(x => x == UserType.companyAdmin)) {
                filters.push({id:this.createUniqueId(), type:NpsFilterType.userType, value: 'Company Admin' });
            }
        }

        if (this.filters.responseTypes.length > 0){
            if (this.filters.responseTypes.find(x => x == NpsResponseType.Detractor)) {
                filters.push({id:this.createUniqueId(), type:NpsFilterType.responseType, value: 'Detractor' });
            }

            if (this.filters.responseTypes.find(x => x == NpsResponseType.Passive)) {
                 filters.push({id:this.createUniqueId(), type:NpsFilterType.responseType, value: 'Passive' });
            }

            if (this.filters.responseTypes.find(x => x == NpsResponseType.Promoter)) {
                filters.push({id:this.createUniqueId(), type:NpsFilterType.responseType, value: 'Promoter' });
            }
        }

        return filters;
    }

    removeNpsFilter(filter: IEnabledNpsFilter) {
        if (this.filters) {
            if(!filter) {
                this.Organization.setValue(null);
                this.filters.clientOrganizationId = null;

                this.ClientCode.setValue(null);
                this.filters.clientId = null;

                this.ClientStatus.setValue(null);
                this.filters.clientStatus = null;

                this.ResolutionStatus.setValue(null);
                this.filters.isResolved = null;

                this.filters.userTypes = [];
                this.UserTypeEmployee.setValue(false);
                this.UserTypeSupervisor.setValue(false);
                this.UserTypeCompanyAdmin.setValue(false);

                this.filters.responseTypes = [];
                this.ResponseTypeDetractor.setValue(false);
                this.ResponseTypePassive.setValue(false);
                this.ResponseTypePromoter.setValue(false);

                // this.ShowResponsesWithoutFeedback.setValue(true);
                // this.filters.hideResponsesWithoutFeedback = false;

                // this.SearchFeedback.setValue(null);
                // this.filters.searchFeedback = null;
            }
            else if (filter.type == NpsFilterType.clientOrganizationId) {
                this.Organization.setValue(null);
                this.filters.clientOrganizationId = null;
            }
            else if (filter.type == NpsFilterType.clientId) {
                this.ClientCode.setValue(null);
                this.filters.clientId = null;
            }
            else if (filter.type == NpsFilterType.clientStatus) {
                this.ClientStatus.setValue(null);
                this.filters.clientStatus = null;
            }
            else if (filter.type == NpsFilterType.isResolved) {
                this.ResolutionStatus.setValue(null);
                this.filters.isResolved = null;
            }
            else if (filter.type == NpsFilterType.userType) {
                if (filter.value == 'Employee') {
                    this.UserTypeEmployee.setValue(false);
                    this.filters.userTypes = this.filters.userTypes.filter(x=> x != UserType.employee);
                }

                if (filter.value == 'Supervisor') {
                    this.UserTypeSupervisor.setValue(false);
                    this.filters.userTypes = this.filters.userTypes.filter(x=> x != UserType.supervisor);
                }

                if (filter.value == 'Company Admin') {
                    this.UserTypeCompanyAdmin.setValue(false);
                    this.filters.userTypes = this.filters.userTypes.filter(x=> x != UserType.companyAdmin);
                }
            }
            else if (filter.type == NpsFilterType.responseType) {
                if (filter.value == 'Detractor') {
                    this.ResponseTypeDetractor.setValue(false);
                    this.filters.responseTypes = this.filters.responseTypes.filter(x=> x != NpsResponseType.Detractor);
                }

                if (filter.value == 'Passive') {
                    this.ResponseTypePassive.setValue(false);
                    this.filters.responseTypes = this.filters.responseTypes.filter(x=> x != NpsResponseType.Passive);
                }

                if (filter.value == 'Promoter') {
                    this.ResponseTypePromoter.setValue(false);
                    this.filters.responseTypes = this.filters.responseTypes.filter(x=> x != NpsResponseType.Promoter);
                }
            }

            this.storeService.set(NPS_DASHBOARD_FILTERS_KEY, this.filters);
            this.enabledFilters = this.getEnabledFilters();
            this.npsService.fetchNpsResponses(this.filters);
            this.npsService.fetchNpsScores(this.filters);
        }
        else {
            this.Organization.setValue(null);
            this.ClientCode.setValue(null);
            this.ClientStatus.setValue(null);
            this.ResolutionStatus.setValue(null);
            this.UserTypeEmployee.setValue(false);
            this.UserTypeSupervisor.setValue(false);
            this.UserTypeCompanyAdmin.setValue(false);
            this.ResponseTypeDetractor.setValue(false);
            this.ResponseTypePassive.setValue(false);
            this.ResponseTypePromoter.setValue(false);
            this.ShowResponsesWithoutFeedback.setValue(true);
            this.SearchFeedback.setValue(null);
        }
    }

    getUserTypeAcronym(userTypeId: number) {
        let acronym = "";
        switch(userTypeId) {
            case 2: {
                acronym = "CA";
               break;
            }
            case 3: {
                acronym = "E";
               break;
            }
            case 4: {
                acronym = "S";
               break;
            }
            default: {
                acronym = "A";
               break;
            }
         }
         return acronym;
    }

    applyFilters() {
        this.setFilters();
        this.npsService.fetchNpsResponses(this.filters);
        this.npsService.fetchNpsScores(this.filters);
    }

    markResolved(item: NpsResponse, newStatus: boolean) {
        item.isResolved = newStatus;
        this.npsService.saveNpsResponse(item).subscribe((updatedItem : NpsResponse) => {
            // item.isResolved = updatedItem.isResolved;
            // item.resolvedByUserName = updatedItem.resolvedByUserName;
            // item.resolvedDate = updatedItem.resolvedDate;
            this.applyFilters();
            this.ngxMsgSvc.setSuccessMessage("Status saved successfully.");
        }, (error: HttpErrorResponse) => {
            this.ngxMsgSvc.setErrorResponse(error);
        });
    }

    identify(index, item) {
        return item.id;
    }

    changeClientOrganization(organization: IOption){
        if(!organization){
            return;
        }
        this.setFilters();
    }

    changeClient(client: IOption){
        if(!client){
            return;
        }
        this.setFilters();
    }
}
