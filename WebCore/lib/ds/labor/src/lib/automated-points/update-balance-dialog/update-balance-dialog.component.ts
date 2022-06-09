import { IUpdateBalanceRequest } from './../shared/update-balance-request.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AutomatedPointsApiService } from './../shared/automated-points-api.service';
import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IUpdateBalanceDialogResult, IUpdateBalanceDialogData } from './update-balance-dialog.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

@Component({
  selector: 'ds-update-balance-dialog',
  templateUrl: './update-balance-dialog.component.html',
  styleUrls: ['./update-balance-dialog.component.scss']
})
export class UpdateBalanceDialogComponent implements OnInit {
    form: FormGroup;
    invalid: boolean = false;

  constructor(private dialogRef: MatDialogRef<UpdateBalanceDialogComponent, IUpdateBalanceDialogResult>,
    private automatedPointsAPi: AutomatedPointsApiService,
    @Inject(MAT_DIALOG_DATA) private data: IUpdateBalanceDialogData,
    private formBuilder: FormBuilder,
    private msg: DsMsgService) { }

  ngOnInit() {
      this.form = this.formBuilder.group({
        expireDate: [null, Validators.required]
      });
  }

  public closeModal(){
      this.dialogRef.close();
  }

  public updateBalance(){
    if (this.form.invalid){
        this.invalid = true;
        return;
    }

    let request: IUpdateBalanceRequest = {
        expireDate: this.form.value.expireDate,
        clientId: this.data.clientId,
        userId: this.data.userId
    }

    this.msg.sending(true);

    this.automatedPointsAPi.updateBalance(request).subscribe(data => {
        //Success
        this.closeModal();
        this.msg.setTemporarySuccessMessage("Balances have been updated sucessfully.");
    }),(data => {
        //Error
        this.msg.showWebApiException;
        this.dialogRef.close();
    });
  }
}
