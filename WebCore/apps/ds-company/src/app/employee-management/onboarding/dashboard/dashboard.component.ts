import * as angular from "angular";
import { Component, OnInit, ViewChild } from '@angular/core';
import { ClientService } from '@ds/core/clients/shared';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from "@ds/core/shared/user-info.model";
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { tap, switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { HttpErrorResponse } from '@angular/common/http';
import { DashboardState } from "apps/ds-company/src/app/services/dashboard-state.service";
import { IOnboardingEmployee } from "apps/ds-company/src/app/models/onboarding-employee.model"
import { EmployeeOnboardingAdminData } from '@ajs/onboarding/shared/models'
import { DsStorageService } from '@ds/core/storage/storage.service';

import { EmployeeDataService } from "apps/ds-company/src/app/services/employee-data.service";

//import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import {DashboardService} from "apps/ds-company/src/app/services/dashboard.service"
import { IEmployeeAvatars } from '@ds/core/employees/shared/employee-avatars.model';
import { IEmployee } from '@ajs/core/ds-resource/models';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { SortDirection } from '@ds/core/shared/sort-direction.enum';
import { ClientSelectorDialogComponent } from "../add-employee/client-selector-dialog/client-selector-dialog.component";
import * as moment from 'moment'
import { Moment } from 'moment';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";


@Component({
  selector: 'ds-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

    hasError = false;
    isLoading = true;
    dashboardState: any;
    isOnboardingCompleted: boolean = true;
    searchText: string;
    searchWord: string = "";
    isNumberSearch:boolean;
    clientId: number;
    searchClientId: number;
    relatedClientId: number;
    
    displayValue: string;
    sortByValue: string;
    sortByOrder: string;
    
    showCustomPagesOption = false;
    showTaskListsOption = false;
    showWelcomeMessageOption = false;
    showFinalDisclaimerOption = false;
    showCertifyI9Option = false;
    showEmailTemplatesOption = false;
    showAddEmployee = false;

    private user: UserInfo;
    employees: Array<IOnboardingEmployee>  = [];
    allEmployees: Array<IOnboardingEmployee>  = [];
    relatedClients = [];
    //employeeData: IOnboardingEmployee;
    employeeId: number;

    totalCount: number;
    displayedCount:number;

    searchHireDateFrom :Moment;
    searchHireDateTo :Moment;

    dataSource: MatTableDataSource<IOnboardingEmployee>;
    paginator: MatPaginator;
    sorter: MatSort;
    displayedColumns: string[] = ['Person', 'Name', 'Number', 'Hire Date', 'Job Title','Status','AdminStatus'];
    errorMsg: string;
    rangeLabel: string;
    employeeRoute: string;

    @ViewChild('paginator', { static: false }) set matPaginator(mp: MatPaginator) {
        if(mp){
            this.paginator = mp;
            this.setDataSourceAttr();
        }
    }
    @ViewChild(MatSort, { static: false }) set matSorter(ms: MatSort) {
        if (ms) {
            this.sorter = ms;
            this.setDataSourceAttr();
        }
      }
    setDataSourceAttr() {
        if (this.dataSource) {
            this.dataSource.paginator = this.paginator;
            if(this.sorter){
                this.dataSource.sort = this.sorter;
                
                this.dataSource.sortingDataAccessor = (item, sortColumn) => {
                    switch (sortColumn) {
                        case 'Name':
                            return item.employeeName.toLowerCase();
                        case 'Number':
                            return item.employeeNumber;
                        case 'Hire Date':
                            return item.rehireDate ? moment(item.rehireDate).format("YYYYMMDD") : (item.hireDate?moment(item.hireDate).format("YYYYMMDD"):"");
                        case 'Job Title':
                            return item.jobTitle.toLowerCase();
                        case 'Status':
                            return item.sortEmployee;
                        case 'AdminStatus':
                            return item.sortAdmin;
                        default:
                            return item[this.sorter.active];
                    }
                };
            }
        }
    }

    constructor(
      private accountService: AccountService,
      private clientService: ClientService,
      private msgSvc: NgxMessageService,
      private dialog: MatDialog ,
      private store: DsStorageService,
      private dsEmployeeData: EmployeeDataService,
      //private dsOnboardingApi: DsOnboardingEmployeeApiService,
      private dashboardApi: DashboardService,
      private router: Router,
      ) {}

    ngOnInit() {
      this.dashboardState = new DashboardState(this.store);
      this.setEmployeeUrl();
      this.isOnboardingCompleted = this.dashboardState.isOnboardingCompleted;
      this.searchText = this.dashboardState.searchText ? this.dashboardState.searchText : '';
      this.searchWord = this.searchText;
      
      this.searchClientId = this.dashboardState.searchClientId ? this.dashboardState.searchClientId : 0;
      this.relatedClientId = this.dashboardState.searchClientId ? this.dashboardState.searchClientId : 0;
      this.displayValue = this.dashboardState.displayValue ? this.dashboardState.displayValue : 'card';
      this.sortByValue = this.dashboardState.sortByValue ? this.dashboardState.sortByValue : 'employeeName';
      this.sortByOrder = this.dashboardState.sortByOrder ? this.dashboardState.sortByOrder : 'arrow_upward';
      this.searchHireDateFrom = this.dashboardState.fromDate ? moment(this.dashboardState.fromDate) : null;
      this.searchHireDateTo = this.dashboardState.toDate ? moment(this.dashboardState.toDate) : null;

      this.accountService.canPerformAction('Onboarding.OnboardingAdministrator')
            .subscribe(canPerform => {
                if (canPerform) {
                    this.showCustomPagesOption = true;
                    this.showEmailTemplatesOption = true;
                    this.showAddEmployee = true;
                    this.showTaskListsOption = true;
                    this.showWelcomeMessageOption = true;
                    this.showFinalDisclaimerOption = true;
                }
            });
      this.accountService.canPerformAction('Onboarding.AllowI9Certification')
            .subscribe(canPerform => { if (canPerform) this.showCertifyI9Option = true; });
      this.accountService.canPerformAction('Onboarding.AllowAddEmployee')
            .subscribe(canPerform => { 
                if (canPerform) {
                    this.showEmailTemplatesOption = true; 
                    this.showAddEmployee = true;
                }
            });
          
      this.dataSource = new MatTableDataSource<IOnboardingEmployee>([]);
      
      this.accountService.getUserInfo().pipe(switchMap( x=> {
          this.user=x;
          return this.dashboardApi.getRelatedClientsList(this.user.selectedClientId())
        })).subscribe(relatedClientsData => {
            if (relatedClientsData) {
                this.relatedClients = relatedClientsData;
            }
        });
      this.loadEmployees(!!this.isOnboardingCompleted);
    }

    updateDisplayValue(type: string){
      this.displayValue = type;
      this.dashboardState.displayValue = type;
      if(this.displayValue == 'table'){
            this.dataSource = new MatTableDataSource<IOnboardingEmployee>(this.employees);
            this.setDataSourceAttr();
      }
    };

    updateRelatedClient = (event) => {
        const clientId = event.target.value;
        this.searchClientId = (clientId == "0") ? null : parseInt( clientId ) ;
        this.dashboardState.searchClientId = this.searchClientId;
        this.applyFilterParams();
    }

    applyFilter(srhTxt:string){
        this.searchText = srhTxt;
        this.dashboardState.searchText = srhTxt;
        this.applyFilterParams();
    }

    employeeFilter = (item:IOnboardingEmployee) => {
        var flag = true;

        if (this.searchClientId) {
            if (item.clientId != this.searchClientId) flag = false;
        }

        if (this.searchText) {
            if (!this.isNumberSearch) {
                if (item.employeeName.toLowerCase().indexOf(this.searchText.toLowerCase()) == -1) flag = false;
            }
            else{
                if (item.employeeNumber.toString().indexOf(this.searchText) == -1) flag = false;
            }
        }
        return flag;
    }

    needAttentionFilter = (item: IOnboardingEmployee): boolean => {
        return (item.isWorkflowComplete == false || item.employeeStarted == null);
    };
    
    inprogressFilter = (item: IOnboardingEmployee): boolean => {
        return (item.pctComplete < 100 && item.employeeStarted != null);
    };
    
    completedFilter = (item: IOnboardingEmployee): boolean => {
        return (item.pctComplete >= 100 && item.adminPctComplete < 100 && item.employeeStarted != null);
    };
    
    closeOnboardingFilter = (item: IOnboardingEmployee): boolean => {
        return (item.pctComplete == 100 && item.adminPctComplete == 100);
    };

    applyDateFilters(){
        this.loadEmployees(true);
        this.dashboardState.fromDate = this.searchHireDateFrom ? this.searchHireDateFrom.toDate() : null;
        this.dashboardState.toDate = this.searchHireDateTo ? this.searchHireDateTo.toDate() : null;        
    }

    private getRangeLabel():string{
        if(!this.isOnboardingCompleted){
            return "";
        }
        else if (!!this.searchHireDateFrom && !!this.searchHireDateTo){
            return `during ${this.searchHireDateFrom.format("MM/DD/YYYY")} to ${this.searchHireDateTo.format("MM/DD/YYYY")}`;
        }
        else if (!!this.searchHireDateFrom) {
            return `since ${this.searchHireDateFrom.format("MM/DD/YYYY")}`;
        }
        else if (!!this.searchHireDateTo) {
            return `until ${this.searchHireDateTo.format("MM/DD/YYYY")}`;
        }
        else return "";
    }

    applyFilterParams():void
    {
        let k = [];
        this.totalCount = 0;
        this.isNumberSearch = !isNaN(this.searchText as any);

        if(this.allEmployees && this.allEmployees.length > 0)
        {
            this.totalCount = this.allEmployees.length;

            k = this.allEmployees.filter( this.employeeFilter );
            if(this.searchText.length == 0){
                k.sort( (a,b) => 
                    a.sortAdmin.localeCompare(b.sortAdmin) != 0 ? a.sortAdmin.localeCompare(b.sortAdmin) :
                    a.employeeName.localeCompare(b.employeeName)
                );
            }
            else {
                k.sort( (a,b) => 
                    a.employeeName.localeCompare(b.employeeName) != 0 ? a.employeeName.localeCompare(b.employeeName) :
                    a.sortAdmin.localeCompare(b.sortAdmin)
                );
            }
        }

        this.employees = k;
        this.displayedCount = k.length;
        if(this.displayValue == 'table'){
            this.dataSource = new MatTableDataSource<IOnboardingEmployee>(k);
            this.setDataSourceAttr();
        }
        //this.dsEmployeeData.setEmployeeList(this.employees);
    }

    loadEmployees(isComplete: boolean) {
      let isSwitch = this.isOnboardingCompleted != isComplete;
      this.errorMsg = "";      
      this.isLoading = true;
      this.isOnboardingCompleted = isComplete;
      this.dashboardState.isOnboardingCompleted = isComplete;
      this.rangeLabel = this.getRangeLabel();
      let params:any = { isOnboardingComplete: isComplete };

      if (isComplete) {
        //this.updateDisplayValue('table');
        this.updateDisplayValue(this.displayValue);
        this.displayValue
        params = { isOnboardingComplete: isComplete, startDate: (!!this.searchHireDateFrom ? this.searchHireDateFrom.format("YYYY-MM-DD") : "")
                    , endDate: (!!this.searchHireDateTo ? this.searchHireDateTo.format("YYYY-MM-DD") : "") };
      } else if(isSwitch){
        //this.updateDisplayValue('card');
        this.updateDisplayValue(this.displayValue);
      }

      this.dashboardApi.getEmployeeList(this.user.selectedClientId(), params)
          .subscribe(employeeData => {
              this.allEmployees = employeeData || [];
              const empsToRemove = [];

              this.allEmployees.forEach((emp: IOnboardingEmployee) => {
                  if (emp.isWorkflowComplete == null && emp.isOnboardingCompleted == true && emp.employeeWorkflow == null) {
                      // user was added to onboarding because they filed a new 2020 Federal W4, and shouldn't be shown
                      empsToRemove.push(emp);
                  }
              });

              empsToRemove.forEach((empToRemove: IOnboardingEmployee) => {
                  const indexToRemove = this.allEmployees.indexOf(empToRemove);
                  this.allEmployees.splice(indexToRemove, 1);
              });

              this.allEmployees.forEach(x => {
                  x = this.dsEmployeeData.mapEmployeeAvatar(x);
                  x = this.dsEmployeeData.mapEmployeeAdminInfo(x);
              });
              this.applyFilterParams();
              this.isLoading = false;
          }, (error: HttpErrorResponse) => {
              let err = error.error || error;
              let m = err.errors[0].msg;
              if (m) {
                if(!m.includes("500 closed records"))
                    this.msgSvc.setErrorMessage(m);
                else
                    this.errorMsg = m;
              }
              this.allEmployees = [];
              this.applyFilterParams();
              this.isLoading = false;
          });
    }

    dashboardDetails(emp:IOnboardingEmployee,type:number){
        this.dsEmployeeData.setEmployeeList( this.employees.filter(x => x.isWorkflowComplete && emp.invitationSent != null) );
        this.dsEmployeeData.selectEmployee(emp.employeeId);

        if (!emp.isWorkflowComplete) {
            if (emp.userId == 0) {
                this.router.navigate(['manage/onboarding/add-employee', emp.clientId, emp.employeeId, 1, 'add']);
            } else {
                this.router.navigate(['manage/onboarding/add-workflow', emp.clientId, emp.employeeId, 2, 'add']);
            }
        } else if (emp.invitationSent == null) {
            if (emp.userId == 0) {
                this.router.navigate(['manage/onboarding/add-employee', emp.clientId, emp.employeeId, 1, 'add']);
            } else {
                this.router.navigate(['manage/onboarding/add-email-template', emp.clientId, emp.employeeId, 3, 'add']);
            }
        } else {
            this.router.navigate(['manage/onboarding/dashboard-detail' , emp.employeeId ]);
        }
    }
    
    redirectToCustomPages() {
        this.router.navigate(['admin/onboarding/custom-pages']);
    };

    redirectToCertifyI9List() {
        this.accountService.getSiteUrls()
          .subscribe(sites => {
            if (!sites) {
              console.error(
                'Could not find base site URLs, please check web configs.'
              );
              return;
            }
            const payroll = sites.find(s => s.siteType === ConfigUrlType.Payroll);
            document.location.href = `${payroll.url}/CertifyI9.aspx?Submenu=Employee&LinkedToOnboarding=1'`;
          });
    };

    setEmployeeUrl() {
        this.accountService.getSiteUrls()
          .subscribe(sites => {
            const payroll = sites.find(s => s.siteType === ConfigUrlType.Payroll);
            this.employeeRoute = `${payroll.url}/employee.aspx`;
          });
    };

    redirectToEmailTemplates() {
        //this.router.navigate(['admin/onboarding/email-invitation-templates']);
        this.router.navigate(['admin/onboarding/correspondence-template/1']);
    };

    redirectToAdminTaskList() {
        this.router.navigate(['admin/admin-tasks']);
    };

    redirectToWelcomeMessage() {
        this.router.navigate(['admin/onboarding/welcome-message']);
    };

    redirectToFinalDisclaimer() {
        this.router.navigate(['admin/onboarding/final-disclaimer']);
    };
    // redirectToEmployeeList() {
    //     $window.location = dsConfig.getAbsoluteUrl('Legacy/Employee.aspx');
    // };

    relatedClientCode = (clientId: number): string => {
        let k = this.relatedClients.find(x=>x.clientId == clientId);
        if(k) return k.clientCode;
        else return "";
    };

    displaySetupPage(employee: IOnboardingEmployee, categoryId: number) {
        if (!employee.isWorkflowComplete) {
            if (employee.userId == 0) {
                this.router.navigate(['manage/onboarding/add-employee', employee.clientId, 0, 1, 'add']);
            } else {
                this.router.navigate(['manage/onboarding/add-workflow', employee.clientId, employee.employeeId, 2, 'add']);
            }
        } else if (employee.invitationSent == null) {
            if (employee.userId == 0) {
                this.router.navigate(['manage/onboarding/add-employee', employee.clientId, 0, 1, 'add']);
            } else {
                this.router.navigate(['manage/onboarding/add-email-template', employee.clientId, employee.employeeId, 3, 'add']);
            }
        } else {
            this.router.navigate(['manage/onboarding/dashboard-detail', employee.employeeId]);  
        }
    }

    addNewHire() {
        if (this.relatedClients.length > 1) {
            let config = new MatDialogConfig<any>();
            config.width = "500px";
            config.data = {clientId: this.user.selectedClientId(), clientList: this.relatedClients};
        
            return this.dialog.open<ClientSelectorDialogComponent, any, number>(ClientSelectorDialogComponent, config)
            .afterClosed()
            .subscribe((selectedClientId: number) => {
                if (selectedClientId) {
                    this.router.navigate(['manage/onboarding/', 'add-employee', selectedClientId, 0, 1, 'add']);
                }
            });
        }
        else {
            this.router.navigate(['manage/onboarding/', 'add-employee', this.user.selectedClientId(), 0, 1, 'add']);
        }
    }
    
    getHireDate(employee: IOnboardingEmployee) {
        return employee.rehireDate ? employee.rehireDate : employee.hireDate;
    }
}
