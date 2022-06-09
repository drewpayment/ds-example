import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef, ViewChild, ViewChildren, QueryList, AfterViewInit, AfterViewChecked } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { forkJoin } from 'rxjs';
import { ClientDivisionDto } from '@ajs/ds-external-api/models';
import { NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling'
import { AccountService } from '@ds/core/account.service';
import { GeneralLedgerService } from '../../../services/general-ledger.service';
import { ClientGLSettings, ClientCostCenter, ClientDepartment, ClientDeduction, ClientGroupDto, AssignmentFilterOptions, ClientGLAssignmentCustom, ClientGLClassGroup, ClientGLCustomClass, GeneralLedgerAccount } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-client-gl-account-assignment',
  templateUrl: './client-gl-account-assignment.component.html',
  styleUrls: ['./client-gl-account-assignment.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClientGlAccountAssignmentComponent implements OnInit {

  editGL: boolean = false;
  user: UserInfo;
  messVisible: boolean;
  emptyState: boolean;
  pnlEditGl: boolean;
  glSettings: ClientGLSettings;
  costCenters: ClientCostCenter[];
  departments: ClientDepartment[];
  divisions: ClientDivisionDto[];
  deductions: ClientDeduction[];
  groups: ClientGroupDto[];
  filterOptions: AssignmentFilterOptions;
  clientGLAssignment: ClientGLAssignmentCustom;
  glAccounts: GeneralLedgerAccount[];
  glClassGroups: ClientGLClassGroup[];
  customClasses: ClientGLCustomClass[];
  isLoading = true;
  isSaving = false;
  showBody = false;
  showLoadingBody = false;
  selectedTab = 'cash';
  inputString: string = "";
  isTimeOutStarted = false;

  constructor(
    private api: GeneralLedgerService,
    private accountService: AccountService,
    private msg: NgxMessageService,
    private cd: ChangeDetectorRef) { }

  @ViewChild('formControl', {static: false}) formControl;
  @ViewChildren('cashSelect') cashSelectList : QueryList<CdkVirtualScrollViewport>;
  @ViewChildren('expenseSelect') expenseSelectList : QueryList<CdkVirtualScrollViewport>;
  @ViewChildren('liabilitySelect') liabilitySelectList : QueryList<CdkVirtualScrollViewport>;
  @ViewChildren('paymentSelect') paymentSelectList : QueryList<CdkVirtualScrollViewport>;
  ngOnInit() {

    this.accountService.getUserInfo().subscribe((u: UserInfo) => {
      this.user = u;
      forkJoin(
        this.api.getAssignmentFilterOptions(),
        this.api.getClientCostCenters(this.user.clientId),
        this.api.getClientDeductions(),
        this.api.getClientDepartments(this.user.clientId),
        this.api.getClientGLSettings(this.user.clientId),
        this.api.getClientGroups(this.user.clientId),
        this.api.getClientDivisions(this.user.clientId),
        this.api.getClientGeneralLedgerAccounts(),
        this.api.getClientGeneralLedgerClassGroups(),
        this.api.getClientGLCustomClass()
      ).subscribe(([filterOptions, costCenters, deductions, departments, glSettings, groups, divisions, accounts, classGroups, classes]) => {
        this.filterOptions = filterOptions,
        this.costCenters   = costCenters;
        this.deductions    = deductions;
        this.departments   = departments;
        this.glSettings    = glSettings;
        this.groups        = groups;
        this.divisions     = divisions;
        this.glAccounts    = accounts;
        this.glClassGroups = classGroups;
        this.customClasses = classes;

        this.isLoading = false;
        this.cd.detectChanges();
      });
    });
  } // end of on init

  filterChange(assignmentId: number) {
    if (assignmentId == 2) {
      this.filterOptions.department = null;
      this.filterOptions.customClass = null;
    } else if (assignmentId == 3) {
      this.filterOptions.costCenter = null;
      this.filterOptions.customClass = null;
    } else if (assignmentId == 4) {
      this.filterOptions.costCenter = null;
      this.filterOptions.department = null;
    }
  }

  changeType() {
    let foreignKeyId = 0;
    let assignmentId = -1;

    // changed after this used to provided an assignmentId
    // now a filter button is clicked

    if (this.filterOptions.costCenter != null && this.filterOptions.costCenter.toString() != "")
      assignmentId = 2;
    if (this.filterOptions.department != null && this.filterOptions.department.toString() != "")
      assignmentId = 3;
    if (this.filterOptions.customClass != null && this.filterOptions.customClass.toString() != "")
      assignmentId = 4;

    if (assignmentId == -1) {
      if (this.showBody) this.showBody = false;
      return;
    }

    this.showLoadingBody = true;
    this.showBody        = true;

    if (assignmentId == 2) {
      foreignKeyId = this.filterOptions.costCenter;
      if (this.filterOptions.costCenter == 0) {
        assignmentId = 1;
      }
    }

    if (assignmentId == 3) {
      foreignKeyId = this.filterOptions.department;
    }

    if (assignmentId == 4) {
      foreignKeyId = this.filterOptions.customClass;
    }

    this.api.getClientGLAssignmentItems(assignmentId, foreignKeyId).subscribe((data: ClientGLAssignmentCustom) => {
      this.clientGLAssignment = data;

      this.clientGLAssignment.cashItemCount      = 0;
      this.clientGLAssignment.liabilityItemCount = 0;
      this.clientGLAssignment.expenseItemCount   = 0;
      this.clientGLAssignment.paymentItemCount   = 0;

      //sorting
      this.clientGLAssignment.cashAssignmentHeaders.sort((x,y) => (x.sequenceId > y.sequenceId) ? 1 : -1);
      _.forEach(this.clientGLAssignment.cashAssignmentHeaders, (head) => {
        this.clientGLAssignment.cashItemCount = head.clientGLAssignments.length + this.clientGLAssignment.cashItemCount;
        head.clientGLAssignments.sort((x,y) => {
          if (x.sequenceId > y.sequenceId)
            return 1;
          if (x.sequenceId < y.sequenceId)
            return -1;
          if (x.description > y.description)
            return 1;
          if (x.description < y.description)
            return -1;
        });
      });
      this.clientGLAssignment.expenseAssignmentHeaders.sort((x,y) => (x.sequenceId > y.sequenceId) ? 1 : -1);
      _.forEach(this.clientGLAssignment.expenseAssignmentHeaders, (head) => {
        this.clientGLAssignment.expenseItemCount = head.clientGLAssignments.length + this.clientGLAssignment.expenseItemCount;
        head.clientGLAssignments.sort((x,y) => {
          if (x.sequenceId > y.sequenceId)
            return 1;
          if (x.sequenceId < y.sequenceId)
            return -1;
          if (x.description > y.description)
            return 1;
          if (x.description < y.description)
            return -1;
        });
      });
      this.clientGLAssignment.liabilityAssignmentHeaders.sort((x,y) => (x.sequenceId > y.sequenceId) ? 1 : -1);
      _.forEach(this.clientGLAssignment.liabilityAssignmentHeaders, (head) => {
        this.clientGLAssignment.liabilityItemCount = head.clientGLAssignments.length + this.clientGLAssignment.liabilityItemCount;
        head.clientGLAssignments.sort((x,y) => {
          if (x.sequenceId > y.sequenceId)
            return 1;
          if (x.sequenceId < y.sequenceId)
            return -1;
          if (x.description > y.description)
            return 1;
          if (x.description < y.description)
            return -1;
        });
      });
      this.clientGLAssignment.paymentAssignmentHeaders.sort((x,y) => (x.sequenceId > y.sequenceId) ? 1 : -1);
      _.forEach(this.clientGLAssignment.paymentAssignmentHeaders, (head) => {
        this.clientGLAssignment.paymentItemCount = head.clientGLAssignments.length + this.clientGLAssignment.paymentItemCount;
        head.clientGLAssignments.sort((x,y) => {
          if (x.sequenceId > y.sequenceId)
            return 1;
          if (x.sequenceId < y.sequenceId)
            return -1;
          if (x.description > y.description)
            return 1;
          if (x.description < y.description)
            return -1;
        });
      });
      //sorting end
      this.setSaveGroupId();
      this.showLoadingBody = false;

      this.cd.detectChanges();
    });
  }

  save(f: NgForm) {
    if (f.valid) {
      this.isSaving = true;
      if (this.filterOptions.controlExists) this.clientGLAssignment.saveGroupId = 0;
      this.api.saveClientGLAssignments(this.clientGLAssignment).subscribe((data) => {
        this.msg.setSuccessMessage("Company General Ledger Assignment saved successfully.");
        // window.scrollTo(0,0);
        this.isSaving = false;
        // this.showBody = false;
        // this.filterOptions.department  = null;
        // this.filterOptions.costCenter  = null;
        // this.filterOptions.customClass = null;
        this.changeType();
        f.form.markAsUntouched();
        this.cd.detectChanges();
      }, (error : HttpErrorResponse) => {
          this.msg.setErrorMessage(error.message);
          this.isSaving = false;
      });
    }
  }

  autoSaveMethod(f: NgForm, tab: string) {
    if (f.valid) {
      if (f.dirty && f.touched) {
        this.isSaving = true;
        if (this.filterOptions.controlExists) this.clientGLAssignment.saveGroupId = 0;
        this.showLoadingBody = true;
        this.api.saveClientGLAssignments(this.clientGLAssignment).subscribe((data) => {
          this.msg.setSuccessMessage("Company General Ledger Assignment "
              + this.selectedTab + " header saved successfully.");
          this.isSaving = false;
          this.selectedTab = tab;
          this.setSaveGroupId();
          this.changeType();
          f.form.markAsUntouched();
          this.showLoadingBody = false;
          this.cd.detectChanges();
        }, (error : HttpErrorResponse) => {
            this.msg.setErrorMessage(error.message);
            this.isSaving = false;
            this.showLoadingBody = false;
        });
      } else {
        this.selectedTab = tab;
        this.setSaveGroupId();
      }

    }
  }

  getActiveTab(tab: string) {
    if (tab != this.selectedTab) {
      if (!this.isSaving) {
        if (this.formControl.valid) {
          this.autoSaveMethod(this.formControl, tab);
        }
      }
    }
  }

  setSaveGroupId() {
    if (this.selectedTab == 'cash') {
      this.clientGLAssignment.saveGroupId = 1;
    } else if (this.selectedTab == 'expense') {
      this.clientGLAssignment.saveGroupId = 3;
    } else if (this.selectedTab == 'liability') {
      this.clientGLAssignment.saveGroupId = 2;
    } else if (this.selectedTab == 'payment') {
      this.clientGLAssignment.saveGroupId = 4;
    }
    this.cd.detectChanges();
  }

  openChange($event: boolean, id: number, headIdx : number, subIdx) {
    if ($event) {
      let itemIdx = 0;
      let i       = 0;
      const index = this.glAccounts.findIndex((element : GeneralLedgerAccount) => { return element.accountId == id});
      if (this.selectedTab == 'cash') {
        for(i = 0; i < headIdx; i++)
          itemIdx = itemIdx + this.clientGLAssignment.cashAssignmentHeaders[i].clientGLAssignments.length;
        itemIdx = itemIdx + subIdx;
        if (this.cashSelectList && this.cashSelectList.length) this.cashSelectList.toArray()[itemIdx].scrollToIndex(index);
      } else if (this.selectedTab == 'expense') {
        for(i = 0; i < headIdx; i++)
          itemIdx = itemIdx + this.clientGLAssignment.expenseAssignmentHeaders[i].clientGLAssignments.length;
        itemIdx = itemIdx + subIdx;
        if (this.expenseSelectList && this.expenseSelectList.length) this.expenseSelectList.toArray()[itemIdx].scrollToIndex(index);
      } else if (this.selectedTab == 'liability') {
        for(i = 0; i < headIdx; i++)
          itemIdx = itemIdx + this.clientGLAssignment.liabilityAssignmentHeaders[i].clientGLAssignments.length;
        itemIdx = itemIdx + subIdx;
        if (this.liabilitySelectList && this.liabilitySelectList.length) this.liabilitySelectList.toArray()[itemIdx].scrollToIndex(index);
      } else if (this.selectedTab == 'payment') {
        for(i = 0; i < headIdx; i++)
          itemIdx = itemIdx + this.clientGLAssignment.paymentAssignmentHeaders[i].clientGLAssignments.length;
        itemIdx = itemIdx + subIdx;
        if (this.paymentSelectList && this.paymentSelectList.length) this.paymentSelectList.toArray()[itemIdx].scrollToIndex(index);
      } // end of selected tab
    } // open event
  } // end of open Change function

  typeToObject($event, headIdx : number, subIdx) {
    let itemIdx = 0;
    let i = 0;
    let input = String.fromCharCode($event.keyCode);
    if (/[a-zA-Z0-9-_ ]/.test(input)) {
      this.inputString = this.inputString + input;
      let index = this.glAccounts.findIndex((element : GeneralLedgerAccount) => {
        return element.description.toLowerCase().startsWith(this.inputString.toLowerCase());
      });
      if (index > -1) {
        index = index + 1;
        if (this.selectedTab == 'cash') {
          for(i = 0; i < headIdx; i++)
            itemIdx = itemIdx + this.clientGLAssignment.cashAssignmentHeaders[i].clientGLAssignments.length;
          itemIdx = itemIdx + subIdx;
          if (this.cashSelectList && this.cashSelectList.length) this.cashSelectList.toArray()[itemIdx].scrollToIndex(index);
        } else if (this.selectedTab == 'expense') {
          for(i = 0; i < headIdx; i++)
            itemIdx = itemIdx + this.clientGLAssignment.expenseAssignmentHeaders[i].clientGLAssignments.length;
          itemIdx = itemIdx + subIdx;
          if (this.expenseSelectList && this.expenseSelectList.length) this.expenseSelectList.toArray()[itemIdx].scrollToIndex(index);
        } else if (this.selectedTab == 'liability') {
          for(i = 0; i < headIdx; i++)
            itemIdx = itemIdx + this.clientGLAssignment.liabilityAssignmentHeaders[i].clientGLAssignments.length;
          itemIdx = itemIdx + subIdx;
          if (this.liabilitySelectList && this.liabilitySelectList.length) this.liabilitySelectList.toArray()[itemIdx].scrollToIndex(index);
        } else if (this.selectedTab == 'payment') {
          for(i = 0; i < headIdx; i++)
            itemIdx = itemIdx + this.clientGLAssignment.paymentAssignmentHeaders[i].clientGLAssignments.length;
          itemIdx = itemIdx + subIdx;
          if (this.paymentSelectList && this.paymentSelectList.length) this.paymentSelectList.toArray()[itemIdx].scrollToIndex(index);
        } // end of selected tab
      } // end of if index > -1
    } // end of is char or num

    if (!this.isTimeOutStarted) {
      this.isTimeOutStarted = true;
      setTimeout(() => { this.inputString = ""; this.isTimeOutStarted = false; }, 3000);
    }

  } // end of method

  getDescription(id: number) {
    const acc = this.glAccounts.find((element : GeneralLedgerAccount) => { return element.accountId == id});

    return acc != null ? acc.number + " (" + acc.description + ")"  : "";
  }

} // end of class
