
import { TaxDeferralsService } from '../tax-deferrals.service';
import { UserInfo } from '@ds/core/shared';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { Subscription } from 'rxjs';
import * as moment from 'moment';
import { AccountService } from '@ds/core/account.service';
import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TaxDeferral } from '../../shared/models/tax-deferral.model';

@Component({
    selector: 'ds-add-deferral-dialog',
    templateUrl: './add-deferral-dialog.component.html',
    styleUrls: ['./add-deferral-dialog.component.scss']
})
export class AddDeferralDialogComponent implements OnInit, OnDestroy {

    user: UserInfo;
    f: FormGroup = this.createForm();
    isSubmitted = false;
    subs: Subscription[] = [];

    constructor(
        private fb: FormBuilder,
        private service: TaxDeferralsService,
        private acct: AccountService,
        @Inject(MAT_DIALOG_DATA) public data: TaxDeferral,
        private ref: MatDialogRef<AddDeferralDialogComponent>,
        private msg: DsMsgService
    ) { }

    ngOnInit() {
        if (this.data != null) this.updateForm(this.data);

        this.subs.push(this.acct.getUserInfo().subscribe(u => this.user = u));
    }

    ngOnDestroy() {
        this.subs.forEach(s => s.unsubscribe());
    }

    onNoClick() {
        this.ref.close();
    }

    reset() {
        this.isSubmitted = false;
        this.f.reset();
        this.ref.close();
    }

    saveForm() {
        this.isSubmitted = true;
        if (this.f.invalid) return;

        const dto = this.prepareModel();

        if (dto.clientTaxDeferralId && dto.clientTaxDeferralId > 0) {
            this.service.updateTaxDeferral(dto)
                .subscribe(updated => {
                    this.msg.setTemporarySuccessMessage('Successfully updated Client Tax Deferral.');
                    this.ref.close(updated);
                }, (err) => {
                   const parsed = this.parseError(err);
                   this.msg.showErrorMsg(parsed);
                });
        } else {
            this.service.createTaxDeferral(dto)
                .subscribe(res => {
                    this.msg.setTemporarySuccessMessage('Successfully created Client Tax Deferral.');
                    this.ref.close(res);
                }, err => {
                    this.msg.showErrorMsg(this.parseError(err));
                });
        }
    }

    private createForm(): FormGroup {
        return this.fb.group({
            clientTaxDeferralId: this.fb.control(''),
            clientId: this.fb.control(''),
            taxType: this.fb.control('', [Validators.required]),
            isDeferred: this.fb.control(false),
            endDate: this.fb.control('')
        });
    }

    private updateForm(t: TaxDeferral): void {
        const update = {} as TaxDeferral;
        for (const p in t) {
            if (t[p] != null) {
                update[p] = t[p];
            }
        }

        this.f.patchValue(update);
    }

    private prepareModel(): TaxDeferral {
        const fv = this.f.value;
        return {
            clientTaxDeferralId: fv.clientTaxDeferralId,
            clientId: fv.clientId || this.user.lastClientId,
            taxType: fv.taxType,
            isDeferred: fv.isDeferred,
            endDate: fv.endDate != null ? moment(fv.endDate).format('YYYY-MM-DD') : null
        };
    }

    private parseError(error: any): string {
        return error.error.errors != null && error.error.errors.length
            ? error.error.errors[0].msg
            : error.message;
    }

}
