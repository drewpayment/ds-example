import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { tap, switchMap, filter, startWith, map, debounceTime } from 'rxjs/operators';
import { JobProfileApiService } from 'apps/ds-company/src/app/services/job-profile-api.service';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

export enum TaskStatus {
  NotStarted = 0,
  Started = 1,
  Completed = 2
}

@Component({
  selector: 'ds-employee-tasks',
  templateUrl: './employee-tasks.component.html',
  styleUrls: ['./employee-tasks.component.css']
})
export class EmployeeTasksComponent implements OnInit {
  @Input() jobProfileId?: number;
  @Input() clientId?: number;
  @Input()  selectedOnboardingWorkflows: any[];
  @Output() onSelectionChanged = new EventEmitter<any[]>();
  @Output() onSelectedStateChanged = new EventEmitter<any>();

  isLoading: boolean = true;
  userInfo: UserInfo;
  form: FormGroup;
  formSubmitted: boolean = false;

  employeeTasks: any[] = [];

  disablePayPref = false;
  jobProfileClassifications: any;
  items: any;
  selectedState: any;
  stateList: any;

  allowCreatingCustomPages: boolean = false;
  isRequriedW4Assist: boolean = false;
  w4AssistIndex: number = 0;

  get StateW4State() { return this.form.controls.StateW4State as FormControl; }
  get HdnChangeTrackHack() { return this.form.controls.HdnChangeTrackHack as FormControl; }
  
  constructor(private accountService: AccountService,
    private jobProfileService: JobProfileApiService, 
    private msg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: ConfirmDialogService,
    private fb: FormBuilder,
    private router: Router,
    private storeService: DsStorageService) { }

  ngOnInit(): void {
    this.isLoading = true;

    this.buildForm();

    this.accountService.getUserInfo().pipe(
      tap(userInfo => {
        this.userInfo = userInfo;
      }), 
      switchMap(userInfo => this.jobProfileService.getW4StateList(this.clientId)), //userInfo.lastClientId || userInfo.clientId)),
        tap(stateList => {
          this.stateList = stateList;
          this.selectedState = stateList[0];
      }),
      switchMap(stateList => this.jobProfileService.getemployeeTasks(this.clientId)), //userInfo.lastClientId || userInfo.clientId)),
        tap(clientWorkflow => {
          for (let i = 0; i < clientWorkflow.length; i++) {
            let menuItem = clientWorkflow[i];
            this.createMenuItem(menuItem);
          }

          this.items = this.employeeTasks;

          for (let i = 0; i < this.employeeTasks.length; i++) {
            // Already chosen items
            const selectedItem = this.selectedOnboardingWorkflows.find( x => x.onboardingWorkflowTaskId == this.items[i].onboardingWorkflowTaskId);
            if ( selectedItem ) {
                this.items[i].isChecked = true;
                //this.items[i].status = selectedItem.status;
                this.items[i].status = selectedItem.isCompleted === false ? TaskStatus.Started : ( selectedItem.isCompleted === true ? TaskStatus.Completed : TaskStatus.NotStarted);

                if (selectedItem.onboardingWorkflowTaskId == 7) {
                    if (!!selectedItem.formTypeId)
                        this.selectedState = this.stateList.find(x => x.formTypeId == selectedItem.formTypeId);

                    this.onSelectedStateChanged.emit(this.selectedState);
                }
            }

            if (this.items[i].onboardingWorkflowTaskId === 4) {
              this.w4AssistIndex = i;
            }

            if (this.items[i].onboardingWorkflowTaskId === 7) {
                this.items[i].stateId = this.selectedState.stateId;
                this.items[i].formTypeId = this.selectedState.formTypeId;
                this.items[i].noDefinitionRequired = this.selectedState.noDefinitionRequired;
            }
          }

          this.updateForm();
          this.isLoading = false;
        })
    ).subscribe();
  }

  buildForm() {
    this.form = this.fb.group({
      StateW4State: this.fb.control(this.selectedState || ''),
      HdnChangeTrackHack: this.fb.control(''),
    });
  }

  updateForm() {
    this.form.patchValue ({
      StateW4State: this.selectedState || '',
      HdnChangeTrackHack: '',
    });
  }

  selectedStateChanged(item, s) {
    this.selectedState = this.StateW4State.value;
    this.onSelectedStateChanged.emit(this.selectedState);
  }

  selectionChanged(task: any, event: Event) {
    this.HdnChangeTrackHack.setValue('');
    this.HdnChangeTrackHack.markAsDirty();
    this.onSelectionChanged.emit(this.items.filter(x=>x.isChecked)); 
  }

  createMenuItem(item) {
    let menuItem = {
        onboardingWorkflowTaskId: item.onboardingWorkflowTaskId,
        title: item.workflowTitle,
        linkToState: item.linkToState,
        mainTaskId: item.mainTaskId,
        formTypeId: item.formTypeId,
        isHeader: item.isHeader,
        isChecked: item.isRequired, // changed DB values to false if not required. This will check items on load if they are required
        status: 0,
        isRequired: item.isRequired,
        description: item.description,
        adminMustSelect: item.adminMustSelect,
        adminDescription: item.adminDescription,
        imgClass: this.getImgClass(item.linkToState),
        clientId: item.clientId,
        isDeleted: item.isDeleted,
        subMenus: []
    };

    this.employeeTasks.push(menuItem);
  }

  getImgClass(state) {
    let css = '';
    switch (state) {
      case 'ess.onboarding.i9':
          css = 'verified'; //i9
          break;
      case 'ess.onboarding.w4Federal':
          css = 'account_balance'; //fedw4
          break;
      case 'ess.onboarding.w4State':
          css = 'statew4';
          break;
      case 'ess.onboarding.emergency-contact':
          css = 'perm_contact_calendar'; //emer-contact
          break;
      case 'ess.onboarding.electronic-consents':
          css = 'file_copy'; //delivery
          break;
      case 'ess.onboarding.dependents':
          css = 'people'; //dependent
          break;
      case 'ess.onboarding.payment-preference':
          css = 'monetization_on'; //pay-pref
          break;
      case 'ess.onboarding.document':
          css = 'doc';
          break;
      case 'ess.onboarding.video':
          css = 'video';
          break;
      case 'ess.onboarding.link':
          css = 'link';
          break;
      case 'ess.onboarding.employee-bio':
          css = 'account_circle'; //employee-bio
          break;
      case 'ess.onboarding.other-info':
          css = 'domain';
          break;
      default:
          break;
    }
    return css;
  }
}