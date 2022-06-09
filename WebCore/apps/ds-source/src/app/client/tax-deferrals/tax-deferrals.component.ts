import { Component, OnInit, OnDestroy } from '@angular/core';
import { TaxDeferralsService } from './tax-deferrals.service';
import { tap, switchMap } from 'rxjs/operators';
import { Observable, Subscription } from 'rxjs';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { TaxTypeEnum } from '@ajs/employee/hiring/shared/models';
import { MatDialog } from '@angular/material/dialog';
import { AddDeferralDialogComponent } from './add-deferral-dialog/add-deferral-dialog.component';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { TaxDeferral, TaxDeferralType } from '../shared/models/tax-deferral.model';

@Component({
    selector: 'ds-tax-deferrals',
    templateUrl: './tax-deferrals.component.html',
    styleUrls: ['./tax-deferrals.component.scss']
})
export class TaxDeferralsComponent implements OnInit, OnDestroy {

    f: FormGroup = this.createForm();
    user: UserInfo;
    subs: Subscription[] = [];
    isLoading = true;

    constructor(
        private accountService: AccountService,
        private service: TaxDeferralsService,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private msg: DsMsgService
    ) { }

    ngOnInit() {
        this.subs.push(this.accountService.getUserInfo()
            .pipe(
                switchMap(u => {
                    this.user = u;
                    return this.service.getTaxDeferrals(this.user.lastClientId);
                }),
            ).subscribe(defs => {
                defs.forEach(d => {
                    const grp = this.createDeferralFormGroup(d);
                    this.deferralsFormArray().push(grp);
                });

                this.isLoading = false;
            }));

    }

    ngOnDestroy() {
        this.subs.forEach(s => s.unsubscribe());
    }

    deferralsFormArray(): FormArray {
        return this.f.get('deferrals') as FormArray;
    }

    private createForm(): FormGroup {
        return this.fb.group({
            deferrals: this.fb.array([])
        });
    }

    private addDeferralFormGroup(d: TaxDeferral) {
        const fg = this.createDeferralFormGroup(d);
        this.deferralsFormArray().push(fg);
    }

    private createDeferralFormGroup(d: TaxDeferral): FormGroup {
        return this.fb.group({
            clientTaxDeferralId: this.fb.control(d.clientTaxDeferralId),
            clientId: this.fb.control(d.clientId),
            taxType: this.fb.control(d.taxType, [Validators.required]),
            isDeferred: this.fb.control(d.isDeferred),
            endDate: this.fb.control(d.endDate)
        });
    }

    getTaxTypeName(frmControlIndex: number): string {
        const type = this.f.get(['deferrals', frmControlIndex]).value.taxType;
        switch (type) {
            case TaxDeferralType.EmployerSocialSecurity:
            default:
                return 'Employer Social Security';
        }
    }

    getTaxTypeDescription(frmControlIndex: number): string {
        const type = this.f.get(['deferrals', frmControlIndex]).value.taxType;
        switch (type) {
            case TaxDeferralType.EmployerSocialSecurity:
            default:
                return `Portion of taxable wage liability that the employer is responsible to of
                    Social Security withholding as a part of FICA Tax.`;
        }
    }

    onIsDeferredChange(event, frmIndex) {
        const isDeferred = event.target.checked;
        const fg = this.deferralsFormArray().at(frmIndex) as FormGroup;
        const dto = this.prepareModel(fg);

        this.service.updateTaxDeferral(dto)
            .subscribe(res => {
                this.msg.setTemporarySuccessMessage('Successfully updated the tax deferral.');
                fg.setValue(this.parseDeferralToFormGroup(res));
            }, err => this.msg.showErrorMsg(this.parseError(err)));
    }

    showAddDialog() {
        this.subs.push(this.dialog.open(AddDeferralDialogComponent, {
                maxWidth: '95vw',
                autoFocus: false,
            })
            .afterClosed()
            .subscribe((result: TaxDeferral) => {
                if (!result) return;

                this.addDeferralFormGroup(result);
            }));
    }

    showEditDialog(frmIndex: number) {
        const fg = this.deferralsFormArray().at(frmIndex) as FormGroup;
        const def = this.prepareModel(fg);

        this.subs.push(this.dialog.open(AddDeferralDialogComponent, {
                data: def,
                maxWidth: '95vw',
                autoFocus: false,
            })
            .afterClosed()
            .subscribe((result: TaxDeferral) => {
                if (!result) return;

                fg.setValue(this.parseDeferralToFormGroup(result));
            }));
    }

    deleteDeferral(frmIndex: number) {
        const fg = this.deferralsFormArray().at(frmIndex) as FormGroup;
        const def = this.prepareModel(fg);

        this.service.deleteTaxDeferral(def.clientId, def.clientTaxDeferralId)
            .subscribe(_ => {
                this.msg.setTemporarySuccessMessage('Successfully deleted the requested tax deferral.');
                this.deferralsFormArray().removeAt(frmIndex);
            }, err => {
                this.msg.showErrorMsg(this.parseError(err));
            });
    }

    private prepareModel(fg: FormGroup): TaxDeferral {
        const fv = fg.value;
        return {
            clientTaxDeferralId: fv.clientTaxDeferralId,
            clientId: fv.clientId,
            taxType: fv.taxType,
            isDeferred: fv.isDeferred,
            endDate: fv.endDate
        };
    }

    private parseDeferralToFormGroup(d: TaxDeferral): any {
        return {
            clientTaxDeferralId: d.clientTaxDeferralId,
            clientId: d.clientId,
            taxType: d.taxType,
            isDeferred: d.isDeferred,
            endDate: d.endDate
        };
    }

    private parseError(error: any): string {
        return error.error.errors != null && error.error.errors.length
            ? error.error.errors[0].msg
            : error.message;
    }

}
