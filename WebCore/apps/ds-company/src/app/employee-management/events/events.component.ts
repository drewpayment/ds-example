import { UserType } from "@ajs/user";
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  OnInit,
} from "@angular/core";
import { AccountService } from "@ds/core/account.service";
import {
  AppConfig,
  APP_CONFIG,
} from "@ds/core/app-config/app-config";
import { ClientService } from "@ds/core/clients/shared";
import {
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

import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { ConfigUrl, ConfigUrlType } from "@ds/core/shared/config-url.model";
import { UserInfo } from "@ds/core/shared";
import { Store } from "@ngrx/store";
import {
  EmployeeState,
  getEmployeeState,
} from "@ds/employees/header/ngrx/reducer";
import { IEmployeeSearchResult } from "@ds/employees/search/shared/models/employee-search-result";
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { Router } from '@angular/router';
import { ManageTopicsDialogComponent } from "@ds/employees/events/manage-topics/manage-topics-dialog.component";

@Component({
  selector: "ds-events",
  templateUrl: "./events.component.html",
})
export class EventsComponent implements OnInit, OnDestroy {
  destroy$ = new Subject();
  baseUrl = this.config.baseSite.url;
  breadcrumb: string;
  formSubmitted: boolean;
  hasError: boolean;
  clientId: number;
  employeeId: number;
  essViewOnly: boolean;
  isSupervisorOnHimself: boolean;
  essProfile: string;
  canManageTopics: boolean;
  userinfo: UserInfo;
  totalCount: number = 0;
  hrBlocked: boolean;
  payrollUrl: ConfigUrl;
  companyUrl: ConfigUrl;
  isLoading: boolean;

  selectedEmployee$ = this.store.select(
    getEmployeeState((x) => x.selectedEmployee)
  ) as any as Observable<IEmployeeSearchResult>;

  constructor(
    private router: Router,
    private accountService: AccountService,
    private clientService: ClientService,
    private ngxMsg: NgxMessageService,
    @Inject(APP_CONFIG) private config: AppConfig,
    private store: Store<EmployeeState>,
    private dialog: MatDialog,
  ) {}

  ngOnInit() {
    this.isLoading = true;
    forkJoin([ this.accountService.getSiteUrls(), this.accountService.getUserInfo(true) ]).pipe(
        tap(
          ([sites, user]: [
            ConfigUrl[],
            UserInfo,
          ]) => {
            this.payrollUrl = sites.find(
              (s) => s.siteType === ConfigUrlType.Payroll
            );
            let essUrl = sites.find((s) => s.siteType === ConfigUrlType.Ess);
            this.companyUrl = sites.find(
              (s) => s.siteType === ConfigUrlType.Company
            );
            this.breadcrumb = `${this.payrollUrl.url}ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=${this.companyUrl.url}manage/events`;
            this.userinfo = user;
            this.employeeId = user.lastEmployeeId || user.employeeId;
            this.clientId = user.lastClientId || user.clientId;
            this.isSupervisorOnHimself = this.userinfo.userEmployeeId == this.employeeId;
            this.hrBlocked = user.isHrBlocked;
            this.essViewOnly = user.isEmployeeSelfServiceViewOnly;
            this.canManageTopics = (this.userinfo.userTypeId == UserType.systemAdmin || this.userinfo.userTypeId == UserType.companyAdmin) && !this.essViewOnly;
            this.essProfile = `/service/events`;
            this.isLoading = false;
            // redirect to no permisions page
            if(this.hrBlocked) this.router.navigate(['error'],  { queryParams:  {showButton: false, showHelpMessage: false, message:'You do not have access to this information.'},  
              queryParamsHandling: "merge" });

            if (this.employeeId == null || this.employeeId < 1) {
              document.location.href = this.breadcrumb;
              return EMPTY;
            }
          }
        ))
      .subscribe();

      this.selectedEmployee$
      .pipe(
        takeUntil(this.destroy$),
        filter((x) => !!x && x.employeeId != this.employeeId),
      )
      .subscribe(( ee :  IEmployeeSearchResult) => {
        if(!this.userinfo) return;
        this.isLoading = false;
        this.employeeId = ee.employeeId ;
        this.isSupervisorOnHimself = this.userinfo.userEmployeeId == this.employeeId;
      }, (error: HttpErrorResponse) => {
        this.ngxMsg.setErrorResponse(error);
        this.isLoading = false;
      });
  }

  manageTopics() {
    let config = new MatDialogConfig<any>();
    config.width = "600px";
    config.data = { clientId: this.clientId };
    
    return this.dialog.open<ManageTopicsDialogComponent, any, string>(ManageTopicsDialogComponent, config)
        .afterClosed()
        .subscribe((returnData: any) => {
        });
  }

  ngOnDestroy() {
    this.destroy$.next();
  }

  reload(){
    this.isLoading = true
  }
}