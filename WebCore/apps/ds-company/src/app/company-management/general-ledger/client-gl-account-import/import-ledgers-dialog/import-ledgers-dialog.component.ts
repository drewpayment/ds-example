import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn, ValidatorFn, ValidationErrors } from '@angular/forms';
import { GeneralLedgerAccount } from '../../../shared/models/general-ledger-account.model';
import { UserInfo } from '@ds/core/shared/user-info.model';

import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import * as moment from 'moment';
import * as saveAs from 'file-saver';
import { GeneralLedgerService } from '../../../shared/general-ledger.service';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
    selector: 'ds-import-ledgers-dialog',
    templateUrl: './import-ledgers-dialog.component.html',
    styleUrls: ['./import-ledgers-dialog.component.scss']
  })
  export class ImportLedgersDialogComponent implements OnInit {
      
    userinfo: UserInfo;
    dataFile: File = null;
    formSubmitted: boolean;
    acceptedFileTypes = ['.csv', '.xlsx'];
    errList:Array<GeneralLedgerAccount> = [];
    totalRecords:number = 0;

    form1: FormGroup;
    get accountFile() { return this.form1.get('accountFile') as FormControl; }    

    constructor(
        private ref:MatDialogRef<ImportLedgersDialogComponent>,
        private ledgerService: GeneralLedgerService, 
        private msg: NgxMessageService,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data:GeneralLedgerAccount,
    ) {
        
    }
    
    ngOnInit() {
        this.form1 = this.fb.group({
            accountFile: this.fb.control('', { validators: Validators.compose([Validators.required,this.extensionValidator(this.acceptedFileTypes) ]) })
        });
    }

    extensionValidator(acceptedTypes:string[]): ValidatorFn{
        return (fg: FormGroup): ValidationErrors | null => {
            let file = (<HTMLInputElement>document.getElementById('importDataFile')).files[0];
            if(!file) return {};
            if(acceptedTypes.findIndex( x => file.name.toLowerCase().indexOf(x) > -1 ) == -1){
                return {'notSupported':true};
            }
            return {};
        }
    }

    public fileName(url:string){
        var inx = url.lastIndexOf('/') > -1 ? url.lastIndexOf('/') : -1;
        if(inx < 0) inx = url.lastIndexOf('\\') > -1 ? url.lastIndexOf('\\') : -1;
        if(inx > -1) return decodeURIComponent(url.substring(inx+1));
        else         return "";
    }    

    clear() {
        this.ref.close(null);
    }

    public browseClicked = ():void => {document.getElementById('importDataFile').click();}

    public onChange($event){
        this.dataFile = (<HTMLInputElement>event.target).files[0];
    }

    download() {
        this.getTemplateFileToDownload();
    }


    public import():void {
        this.formSubmitted = true;
        if(this.form1.invalid) return;

        let subscription:Observable<GeneralLedgerAccount> = null;    
        let updateSubscription$ = this.ledgerService.uploadCompanyLedgers(this.dataFile);
        
        updateSubscription$.subscribe(importResult => {
            if(importResult && importResult.totalRecords > 0){
                this.formSubmitted = false;
                this.errList = importResult.errorRecords;
                this.totalRecords = importResult.totalRecords;

                if(this.errList.length == 0){
                    this.msg.setSuccessMessage("File imported successfully.");
                    this.ref.close(true);
                }
            } else {
                this.msg.setErrorMessage("Unable to import data.");
            }
        }, (err: HttpErrorResponse) => {
            let errMsg = (err.error && err.error.errors && err.error.errors.length) ? err.error.errors[0].msg : err.message;
            this.msg.setErrorMessage(errMsg);
        });
    }

    getTemplateFileToDownload() {
        let byteString = "Description,Number\n";
        let ab = new ArrayBuffer(byteString.length);
        let ia = new Uint8Array(ab);
        
        for (var i = 0; i < byteString.length; i++) {
            ia[i] = byteString.charCodeAt(i);
        }

        var file = new Blob([ab], { type: 'application/octet-stream' });
        saveAs(file, "Template.csv" );
    }
}