import { Component, OnInit } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { MatDialogRef } from '@angular/material/dialog';
import { GeneralLedgerService } from '../../../services/general-ledger.service';
import { ClientGLSettings } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-client-gl-report-options-dialog',
  templateUrl: './client-gl-report-options-dialog.component.html',
  styleUrls: ['./client-gl-report-options-dialog.component.scss']
})
export class ClientGlReportOptionsDialogComponent implements OnInit {

  user: UserInfo;
  isSettingsLoading: boolean = true;
  noSettingsView: Boolean = false;
  glSettings: ClientGLSettings;
  formSubmitted: boolean = false;
  isSaving: boolean = false;
  form: FormGroup = this.createForm();

  constructor(
    private api: GeneralLedgerService,
    private accountAPI: AccountService,
    private msg: NgxMessageService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ClientGlReportOptionsDialogComponent>) { }

  ngOnInit() {
    this.accountAPI.getUserInfo().subscribe(user => {
      this.user = user;
      this.getGLSettings();
    });
  }

  createForm() {
    return this.fb.group({
      secondGroup    : [''],
      secondType     : [''],
      thirdGroup     : [''],
      thirdType      : [''],
      chkCustomClass : [false]
    });
  }

  get f() { return this.form.controls; }

  getGLSettings() {
    this.api.getClientGLSettings(this.user.clientId).subscribe((settings) => {
      if (settings == null) {
        this.glSettings = {
          clientId: this.user.clientId,
          group2: null,
          group3: null,
          group2Type: null,
          group3Type: null,
          groupClassesTogether: false
        };
      } else {
        this.glSettings = settings;
      }

      this.form.patchValue({
        secondGroup    : this.glSettings.group2,
        secondType     : this.glSettings.group2Type,
        thirdGroup     : this.glSettings.group3,
        thirdType      : this.glSettings.group3Type,
        chkCustomClass : this.glSettings.groupClassesTogether
      });

      this.isSettingsLoading = false;
    });
  }

  save() {
    this.formSubmitted = true;
    this.isSaving = true;
    if(this.form.valid) {
      this.prepareGLSettings();
      this.api.saveClientGLSettings(this.glSettings).subscribe((result : any) => {
        this.msg.setSuccessMessage("General Ledger Report Options saved successfully.");
        this.isSaving = false;
        this.dialogRef.close(null);
      }, (error : HttpErrorResponse) => {
        this.msg.setErrorMessage(error.message);
        this.isSaving = false;
      });
    }

  }

  prepareGLSettings() {
    this.glSettings.group2 = this.form.get('secondGroup').value;
    this.glSettings.group2Type  = this.form.get('secondType').value;
    this.glSettings.group3 = this.form.get('thirdGroup').value;
    this.glSettings.group3Type = this.form.get('thirdType').value;
    this.glSettings.groupClassesTogether = this.form.get('chkCustomClass').value;
  }

  cancel() {
    this.dialogRef.close(null);
  }

}
