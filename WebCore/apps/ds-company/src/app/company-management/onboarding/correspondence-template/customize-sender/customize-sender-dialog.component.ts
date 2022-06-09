import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ICustomizeSenderData, IEmailData } from "apps/ds-company/src/app/models/correspodence-template-data";
import { CorrespondenceTemplateApiService } from "apps/ds-company/src/app/services/correspondence-template-api.service";
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: 'ds-customize-sender-dialog',
  templateUrl: './customize-sender-dialog.component.html',
  styleUrls: ['./customize-sender-dialog.component.scss']
})
export class CustomizeSenderDialogComponent implements OnInit {

    public clientId: number;
    public senderData: ICustomizeSenderData;
    public emailData: IEmailData;
    public connectionTypes: Array<object>;
    public onRestoreDefault: boolean =false;
    public dataSaving: boolean = false;
    public errMessage: string;
    public formSubmitted: boolean = false;

    form: FormGroup;
    constructor(
        public dialogRef:MatDialogRef<CustomizeSenderDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:any,
        private apiService: CorrespondenceTemplateApiService,
        private fb: FormBuilder,
        private msg: NgxMessageService,
    ) {}

    ngOnInit() {
        //this.onRestoreDefault = true;
        this.clientId = this.data.clientId;
        this.senderData = this.data.senderInfo;
        this.connectionTypes = [{name:"None",value:"none"}, {name:"SSL",value:"ssl"},{name:"TLS",value:"tls"}];
        if (this.senderData == null) {
            this.setDefaultValues();
        } else {
            if( this.senderData.smtpPort=="" &&
                this.senderData.smtpLogin=="" &&
                this.senderData.smtpPassword=="" &&
                this.senderData.senderEmail === 'DoNotReply@dominionsystems.com' &&
                this.senderData.secureConnection === 'none' )
                {
                    this.onRestoreDefault = true;
                }
        }
        this.initForm();
    }

    private initForm(){
        this.form = this.fb.group({
            host: this.fb.control(this.senderData.smtpHost, (!this.onRestoreDefault) ? [Validators.required, Validators.maxLength(50)] : []),
            port: this.fb.control(this.senderData.smtpPort, (!this.onRestoreDefault) ? [Validators.required, Validators.maxLength(10), Validators.pattern('[0-9]*')] : []),
            senderEmail: this.fb.control(this.senderData.senderEmail, (!this.onRestoreDefault) ? [Validators.required, Validators.maxLength(50), Validators.pattern(/^[\w\.\%\+\-]+@[\w\.\-]+\.[a-zA-Z]{2,4}$/)] : []),
            smtpLogin: this.fb.control(this.senderData.smtpLogin || '', (!this.onRestoreDefault) ? [Validators.required, Validators.maxLength(50)] : []),
            smtpPassword: this.fb.control(this.senderData.smtpPassword || '', (!this.onRestoreDefault) ? [Validators.required, Validators.maxLength(50)] : []),
            connectionType: this.fb.control(this.senderData.secureConnection || '', (!this.onRestoreDefault) ? [Validators.required]  : []),
            restoreDefault: this.fb.control(this.onRestoreDefault),
          });
    }

    private setDefaultValues() {
        this.senderData = {
            clientSMTPSettingId: this.senderData != null ? this.senderData.clientSMTPSettingId : 0,
            clientId: this.clientId,
            senderEmail: 'DoNotReply@dominionsystems.com',
            smtpHost: '',
            smtpPort: '',
            smtpLogin: '',
            smtpPassword: '',
            secureConnection: 'none'
        };
    }

    restoreDefault() {
        if (this.onRestoreDefault)
        {
            this.onRestoreDefault=false;
            this.initForm();
        }
        else{
            this.onRestoreDefault=true;
            this.setDefaultValues();
            this.initForm();
            this.form.controls['host'].disable();
        }
    }

    save() {
        this.formSubmitted = true;
        if (this.form.valid) {
            this.dataSaving=true;
            this.senderData = {
                clientSMTPSettingId: this.senderData != null ? this.senderData.clientSMTPSettingId : 0,
                clientId: this.clientId,
                senderEmail: this.form.value.senderEmail,
                smtpHost: this.form.value.host,
                smtpPort: this.form.value.port,
                smtpLogin: this.form.value.smtpLogin,
                smtpPassword: this.form.value.smtpPassword,
                secureConnection: this.form.value.connectionType
            };
            this.apiService.saveClientSMTPSetting(this.senderData).subscribe(data => {
                this.formSubmitted = false;
                this.dialogRef.close(data);   
                this.msg.setSuccessMessage("Settings were saved successfully.");     
            });
            this.dataSaving=false;
        }
    }

    test() {
        this.emailData = null;
        //this.testFailed = false;
        this.formSubmitted=true;

        if (this.form.valid) {
            this.apiService.testSMTPSetting(this.senderData)
            .subscribe(data => {
                if (data) {
                    this.emailData = data;
                    this.msg.setSuccessMessage("Your test email was sent to " + this.emailData.toAddress + ".");
                } else {
                   // this.testFailed = true;
                    this.msg.setErrorMessage("Unable to send test email");
                }
            })
        }
    }

    onNoClick(){
        this.dialogRef.close();
    }
}