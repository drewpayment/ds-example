import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn, ValidatorFn, ValidationErrors } from '@angular/forms';
import { IAlert, AlertCategory, AlertType } from '../../../../../../../lib/models/src/lib/alert.model';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { AlertService } from '../../..//admin/company-alerts/shared/alert.service';
import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import * as moment from 'moment';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';

@Component({
    selector: 'ds-add-alert-dialog',
    templateUrl: './add-alert-dialog.component.html',
    styleUrls: ['./add-alert-dialog.component.scss']
  })
  export class AddAlertDialogComponent implements OnInit {
    form1: FormGroup;
    formSubmitted: boolean;
    userinfo: UserInfo;
    dialogTitle:string;
    currentAlert: IAlert;
    
    alertFile: File = null;
    alertFileTypeError: boolean;

    alertSecurityLevel: any;
    securityLevels = [  { id: 1, desc: "System Administrators" }, 
                        { id: 2, desc: "Company Administrators" }, 
                        { id: 4, desc: "Supervisors and above" }, 
                        { id: 3, desc: "Employees Only" },
                        { id: 5, desc: "Everyone" },];
                        
    acceptedFileTypes = ['.pdf', '.doc', '.docx', '.txt', '.rtf' ];

    get startDateCtrl(): FormControl {
        return this.form1.controls['startDate'] as FormControl;
    }
    get endDateCtrl(): FormControl {
        return this.form1.controls['endDate'] as FormControl;
    }

    private endDateValidators = [];
    private startDateValidators = [];

    constructor(
        private ref:MatDialogRef<AddAlertDialogComponent>,
        private alertService: AlertService, 
        private msg: DsMsgService,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data:IAlert,
    ) {
        this.currentAlert = data;
        this.dialogTitle = (data.alertId?'Edit Alert':'Add Alert');
        if(this.currentAlert.alertType == 2){
            // Security Level System Admin is not applicable to Company Alerts
            this.securityLevels = this.securityLevels.filter(x=>x.id > 1);
        }
    }
    
    

    ngOnInit() {
        /// Build the form here
        this.form1 = this.fb.group({
            alertCategory:       this.currentAlert.alertCategoryId.toString(),
            title:              [this.currentAlert.title, Validators.required],
            message:            [this.currentAlert.alertText, Validators.required],
            alertURL:           [this.currentAlert.alertLink, null],
            startDate:          [this.currentAlert.dateStartDisplay, Validators.required],
            endDate:            [this.currentAlert.dateEndDisplay, Validators.required],
            securityLevel:      [this.currentAlert.securityLevel.toString(), Validators.required],
        });

        const urlValidator = this.urlValidatorFn(this.form1.controls.alertCategory as FormControl );
        const urlPattValidator = this.urlPatternValidatorFn(this.form1.controls.alertCategory as FormControl );
        (this.form1.controls.alertURL as FormControl).setValidators([urlValidator
            , urlPattValidator]);

        if(this.currentAlert.alertLink && this.currentAlert.alertCategoryId == 1)
            this.alertFile = new File([],this.fileName(this.currentAlert.alertLink), null);
    }

    public fileName(url:string){
        var inx = url.lastIndexOf('/') > -1 ? url.lastIndexOf('/') : -1;
        if(inx < 0) inx = url.lastIndexOf('\\') > -1 ? url.lastIndexOf('\\') : -1;
        if(inx > -1) return decodeURIComponent(url.substring(inx+1));
        else         return "";
    }

    public urlValidatorFn(typeCtrl:FormControl): ValidatorFn{
        return (control: FormControl): ValidationErrors | null => {
            const url = control.value;
            return (typeCtrl.value == '3' && !url) ? {'required': true} : null;
        };
    }
    public urlPatternValidatorFn(typeCtrl:FormControl): ValidatorFn{
        if (typeCtrl.value == '3')
            return Validators.pattern(/^(http[s]?:\/\/(www\.)?|){1}([0-9A-Za-z-\.@:%_\+~#=]+)+(\.[a-zA-Z]{2,3}){1}(\/[^\?]*)?(\?[^\?]*)?$/)
        else
            return Validators.pattern(/.*/);
    }

    setAlertCategory(){
        this.currentAlert.alertCategoryId =  parseInt( this.form1.value.alertCategory );
        this.form1.patchValue({
            alertURL : '',
        });
        this.alertFile = null;
    }

    clear() {
        this.ref.close(null);
    }

    public browseClicked = ():void => {document.getElementById('alertAdded').click();}

    public onChange($event){
        this.alertFile = (<HTMLInputElement>event.target).files[0];
        if(!this.form1.value.alertName){
            this.form1.patchValue({
                alertName              : this.alertFile.name.split('.')[0],
            });
        }
    }

    public setSecurityLevel(){
        this.currentAlert.securityLevel = this.form1.value.securityLevel;
    }

    public save():void {
        this.formSubmitted = true;
 
        // File & Extn validations
        if(this.currentAlert.alertCategoryId == 1){
            if(this.alertFile){
                if(this.acceptedFileTypes.findIndex( x => this.alertFile.name.toLowerCase().indexOf(x) > -1 ) == -1){
                    this.alertFileTypeError = true;
                    return;
                }
            } else {
                // No alert attachment!!!
                return;
            }
        }
        console.log(this.form1.valid);

        let subscription:Observable<IAlert> = null;
        if(this.form1.valid){
            this.currentAlert.alertCategoryId     = parseInt( this.form1.value.alertCategory);
            this.currentAlert.title     = this.form1.value.title;
            this.currentAlert.alertText     = this.form1.value.message;
            this.currentAlert.alertLink     = this.form1.value.alertURL;
            this.currentAlert.securityLevel   = this.form1.value.securityLevel;
            this.currentAlert.dateStartDisplay   = this.form1.value.startDate;
            this.currentAlert.dateEndDisplay   = this.form1.value.endDate;            
            
            let updateSubscription = null;
            // If there is no content associated with alert, then the alert has already been uploaded
            if(this.alertFile && this.alertFile.size > 0 )
                updateSubscription = this.alertService.uploadAlert(this.currentAlert, this.alertFile);
            else
                updateSubscription = this.alertService.updateAlert(this.currentAlert);
            
            updateSubscription.subscribe(alert => {
                this.ref.close(alert);
            }, (resp: HttpErrorResponse) => {
                let msg = resp.error.errors != null && resp.error.errors.length ? resp.error.errors[0].msg : resp.message;
                this.msg.setTemporaryMessage(msg, MessageTypes.error);
            });
        }
    }
}