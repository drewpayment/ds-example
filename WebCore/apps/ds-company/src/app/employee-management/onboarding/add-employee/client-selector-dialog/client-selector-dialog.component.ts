import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn, ValidatorFn, ValidationErrors } from '@angular/forms';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { tap } from 'rxjs/operators';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import {DashboardService} from "apps/ds-company/src/app/services/dashboard.service";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: 'ds-client-selector-dialog',
  templateUrl: './client-selector-dialog.component.html',
  styleUrls: ['./client-selector-dialog.component.scss']
})
export class ClientSelectorDialogComponent implements OnInit {
  form: FormGroup;
  formSubmitted: boolean;
  isLoading: boolean = true;
  userAccount: UserInfo;
  clientId?: number;
  selectedClientId: number;
  clientList: any[];
  allData: any;

  constructor(
    private ref:MatDialogRef<ClientSelectorDialogComponent>,
    private dashboardApiService: DashboardService,
    private msg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: MatDialog,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data:any,
  ) { 
    this.allData = data;
    this.clientList = data.clientList;
    this.clientId = data.clientId;
  }

  ngOnInit(): void {
    this.buildForm();
    this.selectedClientId = this.form.get('clientSelector').value;
    this.isLoading = false;
  }

  buildForm() {
    this.form = this.fb.group({
      clientSelector:  [this.clientId, Validators.compose([Validators.required])],
    });

    this.form.get('clientSelector').valueChanges.subscribe(val => {
      this.selectedClientId = this.form.get('clientSelector').value;
    });
  }

  save() {
    this.formSubmitted = true;
    if (!this.form.valid) {
      return;
    }

    this.ref.close(this.selectedClientId);
  }

  clear() {
    this.ref.close(null);
  }
}