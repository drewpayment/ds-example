import { Component, OnInit, ChangeDetectorRef, ViewChild } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import * as _ from "lodash"
import { NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AccountService } from '@ds/core/account.service';
import { GeneralLedgerService } from '../../../services/general-ledger.service';
import { ClientGLControl, ClientGLControlItem, ClientGLSettings } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-client-gl-control',
  templateUrl: './client-gl-control.component.html',
  styleUrls: ['./client-gl-control.component.scss']
})
export class ClientGlControlComponent implements OnInit {

  user: UserInfo;
  isLoading: Boolean = true;
  isOverallOptionsLoading: boolean = true;
  noSettingsView: Boolean = false;
  glControl: ClientGLControl;
  glSettings: ClientGLSettings;
  @ViewChild("formControl", {static: false}) form: NgForm;

  buttonGroup = [
    { name: 'Split', value: 0, isChecked: false },
    { name: 'Company Total', value: 1, isChecked: false },
  ];

  checkBoxes = [
    { name: 'Cost Center', value: 2, isChecked: false },
    { name: 'Department', value: 3, isChecked: false },
    { name: 'Custom Class', value: 4, isChecked: false }
  ];

  cashCount: number = 0;
  expenseCount: number = 0;
  liabilityCount: number = 0;
  paymentCount: number = 0;
  isSplit: Boolean = false;
  isSaving = false;

  selectedTab = 'cash';

  constructor(
    private api: GeneralLedgerService,
    private accountAPI: AccountService,
    private cd: ChangeDetectorRef,
    private msg: NgxMessageService) { }

  ngOnInit() {
    this.accountAPI.getUserInfo().subscribe(user => {
      this.user = user;
      //this.getGLSettings();
      this.getControlItems();
    });
  }

  getControlItems() {
    this.api.getClientGLControl().subscribe((control) => {
      _.forEach(control.clientGLControlItems, (item) => {
        if (item.assignmentMethodId > 1)
          item._isSplit = true;
      });
      this.glControl = control;
      this.setSaveGroupId();
      this.glControl.cashControlHeaders.sort((x, y) => (x.sequenceId > y.sequenceId) ? 1 : -1);
      _.forEach(this.glControl.cashControlHeaders, (head) => {
        head.clientGLControlItems.sort((x, y) => {
          x._isSplit = (x.assignmentMethodId > 1);
          y._isSplit = (y.assignmentMethodId > 1);
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
      this.glControl.expenseControlHeaders.sort((x, y) => (x.sequenceId > y.sequenceId) ? 1 : -1);
      _.forEach(this.glControl.expenseControlHeaders, (head) => {
        head.clientGLControlItems.sort((x, y) => {
          x._isSplit = (x.assignmentMethodId > 1);
          y._isSplit = (y.assignmentMethodId > 1);
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
      this.glControl.liabilityControlHeaders.sort((x, y) => (x.sequenceId > y.sequenceId) ? 1 : -1);
      _.forEach(this.glControl.liabilityControlHeaders, (head) => {
        head.clientGLControlItems.sort((x, y) => {
          x._isSplit = (x.assignmentMethodId > 1);
          y._isSplit = (y.assignmentMethodId > 1);
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
      this.glControl.paymentControlHeaders.sort((x, y) => (x.sequenceId > y.sequenceId) ? 1 : -1);
      _.forEach(this.glControl.paymentControlHeaders, (head) => {
        head.clientGLControlItems.sort((x, y) => {
          x._isSplit = (x.assignmentMethodId > 1);
          y._isSplit = (y.assignmentMethodId > 1);
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

      this.isOverallOptionsLoading = false;
      this.isLoading = false;

    });
  }

  setSplit(item: ClientGLControlItem) {
    if (item.assignmentMethodId > 1 && item._isSplit) {
      item.assignmentMethodId = 0;
      item._isSplit = false;
    } else if (item.assignmentMethodId == 1) {
      item._isSplit = true;
      item.assignmentMethodId = 0;
    } else {
      if (item._isSplit == null) {
        item._isSplit = true
      } else {
        item._isSplit = !item._isSplit;
      }
    }
  }

  setAssignment(item: ClientGLControlItem, assignmentId: number) {
    this.form.form.markAsDirty();
    this.form.form.markAsTouched();
    if (item.assignmentMethodId == 1 && assignmentId == 1) {
      item.assignmentMethodId = 0;
    } else if (assignmentId < 2) {
      item.assignmentMethodId = assignmentId;
      item._isSplit = false;
    } else {
      item.assignmentMethodId = assignmentId;
    }

  }

  save(form: NgForm) {
    if (form.valid) {
      this.isSaving = true;
      this.api.saveClientGLControl(this.glControl).subscribe((controlResult: any) => {
        this.msg.setSuccessMessage("General Ledger Control saved successfully.");
        window.scrollTo(0, 0);
        this.isSaving = false;
        form.resetForm();
        this.loadForm();
      }, (error: HttpErrorResponse) => {
        this.msg.setErrorMessage(error.message);
        this.isSaving = false;
      });
    }
  }

  autoSave(tab: string) {
    if (this.form.valid) {
      if (this.form.dirty && this.form.touched) {
        this.isSaving = true;
        this.isLoading = true;
        this.isOverallOptionsLoading = true;
        this.api.saveClientGLControl(this.glControl).subscribe((controlResult: any) => {
          this.msg.setSuccessMessage("General Ledger Control"
            + this.selectedTab + " header saved successfully.", 5000);
          this.isSaving = false;
          this.selectedTab = tab;
          this.form.resetForm();
          this.loadFormFromAutoSave();
          this.setSaveGroupId();
        }, (error: HttpErrorResponse) => {
          this.msg.setErrorMessage(error.message);
          this.isSaving = false;
        });
      } else {
        this.selectedTab = tab;
        this.setSaveGroupId();
      }
    }
  }

  getActiveTab(tab: string, event: Event) {
    if (tab != this.selectedTab)
      if (!this.isSaving) {
        if (this.form.invalid) this.form.onSubmit(event);
        this.autoSave(tab);
      }
  }

  setSaveGroupId() {
    if (this.selectedTab == 'cash') {
      this.glControl.saveGroupId = 1;
    } else if (this.selectedTab == 'expense') {
      this.glControl.saveGroupId = 3;
    } else if (this.selectedTab == 'liability') {
      this.glControl.saveGroupId = 2;
    } else if (this.selectedTab == 'payment') {
      this.glControl.saveGroupId = 4;
    }
  }

  loadForm() {
    this.isLoading = true;
    this.isOverallOptionsLoading = true;
    this.getControlItems();
  }

  loadFormFromAutoSave() {
    this.getControlItems();
  }

}
