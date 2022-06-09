import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BanksApiService } from '../../shared/banks-api.service';
import { IBankInfo } from '../../shared/bank-info.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'ds-bank-setup-form',
    templateUrl: './bank-setup-form.component.html',
    styleUrls: ['./bank-setup-form.component.scss']
})
export class BankSetupFormComponent implements OnInit {

    @ViewChild("f", { static: false} )
    form: NgForm;

    banks: IBankInfo[] = [];
    selectedBankId: number = null;
    selectedBank: IBankInfo;
    isSubmitted = false;

    isLoading = true;

    constructor(
        private api: BanksApiService,
        private msg: DsMsgService,
        private confirm: DsConfirmService
    ) { }

    ngOnInit() {
        this.selectedBank = <IBankInfo>{};
        this.selectedBankId = null;

        this.api.getBanks().subscribe(data => {
            this.banks = data.sort((b1, b2) => b1.name > b2.name ? 1 : -1);
            this.isLoading = false;
        })
    }

    bankSelected() {
        this.selectedBank = this.banks.find(b => b.bankId === this.selectedBankId) || <IBankInfo>{};
    }

    save() {
        this.isSubmitted = true;
        if (this.form.invalid)
            return; 

        this.isLoading = true;

        this.api.saveBank(this.selectedBank)
        .subscribe(data => {
            if (!this.selectedBankId) {
                this.banks.push(data);
            } else {
                Object.assign(this.banks.find(b => b.bankId === this.selectedBankId), data);
            }

            this.sortBanks();
            this.selectedBankId = data.bankId;
            this.bankSelected();

            this.msg.setTemporarySuccessMessage("Bank saved successfully");
            this.isLoading = false;
            this.isSubmitted = false;
        },
        (error:HttpErrorResponse) => {
            this.msg.showWebApiException(error.error);
            this.isLoading = false;
        });
    }

    delete() {
        this.confirm.show(null, {
            bodyText: "Are you sure you wish to delete this bank?",
            actionButtonText: "Delete",
            closeButtonText: "Cancel",
            closeButtonClass: "btn-delete",
            swapOkClose: true
        }).then(() => {
            this.isLoading = true;
            this.api.deleteBank(this.selectedBankId).subscribe(result => {
                let idx = this.banks.findIndex(x => x.bankId === this.selectedBankId);
                this.banks.splice(idx, 1);
                this.selectedBankId = null;
                this.bankSelected();
                this.msg.setTemporarySuccessMessage("Bank deleted successfully");
                this.isLoading = false;
                this.form.resetForm();
            },
            (error:HttpErrorResponse) => {
                this.msg.showWebApiException(error.error);
                this.isLoading = false;
            });
        });        
    }

    private sortBanks() {
        this.banks = this.banks.sort((b1, b2) => b1.name > b2.name ? 1 : -1);
    }
}
