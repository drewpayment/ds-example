import { UserType } from "@ajs/user";
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  OnInit,
  HostListener,
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
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { Store } from "@ngrx/store";
import {
  EmployeeState,
  getEmployeeState,
} from "@ds/employees/header/ngrx/reducer";
import { IEmployeeSearchResult } from "@ds/employees/search/shared/models/employee-search-result";

import { ClientAccountFeature } from '@ds/core/clients/shared/client-account-feature.model';
import { DecimalPipe } from "@angular/common";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { IEmployeeAccrualList, IEmployeeAccrualInfo, IEmployeeBenefitSettings, IClientEmploymentClass, IBenefitPackage, ICustomBenefitField,
  IBenefitDependent, ICustomBenefitFieldValue, IClientBenefitSetting } from '../../models/employee-accruals/employee-accruals.model';
import { EmployeeAccrualsService } from '../../services/employee-accruals.service';
import {  BenefitsAdminService } from '../../services/benefits-admin.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SetEmployee } from '@ds/employees/header/ngrx/actions';
import { ChangeTrackerService } from '@ds/core/ui/forms/change-track/change-tracker.service';
import { Router } from '@angular/router';
import { changeDrawerHeightOnOpenSm, matDrawerAfterHeightChange } from "@ds/core/ui/animations/drawer-auto-height-animation";
import { Moment } from 'moment';
import { IEmployee } from '@ajs/core/ds-resource/models';
import { IEmployeeDependent } from '@ds/employees/profile/shared/employee-dependent.model';
import { IContact } from '@ds/core/contacts';
import { AddClassDialogComponent } from './add-class-dialog/add-class-dialog.component';

@Component({
  selector: "ds-employee-accruals",
  templateUrl: "./employee-accruals.component.html",
  styleUrls: ["./employee-accruals.component.scss"],
  animations: [
    changeDrawerHeightOnOpenSm,
    matDrawerAfterHeightChange
  ]
})
export class EmployeeAccrualsComponent implements OnInit, OnDestroy {
  destroy$ = new Subject();
  baseUrl = this.config.baseSite.url;
  f: FormGroup;
  customF: FormGroup;
  formSubmitted: boolean;

  hasError: boolean;
  clientId: number;
  employeeId: number;
  employeeInfo: IContact;
  isLoading: boolean = true;
  includeInActive: boolean = false;
  isBenefitPortalEnabled: boolean = false;
  minDate: Moment = null;

  employeeAccrualList:Array<IEmployeeAccrualInfo>;
  employmentClasses:Array<IClientEmploymentClass>;
  benefitPackages:Array<IBenefitPackage>;

  customFields:Array<ICustomBenefitField>;
  employeecustomFieldValues:Array<ICustomBenefitFieldValue>;
  employeeBenefitSettings: IEmployeeBenefitSettings = null;
  employeeDependents: Array<IEmployeeDependent>;
  benefitDependents:  Array<IBenefitDependent>;
  showAddDependent: boolean = false;
  showRetirementEligibilityOption: boolean = true;

  activeBenefitDependentId: number;
  activeBenefitDependent: IBenefitDependent;
  activeAccrual:IEmployeeAccrualInfo = null;
  activeAccrualOrig:IEmployeeAccrualInfo = null;
  activeId:number = 0;
  accrualKlicked:IEmployeeAccrualInfo = null;
  submitted:boolean;
  reminderDateActive: boolean;
  reminderDate: Moment;

  userinfo: UserInfo;
  totalCount: number = 0;
  hrBlocked: boolean;
  allowAddEmployee: boolean;
  employeeListUrl: string;
  isSupervisorOnHimself: boolean;
  essViewOnly: boolean;
  showSSN: boolean;
  showCustomFields: boolean;

  payrollUrl: ConfigUrl;
  companyUrl: ConfigUrl;
  legacyDependentsUrl:string;

  selectedEmployee$ = this.store.select(
    getEmployeeState((x) => x.selectedEmployee)
  ) as any as Observable<IEmployeeSearchResult>;

  get accruals() {
    return this.includeInActive ? this.employeeAccrualList : this.employeeAccrualList.filter(x=>x.isActive);
  }

  loadMasterData$ = (clientId) =>
    forkJoin( this.benAdminApi.getBenefitPackages(clientId),
      this.benAdminApi.getClientEmploymentClasses(clientId),
      this.benAdminApi.getClientCustomFields(clientId),
      this.accountService.getClientAccountFeature( clientId, Features.Benefit_Portal),
      this.benAdminApi.getClientBenefitSetup(clientId),  )
    .pipe( tap((x : [any,any,any,  any, any]) => {
      this.benefitPackages = x[0] || [];
      this.employmentClasses = x[1] || [];
      this.customFields = x[2] || [];
      this.showCustomFields = this.customFields.length > 0;
      this.isBenefitPortalEnabled = !!x[3];
      this.showRetirementEligibilityOption = (x[4]||{}).showRetirementEligibilityOption;
    }));

  loadEmployeeData$ = (clientId, employeeId) =>
    forkJoin(this.employeeAccrualsApi.getEmployeeAccrualList(clientId, employeeId) ,
    this.employeeAccrualsApi.getEmployeeBenefitSettings(clientId,employeeId) ,
    this.employeeAccrualsApi.getEmployeeClientCustomFields(clientId,employeeId),
    this.employeeAccrualsApi.getEmployeeProfileCard(employeeId),
    this.employeeAccrualsApi.getEmployeeDependents(employeeId),)
    .pipe( tap((x: [[],any,[],any,[]]) => {
        this.employeeAccrualList = x[0] || [];
        if(this.employeeAccrualList.length > 0 && this.employeeAccrualList.length == this.employeeAccrualList.filter(x=>!x.isActive).length)
          this.includeInActive = true;
        this.employeeBenefitSettings = x[1] || <any>{};
        this.employeecustomFieldValues = x[2] || [];
        this.employeeInfo = x[3];
        this.employeeDependents = x[4]||[];
        this.benefitDependents = this.employeeAccrualsApi.mapCustomValuesToDependents(this.employeeInfo,
          this.employeeDependents, this.customFields, this.employeecustomFieldValues);
        this.showAddDependent = this.benefitDependents.filter(x=>x.dependentId > 0).length === 0;
        this.resetPage();
      })
    );

  constructor(
    private router: Router,
    private accountService: AccountService,
    private clientService: ClientService,
    private msg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: ConfirmDialogService,
    @Inject(APP_CONFIG) private config: AppConfig,
    private store: Store<EmployeeState>,
    private employeeAccrualsApi: EmployeeAccrualsService,
    private benAdminApi: BenefitsAdminService,
    private changeTrackerService: ChangeTrackerService,
    private fb: FormBuilder,
  ) {}

  ngOnInit() {
    this.isLoading = true;
    this.employeeAccrualList = [];
    this.customFields = [];
    this.employeeBenefitSettings = <any>{};
    this.activeBenefitDependentId = 0;
    this.activeId = 0;
    this.reminderDate = moment().add(1,'day');
    this.minDate = moment().add(1,'day');
    this.reminderDateActive = false;

    if(!this.f) this.createForm();

    forkJoin([this.accountService.getUserInfo(true), this.accountService.getSiteUrls(), ]).subscribe( ([user,sites]) => {
      this.userinfo = user;
      this.clientId = user.selectedClientId();
      this.hrBlocked = user.isHrBlocked;
      this.essViewOnly = user.isEmployeeSelfServiceViewOnly;
      this.allowAddEmployee = user.addEmployee;
      this.employeeId = user.lastEmployeeId ;
      this.showSSN = (this.userinfo.userTypeId != UserType.supervisor);

      // redirect to no permisions page
      if(this.hrBlocked) this.router.navigate(['error']);

      this.payrollUrl = sites.find((s) => s.siteType === ConfigUrlType.Payroll);
      let essUrl      = sites.find((s) => s.siteType === ConfigUrlType.Ess);
      this.companyUrl = sites.find((s) => s.siteType === ConfigUrlType.Company);

      this.legacyDependentsUrl = `${this.payrollUrl.url}employeedependents.aspx`;
      this.employeeListUrl = `${this.payrollUrl.url}ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=${this.companyUrl.url}manage/benefits`;
      // if the user doesn't have an employee selected, redirect them to the employee select list
      if (this.employeeId == null || this.employeeId < 1) {
        document.location.href = this.employeeListUrl;
        return;
      }

      this.isSupervisorOnHimself = this.userinfo.userEmployeeId == this.employeeId;
      let api$ = of(null);
      if (!this.isSupervisorOnHimself)
        api$ = this.loadEmployeeData$(this.clientId, this.employeeId);

      this.loadMasterData$(this.clientId)
      .pipe(switchMap(x => api$))
      .subscribe((x) => {
        if(x) this.updateForm();
        this.isLoading = false;
      }, (error: HttpErrorResponse) => {
        this.msg.setErrorResponse(error);
        this.isLoading = false;
      });
    });


    this.selectedEmployee$
      .pipe(
        takeUntil(this.destroy$),
        filter((x) => !!x && x.employeeId != this.employeeId),
        switchMap(( ee :  IEmployeeSearchResult) => {
          if(!this.userinfo) return EMPTY;
          this.isLoading = true;
          this.employeeId = ee.employeeId ;
          this.isSupervisorOnHimself = this.userinfo.userEmployeeId == this.employeeId;
          let api$ = of(null);
          if (!this.isSupervisorOnHimself)
            api$ = this.loadEmployeeData$(this.clientId, this.employeeId);
          return api$;
        }),
      )
      .subscribe((x ) => {
        if(x) this.updateForm();
        this.isLoading = false;
      }, (error: HttpErrorResponse) => {
        this.msg.setErrorResponse(error);
        this.isLoading = false;
      });
  }
  switchEmployee () {
    this.isLoading = true;
  }
  private createForm(): void {
    this.f = this.fb.group({
        benefitEligible: this.fb.control('', []),
        eligibleDate: this.fb.control('', []),
        benefitPackage: this.fb.control('', []),
        salaryMethod: this.fb.control('', []),
        tobaccoUser: this.fb.control( false, []),
        employmentClass: this.fb.control('', []),
        retirementEligible: this.fb.control( false, []),
    });
    this.customF = this.fb.group({
    });
  }
  private updateForm(): void{
      this.f.setValue({
          benefitEligible: this.employeeBenefitSettings.isBenefitEligible || false,
          eligibleDate: this.employeeBenefitSettings.benefitEligibilityDate || '',
          benefitPackage: this.employeeBenefitSettings.benefitPackageId || '',
          salaryMethod: this.employeeBenefitSettings.defaultSalaryMethod || '',
          tobaccoUser: this.employeeBenefitSettings.isTobaccoUser || false,
          employmentClass: this.employeeBenefitSettings.clientEmploymentClassId || '',
          retirementEligible: this.employeeBenefitSettings.isRetirementEligible || false,
      });
  }
  private updateCustomFieldsForm(): void{
    this.customF.reset();
    this.customF = this.fb.group({});
    this.customFields.forEach( (fi,index) => {
      let k = this.activeBenefitDependent.customFields.find(x=>x.customFieldId == fi.customFieldId);
      let valu = '';
      if(k) valu = k.textValue;
      this.customF.addControl( 'field' + index, this.fb.control(valu));
    });
  }
  private updateModel(): IEmployeeBenefitSettings {
    let keepActual = this.reminderDateActive;
    let settings = this.employeeBenefitSettings;
    if(keepActual){
      settings = Object.assign({},this.employeeBenefitSettings);
    }
    settings.isBenefitEligible = this.f.value.benefitEligible;
    settings.benefitEligibilityDate = this.f.value.eligibleDate;
    settings.benefitPackageId = this.f.value.benefitPackage;
    settings.defaultSalaryMethod = this.f.value.salaryMethod;
    settings.isTobaccoUser = this.f.value.tobaccoUser;
    settings.clientEmploymentClassId = this.f.value.employmentClass;
    settings.isRetirementEligible = this.f.value.retirementEligible;
    return settings;
  }

  getAgeInYears(birthDate) {
    const today = moment();
    let hireDate = moment(birthDate);
    let lengthOfYears: number = today.diff(hireDate, 'years');
    return lengthOfYears.toString() + ' years';
  }
  getPackageName(Id:number){
    let p = this.benefitPackages.find(x=>x.benefitPackageId == Id);
    return p ? p.name : "" ;
  }
  getSalaryMethod(Id:number){
    switch(Id){
      case 1: return "Projected";
      case 2: return "Average";
      case 3: return "Custom";
    }
    return "";
  }
  getEmployementClass(Id:number){
    let p = this.employmentClasses.find(x=>x.clientEmploymentClassId == Id);
    return p ? p.description : "" ;
  }

  ngOnDestroy() {
    this.destroy$.next();
  }

  //#region "Accruals Select/UnSelect"
  saveEmployeeAccrual() {
    let accrualId = this.activeAccrual.clientAccrualId;
    let desc = this.activeAccrual.description;
    this.msg.loading(true, 'Sending...');
    this.employeeAccrualsApi.saveEmployeeAccrual( this.activeAccrual, this.employeeId, this.reminderDateActive, this.reminderDate)
    .subscribe((x : boolean) => {
      if(this.reminderDateActive)
        this.msg.setSuccessMessage(`"${desc}" update request have been saved to be applied at a later date.`);
      else
        this.msg.setSuccessMessage(`"${desc}" updated successfully.`);

      // For reminder check box active, reset to original value
      if(this.reminderDateActive && this.activeAccrualOrig) {
        let accr = this.employeeAccrualList.find(x => {
          return (x.clientAccrualId == accrualId);
        });
        accr.isActive = this.activeAccrualOrig.isActive;
        accr.allowScheduledAwards = this.activeAccrualOrig.allowScheduledAwards;
      }
    }, (error: HttpErrorResponse) => {
      this.msg.setErrorResponse(error);
    });
  }

  cogClicked(tl:IEmployeeAccrualInfo) {
    this.accrualKlicked = tl;
    this.accrualKlicked.hovered = false;
    setTimeout(() => {
      tl.hovered = true;
    }, 100);
  }

  toggleActiveAccrual(listId: number) {
    this.changeTrackerService.clearIsDirty();
    if (this.activeId == listId) {
      this.clearDrawer();
    } else {
      this.activeId = listId;
      this.activeAccrual = this.employeeAccrualList.find(x => {
        return (x.clientAccrualId == this.activeId);
      });
      this.activeAccrualOrig = Object.assign({},this.activeAccrual);
    }
  }

  clearDrawer() {
    if(this.activeAccrual && this.activeAccrual.isDirty){
      this.activeAccrual.isDirty = false;
      this.saveEmployeeAccrual();
    }
      this.activeId = 0;
      this.activeAccrual = null;
  }
  //#endregion

  //#region "Benefit dependents Select/Unselect"

  toggleActiveBenefitDependent(id:number){
    this.changeTrackerService.clearIsDirty();
    if (this.activeBenefitDependentId == id) {
      this.clearDrawer();
    } else {

      this.activeBenefitDependentId = id;
      this.activeBenefitDependent = this.benefitDependents.find(x => {
        return (x.dependentId == this.activeBenefitDependentId);
      });
      this.updateCustomFieldsForm();
    }
  }

  saveCustomFieldValues(){
    let depName = this.activeBenefitDependent.dependentId > 0 ? (this.activeBenefitDependent.firstName + ' ' + this.activeBenefitDependent.lastName) : 'Employee';
    let savables:ICustomBenefitFieldValue[] = [];
    this.customFields.forEach( (fi,index) => {
      let isModified: boolean = false;
      let k = this.activeBenefitDependent.customFields.find(x=>x.customFieldId == fi.customFieldId);
      let valu = this.customF.controls['field'+index].value;
      if(k) {
        isModified = k.textValue != valu;
        k.textValue = valu;
      }
      else {
        k = {
          customFieldId: fi.customFieldId,
          clientId: this.clientId,
          dependentId: this.activeBenefitDependentId,
          employeeId: this.employeeId,
          name: fi.name,
          textValue: valu,
          customFieldValueId: 0,
        };
        isModified = true;
        this.activeBenefitDependent.customFields.push(k);
        this.employeecustomFieldValues.push(k);
      }
      if(isModified) savables.push(k);
    });

    if(savables.length > 0){
      this.msg.loading(true, 'Sending...');
      this.employeeAccrualsApi.saveEmployeeClientCustomFields(this.clientId, savables)
      .subscribe(x => {
        x.forEach((s,index) => savables[index].customFieldValueId = s.customFieldValueId )
        this.msg.setSuccessMessage(`Your changes were saved successfully.`);
        this.changeTrackerService.clearIsDirty();
      }, (error: HttpErrorResponse) => {
        this.msg.setErrorResponse(error);
      });
    }
  }

  clearFieldsDrawer(){
    if(this.activeBenefitDependent && this.activeBenefitDependent.isDirty){
      this.saveCustomFieldValues();
      this.activeBenefitDependent.isDirty = false;
    }

    this.activeBenefitDependentId = 0;
    this.activeBenefitDependent = null;
  }
  //#endregion

  manageClassesDialog(){
    let config = new MatDialogConfig<any>();
    config.width = '400px';
    config.data = { user: this.userinfo, classes: this.employmentClasses };
    return this.dialog
      .open<AddClassDialogComponent, any, IClientEmploymentClass[]>(
        AddClassDialogComponent,
        config
      )
      .afterClosed()
      .subscribe((modifiedClasses: IClientEmploymentClass[]) => {
        if (!modifiedClasses) return;
        this.employmentClasses = modifiedClasses;
      });
  }

  saveBenefitSettings(){
    this.formSubmitted = true;
    if(this.f.valid){
      this.clearDrawer();
      this.clearFieldsDrawer();

      let toSave = this.updateModel();
      this.msg.loading(true, 'Sending...');
      this.employeeAccrualsApi.saveEmployeeBenefitSettings(toSave, this.employeeId, this.reminderDateActive, this.reminderDate)
      .subscribe(x => {
        if(this.reminderDateActive)
          this.msg.setSuccessMessage( 'Your changes have been saved to be applied at a later date.');
        else
          this.msg.setSuccessMessage( 'Your changes were saved successfully.');
        this.changeTrackerService.clearIsDirty();
        this.updateForm();
      }, (error: HttpErrorResponse) => {
        this.msg.setErrorResponse(error);
      });
    }
  }

  resetPage(){
    this.changeTrackerService.clearIsDirty();
    this.activeBenefitDependentId = 0;
    this.activeBenefitDependent = null;
    this.activeId = 0;
    this.activeAccrual = null;
  }
}
