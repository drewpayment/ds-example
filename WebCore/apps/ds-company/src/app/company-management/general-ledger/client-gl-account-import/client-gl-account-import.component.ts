import { Component, OnInit, Input, Output, Inject, ViewChild } from '@angular/core';
import { AccountService } from '@ds/core/account.service';

import { DOCUMENT } from '@angular/common';
import { Observable, from, iif, of, forkJoin, merge  } from 'rxjs';
import { map, tap, switchMap } from 'rxjs/operators';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ImportLedgersDialogComponent } from './import-ledgers-dialog/import-ledgers-dialog.component';
import { date } from '@ajs/applicantTracking/application/inputComponents';
import { HttpErrorResponse } from '@angular/common/http';
import { GeneralLedgerService } from '../../shared/general-ledger.service';
import { GeneralLedgerAccount } from '../../shared/models/general-ledger-account.model';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-client-gl-account-import',
  templateUrl: './client-gl-account-import.component.html',
  styleUrls: ['./client-gl-account-import.component.scss']
})
export class ClientGlAccountImportComponent implements OnInit {
   
    isLoading: boolean = true;
    userinfo: UserInfo;
    form1: FormGroup;
    companyLedgers: Array<GeneralLedgerAccount>;
    selectedLedgerId: number;
    formSubmitted: boolean;

    get accountId() { return this.form1.get('accountId') as FormControl; }
    get accountName() { return this.form1.get('accountName') as FormControl; }
    get accountNumber() { return this.form1.get('accountNumber') as FormControl; }

    constructor(private accountService: AccountService,
        private ledgerService: GeneralLedgerService, 
        private msg: NgxMessageService,
        private dialog: MatDialog,
        private fb: FormBuilder,
        private confirm: ConfirmDialogService,
        @Inject(DOCUMENT) private document: Document) {
        
    }
    ngOnInit() {
        this.isLoading = true;
        this.formSubmitted = false;
        this.createForm();

        this.checkCurrentUser().pipe(
            switchMap(x => this.ledgerService.getClientGeneralLedgerAccounts()),
            tap( ledgers => {
                this.prepareList(ledgers);
                this.isLoading = false; 
            })
        ).subscribe();
    }

    private prepareList(ledgers:GeneralLedgerAccount[]){
        ledgers.sort((x,y) => x.number.toLowerCase().localeCompare(y.number.toLowerCase()));
        let opts = [<GeneralLedgerAccount>{accountId:0,description:'--Add Account--',number:''}];
        if(ledgers) opts.push(...ledgers);
        this.companyLedgers = opts;
        this.selectedLedgerId = 0;
    }

    private createForm(): void {
        this.selectedLedgerId = 0;

        this.form1 = this.fb.group({
            accountId: ['0'],
            accountName: ['',   [Validators.required, Validators.maxLength(50)]],
            accountNumber: ['', [Validators.required, Validators.maxLength(75)]],
        });
    }

    private accountChange():void{
        this.selectedLedgerId = Number(this.accountId.value);
        let k = this.companyLedgers.find(x=>x.accountId == this.selectedLedgerId );
        if(k)   this.updateForm(k);
        
        this.form1.markAsUntouched();
        this.formSubmitted = false;
    }

    private updateForm(gl:GeneralLedgerAccount): void{
        this.form1.patchValue({
            accountName: !gl.number ? '' : gl.description,
            accountNumber: gl.number,
        });
    }

    private save(frm:FormGroup): void{
        this.formSubmitted = true;
        if(!frm.invalid){
            if( this.companyLedgers.find(x => x.description.toLowerCase() == this.accountName.value.toLowerCase() 
                 && x.accountId != this.selectedLedgerId)) {
                this.accountName.setErrors({ duplicate: true });
                return;
            }
            if( this.companyLedgers.find(x => x.number.toLowerCase() == this.accountNumber.value.toLowerCase()
                 && x.accountId != this.selectedLedgerId)) {
                this.accountNumber.setErrors({ duplicate: true });
                return;
            }

            let refOpt:GeneralLedgerAccount = null, copyOpt:GeneralLedgerAccount = null;
            if(this.selectedLedgerId){
                copyOpt = Object.assign( {} , this.companyLedgers.find(x=>x.accountId == this.selectedLedgerId ));
                copyOpt.description   = this.accountName.value;
                copyOpt.number        = this.accountNumber.value;
            } else {
                copyOpt = <GeneralLedgerAccount>{accountId:0,
                    description:this.accountName.value,
                    number:this.accountNumber.value};
            }

            this.ledgerService.updateCompanyLedger(copyOpt).subscribe(result => {
                if(!copyOpt.accountId){
                    this.msg.setSuccessMessage("General ledger added successfully.");
                    this.companyLedgers.push(result);
                    refOpt = result;
                    
                    this.selectedLedgerId = refOpt.accountId;
                    this.form1.patchValue({
                        accountId: refOpt.accountId.toString(),
                        accountName: refOpt.description,
                        accountNumber: refOpt.number,
                    });
                } else {
                    refOpt = this.companyLedgers.find(x=>x.accountId == this.selectedLedgerId );
                    this.msg.setSuccessMessage("General ledger updated successfully.");
                }

                refOpt.description = copyOpt.description;
                refOpt.number = copyOpt.number;
                this.formSubmitted = false;                    
            }, err => {
                let errMsg = (err.error && err.error.errors && err.error.errors.length) ? err.error.errors[0].msg : err.message;
                this.msg.setErrorMessage(errMsg);
            });
        }
    }

    private delete(): void{
        const options = {
            title: '',
            message: "Are you sure you wish to delete this General Ledger Account?",
            confirm: 'Delete'
        };
        this.confirm.open(options);
        this.confirm.confirmed().subscribe(confirmed => {
            if ( confirmed ) {
                let k = Object.assign( {} , this.companyLedgers.find(x=>x.accountId == this.selectedLedgerId ));
                this.ledgerService.deleteCompanyLedger(k).subscribe(result => {
                    let idx = this.companyLedgers.findIndex(x => x.accountId === this.selectedLedgerId);
                    this.companyLedgers.splice(idx, 1);
                    this.msg.setSuccessMessage("General ledger deleted successfully.");

                    this.form1.patchValue({
                        accountId: '0',
                        accountName: '',
                        accountNumber: '',
                    });
                    this.form1.markAsUntouched();
                    this.selectedLedgerId = 0;
                },
                (err:HttpErrorResponse) => {
                    let errMsg = (err.error && err.error.errors && err.error.errors.length) ? err.error.errors[0].msg : err.message;
                    this.msg.setErrorMessage("This account is used in your general ledger and cannot be deleted." );
                    this.isLoading = false;
                });
            }
        });
    }

    checkCurrentUser(): Observable<UserInfo>{
        return iif(() => this.userinfo == null, 
            this.accountService.getUserInfo().pipe(tap(u => {
                this.userinfo = u;
            })),
            of(this.userinfo));
    }

    popupImportDialog() {
        let config = new MatDialogConfig<any>();
        config.width = "500px";
        config.data = {};

        return this.dialog.open<ImportLedgersDialogComponent, any, GeneralLedgerAccount>(ImportLedgersDialogComponent, config)
        .afterClosed()
        .subscribe((ok: any) => {
            if(ok === true){
                this.isLoading = true; 
                this.ledgerService.getClientGeneralLedgerAccounts().pipe(
                tap( ledgers => {
                    this.isLoading = false;
                    this.prepareList(ledgers);
                })).subscribe();
            }
        });
    }

    public fileName(url:string){
        var inx = url.lastIndexOf('/') > -1 ? url.lastIndexOf('/') : -1;
        if(inx < 0) inx = url.lastIndexOf('\\') > -1 ? url.lastIndexOf('\\') : -1;
        if(inx > -1) return decodeURIComponent(url.substring(inx+1));
        else         return "";
    }
}