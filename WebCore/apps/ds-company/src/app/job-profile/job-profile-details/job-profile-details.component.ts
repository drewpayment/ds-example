import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { AccountService } from '@ds/core/account.service';
import { JobProfileApiService } from 'apps/ds-company/src/app/services/job-profile-api.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { tap, switchMap, filter, startWith, map, debounceTime, catchError } from 'rxjs/operators';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { IBenefitPackagesData, IClientDepartmentData, IClientDivisionData, IClientGroupData, IClientShiftData, IClientWorkersCompData, ICoreClientCostCenterData, IDirectSupervisorData, IEEOCJobCategoryData, IEEOCLocationData, IEmployeeStatusData, IJobDetailData, IJobProfileAccrualsData, IJobProfileClassificationsData, IJobProfileCompensationData, IJobProfileOnboardingWorkflowData, IJobResponsibilitiesData, IJobSkillsData, IOnboardingAdminTaskListData, IPayFrequencyListData, ISalaryDeterminationMethodData } from 'apps/ds-company/src/app/models/job-profile.model';
import { DsStorageService } from '@ds/core/storage/storage.service';
import * as angular from 'angular';
import { empty, EMPTY, Observable, of } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { ICompetencyModelBasic } from '@ds/performance/competencies/shared/competency-model.model';
import { IJobProfileAccrualData } from '@ajs/employee/add-employee/shared/models';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';
import { IOnboardingWorkflowTask } from '@models';
import { JobProfileTitleDialogComponent } from '../job-profile-title-dialog/job-profile-title-dialog.component';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { UserType } from '@ds/core/shared';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-job-profile-details',
  templateUrl: './job-profile-details.component.html',
  styleUrls: ['./job-profile-details.component.css']
})
export class JobProfileDetailsComponent implements OnInit {
  jobProfile: IJobDetailData = null;
  
  jobResponsibilities: IJobResponsibilitiesData[] = [];
  jobResponsibilitiesClean: IJobResponsibilitiesData[] = [];
  selectedJobResponsibility:IJobResponsibilitiesData;
  filteredJobResponsibilitiesOptions: IJobResponsibilitiesData[];

  jobSkills: IJobSkillsData[] = [];
  jobSkillsClean: IJobSkillsData[] = [];
  selectedJobSkill:IJobSkillsData;
  filteredJobSkillsOptions: IJobSkillsData[];

  jobProfileClassifications: IJobProfileClassificationsData;
  jobProfileClassificationsClean: IJobProfileClassificationsData;

  jobProfileCompensation: IJobProfileCompensationData;
  jobProfileCompensationClean: IJobProfileCompensationData;

  onboardingAdminTaskList: IOnboardingAdminTaskListData[] = [];

  isLoading: boolean = true;
  userInfo: UserInfo;
  isApplicantAdmin: boolean = true;
  form: FormGroup;
  formSubmitted: boolean = false;
  
  hasCompetencyAccess: boolean = false;
  allowCreatingCustomPages: boolean = false;
  showOvertimeExempt: boolean = false;
  showTippedEmployee: boolean = false;

  formJobProfileTitle: FormGroup;
  formJobProfileTitleSubmitted: boolean = false;
  tmpTitle: string;
  editingTitle: boolean = false;

  jpId: number;
  cId: number;

  descWidgetColor: string = 'disabled';
  employeeRecordWidgetColor: string = 'disabled';
  competenciesWidgetColor: string = 'disabled';
  onboardingTasksWidgetColor: string = 'disabled';

  currentPageType: number;
  currentTask: IOnboardingWorkflowTask = null;

  formJobResponsibilitySubmitted: boolean = false;
  isAddingJobResponsibility: boolean = false;
  get AddJobResponsibilityName(): FormControl {
      return this.form.controls.jobResponsibilitiesForm.get('AddJobResponsibilityName') as FormControl
  }

  get EditJobResponsibilityName(): FormControl {
    return this.form.controls.jobResponsibilitiesForm.get('EditJobResponsibilityName') as FormControl
  }

  formJobSkillSubmitted: boolean = false;
  isAddingJobSkill: boolean = false;
  get AddJobSkillName(): FormControl {
    return this.form.controls.jobSkillsForm.get('AddJobSkillName') as FormControl
  }

  get EditJobSkillName(): FormControl {
    return this.form.controls.jobSkillsForm.get('EditJobSkillName') as FormControl
  }

  clientDivisionsList: IClientDivisionData[] = [];
  clientDepartmentsList: IClientDepartmentData[] = [];
  clientGroupsList: IClientGroupData[] = [];
  directSupervisorList: IDirectSupervisorData[] = [];

  payFrequencies: IPayFrequencyListData[];
  clientShiftsList: IClientShiftData[];
  jobStatusList: IEmployeeStatusData[];
  costCentersList: ICoreClientCostCenterData[];

  benefitPackages: IBenefitPackagesData[];
  workersCompsList: IClientWorkersCompData[];
  salaryMethodTypes: ISalaryDeterminationMethodData[];

  eeocLocationsList: IEEOCLocationData[];
  eeocJobCategoriesList: IEEOCJobCategoryData[];

  jobProfileAccruals: IJobProfileAccrualsData[];
  jobProfileAccrualsClean: IJobProfileAccrualsData[];

  clientAccruals: IJobProfileAccrualsData[];
  clientAccrualsClean: IJobProfileAccrualsData[];

  selectedJobProfileAccruals: IJobProfileAccrualsData[] = [];
  allJobProfileAccruals: IJobProfileAccrualsData[] = [];
  filteredJobProfileAccruals: Observable<IJobProfileAccrualsData[]>;

  jobProfileOnboardingWorkflows: IJobProfileOnboardingWorkflowData[] = [];

  get JobProfileTitle() { return this.formJobProfileTitle.controls.JobProfileTitle as FormControl; }
  get Requirements() { return this.form.controls.Requirements as FormControl; }
  get ClientDivision() { return this.form.controls.ClientDivision as FormControl; }
  get ClientDepartment() { return this.form.controls.ClientDepartment as FormControl; }
  get DirectSupervisor() { return this.form.controls.DirectSupervisor as FormControl; }
  get ClientGroup() { return this.form.controls.ClientGroup as FormControl; }
  get JobClass() { return this.form.controls.JobClass as FormControl; }

  get WorkingConditions() { return this.form.controls.WorkingConditions as FormControl; }
  get Benefits() { return this.form.controls.Benefits as FormControl; }

  get PayFrequency() { return this.form.controls.PayFrequency as FormControl; }
  get ClientShift() { return this.form.controls.ClientShift as FormControl; }
  get EmploymentStatus() { return this.form.controls.EmploymentStatus as FormControl; }
  get Hours() { return this.form.controls.Hours as FormControl; }
  get CostCenter() { return this.form.controls.CostCenter as FormControl; }
  get PayrollType() { return this.form.controls.PayrollType as FormControl; }
  get IsTippedEmployee() { return this.form.controls.IsTippedEmployee as FormControl; }
  get IsOvertimeExempt() { return this.form.controls.IsOvertimeExempt as FormControl; }
  get BenefitPackage() { return this.form.controls.BenefitPackage as FormControl; }
  get WorkersComp() { return this.form.controls.WorkersComp as FormControl; }
  get SalaryMethodType() { return this.form.controls.SalaryMethodType as FormControl; }
  get EEOCLocation() { return this.form.controls.EEOCLocation as FormControl; }
  get EEOCJobCategory() { return this.form.controls.EEOCJobCategory as FormControl; }
  get JobProfileAccrual() { return this.form.controls.JobProfileAccrual as FormControl; }
  get AdminTaskList() { return this.form.controls.AdminTaskList as FormControl; }
  get BenefitsEligible() { return this.form.controls.BenefitsEligible as FormControl; }

  constructor(private accountService: AccountService,
    private jobProfileService: JobProfileApiService, 
    private msg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: ConfirmDialogService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private storeService: DsStorageService) { 
    }

  ngOnInit(): void {
    this.isLoading = true;

    this.cId = this.route.snapshot.params['cId'];
    this.jpId = this.route.snapshot.params['jpId'];

    this.formSubmitted = false;
    this.formJobResponsibilitySubmitted = false;
    this.formJobSkillSubmitted = false;

    this.isAddingJobResponsibility = false;
    this.isAddingJobSkill = false;

    this.accountService.getUserInfo().pipe(
      switchMap(userInfo => {
        this.userInfo = userInfo;
        if (userInfo.userTypeId == UserType.supervisor) {
          this.isApplicantAdmin = userInfo.isApplicantAdmin;
          if (this.isApplicantAdmin)
            return of(userInfo);
          else {
            this.redirectToJobProfiles();
            return EMPTY;  
          }
        }
        return of(userInfo);
      }), 
      switchMap(userInfo => this.accountService.canPerformActions(PERFORMANCE_ACTIONS.Performance.AssignJobProfileCompetencyModel)),
      catchError((err, caught) => {
        return of(false);
      }),
      tap(hasAccess => {
          this.hasCompetencyAccess = !!hasAccess;
      }),
      switchMap(userInfo => this.accountService.canPerformActions("JobManagement.JobProfileAdministrator")),
      catchError((err, caught) => {
        return of(false);
      }),
      tap(hasAccess => {
          this.allowCreatingCustomPages = !!hasAccess;
      }),
      switchMap(hasAccess => this.jobProfileService.getJobProfileDetails(this.jpId, this.cId)), //userInfo.lastClientId || userInfo.clientId)),
        tap(jobProfileInitData => {
          if (jobProfileInitData.data) {
            let data = jobProfileInitData.data;
            let jobProfileDto = data.jobProfileDto;
            let jobSkills = data.skills;
            let jobResponsibilities = data.responsibilities;
            let clientAccruals = data.clientAccrualList;
            let jobProfileAccruals = jobProfileDto.jobProfileAccruals;
            this.tmpTitle = jobProfileDto.description;

            this.jobProfile = {
              jobProfileId: jobProfileDto.jobProfileId,
              clientId: jobProfileDto.clientId,
              description: jobProfileDto.description,
              code: jobProfileDto.code,
              requirements: jobProfileDto.requirements,
              isActive: jobProfileDto.isActive,
              workingConditions: jobProfileDto.workingConditions,
              benefits: jobProfileDto.benefits,
              isBenefitPortalOn: jobProfileDto.isBenefitPortalOn,
              isApplicantTrackingOn: jobProfileDto.isApplicantTrackingOn,
              sourceURL: jobProfileDto.sourceURL,
              competencyModelId: jobProfileDto.competencyModelId,
              isOnboardingEnabled: jobProfileDto.isOnboardingEnabled,
              isPerformanceReviewsEnabled: jobProfileDto.isPerformanceReviewsEnabled,
              onboardingAdminTaskListId: jobProfileDto.onboardingAdminTaskListId
            };

            this.clientAccruals = clientAccruals;
            this.clientAccrualsClean = angular.copy(this.clientAccruals);
            this.allJobProfileAccruals = angular.copy(this.clientAccruals) || [];

            this.jobProfileAccruals = jobProfileAccruals;
            this.jobProfileAccrualsClean = angular.copy(this.jobProfileAccruals);
            this.selectedJobProfileAccruals = angular.copy(this.jobProfileAccruals) || [];

            this.allJobProfileAccruals = this.allJobProfileAccruals.filter(ar => !this.jobProfileAccruals.find(rm => (rm.clientAccrualId === ar.clientAccrualId)));            

            this.jobResponsibilities = data.responsibilities;
            this.jobResponsibilitiesClean = angular.copy(this.jobResponsibilities);

            this.jobSkills = data.skills;
            this.jobSkillsClean = angular.copy(this.jobSkills);

            this.jobProfileClassifications = jobProfileDto.classifications;
            this.jobProfileClassifications.jobResponsibilities = this.jobProfileClassifications.jobResponsibilities || [];
            this.jobProfileClassifications.jobSkills = this.jobProfileClassifications.jobSkills || [];
            this.jobProfileClassificationsClean = angular.copy(this.jobProfileClassifications);

            this.jobProfileCompensation = jobProfileDto.compensation;
            this.jobProfileCompensationClean = angular.copy(this.jobProfileCompensation);
            if ((this.jobProfileCompensation.employeeTypeID || 1) == 1)
              this.showTippedEmployee = true;
            else
              this.showOvertimeExempt = true;

            this.jobProfileOnboardingWorkflows = jobProfileDto.jobProfileOnboardingWorkflows;

            if (this.jobProfileClassifications.departments) {
              this.clientDepartmentsList = this.jobProfileClassifications.departments;
              //this.ClientDepartment.enable();
            } else {
              this.clientDepartmentsList = null;
              //this.ClientDepartment.disable();
            }

            this.clientDivisionsList = data.clientDivisionList;
            this.clientGroupsList = data.clientGroupList;
            this.directSupervisorList = data.supervisorAndCompAdminList;

            this.payFrequencies = data.payFrequencyList;
            this.clientShiftsList = data.clientShiftList;
            this.jobStatusList = data.employeeStatusList;
            this.costCentersList = data.clientCostCenterList;

            this.benefitPackages = data.benefitPackageList;
            this.workersCompsList = data.clientWorkersCompList;
            this.salaryMethodTypes = data.salaryDeterminationMethodList;

            this.eeocLocationsList = data.eeocLocationList;
            this.eeocJobCategoriesList = data.eeocJobCategoryList;

            this.onboardingAdminTaskList = data.onboardingAdminTaskList;

            this.buildForm();

            //Set Widget Colors
            this.setWidgetColors();

            this.form.valueChanges.subscribe(obj => {
              this.prepareModel();
              this.setWidgetColors();
            });

            this.Requirements.valueChanges.subscribe(val => {
              this.jobProfile.requirements = this.Requirements.value;
            });
        
            this.PayrollType.valueChanges.subscribe(val => {
              if (val == 1) {
                this.form.patchValue ({
                  IsOvertimeExempt: false,
                  IsTippedEmployee: false
                });
            
                this.showTippedEmployee = true;
                this.showOvertimeExempt = false;
              }
              else {
                this.form.patchValue ({
                  IsOvertimeExempt: false,
                  IsTippedEmployee: false
                });

                this.showTippedEmployee = false;
                this.showOvertimeExempt = true;
              }
            });

            this.AddJobResponsibilityName.valueChanges.pipe(tap(x => {
              if(x){
                  if(typeof x === 'object') 
                      this.selectedJobResponsibility = x;
                  else
                      this.filteredJobResponsibilitiesOptions = this.jobResponsibilities
                        .filter(m => this.jobProfileClassifications.jobResponsibilities.map(k=>k.jobResponsibilityId).indexOf(m.jobResponsibilityId) == -1 );
              } else {
                  this.filteredJobResponsibilitiesOptions = this.jobResponsibilities
                    .filter(m => this.jobProfileClassifications.jobResponsibilities.map(k=>k.jobResponsibilityId).indexOf(m.jobResponsibilityId) == -1 );
              }
            })).subscribe();

            this.AddJobSkillName.valueChanges.pipe(tap(x => {
              if(x){
                  if(typeof x === 'object') 
                      this.selectedJobSkill = x;
                  else
                      this.filteredJobSkillsOptions = this.jobSkills
                        .filter(m => this.jobProfileClassifications.jobSkills.map(k=>k.jobSkillId).indexOf(m.jobSkillId) == -1 );
              } else {
                  this.filteredJobSkillsOptions = this.jobSkills
                    .filter(m => this.jobProfileClassifications.jobSkills.map(k=>k.jobSkillId).indexOf(m.jobSkillId) == -1 );
              }
            })).subscribe();

            this.WorkingConditions.valueChanges.subscribe(val => {
              this.jobProfile.workingConditions = this.WorkingConditions.value;
            });

            this.Benefits.valueChanges.subscribe(val => {
              this.jobProfile.benefits = this.Benefits.value;
            });

            this.filteredJobProfileAccruals = this.JobProfileAccrual.valueChanges.pipe(
              debounceTime(250),
              startWith<string | IJobProfileAccrualsData>(''),
              map(val => typeof val === 'string' ? val : (val?val.description:'') ),
              map((accrualDesc: string | null) => accrualDesc ? this._filter(accrualDesc) : this.allJobProfileAccruals.slice()));      

            this.isLoading = false;
          }
        })
    ).subscribe();
  }

  private setWidgetColors() {
    if (this.jobProfile.requirements ||
      this.jobProfile.benefits || 
      this.jobProfile.workingConditions ||
      this.jobProfileClassifications.jobResponsibilities.length > 0 ||
      this.jobProfileClassifications.jobSkills.length > 0
    ) {
      this.descWidgetColor = 'info';
    }
    else {
      this.descWidgetColor = 'disabled';
    }

    if (this.jobProfileClassifications.clientDivisionId ||
      this.jobProfileClassifications.clientDepartmentId || 
      this.jobProfileClassifications.directSupervisorId ||
      this.jobProfileClassifications.clientGroupId ||
      this.jobProfileClassifications.jobClass ||
      this.jobProfileCompensation.payFrequencyID ||
      this.jobProfileClassifications.clientShiftId ||
      this.jobProfileClassifications.employeeStatusId ||
      this.jobProfileCompensation.hours ||
      this.jobProfileClassifications.clientCostCenterId ||
      this.jobProfileCompensation.benefitPackageId ||
      this.jobProfileClassifications.clientWorkersCompId ||
      this.jobProfileCompensation.salaryMethodTypeId ||
      this.jobProfileClassifications.eeocLocationId ||
      this.jobProfileClassifications.eeocJobCategoryId ||
      this.selectedJobProfileAccruals.length > 0) {
        this.employeeRecordWidgetColor = 'info';
    }
    else {
      this.employeeRecordWidgetColor = 'disabled';
    }    

    if (this.jobProfile.competencyModelId && this.jobProfile.competencyModelId > 0) {
      this.competenciesWidgetColor = 'info';
    }
    else {
      this.competenciesWidgetColor = 'disabled';
    }    

    if ((this.jobProfile.onboardingAdminTaskListId && this.jobProfile.onboardingAdminTaskListId > 0) ||
      this.jobProfileOnboardingWorkflows.length > 0) {
        this.onboardingTasksWidgetColor = 'info';
    }
    else {
      this.onboardingTasksWidgetColor = 'disabled';
    }    
  }

  buildForm() {
    this.formJobProfileTitle = this.fb.group({
      JobProfileTitle: this.fb.control(this.jobProfile.description || '', [Validators.required])
    });

    this.form = this.fb.group({
      Requirements: this.fb.control(this.jobProfile.requirements || ''),
      jobResponsibilitiesForm: this.fb.group({
        AddJobResponsibilityName: this.fb.control(''),
        EditJobResponsibilityName: this.fb.control(''),
      }),
      jobSkillsForm: this.fb.group({
        AddJobSkillName: this.fb.control(''),
        EditJobSkillName: this.fb.control(''),
      }),
      WorkingConditions: this.fb.control(this.jobProfile.workingConditions || ''),
      Benefits: this.fb.control(this.jobProfile.benefits || ''),

      ClientDivision: this.fb.control(this.jobProfileClassifications.clientDivisionId || ''),
      ClientDepartment: this.fb.control({value: this.jobProfileClassifications.clientDepartmentId || '', disabled: false}),
      DirectSupervisor: this.fb.control(this.jobProfileClassifications.directSupervisorId || ''),
      ClientGroup: this.fb.control(this.jobProfileClassifications.clientGroupId || ''),
      JobClass: this.fb.control(this.jobProfileClassifications.jobClass || ''),

      PayFrequency: this.fb.control(this.jobProfileCompensation.payFrequencyID || ''),
      ClientShift: this.fb.control(this.jobProfileClassifications.clientShiftId || ''),
      EmploymentStatus: this.fb.control(this.jobProfileClassifications.employeeStatusId || ''),
      Hours: this.fb.control(this.jobProfileCompensation.hours || '', [Validators.pattern('/^[0-9]\d{0,9}(\.\d{1,3})?%?$/')]),
      CostCenter: this.fb.control(this.jobProfileClassifications.clientCostCenterId || ''),
      Amount: this.fb.control({value: '100', disabled: true}),
      PayrollType: this.fb.control( (this.jobProfileCompensation.employeeTypeID || 1).toString()),
      IsTippedEmployee: this.fb.control(this.jobProfileCompensation.isTipped || false),
      IsOvertimeExempt: this.fb.control(this.jobProfileCompensation.isExempt || false),
      BenefitPackage: this.fb.control(this.jobProfileCompensation.benefitPackageId || ''),
      WorkersComp: this.fb.control(this.jobProfileClassifications.clientWorkersCompId || ''),
      SalaryMethodType: this.fb.control(this.jobProfileCompensation.salaryMethodTypeId || ''),
      BenefitsEligible: this.fb.control( (this.jobProfileCompensation.isBenefitsEligibility|| false).toString() ),
      EEOCLocation: this.fb.control(this.jobProfileClassifications.eeocLocationId || ''),
      EEOCJobCategory: this.fb.control(this.jobProfileClassifications.eeocJobCategoryId || ''),
      JobProfileAccrual: this.fb.control(''),
      AdminTaskList: this.fb.control(this.jobProfile.onboardingAdminTaskListId || ''),
    });
  }

  public responsibilitiesDisplayFn(responsibility?: any): string | undefined {
    return responsibility ? responsibility.description : undefined;
  }

  public skillsDisplayFn(skill?: any): string | undefined {
    return skill ? skill.description : undefined;
  }

  //#region "Manage Job Responsibilities"
  provisionJobResponsibility(index: number) { 
    this.resetJobResponsibility();

    if (index == -1) { //Add
      this.isAddingJobResponsibility = true;
      this.AddJobResponsibilityName.setValidators(Validators.required);
      this.focusAddJobResponsibilityName();
    }
    else { //Edit
      this.EditJobResponsibilityName.setValidators(Validators.required);

      this.jobProfileClassifications.jobResponsibilities[index].isResponsibilityEditing = true;
      this.EditJobResponsibilityName.setValue( this.jobProfileClassifications.jobResponsibilities[index].description );
      this.focusEditJobResponsibilityName();
    }
  }

  resetJobResponsibility(){
    this.jobProfileClassifications.jobResponsibilities.forEach(x=>x.isResponsibilityEditing = false);
    this.form.controls.jobResponsibilitiesForm.reset();
    this.formJobResponsibilitySubmitted = false;
    this.isAddingJobResponsibility = false;
    this.EditJobResponsibilityName.clearValidators();
    this.AddJobResponsibilityName.clearValidators();
  }

  removeJobResponsibility(jobResponsibility:IJobResponsibilitiesData, index: number){
    this.resetJobResponsibility();
    const options = {
        title: 'Are you sure you want to remove this responsibility?',
        confirm: "Remove",
        width: "300px",
    };
    this.confirmDialog.open(options);
    this.confirmDialog.confirmed().pipe(
    filter(ok => !!ok),
    tap(x => {
      this.msg.setWarningMessage('Sending...');
      this.jobProfileService.deleteJobProfileResponsibility({
        jobProfileId: this.jobProfile.jobProfileId,
        jobResponsibilityId: jobResponsibility.jobResponsibilityId
      })
      .subscribe((result) => {
        if (result.success) {
          this.resetJobResponsibility();
          this.jobProfileClassifications.jobResponsibilities = result.data;
          this.msg.setSuccessMessage("Job profile responsibility removed successfully.");
        }
      }, (error: HttpErrorResponse) => {
          this.msg.setErrorResponse(error);
      });
    })).subscribe();
  }

  //Add
  addJobResponsibility() {
    this.provisionJobResponsibility(-1);
  }

  addJobResponsibilityDetails() {
    this.formJobResponsibilitySubmitted = true;
    if (this.form.controls.jobResponsibilitiesForm.valid) {
      this.msg.setWarningMessage('Sending...');
      let responsibilityId = null;
      let responsibilityName = '';

      if (typeof this.AddJobResponsibilityName.value === 'object') {
        responsibilityId = this.AddJobResponsibilityName.value.jobResponsibilityId;
        responsibilityName = this.AddJobResponsibilityName.value.description;
      }
      else 
        responsibilityName = this.AddJobResponsibilityName.value;

      const responsibilityAlreadyExists = this.jobProfileClassifications.jobResponsibilities.some(obj => obj.description.toLowerCase() === responsibilityName.toLowerCase())  
      if (responsibilityAlreadyExists) {
        this.AddJobResponsibilityName.setErrors({'incorrect': true});
        this.msg.setErrorMessage('Job Responsibility: '+ responsibilityName +' already exists.', 2000);
        return false;
      }

      this.jobProfileService.updateJobProfileResponsibility({
        jobResponsibilityId: responsibilityId,
        description: responsibilityName,
        clientId: this.userInfo.lastClientId || this.userInfo.clientId,
        jobProfileId: this.jobProfile.jobProfileId
      })
      .subscribe((result) => {
        this.jobProfileClassifications.jobResponsibilities = result.data;
        this.resetJobResponsibility();
        this.msg.setSuccessMessage("Job Responsibility added successfully.");
      }, (error: HttpErrorResponse) => {
          this.msg.setErrorResponse(error);
      });
    }
  }

  focusAddJobResponsibilityName(){
    setTimeout(()=>{
        let elementRef = document.getElementById('AddJobResponsibilityNameCtrl');
        if(elementRef) elementRef.focus();
    },300)
  }

  //Edit
  editJobResponsibility(jobResponsibility:IJobResponsibilitiesData, index: number){
    this.provisionJobResponsibility(index);
  }

  updateJobResponsibility(jobResponsibility, index) {
    this.formJobResponsibilitySubmitted = true;
    if(this.form.controls.jobResponsibilitiesForm.valid) {
      this.msg.setWarningMessage('Sending...');
      const responsibilityAlreadyAssigned = this.jobProfileClassifications.jobResponsibilities.some(obj => obj.jobResponsibilityId != jobResponsibility.jobResponsibilityId && obj.description.toLowerCase() === this.EditJobResponsibilityName.value.toLowerCase())  
      if (responsibilityAlreadyAssigned) {
        this.EditJobResponsibilityName.setErrors({'incorrect': true});
        this.msg.setErrorMessage('Job Responsibility: '+ this.EditJobResponsibilityName.value +' is already assigned.', 2000);
        return false;
      }

      const responsibilityAlreadyExists = this.jobResponsibilities.some(obj => obj.jobResponsibilityId != jobResponsibility.jobResponsibilityId && obj.description.toLowerCase() === this.EditJobResponsibilityName.value.toLowerCase())  
      if (responsibilityAlreadyExists) {
        this.EditJobResponsibilityName.setErrors({'incorrect': true});
        this.msg.setErrorMessage('Job Responsibility: '+ this.EditJobResponsibilityName.value +' already exists. You can remove this one and that one.', 2000);
        return false;
      }

      this.jobProfileService.updateJobResponsibility({
        jobProfileId: this.jobProfile.jobProfileId,
        clientId: this.userInfo.lastClientId || this.userInfo.clientId,
        jobResponsibilityId: jobResponsibility.jobResponsibilityId,
        description: this.EditJobResponsibilityName.value
      })
      .subscribe((result) => {
        this.jobProfileClassifications.jobResponsibilities = result.data;
        this.resetJobResponsibility();
        this.msg.setSuccessMessage("Job Responsibility updated successfully.");
      }, (error: HttpErrorResponse) => {
          this.msg.setErrorResponse(error);
      });
    }
  }

  focusEditJobResponsibilityName(){
    setTimeout(()=>{
        let elementRef = document.getElementById('EditJobResponsibilityNameCtrl');
        if(elementRef) elementRef.focus();
    },300)
  }
  //#endregion

  //#region "Manage Job Skills"
  provisionJobSkill(index: number) { 
    this.resetJobSkill();

    if (index == -1) { //Add
      this.isAddingJobSkill = true;
      this.AddJobSkillName.setValidators(Validators.required);
      this.focusAddJobSkillName();
    }
    else { //Edit
      this.EditJobSkillName.setValidators(Validators.required);

      this.jobProfileClassifications.jobSkills[index].isSkillEditing = true;
      this.EditJobSkillName.setValue( this.jobProfileClassifications.jobSkills[index].description );
      this.focusEditJobSkillName();
    }
  }

  resetJobSkill(){
    this.jobProfileClassifications.jobSkills.forEach(x=>x.isSkillEditing = false);
    this.form.controls.jobSkillsForm.reset();
    this.formJobSkillSubmitted = false;
    this.isAddingJobSkill = false;
    this.EditJobSkillName.clearValidators();
    this.AddJobSkillName.clearValidators();
  }

  removeJobSkill(jobSkill:IJobSkillsData, index: number){
    this.resetJobSkill();
    const options = {
        title: 'Are you sure you want to remove this skill?',
        confirm: "Remove",
        width: "300px",
    };
    this.confirmDialog.open(options);
    this.confirmDialog.confirmed().pipe(
    filter(ok => !!ok),
    tap(x => {
      this.msg.setWarningMessage('Sending...');
      this.jobProfileService.deleteJobProfileSkill({
        jobProfileId: this.jobProfile.jobProfileId,
        jobSkillId: jobSkill.jobSkillId
      })
      .subscribe((result) => {
        if (result.success) {
          this.resetJobSkill();
          this.jobProfileClassifications.jobSkills = result.data;
          this.msg.setSuccessMessage("Job profile Skill removed successfully.");
        }
      }, (error: HttpErrorResponse) => {
          this.msg.setErrorResponse(error);
      });
    })).subscribe();
  }

  //Add
  addJobSkill() {
    this.provisionJobSkill(-1);
  }

  addJobSkillDetails() {
    this.formJobSkillSubmitted = true;
    if (this.form.controls.jobSkillsForm.valid) {
      this.msg.setWarningMessage('Sending...');
      let skillId = null;
      let skillName = '';

      if (typeof this.AddJobSkillName.value === 'object') {
        skillId = this.AddJobSkillName.value.jobSkillId;
        skillName = this.AddJobSkillName.value.description;
      }
      else 
        skillName = this.AddJobSkillName.value;

      const skillAlreadyExists = this.jobProfileClassifications.jobSkills.some(obj => obj.description.toLowerCase() === skillName.toLowerCase())  
      if (skillAlreadyExists) {
        this.AddJobSkillName.setErrors({'incorrect': true});
        this.msg.setErrorMessage('Job Skill: '+ skillName +' already exists.', 2000);
        return false;
      }

      this.jobProfileService.updateJobProfileSkill({
        jobSkillId: skillId,
        description: skillName,
        clientId: this.userInfo.lastClientId || this.userInfo.clientId,
        jobProfileId: this.jobProfile.jobProfileId
      })
      .subscribe((result) => {
        this.jobProfileClassifications.jobSkills = result.data;
        this.resetJobSkill();
        this.msg.setSuccessMessage("Job Skill added successfully.");
      }, (error: HttpErrorResponse) => {
          this.msg.setErrorResponse(error);
      });
    }
  }

  focusAddJobSkillName(){
    setTimeout(()=>{
        let elementRef = document.getElementById('AddJobSkillNameCtrl');
        if(elementRef) elementRef.focus();
    },300)
  }

  //Edit
  editJobSkill(jobSkill:IJobSkillsData, index: number){
    this.provisionJobSkill(index);
  }

  updateJobSkill(jobSkill, index) {
    this.formJobSkillSubmitted = true;
    if(this.form.controls.jobSkillsForm.valid) {
      this.msg.setWarningMessage('Sending...');
      const skillAlreadyAssigned = this.jobProfileClassifications.jobSkills.some(obj => obj.jobSkillId != jobSkill.jobSkillId && obj.description.toLowerCase() === this.EditJobSkillName.value.toLowerCase())  
      if (skillAlreadyAssigned) {
        this.EditJobSkillName.setErrors({'incorrect': true});
        this.msg.setErrorMessage('Job Skill: '+ this.EditJobSkillName.value +' is already assigned.', 2000);
        return false;
      }

      const skillAlreadyExists = this.jobSkills.some(obj => obj.jobSkillId != jobSkill.jobSkillId && obj.description.toLowerCase() === this.EditJobSkillName.value.toLowerCase())  
      if (skillAlreadyExists) {
        this.EditJobSkillName.setErrors({'incorrect': true});
        this.msg.setErrorMessage('Job Skill: '+ this.EditJobSkillName.value +' already exists. You can remove this one and that one.', 2000);
        return false;
      }

      this.jobProfileService.updateJobSkill({
        jobProfileId: this.jobProfile.jobProfileId,
        clientId: this.userInfo.lastClientId || this.userInfo.clientId,
        jobSkillId: jobSkill.jobSkillId,
        description: this.EditJobSkillName.value
      })
      .subscribe((result) => {
        this.jobProfileClassifications.jobSkills = result.data;
        this.resetJobSkill();
        this.msg.setSuccessMessage("Job Skill updated successfully.");
      }, (error: HttpErrorResponse) => {
          this.msg.setErrorResponse(error);
      });
    }
  }

  focusEditJobSkillName(){
    setTimeout(()=>{
        let elementRef = document.getElementById('EditJobSkillNameCtrl');
        if(elementRef) elementRef.focus();
    },300)
  }
  //#endregion

  loadDepartments() {
    if (this.ClientDivision.value) {
      this.jobProfileService.getClientDepartmentsList(this.ClientDivision.value)
      .subscribe((clientDepartments) => {
        if (clientDepartments) {
          this.clientDepartmentsList = clientDepartments;
          //this.ClientDepartment.enable();
        }
        else {
          this.clientDepartmentsList = null;
          //this.ClientDepartment.disable();
        }
      }, (error: HttpErrorResponse) => {
          this.msg.setErrorResponse(error);
      });
    }
    else {
      this.clientDepartmentsList = null;
      //this.ClientDepartment.disable();
    }
  }

  //#region "Competencies Related"
	updateSelectedModel(model: ICompetencyModelBasic) {
    if (model) {
        this.jobProfile.competencyModelId = model.competencyModelId;
    } else {
        this.jobProfile.competencyModelId = null;
    }

    if (this.jobProfile.competencyModelId && this.jobProfile.competencyModelId > 0) {
      this.competenciesWidgetColor = 'info';
    }
    else {
      this.competenciesWidgetColor = 'disabled';
    }
  }
  //#endregion

  //#region "Time Off Accrual Related"
  accrualSelected(event: MatAutocompleteSelectedEvent): void {
    this.selectedJobProfileAccruals.push(event.option.value);

    const index = this.allJobProfileAccruals.findIndex(item => item.clientAccrualId == event.option.value.clientAccrualId);
    this.allJobProfileAccruals.splice(index, 1);

    setTimeout(()=>{
      let elementRef = document.getElementById('jpAccrualInput') as HTMLInputElement;
      if(elementRef)  {
        elementRef.value = '';
        this.JobProfileAccrual.setValue(null);
        (document.getElementById('amountInput') as HTMLInputElement).focus();
        elementRef.focus();
      }
    },300);
  }

  removeAccrual(accrual: IJobProfileAccrualsData): void {
    const index = this.selectedJobProfileAccruals.findIndex(item => item.clientAccrualId == accrual.clientAccrualId);
    if (index >= 0)
      this.selectedJobProfileAccruals.splice(index, 1);

    const index2 = this.allJobProfileAccruals.findIndex(item => item.clientAccrualId == accrual.clientAccrualId);
    if (index2 < 0)
      this.allJobProfileAccruals.push(accrual);

      (document.getElementById('jpAccrualInput') as HTMLInputElement).value = '';
      this.JobProfileAccrual.setValue(null);
  }

  private _filter(value: string): IJobProfileAccrualsData[] {
    const filterValue = value.toLowerCase();
    return this.allJobProfileAccruals.filter(accrual => accrual.description.toLowerCase().includes(filterValue));
  }
  //#endregion

  updateJobProfileStatus(jobProfile) {
    let data = {
        jobProfileId: jobProfile.jobProfileId,
        isActive: !jobProfile.isActive
    };
    this.msg.setWarningMessage('Sending...');
    this.jobProfileService.updateJobProfileStatus(data)
    .subscribe((result) => {
      this.msg.setSuccessMessage("Job profile status updated successfully.");
      jobProfile.isActive = !jobProfile.isActive;
    }, (error: HttpErrorResponse) => {
        this.msg.setErrorResponse(error);
    });    
  }

  doSave() {
    this.formSubmitted = true;

    if (this.hasCompetencyAccess) {
        length = this.jobProfileOnboardingWorkflows.filter(function(item){
          return item.onboardingWorkflowTaskId != 1 && item.onboardingWorkflowTaskId != 2 && item.onboardingWorkflowTaskId != 8;
        }).length;
    
        if (length == 0) {
          for (var i = this.jobProfileOnboardingWorkflows.length - 1; i >= 0; --i) {
            if (this.jobProfileOnboardingWorkflows[i].onboardingWorkflowTaskId == 1 || this.jobProfileOnboardingWorkflows[i].onboardingWorkflowTaskId == 2 || this.jobProfileOnboardingWorkflows[i].onboardingWorkflowTaskId == 8) {
              this.jobProfileOnboardingWorkflows.splice(i,1);
            }
          }      
        }
        else {
          if (!this.jobProfileOnboardingWorkflows.some(t => t.onboardingWorkflowTaskId == 8))
              this.jobProfileOnboardingWorkflows.unshift({formTypeId: null, jobProfileId: this.jpId, onboardingWorkflowTaskId: 8});

          if (!this.jobProfileOnboardingWorkflows.some(t => t.onboardingWorkflowTaskId == 2))
              this.jobProfileOnboardingWorkflows.unshift({formTypeId: null, jobProfileId: this.jpId, onboardingWorkflowTaskId: 2});

          if (!this.jobProfileOnboardingWorkflows.some(t => t.onboardingWorkflowTaskId == 1))
              this.jobProfileOnboardingWorkflows.unshift({formTypeId: null, jobProfileId: this.jpId, onboardingWorkflowTaskId: 1});
        }
    }  
  
    this.msg.setWarningMessage("Sending");
    this.prepareModel();


    let data = this.jobProfile;
    data.classifications = angular.copy(this.jobProfileClassifications);
    data.compensation = angular.copy(this.jobProfileCompensation);
    data.jobProfileAccruals = angular.copy(this.selectedJobProfileAccruals);
    data.jobProfileOnboardingWorkflows = this.jobProfileOnboardingWorkflows;

    this.jobProfileService.saveJobProfile(data)
    .subscribe((result) => {
      this.msg.setSuccessMessage("Job Profile saved successfully.");
    }, (error: HttpErrorResponse) => {
        this.msg.setErrorResponse(error);
    });    
  }

  //#region "Job profile Title Related"
  editJobProfileTitle(){
    let config = new MatDialogConfig<any>();
    config.width = "500px";
    config.data = {title: this.jobProfile.description};

    return this.dialog.open<JobProfileTitleDialogComponent, any, string>(JobProfileTitleDialogComponent, config)
      .afterClosed()
      .subscribe((jobProfileTitle: any) => {
        this.msg.setWarningMessage('Sending...');
        if (jobProfileTitle) {
          let data = {
            jobProfileId: this.jobProfile.jobProfileId,
            description: jobProfileTitle,
          };

          this.jobProfileService.updateJobProfileTitle(data)
          .subscribe((result) => {
            this.jobProfile.description = jobProfileTitle;          
            this.msg.setSuccessMessage("Job profile Title updated successfully.");
          }, (error: HttpErrorResponse) => {
              this.msg.setErrorResponse(error);
          });
        }
      });
  }
  //#endregion

  updateSelections(tasks: any[]) {
    this.jobProfileOnboardingWorkflows = tasks;
  }

  updateSelectedState(selectedState: any) {
    const selectedItem = this.jobProfileOnboardingWorkflows.find( x => x.onboardingWorkflowTaskId == 7);
    if (selectedItem)
      selectedItem.formTypeId = selectedState.formTypeId;
  }

  private prepareModel() {
    this.jobProfile.requirements = this.Requirements.value;
    this.jobProfile.benefits = this.Benefits.value;
    this.jobProfile.workingConditions = this.WorkingConditions.value;
    this.jobProfile.onboardingAdminTaskListId = this.AdminTaskList.value;

    this.jobProfileClassifications.clientDivisionId = this.ClientDivision.value;
    this.jobProfileClassifications.clientDepartmentId = this.ClientDepartment.value;
    this.jobProfileClassifications.directSupervisorId = this.DirectSupervisor.value;
    this.jobProfileClassifications.clientGroupId = this.ClientGroup.value;
    this.jobProfileClassifications.jobClass = this.JobClass.value;
    this.jobProfileClassifications.eeocLocationId = this.EEOCLocation.value;
    this.jobProfileClassifications.eeocJobCategoryId = this.EEOCJobCategory.value;
    this.jobProfileClassifications.clientWorkersCompId = this.WorkersComp.value;
    this.jobProfileClassifications.clientShiftId = this.ClientShift.value;
    this.jobProfileClassifications.employeeStatusId = this.EmploymentStatus.value;
    this.jobProfileClassifications.clientCostCenterId = this.CostCenter.value;

    this.jobProfileCompensation.isBenefitsEligibility = this.BenefitsEligible.value;
    this.jobProfileCompensation.employeeTypeID = this.PayrollType.value;
    this.jobProfileCompensation.isExempt = this.IsOvertimeExempt.value;
    this.jobProfileCompensation.benefitPackageId = this.BenefitPackage.value;
    this.jobProfileCompensation.salaryMethodTypeId = this.SalaryMethodType.value;
    this.jobProfileCompensation.isTipped = this.IsTippedEmployee.value;
    this.jobProfileCompensation.payFrequencyID = this.PayFrequency.value;
    this.jobProfileCompensation.hours = this.Hours.value;

    this.jobProfileAccruals = this.selectedJobProfileAccruals;
  }

  redirectToJobProfiles() {
    this.router.navigate(['admin/job-profiles']);  
  }
  
  addCustomPage(route: string) {
    this.currentPageType = Utilities.getPageTypeByRoute(route);

    this.currentTask = {
      onboardingWorkflowTaskId: 0,
      route: route,
      route1: route,
      linkToState: Utilities.getLinkToStateByRoute(route),
      modifiedBy: 0,
      modified: new Date(),
      clientId: this.userInfo.lastClientId || this.userInfo.clientId,
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

    this.router.navigate(['admin/onboarding/manage-resources', this.currentTask.onboardingWorkflowTaskId, this.currentPageType, 'add', 'client', this.cId, 'job-profile', this.jpId]);  
  }

  addPost() {
    window.location.href = this.jobProfile.sourceURL +
        '/ApplicantCompanyPosting.aspx?Add=True&Submenu=Applicant%20Tracking&JobProfileId=' + this.jobProfile.jobProfileId;
  }

  selectOption(e: Event, trigger: MatAutocompleteTrigger) {
    e.stopPropagation();
    trigger.openPanel();
  }
}