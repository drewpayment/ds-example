import { UserType } from "@ajs/user";
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  OnInit,
} from "@angular/core";
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { AccountService } from "@ds/core/account.service";
import {
  AppConfig,
  APP_CONFIG,
} from "@ds/core/app-config/app-config";
import { ClientService } from "@ds/core/clients/shared";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import {
  BehaviorSubject,
  EMPTY,
  forkJoin,
  NEVER,
  Observable,
  of,
  Subject,
  throwError,
} from "rxjs";
import {
  catchError,
  filter,
  map,
  switchMap,
  takeUntil,
  tap,
} from "rxjs/operators";
import { Features } from "@ds/admin/client-statistics/shared/models/featureEnum";
import { ConfigUrl, ConfigUrlType } from "@ds/core/shared/config-url.model";
import { UserInfo } from "@ds/core/shared";
import * as moment from "moment";
import { MatTableDataSource } from "@angular/material/table";
import { MatDialog } from "@angular/material/dialog";
import { Store } from "@ngrx/store";
import {
  EmployeeState,
  getEmployeeState,
} from "@ds/employees/header/ngrx/reducer";
import { IEmployeeSearchResult } from "@ds/employees/search/shared/models/employee-search-result";

import { ClientAccountFeature } from '@ds/core/clients/shared/client-account-feature.model';
import { DecimalPipe } from "@angular/common";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { IEmployeeDependent, IEmployeeDependentRelationship } from '../../models/employee-dependents/employee-dependent.model';
import { EmployeeDependentsService } from '../../Services/employee-dependents.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SetEmployee } from '@ds/employees/header/ngrx/actions';
import { ChangeTrackerService } from '@ds/core/ui/forms/change-track/change-tracker.service';
import { Router } from '@angular/router';
import { changeDrawerHeightOnOpen, matDrawerAfterHeightChange } from "@ds/core/ui/animations/drawer-auto-height-animation";

@Component({
  selector: "ds-employee-dependents",
  templateUrl: "./employee-dependents.component.html",
  styleUrls: ["./employee-dependents.component.scss"],
  animations: [
    changeDrawerHeightOnOpen,
    matDrawerAfterHeightChange
  ]
})
export class EmployeeDependentsComponent implements OnInit, OnDestroy {
  destroy$ = new Subject();
  baseUrl = this.config.baseSite.url;
  formSubmitted: boolean;
  
  hasError: boolean;
  clientId: number;
  employeeId: number;
  isLoading: boolean = true;
  isAdding: boolean = false;
  dependentsList:Array<IEmployeeDependent>;
  activeDependent:IEmployeeDependent = null;
  activeId:number = 0;
  dependentClicked:IEmployeeDependent = null;
  submitted:boolean;

  userinfo: UserInfo;
  totalCount: number = 0;
  hrBlocked: boolean;
  allowAddEmployee: boolean;
  employeeListUrl: string;
  essProfile: string;
  isSupervisorOnHimself: boolean;
  showSSN: boolean;

  payrollUrl: ConfigUrl;
  companyUrl: ConfigUrl;

  selectedEmployee$ = this.store.select(
    getEmployeeState((x) => x.selectedEmployee)
  ) as any as Observable<IEmployeeSearchResult>;

  constructor(
    private router: Router,
    private accountService: AccountService,
    private clientService: ClientService,
    private msg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: ConfirmDialogService,
    @Inject(APP_CONFIG) private config: AppConfig,
    private store: Store<EmployeeState>,
    private employeeDependentsApi: EmployeeDependentsService,
    private changeTrackerService: ChangeTrackerService
  ) {}

  ngOnInit() {
    this.isLoading = true;
    this.dependentsList = [];
    this.activeDependent = null;
    this.activeId = 0;

    forkJoin([this.accountService.getUserInfo(true), this.accountService.getSiteUrls()]).subscribe( ([user,sites]) => {
      this.userinfo = user;
      this.clientId = user.selectedClientId();
      this.hrBlocked = user.isHrBlocked;
      this.allowAddEmployee = user.addEmployee;
      this.employeeId = user.lastEmployeeId ;
      this.showSSN = (this.userinfo.userTypeId != UserType.supervisor);

      // redirect to no permisions page
      if(this.hrBlocked) 
        this.router.navigate(['error'],  { queryParams:  {showButton: false, showHelpMessage: false, message:'You do not have access to this information.'},  
                                         queryParamsHandling: "merge" });      

      this.payrollUrl = sites.find((s) => s.siteType === ConfigUrlType.Payroll);
      let essUrl      = sites.find((s) => s.siteType === ConfigUrlType.Ess);
      this.companyUrl = sites.find((s) => s.siteType === ConfigUrlType.Company);
      this.essProfile = `${essUrl.url}profile`;

      this.employeeListUrl = `${this.payrollUrl.url}ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=${this.companyUrl.url}manage/dependents`;
      // if the user doesn't have an employee selected, redirect them to the employee select list
      if (this.employeeId == null || this.employeeId < 1) {
        document.location.href = this.employeeListUrl;
        return;
      }

      this.isSupervisorOnHimself = this.userinfo.employeeId == this.employeeId;
      let api$ = of([]);
      if (!this.isSupervisorOnHimself)
        api$ = this.employeeDependentsApi.getEmployeeDependents(this.employeeId);

      api$.subscribe((x : IEmployeeDependent[]) => {
        this.loadDependents(x);
        this.isLoading = false;
      }, (error: HttpErrorResponse) => {
        this.msg.setErrorResponse(error);
        this.isLoading = false;
      })
    })

    this.selectedEmployee$
      .pipe(
        takeUntil(this.destroy$),
        filter((x) => !!x && x.employeeId != this.employeeId),
        switchMap(( ee :  IEmployeeSearchResult) => {
          if(!this.userinfo) return EMPTY;
          this.isLoading = true;
          this.employeeId = ee.employeeId ;
          this.isSupervisorOnHimself = this.userinfo.employeeId == this.employeeId;
          let api$ = of([]);
          if (!this.isSupervisorOnHimself)
            api$ = this.employeeDependentsApi.getEmployeeDependents(this.employeeId);
          return api$;
        }),
      )
      .subscribe((x : IEmployeeDependent[]) => {
        this.loadDependents(x);
        this.isLoading = false;
      }, (error: HttpErrorResponse) => {
        this.msg.setErrorResponse(error);
        this.isLoading = false;
      });
  }

  loadDependents(dependentsList: IEmployeeDependent[]){
    this.dependentsList = dependentsList.sort((a, b) => (a.employeeDependentId < b.employeeDependentId) ? 1 : -1).sort((a, b) => (a.isInactive > b.isInactive) ? 1 : -1);
    this.isLoading = false;
    this.clearDrawer();
  }

  ngOnDestroy() {
    this.destroy$.next();
  }

  getNext(){
    let minId = Math.min(...this.dependentsList.map(x=>x.employeeDependentId));
    if(!minId || minId>0) minId = 0;
    minId = (minId-1);
    var newName = "Noname " + (-minId);
    while(this.dependentsList.find(x=>x.firstName == newName) )
        newName = "Noname " + newName;
    return {id:minId,name:newName};
  }

  addDependent(){
      this.clearDrawer();
      var next = this.getNext();

      let newItem = <IEmployeeDependent>{
        employeeDependentId : next.id,  
        employeeId: this.employeeId,
        clientId: this.userinfo.lastClientId || this.userinfo.clientId,
        firstName: '',
        middleInitial: '',
        lastName: '',
        unmaskedSocialSecurityNumber: '',
        maskedSocialSecurityNumber: '',
        relationship: 'Other',
        gender: '',
        comments: '',
        insertStatus: 1,
        lastModifiedByDescription: '',
        isAStudent: false,
        hasADisability: false,
        tobaccoUser: false,
        isSelected: false,
        hasPcp: false,
        isChild: false,
        isSpouse: false,
        isInactive: false,
        workPhoneNumber: '',
        cellPhoneNumber: ''
      };
      this.dependentsList.push(newItem);
      this.toggleActiveDependent(next.id);
      this.changeTrackerService.setIsDirty(true);
      this.isAdding = true;
  }

  makeHovered(tl) {
    if (this.dependentClicked) {
      this.dependentClicked.hovered = false;
      this.dependentClicked = null;
    }
    tl.hovered = true;
  }
  
  cogClicked(tl:IEmployeeDependent) {
      this.dependentClicked = tl;
      this.dependentClicked.hovered = false;
      setTimeout(() => {
        tl.hovered = true;
      }, 100);
  }

  deleteDependentDialog(tl:IEmployeeDependent) {
      const options = {
          title: `Are you sure you want to delete this dependent?`,
          message: "",
          confirm: "Delete",
        };

      this.confirmDialog.open(options);
      this.confirmDialog.confirmed()
      .pipe(
      switchMap((confirmed) => {
          if (confirmed) {
              if(tl.employeeDependentId < 0 ) 
              return of(true);
              else  {
                this.msg.setWarningMessage('Sending');
                return this.employeeDependentsApi.deleteActiveDependent(tl);
              }
          } else {
          return EMPTY;
          }
      })
      )
      .subscribe(done => {
          if(done) {
              // remove from the list
              var inx = this.dependentsList.findIndex(
                  (x) => x.employeeDependentId == tl.employeeDependentId
              );
              if (inx >-1) this.dependentsList.splice(inx, 1);

              this.msg.setSuccessMessage('Dependent deleted successfully.');
              if(this.dependentsList.length){
                  this.clearDrawer();
              }
          }}, (error: HttpErrorResponse) => {
            let err = error.error || error;
            let m = err.errors[0].msg;
            if (m) {
              if(m.toLowerCase().includes('delete statement conflicted with the reference constraint'))
                  this.msg.setErrorMessage('This dependent cannot be deleted.There is historical data associated with the dependent.Please deactivate this dependent if you want it to be removed.');
              else
              this.msg.setErrorResponse(m);
            }
            else 
              this.msg.setErrorResponse(error);
          });
  }

  toggleActiveDependent(listId: number) {
      this.isAdding = false;
      this.changeTrackerService.setIsDirty(false);
      if (this.activeId == listId) {
        this.clearDrawer();
      } else {
        this.activeId = listId;
        this.activeDependent = this.dependentsList.find(x => {
          return (x.employeeDependentId == this.activeId);
        });
        this.employeeDependentsApi.setActiveDependent(this.activeDependent);
      }    
  }

  clearDrawer() {
    this.dependentsList = this.dependentsList.sort((a, b) => (a.employeeDependentId < b.employeeDependentId) ? 1 : -1).sort((a, b) => (a.isInactive > b.isInactive) ? 1 : -1);
    let addNewIndex = this.dependentsList.findIndex(x=>x.employeeDependentId<0);
    if (addNewIndex > -1) {
      this.dependentsList.splice(addNewIndex, 1);
      this.isAdding = false;
      this.changeTrackerService.setIsDirty(false);
    }

    this.activeId = 0;
    this.activeDependent = null;
  }

  refresh(dependentId: number) {
    if(dependentId<0)
        this.clearDrawer();
    else
        this.toggleActiveDependent(dependentId);
  }

  switchEmployee () {
      this.isLoading = true;
  }
}