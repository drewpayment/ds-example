import * as angular from "angular";
import { Component, OnInit, ViewChild } from '@angular/core';
import { ClientService } from '@ds/core/clients/shared';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from "@ds/core/shared/user-info.model";
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { tap, switchMap, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { HttpErrorResponse } from '@angular/common/http';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { EmployeeDataService } from "apps/ds-company/src/app/services/employee-data.service";
import {DashboardService} from "apps/ds-company/src/app/services/dashboard.service";
import { IEmployee } from '@ajs/core/ds-resource/models';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';
import { PERFORMANCE_ACTIONS } from "@ds/performance/shared/performance-actions";
import { empty, EMPTY, forkJoin, of } from "rxjs";
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { JobTitleConfirmDialogComponent } from "./job-title-confirm-dialog/job-title-confirm-dialog.component";
import { MatTableDataSource } from "@angular/material/table";


import { map } from 'rxjs/operators';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { MatStepper } from '@angular/material/stepper';
import { ChangeTrackerService } from "@ds/core/ui/forms/change-track/change-tracker.service";
import { UserType } from "@ds/core/shared";
import { DepartmentsComponent } from "../../../company-management/labor/departments/departments.component";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { ApplicantCorrespondenceTypeEnum, IOnboardingWorkflowTask } from '@models';



@Component({
  selector: 'ds-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.scss']
})
export class AddEmployeeComponent implements OnInit {
    hasError = false;
    isLoading = true;
    selectedStepper = 0;

    form1: FormGroup;
    form2: FormGroup;
    form3: FormGroup;

    form1Submitted: boolean;
    form2Submitted: boolean;
    form3Submitted: boolean;

    isSaving: boolean = false;

    paneId: number = 1;
    clientId: number;
    employeeId: number;
    isNewEmployee: boolean = false;
    employeeData: any;
    clientOnboardingWorkflows: any[] = [];
    selectedEmployeeOnboardingWorkflows: any[] = [];
    selectedEmployeeOnboardingWorkflowsAtStart: any[] = [];
    newStateSelection: any = null;
    currentTask: IOnboardingWorkflowTask = null;
    currentPageType: number;

    showOvertimeExempt: boolean = false;
    showTippedEmployee: boolean = false;

    private userInfo: UserInfo;
    hasOnboardingAdminAccess: boolean = false;
    hasCustomPageAccess: boolean = true;

    benefitsData: any;
    additionalInfoNotInForm: any = {};
    employeeStatusList: any[] = [];
    jobTitlesList: any[] = [];
    clientDivisionsList: any[] = [];
    clientDepartmentsList: any[] = [];
    costCentersList: any[] = [];
    clientGroupsList: any[] = [];
    clientShiftsList: any[] = [];
    workersCompsList: any[] = [];
    eeocLocationsList: any[] = [];
    eeocJobCategoriesList: any[] = [];
    directSupervisorList: any[] = [];

    payFrequencies: any[] = [];
    sutaStates: any[] = [];
    timePolicies: any[] = [];

    clientRateList: any[] = [];
    clientRatesDisplayColumns: string[] = [
      "clientRateName",
      "clientRateAmount",
    ];
    employeeClientRatesDatasource = new MatTableDataSource<any>([]);

    templateCategories: any[] = [{id: 4, desc:'General', isActive:true },
                                 {id: 1, desc:'Applicant Tracking', isActive:true }
                                ];

    allTemplates: any[] = [];
    onboardingInvitationTemplates: any[] = [];

    usernamePattern = "^[^ ]{8,15}$";

    returnsToDetail: boolean = false;
    pageTitle:string = 'New Hire';

    get ClientRatesArray() { return this.form1.get('clientRatesArray') as FormArray; }
    get FirstName() { return this.form1.controls.FirstName as FormControl; }
    get MiddleInitial() { return this.form1.controls.MiddleInitial as FormControl; }
    get LastName() { return this.form1.controls.LastName as FormControl; }

    get HireDate() { return this.form1.controls.HireDate as FormControl; }
    get EmployeeNumber() { return this.form1.controls.EmployeeNumber as FormControl; }
    get EmployeeStatus() { return this.form1.controls.EmployeeStatus as FormControl; }

    get JobTitle() { return this.form1.controls.JobTitle as FormControl; }
    get ClientDivision() { return this.form1.controls.ClientDivision as FormControl; }
    get ClientDepartment() { return this.form1.controls.ClientDepartment as FormControl; }

    get CostCenter() { return this.form1.controls.CostCenter as FormControl; }
    get ClientGroup() { return this.form1.controls.ClientGroup as FormControl; }
    get ClientShift() { return this.form1.controls.ClientShift as FormControl; }

    get WorkersComp() { return this.form1.controls.WorkersComp as FormControl; }
    get EeocJobLocation() { return this.form1.controls.EeocJobLocation as FormControl; }
    get EeocJobCategory() { return this.form1.controls.EeocJobCategory as FormControl; }

    get DirectSupervisor() { return this.form1.controls.DirectSupervisor as FormControl; }

    get PayrollType() { return this.form1.controls.PayrollType as FormControl; }
    get IsTippedEmployee() { return this.form1.controls.IsTippedEmployee as FormControl; }
    get IsOvertimeExempt() { return this.form1.controls.IsOvertimeExempt as FormControl; }

    get PayFrequency() { return this.form1.controls.PayFrequency as FormControl; }
    get SutaState() { return this.form1.controls.SutaState as FormControl; }
    get TimePolicy() { return this.form1.controls.TimePolicy as FormControl; }

    get DefaultRate() { return this.form1.controls.DefaultRate as FormControl; }
    get Salary() { return this.form1.controls.Salary as FormControl; }

    get Username() { return this.form1.controls.Username as FormControl; }
    get EmailAddress() { return this.form1.controls.EmailAddress as FormControl; }

    get InvitationTemplateCategory() { return this.form3.controls.InvitationTemplateCategory as FormControl; }
    get InvitationTemplate() { return this.form3.controls.InvitationTemplate as FormControl; }
    get InvitationEmail() { return this.form3.controls.InvitationEmail as FormControl; }
    get InvitationSubjectLine() { return this.form3.controls.InvitationSubjectLine as FormControl; }
    get InvitationMessageBody() { return this.form3.controls.InvitationMessageBody as FormControl; }


    isHandset$: Observable<boolean> = this.breakpointObserver.observe([Breakpoints.XSmall])
        .pipe(
            map(result => result.matches)
        );

    constructor(
      private accountService: AccountService,
      private clientService: ClientService,
      private msgSvc: NgxMessageService,
      private store: DsStorageService,
      private dsEmployeeData: EmployeeDataService,
      private dashboardApi: DashboardService,
      private router: Router,
      private route: ActivatedRoute,
      private fb1: FormBuilder,
      private fb2: FormBuilder,
      private fb3: FormBuilder,
      private dialog: MatDialog,
      private breakpointObserver: BreakpointObserver,
      private changeTrackerService: ChangeTrackerService
      ) {}

    ngOnInit() {
      this.isLoading = true;
      this.form1Submitted = false;
      this.form2Submitted = false;
      this.form3Submitted = false;

      this.paneId = this.route.snapshot.params['paneId'];
      this.clientId = this.route.snapshot.params['clientId'];
      this.employeeId = this.route.snapshot.params['employeeId'] || 0;
      this.returnsToDetail = this.route.snapshot.data.returnsToDetail || false;

      if(this.returnsToDetail) this.pageTitle = "Edit Onboarding Setup";

      this.buildForm();

      // if (this.employeeId == 0 && this.paneId != 1) {
      //   //this.changeTrackerService.clearIsDirty();
      //   this.router.navigate(['manage/onboarding/', 'add-employee', this.clientId, this.employeeId, 1, 'add']);
      // }

      // if (this.employeeId > 0 && this.paneId == 1) {
      //   //this.changeTrackerService.clearIsDirty();
      //   this.router.navigate(['manage/onboarding/', 'add-workflow', this.clientId, this.employeeId, 2, 'add']);
      // }

      this.selectedStepper = this.paneId-1;

      if (this.paneId == 1) {
        this.showTippedEmployee = true;
        this.employeeData = {
          hireDate: null,
          sutaState: null,
          username: null,
          email: null,
          isOtExempt: false,
          employeeStatusId: null,
          employeeNumber: ''
        };
      }

      if (this.employeeId == 0) {
        this.isNewEmployee = true;
        this.dashboardApi.getNextEmployeeNumber(this.clientId).pipe(
          tap(dto => {
            this.employeeData.employeeNumber = dto.nextEmployeeNumber;
          })).subscribe();
      }

      const $employeeInfo = this.dashboardApi.getEmployeeList(this.clientId, {isOnboardingComplete: null, employeeId: this.employeeId}).pipe(
        tap(employeeData => {
          this.employeeData = employeeData[0];
          if(this.returnsToDetail) this.pageTitle = "Edit Onboarding Setup for " + this.employeeData.employeeFirstName + ' ' +this.employeeData.employeeLastName;
        })
      );

      const $employeeClientRatesInfo = this.dashboardApi.getEmployeeClientRateList(this.employeeId).pipe(
        tap(employeeClientRates => {
          this.employeeClientRatesDatasource.data = employeeClientRates;
        })
      );


      const $workflowInfo = this.dashboardApi.getClientOnboardingWorkflows(this.clientId).pipe(
          tap(clientWorkflow => {
            this.clientOnboardingWorkflows = clientWorkflow;
          }),
        switchMap(hasOnboardingAdminAccess => this.dashboardApi.getEmployeeWorkflow(this.employeeId)),
          tap(empWorkflow => {
            this.selectedEmployeeOnboardingWorkflows = empWorkflow.data;
            this.selectedEmployeeOnboardingWorkflowsAtStart = empWorkflow.data;
          }),
      );

      const $invitationInfo = this.dashboardApi.getEmailTemplates(this.clientId).pipe(
        tap(templates => {
          this.allTemplates = templates;
          this.onboardingInvitationTemplates = this.allTemplates.filter(item => item.applicantCorrespondenceTypeId == ApplicantCorrespondenceTypeEnum.onboardingInvitation);

          this.templateCategories[0].isActive = templates.filter(item => item.applicantCorrespondenceTypeId == ApplicantCorrespondenceTypeEnum.onboardingInvitation).length > 0;
          this.templateCategories[1].isActive = templates.filter(item => item.applicantCorrespondenceTypeId == ApplicantCorrespondenceTypeEnum.applicationResponse).length > 0;
          this.templateCategories = this.templateCategories.filter(item => item.isActive == true);
        })
      );

      this.accountService.getUserInfo().pipe(
        tap(userInfo => {
          this.userInfo = userInfo;
          if (this.userInfo.userTypeId == UserType.supervisor)
            this.hasCustomPageAccess = false;
        }),
        switchMap(userInfo => this.accountService.canPerformActions('Onboarding.OnboardingAdministrator')),
        catchError((err, caught) => {
          return of(false);
        }),
        tap(hasAccess => {
            this.hasOnboardingAdminAccess = !!hasAccess;
        }),
        switchMap(hasAccess => {
          if (hasAccess && this.employeeId > 0 && this.paneId == 1) //Should never reach here
            return forkJoin([$employeeInfo, $employeeClientRatesInfo]);
          else if (hasAccess && this.employeeId > 0 && this.paneId == 2)
            return forkJoin([$employeeInfo, $workflowInfo]);
          else if (hasAccess && this.employeeId > 0 && this.paneId == 3)
            return forkJoin([$employeeInfo, $invitationInfo]);
          else
            return of(null);
        })
        ).subscribe(x => {
          if (this.paneId == 1) {
            const $employeeStatusDropDownData = this.dashboardApi.getEmployeeStatusList().pipe(
              tap(employeeStatusList => {
                this.employeeStatusList = employeeStatusList;
              })
            );

            const $jobTitleDropDownData = this.dashboardApi.getClientJobTitleList(this.clientId).pipe(
              tap(jobTitlesList => {
                this.jobTitlesList = jobTitlesList;
              })
            );

            const $clientDivisionsDropDownData = this.dashboardApi.getClientDivisionsList(this.clientId).pipe(
              tap(clientDivisionsList => {
                this.clientDivisionsList = clientDivisionsList;
                this.loadDepartments().subscribe();
              })
            );

            const $costCenterDropDownData = this.dashboardApi.getClientCostCentersList(this.clientId).pipe(
              tap(costCentersList => {
                this.costCentersList = costCentersList;
              })
            );

            const $clientGroupDropDownData = this.dashboardApi.getClientGroupsList(this.clientId).pipe(
              tap(clientGroupsList => {
                this.clientGroupsList = clientGroupsList;
              })
            );

            const $clientShiftDropDownData = this.dashboardApi.getClientShiftsList(this.clientId).pipe(
              tap(clientShiftsList => {
                this.clientShiftsList = clientShiftsList;
              })
            );

            const $workersCompDropDownData = this.dashboardApi.getClientWorkersCompList(this.clientId).pipe(
              tap(workersCompsList => {
                this.workersCompsList = workersCompsList;
              })
            );

            const $eeocJobLocationDropDownData = this.dashboardApi.getClientEEOCLocationList(this.clientId).pipe(
              tap(eeocLocationsList => {
                this.eeocLocationsList = eeocLocationsList;
              })
            );

            const $eeocJobCategoriesDropDownData = this.dashboardApi.getEEOCJobCategoriesList()
            .pipe(
              catchError((error, caught) => {
                this.eeocJobCategoriesList = [];
                //return EMPTY;
                return of(null);
              }),
              tap((eeocJobCategoriesList) => {
                if (!!eeocJobCategoriesList)
                  this.eeocJobCategoriesList = eeocJobCategoriesList;
              }
            ));


            const $directSupervisorsDropDownData = this.dashboardApi.getDirectSupervisorsForClient(this.clientId).pipe(
              tap(directSupervisorList => {
                this.directSupervisorList = directSupervisorList;
              })
            );

            const $payFrequencyDropDownData = this.dashboardApi.getPayFrequencyList().pipe(
              tap(payFrequencies => {
                this.payFrequencies = payFrequencies;
              })
            );

            const $sutaStateDropDownData = this.dashboardApi.getClientSutaStateList(this.clientId).pipe(
              tap(sutaStates => {
                this.sutaStates = sutaStates;
              })
            );

            const $timePolicyDropDownData = this.dashboardApi.getTimePolicyList(this.clientId).pipe(
              tap(timePolicies => {
                this.timePolicies = timePolicies;
              })
            );

            const $clientRateDropDownData = this.dashboardApi.getClientRateList(this.clientId).pipe(
              tap(clientRates => {
                this.clientRateList = clientRates;
              })
            );

            const $benefitsData = this.dashboardApi.getBenefitsDataList(this.clientId).pipe(
              tap(benefitsData => {
                this.benefitsData = benefitsData;
              })
            );

            forkJoin([$employeeStatusDropDownData, $jobTitleDropDownData, $clientDivisionsDropDownData, $costCenterDropDownData,
              $clientGroupDropDownData, $clientShiftDropDownData, $workersCompDropDownData, $eeocJobLocationDropDownData,
              $eeocJobCategoriesDropDownData, $directSupervisorsDropDownData, $payFrequencyDropDownData, $sutaStateDropDownData,
              $timePolicyDropDownData, $clientRateDropDownData, $benefitsData
            ]).subscribe(y => {
                this.updateForm();
                this.isLoading = false;
              }
            );
          }

          if (this.paneId == 2) {
            this.isLoading = false;
          }

          if (this.paneId == 3) {
            this.updateForm();
            this.isLoading = false;
          }
      });
    }

    //#region "Add Employee related"

    private addToClientRatesArray(clientRatesArray: FormArray, item: any) {
      clientRatesArray.push(this.fb1.group({
        clientRateName: item.clientRateId,
        isDefaultRate: item.isDefaultRate,
        clientRateAmount: [item.rate, Validators.compose([Validators.required, Validators.pattern('^\\d*(,\\d+)*\\.?[0-9]+$')])],
      }));
    }

    buildForm() {
      if (this.paneId == 1) {
        this.form1 = this.fb1.group({
          clientRatesArray: this.fb1.array([]),
          FirstName: this.fb1.control('', Validators.compose([Validators.required, Validators.maxLength(50)])),
          MiddleInitial: this.fb1.control(''),
          LastName: this.fb1.control('', Validators.compose([Validators.required, Validators.maxLength(50)])),

          HireDate: this.fb1.control('', Validators.compose([Validators.required])),
          EmployeeNumber: this.fb1.control('', Validators.compose([Validators.required, Validators.pattern('^[1-9][0-9]*$')])),
          EmployeeStatus: this.fb1.control('', Validators.compose([Validators.required])),

          JobTitle: this.fb1.control(''),
          ClientDivision: this.fb1.control(''),
          ClientDepartment: this.fb1.control(''),//{value: '', disabled: false}

          CostCenter: this.fb1.control(''),
          ClientGroup: this.fb1.control(''),
          ClientShift: this.fb1.control(''),

          WorkersComp: this.fb1.control(''),
          EeocJobLocation: this.fb1.control(''),
          EeocJobCategory: this.fb1.control(''),

          DirectSupervisor: this.fb1.control(''),

          PayrollType: this.fb1.control('1'),
          IsTippedEmployee: this.fb1.control(false),
          IsOvertimeExempt: this.fb1.control(false),

          PayFrequency: this.fb1.control('', Validators.compose([Validators.required])),
          SutaState: this.fb1.control('', Validators.compose([Validators.required])),
          TimePolicy: this.fb1.control(''),

          DefaultRate: this.fb1.control('', Validators.compose([Validators.required])),
          Salary: this.fb1.control(''),

          Username: this.fb1.control('', Validators.compose([Validators.required, Validators.pattern(this.usernamePattern)])),
          EmailAddress: this.fb1.control('', Validators.compose([Validators.required])),
        });

        this.PayrollType.valueChanges.subscribe(val => {
          if (val == 1) {
            this.form1.patchValue ({
              IsOvertimeExempt: false,
              IsTippedEmployee: false,
              DefaultRate: this.clientRateList[0].clientRateId
            });

            this.showTippedEmployee = true;
            this.showOvertimeExempt = false;

            this.Salary.setValidators(null);
            this.Salary.updateValueAndValidity();
            this.DefaultRate.setValidators([Validators.required]);
            this.DefaultRate.updateValueAndValidity();

            this.clearEmployeeClientRates();
            this.initializeEmployeeClientRate();
          }
          else {
            this.form1.patchValue ({
              IsOvertimeExempt: false,
              IsTippedEmployee: false,
              DefaultRate: ''
            });

            this.showTippedEmployee = false;
            this.showOvertimeExempt = true;

            this.DefaultRate.setValidators(null);
            this.DefaultRate.updateValueAndValidity();
            this.Salary.setValidators([Validators.required]);
            this.Salary.updateValueAndValidity();

            this.clearEmployeeClientRates();
            this.initializeEmployeeClientRate();
          }
        });
      }
      else
        this.form1 = this.fb1.group({});

      if (this.paneId == 2) {
        this.form2 = this.fb2.group({
        });
      }
      else
        this.form2 = this.fb2.group({});

      if (this.paneId == 3) {
        this.form3 = this.fb3.group({
          InvitationTemplateCategory: this.fb3.control('', Validators.compose([Validators.required])),
          InvitationTemplate: this.fb3.control('', Validators.compose([Validators.required])),
          InvitationEmail: this.fb3.control('', Validators.compose([Validators.required, Validators.maxLength(75)])),
          InvitationSubjectLine: this.fb3.control('', Validators.compose([Validators.required, Validators.maxLength(75)])),
          InvitationMessageBody: this.fb3.control('', Validators.compose([Validators.required])),
        });
      }
      else
        this.form3 = this.fb3.group({});
    }

    clearEmployeeClientRates() {
      this.employeeClientRatesDatasource = new MatTableDataSource<any>([]);
      this.ClientRatesArray.clear();
    }

    initializeEmployeeClientRate() {
      if(this.employeeData.employeeClientRates && this.employeeData.employeeClientRates.length){
        this.employeeData.employeeClientRates.forEach(clientRate => {
          this.addToClientRatesArray(this.ClientRatesArray, clientRate);
          this.employeeClientRatesDatasource.data.push(clientRate);
          this.employeeClientRatesDatasource.data = this.employeeClientRatesDatasource.data.slice();
        });

        return;
      }
      let defaultClientRate = {clientRateId: this.clientRateList[0].clientRateId, rate: 0.00, isDefaultRate: true};
      this.employeeClientRatesDatasource.data.push(defaultClientRate);
      this.employeeClientRatesDatasource.data = this.employeeClientRatesDatasource.data.slice();
      this.addToClientRatesArray(this.ClientRatesArray, defaultClientRate);
    }

    updateForm() {

      if (this.paneId == 1) {
        this.employeeData = this.employeeData ? this.employeeData : <any>{};
        this.employeeData.employeePayInfo = this.employeeData.employeePayInfo ? this.employeeData.employeePayInfo[0] : <any>{};
        this.form1.patchValue ({
          FirstName: this.employeeData.employeeFirstName || '',
          MiddleInitial: this.employeeData.employeeMiddleName || '',
          LastName: this.employeeData.employeeLastName || '',
          HireDate: this.employeeData.hireDate || '',
          EmployeeNumber: this.employeeData.employeeNumber || '',
          EmployeeStatus: this.employeeData.employeeStatus || '',
          JobTitle: this.jobTitlesList.find(x => x.jobProfileId == this.employeeData.jobProfileId) || '',
          ClientDivision: this.employeeData.clientDivisionId || '',
          ClientDepartment: this.employeeData.clientDepartmentId || '',
          CostCenter: this.employeeData.clientCostCenterId || '',
          ClientGroup: this.employeeData.clientGroupId || '',
          ClientShift: this.employeeData.employeePayInfo.clientShiftId || '',
          WorkersComp: this.employeeData.clientWorkersCompId || '',
          EeocJobLocation: this.employeeData.eeocLocationId || '',
          EeocJobCategory: this.employeeData.eeocJobCategoryId || '',
          DirectSupervisor: this.employeeData.directSupervisorID || '',
          PayrollType: (this.employeeData.payType || 1).toString(),
          IsTippedEmployee: this.employeeData.employeePayInfo.isTippedEmployee || false,
          IsOvertimeExempt: this.employeeData.isExempt || false,
          PayFrequency: this.employeeData.employeePayInfo.payFrequencyId || '',
          SutaState: this.employeeData.employeePayInfo.clientTaxId || '',
          TimePolicy: '',
          DefaultRate: this.employeeData.employeeClientRates ? this.employeeData.employeeClientRates[0].clientRateId : <any>{},
          Salary: this.employeeData.employeePayInfo.salaryAmount || '',
          Username: '',
          EmailAddress: this.employeeData.emailAddress
        });

        if(this.ClientDepartment.value && this.ClientDepartment.value !== ''){
          this.loadDepartments().subscribe();
        }

        // let defaultClientRate = {clientRateId: this.clientRateList[0].clientRateId, rate: 0.00, isDefaultRate: true};
        // this.employeeClientRatesDatasource.data.push(defaultClientRate);
        // this.employeeClientRatesDatasource.data = this.employeeClientRatesDatasource.data.slice();
        // this.addToClientRatesArray(this.ClientRatesArray, defaultClientRate);
      }

      if (this.paneId == 3) {
        this.form3.patchValue ({
          // InvitationTemplateCategory: this.templateCategories.find((item) => {
          //   return item => item.id == ApplicantCorrespondenceTypeEnum.onboardingInvitation;
          // }),

          InvitationTemplateCategory: this.templateCategories.find(item =>
            item => item.id == ApplicantCorrespondenceTypeEnum.onboardingInvitation
          ),


          InvitationTemplate: this.onboardingInvitationTemplates[0],
          InvitationEmail: this.employeeData.emailAddress,
          InvitationSubjectLine: this.onboardingInvitationTemplates[0].subject,
          InvitationMessageBody: this.onboardingInvitationTemplates[0].body.replace(/<br\s{0,1}\/>/g, '\n')
        });
        this.invitationTemplateChanged();
      }
    }

    setDefaultClientRate() {
      let defaultExistsInTable: boolean = false;
      this.ClientRatesArray.controls.forEach((element, index) => {
        if (element.get('clientRateName').value == this.DefaultRate.value) {
          defaultExistsInTable = true;
          return;
        }
      });

      if (this.DefaultRate.value && !defaultExistsInTable) {
        let defaultClientRate = {clientRateId: this.DefaultRate.value, rate: 0.00, isDefaultRate: true};
        this.employeeClientRatesDatasource.data.push(defaultClientRate);
        this.employeeClientRatesDatasource.data = this.employeeClientRatesDatasource.data.slice();
        this.addToClientRatesArray(this.ClientRatesArray, defaultClientRate);
      }
      // this.employeeClientRatesDatasource.data.forEach( (item) => {
      //   if (item.isDefaultRate && item.clientRateId != this.DefaultRate.value)
      //     item.isDefaultRate = false;

      //   if (!item.isDefaultRate && item.clientRateId == this.DefaultRate.value)
      //     item.isDefaultRate = true;
      // });
    }

    loadDepartments() {
      if (this.ClientDivision.value) {
        return this.dashboardApi.getClientDepartmentsList(this.ClientDivision.value)
        .pipe(
          catchError((error, caught) => {
            this.msgSvc.setErrorResponse(error);
            return EMPTY;
          }),
          tap((clientDepartments) => {
            if (clientDepartments) {
              this.clientDepartmentsList = clientDepartments;
              this.ClientDepartment.enable();
            }
            else {
              this.clientDepartmentsList = null;
              this.ClientDepartment.disable();
            }
          }
        ));
      }
      else {
        this.clientDepartmentsList = null;
        this.ClientDepartment.disable();
        return EMPTY;
      }
    }

    loadJobTitleConfirmDialog() {
      if (this.JobTitle.value) {
        let config = new MatDialogConfig<any>();
        config.width = "800px";
        config.data = {clientId: this.clientId, jobProfileId: this.JobTitle.value.jobProfileId};

        return this.dialog.open<JobTitleConfirmDialogComponent, any, number>(JobTitleConfirmDialogComponent, config)
        .afterClosed()
        .subscribe((jobProfileData: any) => {
            if (jobProfileData) {
              let classifications = jobProfileData.data.jobProfileDto.classifications;
              let compensation    = jobProfileData.data.jobProfileDto.compensation;

              if (classifications.clientDivisionId) {
                this.form1.patchValue ({
                  ClientDivision: classifications.clientDivisionId || '',
                });
                this.loadDepartments().subscribe(x =>
                  this.form1.patchValue ({
                    ClientDepartment: classifications.clientDepartmentId || '',
                  })
                );
              }
              else {
                this.form1.patchValue ({
                  ClientDivision: '',
                  ClientDepartment: ''
                });
              }

              if (classifications.clientGroupId) {
                this.form1.patchValue ({
                  ClientGroup: classifications.clientGroupId || '',
                });
              }

              if (classifications.eeocLocationId) {
                this.form1.patchValue ({
                  EeocJobLocation: classifications.eeocLocationId || '',
                });
              }

              if (classifications.eeocJobCategoryId) {
                this.form1.patchValue ({
                  EeocJobCategory: classifications.eeocJobCategoryId || '',
                });
              }

              if (compensation.employeeTypeID) {
                this.form1.patchValue ({
                  PayrollType: compensation.employeeTypeID.toString() || '1',
                  IsOvertimeExempt: false,
                  IsTippedEmployee: false
                });

                if (compensation.employeeTypeID == 2) {
                  this.showTippedEmployee = false;
                  this.showOvertimeExempt = true;

                  this.clearEmployeeClientRates();
                  let defaultClientRate = {clientRateId: this.clientRateList[0].clientRateId, rate: 0.00, isDefaultRate: true};
                  this.employeeClientRatesDatasource.data.push(defaultClientRate);
                  this.addToClientRatesArray(this.ClientRatesArray, defaultClientRate);

                }
                else {
                  this.showTippedEmployee = true;
                  this.showOvertimeExempt = false;

                  this.clearEmployeeClientRates();
                  this.initializeEmployeeClientRate();
                  // this.employeeClientRatesDatasource.data = []; //Clear
                  // let defaultClientRate = {clientRateId: this.clientRateList[0].clientRateId, rate: 0.00, isDefaultRate: true};
                  // this.employeeClientRatesDatasource.data.push(defaultClientRate);
                  // this.employeeClientRatesDatasource.data = this.employeeClientRatesDatasource.data.slice();
                  // this.addToClientRatesArray(this.ClientRatesArray, defaultClientRate);
                }
              }

              if (compensation.employeeTypeID == 1 && compensation.isExempt) {
                this.form1.patchValue ({
                  IsOvertimeExempt: compensation.isExempt,
                });
              }

              if (compensation.payFrequencyID) {
                this.form1.patchValue ({
                  PayFrequency: compensation.payFrequencyID || '',
                });
              }

              if (classifications.clientShiftId) {
                this.form1.patchValue ({
                  ClientShift: classifications.clientShiftId || '',
                });
              }

              if (classifications.clientWorkersCompId) {
                this.form1.patchValue ({
                  WorkersComp: classifications.clientWorkersCompId || '',
                });
              }

              if (classifications.clientCostCenterId) {
                this.form1.patchValue ({
                  CostCenter: classifications.clientCostCenterId || '',
                });
              }

              if (classifications.employeeStatusId) {
                this.form1.patchValue ({
                  EmployeeStatus: classifications.employeeStatusId || '',
                });
              }

              if (classifications.directSupervisorId) {
                this.form1.patchValue ({
                  DirectSupervisor: classifications.directSupervisorId || '',
                });
              }

              if (compensation.isBenefitsEligibility)
                this.additionalInfoNotInForm.isBenefitsEligibility = compensation.isBenefitsEligibility;

              if (compensation.benefitPackageId)
                this.additionalInfoNotInForm.benefitPackageId = compensation.benefitPackageId;

              if (compensation.salaryMethodTypeId)
                this.additionalInfoNotInForm.salaryMethodTypeId = compensation.salaryMethodTypeId;

              if (jobProfileData.data.jobProfileDto.competencyModelId)
                this.additionalInfoNotInForm.competencyModelId = jobProfileData.data.jobProfileDto.competencyModelId;
            }
        });
      }
    }

    addEmployeeClientRate() {
      if (this.employeeClientRatesDatasource.data.length < this.clientRateList.length) {
        let additionalClientRate = {rate: 0.00, isDefaultRate: false};
        this.employeeClientRatesDatasource.data.push(additionalClientRate);
        this.employeeClientRatesDatasource.data = this.employeeClientRatesDatasource.data.slice();
        this.addToClientRatesArray(this.ClientRatesArray, additionalClientRate);
      }
      else {
        this.msgSvc.setErrorMessage("Limit reached for Employee Client Rates.");
      }
    }

    getClientRateName(clientRateId) {
      let clientRateName = '';
      if (clientRateId)
        clientRateName = this.clientRateList.find(x => x.clientRateId == clientRateId).description;

      return clientRateName;
    }
    //#endregion

    //#region "Send Email related"

    templateCategoryChanged() {
      this.onboardingInvitationTemplates = this.allTemplates.filter(item => item.applicantCorrespondenceTypeId == this.InvitationTemplateCategory.value.id);
      if (this.onboardingInvitationTemplates.length > 0) {
        this.form3.patchValue ({
          InvitationTemplate: this.onboardingInvitationTemplates[0],
          InvitationSubjectLine: this.onboardingInvitationTemplates[0].subject,
          InvitationMessageBody: this.onboardingInvitationTemplates[0].body.replace(/<br\s{0,1}\/>/g, '\n'),
        });
      } else {
        this.form3.patchValue ({
          InvitationTemplate: '',
          InvitationSubjectLine: '',
          InvitationMessageBody: '',
        });
      }
    }

    invitationTemplateChanged() {
      if (this.InvitationTemplate.value != null) {
        let messageBody = this.InvitationTemplate.value.body;

        if (!this.employeeData.userAddedDuringOnboarding && this.InvitationTemplate.value.applicantCorrespondenceTypeId == ApplicantCorrespondenceTypeEnum.onboardingInvitation) {
          messageBody = messageBody.replace('Your username and temporary password are provided below.', 'Use your existing username and password from when you applied for this position. If you don’t remember your password please click the Forgot Password link.');
          messageBody = messageBody.replace('Password: {*Password}', '');
        }

        this.form3.patchValue ({
          InvitationSubjectLine: this.InvitationTemplate.value.subject,
          InvitationMessageBody: messageBody.replace(/<br\s{0,1}\/>/g, '\n'),
        });
      }
    }

    checkMsg() {
      let isMsgValid = false;  // Default to invalid.
      if (this.employeeData.userAddedDuringOnboarding) { //Have to fetch this value***********
        if (this.InvitationMessageBody.value.indexOf('{*UserName}') > -1 &&
          this.InvitationMessageBody.value.indexOf('{*OnboardingUrl}') > -1 &&
          this.InvitationMessageBody.value.indexOf('{*Password}') > -1) {
            isMsgValid = true;
        } else {
          this.msgSvc.setErrorMessage('Message is missing required field(s): {*UserName},{*Password}, {*OnboardingUrl}.');
        }
      } else {
          if (this.InvitationMessageBody.value.indexOf('{*UserName}') > -1 &&
            this.InvitationMessageBody.value.indexOf('{*OnboardingUrl}') > -1) {
              isMsgValid = true;
          } else {
            this.msgSvc.setErrorMessage('Message is missing required field(s): {*UserName}, {*OnboardingUrl}.');
          }
      }
      return isMsgValid ;
    }

    //#endregion

    //#region "Add Workflow related"

    addCustomPage(route: string) {
      this.currentPageType = Utilities.getPageTypeByRoute(route);

      this.currentTask = {
        onboardingWorkflowTaskId: 0,
        route: route,
        route1: route,
        linkToState: Utilities.getLinkToStateByRoute(route),
        modifiedBy: 0,
        modified: new Date(),
        clientId: this.clientId,
        resources: [],
        workflowTitle: null,
        description: null,
        adminDescription: null,
        adminMustSelect: true,
        signatureDescription: null,
        isReferred: false,
        hasActiveWorkflowReference: false,
        requireWorkFlowTaskId: false,
        uploadDescription: null,
        userMustUpload: false,
        userMustUploadResource: false,
        userMustCheckAgreement: false
      };

      //this.changeTrackerService.clearIsDirty();
      this.router.navigate(['admin/onboarding/manage-resources', this.currentTask.onboardingWorkflowTaskId, this.currentPageType, 'add', 'workflow', this.clientId, this.employeeId]);
    }

    updateSelectedState(selectedState: any) {
      this.newStateSelection = selectedState;
      const selectedItem = this.selectedEmployeeOnboardingWorkflows.find(x => x.onboardingWorkflowTaskId == 7);
      if (selectedItem) {
        selectedItem.formTypeId = this.newStateSelection.formTypeId;
        selectedItem.stateId = this.newStateSelection.stateId;
      }
    }

    updateSelections(tasks: any[]) {
      if (this.newStateSelection) {
        const selectedItem = tasks.find(x => x.onboardingWorkflowTaskId == 7);
        if (selectedItem) {
          selectedItem.formTypeId = this.newStateSelection.formTypeId;
          selectedItem.stateId = this.newStateSelection.stateId;
        }
      }
      this.selectedEmployeeOnboardingWorkflows = tasks;
    }

    //#endregion

    //#region "Save, Cancel"

    private prepareModel(): any {
      if (this.paneId == 1) {
        this.additionalInfoNotInForm = this.additionalInfoNotInForm ? this.additionalInfoNotInForm : <any>{};
        return {
          firstName: this.FirstName.value,
          middleInitial: this.MiddleInitial.value,
          lastName: this.LastName.value,

          hireDate: this.HireDate.value,
          employeeId: this.employeeId,
          employeeNumber: this.EmployeeNumber.value,
          employeeStatusId: this.EmployeeStatus.value,

          jobTitle: this.JobTitle.value.description, //Have to change it to text
          jobProfileId: this.JobTitle.value.jobProfileId,
          clientDivisionId: this.ClientDivision.value,
          clientDepartmentId: this.ClientDepartment.value,

          clientCostCenterId: this.CostCenter.value,
          clientGroupId: this.ClientGroup.value,
          clientShiftId: this.ClientShift.value,

          clientWorkersCompId: this.WorkersComp.value,
          eEOCLocationId: this.EeocJobLocation.value,
          eEOCJobCategoryId: this.EeocJobCategory.value,

          directSupervisorId: this.DirectSupervisor.value,

          payTypeId: this.PayrollType.value,
          isTippedEmployee: this.IsTippedEmployee.value,
          isOtExempt: this.IsOvertimeExempt.value,
          salaryAmount: this.Salary.value,

          payFrequencyId: this.PayFrequency.value,
          sutaState: this.SutaState.value,
          clockClientTimePolicyId: this.TimePolicy.value,
          userId: 0,
          username: this.Username.value,
          email: this.EmailAddress.value,

          employeeWorkflow: [],
          isFromAddEmployee: false,

          isBenefitsEligibility: this.additionalInfoNotInForm.isBenefitsEligibility || '',
          benefitPackageId: this.additionalInfoNotInForm.benefitPackageId || '',
          salaryMethodTypeId: this.additionalInfoNotInForm.salaryMethodTypeId || '',
          competencyModelId: this.additionalInfoNotInForm.competencyModelId || ''
        };
      }
    }

    save() {
      if (!this.hasOnboardingAdminAccess)
        return;

      if (this.paneId == 1) {
        this.form1.markAllAsTouched();

        if (this.form1.invalid)
          return;

        var valueArr = this.ClientRatesArray.controls.map((item) => {
          return item.get('clientRateName').value;
        });

        var hasDuplicate = valueArr.some((item, idx) => {
            return valueArr.indexOf(item) != idx
        });

        if (hasDuplicate) {
          this.msgSvc.setErrorMessage("Each rate can only be added once.");
          return;
        }
        let employeeClientRates: any[] = [];
        this.ClientRatesArray.controls.forEach((element, index) => {
          employeeClientRates.push({clientRateId: element.get('clientRateName').value,
                                    isDefaultRate: this.DefaultRate.value == element.get('clientRateName').value,
                                    rate: element.get('clientRateAmount').value
                                  });
        });

        const dto = this.prepareModel();
        dto.clientId = this.clientId;
        dto.isBenefitPortalOn = this.benefitsData.isBenefitPortalOn;
        dto.employeeClientRates = employeeClientRates;

        var parsedDate = Date.parse(dto.hireDate);
        if (isNaN(parsedDate)) {
          this.msgSvc.setErrorMessage("Please enter a valid hire date.");
          return;
        }

        this.msgSvc.setWarningMessage('Sending...');
        this.form1Submitted = true;
        this.dashboardApi.isUsernameAvailable(this.userInfo.userId, dto.username).subscribe((data) => {
          if (data.isValid) {
            //To give access to the Supervisor who added this New Hire
            if (this.userInfo.userTypeId == UserType.supervisor && !dto.directSupervisorId)
              dto.directSupervisorId = this.userInfo.userId;

            this.dashboardApi.saveNewEmployee(dto).subscribe((savedEmployee) => {
              dto.clientId                  = savedEmployee.clientId;
              dto.employeeId                = savedEmployee.employeeId;
              dto.userName                  = savedEmployee.username;
              dto.password                  = savedEmployee.password;
              dto.emailAddress              = savedEmployee.email;
              dto.firstName                 = savedEmployee.firstName;
              dto.lastName                  = savedEmployee.lastName;
              dto.userAddedDuringOnboarding = true;
              dto.clientName                = savedEmployee.clientName;
              dto.userId                    = savedEmployee.userId;
              dto.employeeWorkflow          = savedEmployee.employeeWorkflow;

              this.msgSvc.setSuccessMessage("Employee saved successfully.");
              this.changeTrackerService.clearIsDirty();
              this.router.navigate(['manage/onboarding/', 'add-workflow', this.clientId, dto.employeeId, 2, 'add']);

            }, error => {
              this.msgSvc.setErrorMessage(error.error.errors[0].msg);
              this.form1Submitted = false;
              return;
            });
          }
          else {
            this.msgSvc.setErrorMessage('Username already exists.');
            this.form1Submitted = false;
            return;
          }
        }, error => {
            if (error.error.errors[0].errorMessage.indexOf('Username already exists') > 0) {
              this.msgSvc.setErrorMessage(error.error.errors[0].errorMessage);
              this.form1Submitted = false;
              return;
            } else { // display first general error message found
              this.msgSvc.setErrorMessage(error.error.errors[0].errorMessage);
              this.form1Submitted = false;
              return;
            }
        });
      }

      if (this.paneId == 2) {
        length = this.selectedEmployeeOnboardingWorkflows.filter(item => item.onboardingWorkflowTaskId != 1 && item.onboardingWorkflowTaskId != 2 && item.onboardingWorkflowTaskId != 8).length;

        //Save is not allowed without atleast a single non required task.
        if (length == 0) {
          this.msgSvc.setErrorMessage("At least one workflow should be selected.");
          return;
        }

        this.msgSvc.setWarningMessage('Sending...');
        this.form2Submitted = true;
        //Get the list of MainTaskIds referred on the Selected Workflows
        let selectedMainTaskIds = [];
        for (let idx = 0; idx < this.selectedEmployeeOnboardingWorkflows.length; idx++) {
            //Add EmployeeId if not exists
            if(!this.selectedEmployeeOnboardingWorkflows[idx].hasOwnProperty('employeeId'))
              (this.selectedEmployeeOnboardingWorkflows[idx])['employeeId'] = this.employeeId;

            if (this.selectedEmployeeOnboardingWorkflows[idx].mainTaskId != null) {
                if (this.selectedEmployeeOnboardingWorkflows[idx].isChecked) {
                    // collect the main tasks selected based on sub task selection
                    if (selectedMainTaskIds.indexOf(this.selectedEmployeeOnboardingWorkflows[idx].mainTaskId) < 0)
                        selectedMainTaskIds.push(this.selectedEmployeeOnboardingWorkflows[idx].mainTaskId);
                }
            }
        }

        // Make sure the main tasks are checked
        for (let main = 0; main < selectedMainTaskIds.length; main++) {
          if (!this.selectedEmployeeOnboardingWorkflows.some(t => t.onboardingWorkflowTaskId == selectedMainTaskIds[main])) {
              this.selectedEmployeeOnboardingWorkflows.unshift({formTypeId: null, employeeId: this.employeeId, onboardingWorkflowTaskId: selectedMainTaskIds[main]});
          }
        }

        //Make sure the Required tasks(1, 2, 8) are checked
        this.clientOnboardingWorkflows.forEach( (workflow) => {
          if (workflow.isRequired && !this.selectedEmployeeOnboardingWorkflows.some(t => t.onboardingWorkflowTaskId == workflow.onboardingWorkflowTaskId)) {
            this.selectedEmployeeOnboardingWorkflows.unshift({formTypeId: null, employeeId: this.employeeId, onboardingWorkflowTaskId: workflow.onboardingWorkflowTaskId});
          }
        });

        let hireDate: string = this.employeeData.rehireDate
        ? new Date(this.employeeData.rehireDate).toISOString().slice(0, 10)
        : new Date(this.employeeData.hireDate).toISOString().slice(0, 10);

        this.dashboardApi.addEmployeeWorkflow(this.selectedEmployeeOnboardingWorkflows, hireDate.split('-').join(''))
        .subscribe((result) => {
          this.msgSvc.setSuccessMessage("Employee workflow saved successfully.");
          this.changeTrackerService.clearIsDirty();
          if(this.returnsToDetail)
            this.router.navigate(['manage/onboarding/', 'dashboard-detail', this.employeeId ]);
          else
            this.router.navigate(['manage/onboarding/', 'add-email-template', this.clientId, this.employeeId, 3, 'add']);
        }, (error: HttpErrorResponse) => {
            this.msgSvc.setErrorResponse(error);
        });
      }

      if (this.paneId == 3) {
        this.form3.markAllAsTouched();

        if (this.form3.invalid)
          return;

        if (!this.checkMsg())
          return;

        this.isSaving = true;
        this.form3Submitted = true;
        this.msgSvc.setWarningMessage('Sending...');

        let dto = this.employeeData;
        dto.subject = this.InvitationTemplate.value.subject;
        dto.msg = this.InvitationTemplate.value.body.replace(/<br\s{0,1}\/>/g, '\n').replace(/\n/g, '<br/>');;
        dto.correspondenceId = (this.InvitationTemplate.value) ? this.InvitationTemplate.value.applicantCompanyCorrespondenceId : null;
        dto.emailAddress = this.employeeData.emailAddress;

        const $updateAndSendEmail = this.dashboardApi.updateEmployeeEmail(dto).pipe(
          tap(response => {

          }),
          switchMap(hasOnboardingAdminAccess => this.dashboardApi.sendEmployeeOnboardingEmail(dto)),
          tap(response => {
          }, error => {
            this.msgSvc.setErrorMessage("An error occurred sending the email message.");
            this.form3Submitted = false;
            return;
          }),
          switchMap(hasOnboardingAdminAccess => this.dashboardApi.updateInvitationEmailSent(this.employeeId)),
          tap(result => {
            this.isSaving = false;
            this.msgSvc.setSuccessMessage("Onboarding Email sent successfully.");
            this.changeTrackerService.clearIsDirty();
            this.redirectToDashboard();
          }),
        );

        const $sendEmailWithoutUpdating = this.dashboardApi.sendEmployeeOnboardingEmail(dto).pipe(
          tap(response => {
          }, error => {
            this.msgSvc.setErrorMessage("An error occurred sending the email message.");
            this.form3Submitted = false;
            return;
          }),
          switchMap(hasOnboardingAdminAccess => this.dashboardApi.updateInvitationEmailSent(this.employeeId)),
          tap(result => {
            this.isSaving = false;
            this.msgSvc.setSuccessMessage("Onboarding Email sent successfully.");
            this.changeTrackerService.clearIsDirty();
            this.redirectToDashboard();
          }),
        );

        if (this.employeeData.emailAddress != this.InvitationEmail.value) {
          dto.emailAddress = this.InvitationEmail.value;
          forkJoin([$updateAndSendEmail]).subscribe(y => {});
        }
        else {
          forkJoin([$sendEmailWithoutUpdating]).subscribe(y => {});
        }
      }
    }

    redirectToDashboard() {
      if(this.returnsToDetail)
        this.router.navigate(['manage/onboarding/', 'dashboard-detail', this.employeeId ]);
      else
        this.router.navigate(['manage/onboarding/dashboard']);
    }

    //#endregion
}
