import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { EmployeeEEOCApiService } from '@ds/core/employees/shared/employee-eeoc-api.service';
import { IClientPayroll, IPayPeriod, IClientPay } from '@ajs/job-profiles/shared/models/client-payroll.interface';
import { EmpEeocService } from '../emp-eeoc.service';
import { Observable, of } from 'rxjs';
import { map, switchMap, filter, catchError } from 'rxjs/operators';
import { coerceNumberProperty } from '@angular/cdk/coercion';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

export interface FeinGroup {
    fein: number;
    clients: IClientPayroll[];
}

@Component({
    selector: 'ds-eeoc-company-select',
    templateUrl: './eeoc-company-select.component.html',
    styleUrls: ['./eeoc-company-select.component.scss']
})
export class EEOCCompanySelectComponent implements OnInit {
    public f: FormGroup = this.createForm();
    isLoading = true;
    hasErr = false;
    currFein: number;
    formInit: Observable<any>;
    isStepValid = true;

    constructor(
        private eeocApiService: EmployeeEEOCApiService,
        private fb: FormBuilder,
        private service: EmpEeocService,
        private msg: DsMsgService,
    ) { }

    ngOnInit() {

        this.formInit = this.service.selectedW2Year
            .pipe(
                filter(year => year && year > 0),
                switchMap(year => {
                    this.isLoading = true;
                    while (this.feinGroupArray().length) {
                        this.feinGroupArray().removeAt(0);
                    }
                    return this.eeocApiService.getClientPayrollsByYear(year).pipe(
                        catchError(() => {
                            this.isLoading = false;
                            this.hasErr = true;
                            return of([]);
                        }),
                    );
                }),
                map(clients => {
                    const res: FeinGroup[] = [];
                    clients.forEach(c => {
                        if (!c) return;
                        const feinNo = coerceNumberProperty(c.fein.replace(/-/g, ''));
                        const fg = res.find(r => r.fein === feinNo);

                        if (fg) {
                            fg.clients.push(c);
                        } else {
                            res.push({
                                fein: feinNo,
                                clients: [c]
                            });
                        }
                    });
                    this.isLoading = false;
                    this.updateFeinGroupFG(res);
                    if (clients.length > 0) this.hasErr = false;
                    if (this.feinGroupArray().value.length === 0) this.hasErr = true;

                    return true;
                }),
                catchError(err => {
                    this.isLoading = false;
                    this.msg.setTemporaryMessage(err.error.message, this.msg.messageTypes.error);
                    this.hasErr = true;
                    return of({});
                }),
            );

        this.f.valueChanges.subscribe(_ => this.service.eeocFormGroup.get('companySelector').setValue(this.f.valid || null));
    }

    dropdownChange(formControlGroup: FormGroup): void {
        if (!formControlGroup.value.selected) this.clientClicked(formControlGroup);
    }

    clientClicked(client: FormGroup): void {
        const fv = client.value;
        const newValue = !fv.selected;
        const fein = fv.fein.replace(/-/g, '');

        if (!this.currFein) {
            this.currFein = fein;
            this.isStepValid = true;
        }
        if (this.currFein != fein) return;

        client.patchValue({
            selected: newValue
        });

        if (!newValue) {
            // check all clients in the FEIN group and see if you can disable them all
            const feinGroup = this.feinGroupArray().controls.find((tmpFein) => tmpFein.value.fein == fein);
            if (!feinGroup.value.clients.find((tmpClient) => tmpClient.selected)) {
                this.currFein = null;
                this.isStepValid = false;
            }
        }
    }

    outputPayrollIdsAndClients() {
        const currFeinGroup = this.f.get('feinGroups').value.find((feinGroup) => this.currFein && feinGroup.fein == this.currFein);
        if (!currFeinGroup) {
            this.isStepValid = false;
            return false;
        }
        const selectedClientIds: number[] = [];
        const payrollIdArr: number[] = [];
        const clientPayrollList: IClientPay[] = [];
        currFeinGroup.clients.forEach((client) => {
            if (client.selected) {
                clientPayrollList.push({
                    clientId: client.clientId,
                    payrollId: client.selectedPayPeriod
                });
                payrollIdArr.push(client.selectedPayPeriod);
                selectedClientIds.push(client.clientId);
            }
        });
        this.service.updateClientPayrollList(clientPayrollList);
        this.service.updateSelectedClientIdsList(selectedClientIds);
        this.service.updatePayrolls(payrollIdArr);
        this.service.updateSelectedFein(currFeinGroup.fein);
        this.service.updateDisableYearList(true);
        // return false;
    }

    feinGroupArray(): FormArray {
        return this.f.get('feinGroups') as FormArray;
    }

    clientArray(parent: number): FormArray {
        return this.feinGroupArray().at(parent).get('clients') as FormArray;
    }

    payPeriodArray(parent: number, index: number): FormArray {
        return this.feinGroupArray().at(parent).get(['clients', index, 'payPeriods']) as FormArray;
    }

    feinGroup(index: number): FormGroup {
        return this.feinGroupArray().at(index) as FormGroup;
    }

    clientGroup(parent: number, index: number): FormGroup {
        return this.feinGroupArray().at(parent).get(['clients', index]) as FormGroup;
    }

    payPeriod(parentFein: number, parentClient: number, index: number): FormGroup {
        return this.feinGroupArray().at(parentFein).get(['clients', parentClient, 'payPeriods', index]) as FormGroup;
    }

    private createForm(): FormGroup {
        return this.fb.group({
            feinGroups: this.fb.array([], this.selectedValidator)
        });
    }

    private selectedValidator(frmCtrl: FormControl): any {
        const frmArr = (<unknown>frmCtrl) as FormArray;
        if (frmArr.value.length <= 0) return null;
        const rowArr = frmArr.value;
        let valid = false;
        rowArr.forEach((row) => {
            if (!valid) valid = row.clients.find(c => c.selected) != null;
        });
        return !valid ? { hasSelected: false } : null;
    }

    private createFeinGroupFG(fg: FeinGroup): FormGroup {
        return this.fb.group({
            fein: fg.fein,
            feinStr: this.fb.control(''),
            clients: this.fb.array([])
        });
    }

    private createFeinGroupClientFG(c: IClientPayroll): FormGroup {
        const payPeriodsArr = this.createClientPayPeriodsArray(c.payPeriods);

        return this.fb.group({
            code: this.fb.control(c.code),
            companyName: this.fb.control(c.companyName),
            fein: this.fb.control(c.fein),
            payPeriods: payPeriodsArr,
            selectedPayPeriod: this.fb.control(payPeriodsArr.at(0).value.payrollId),
            clientId: this.fb.control(c.clientId),
            selected: this.fb.control(false)
        });
    }

    private createClientPayPeriodsArray(periods: IPayPeriod[]): FormArray {
        const arr = this.fb.array([]);
        periods.forEach(p => {
            arr.push(this.fb.group({
                description: this.fb.control(p.description),
                payrollId: this.fb.control(p.payrollId),
            }));
        });
        return arr;
    }

    private updateFeinGroupFG(fgs: FeinGroup[]) {
        fgs.forEach(fg => {
            const formGroup = this.createFeinGroupFG(fg);
            const clients = formGroup.get('clients') as FormArray;

            if (fg.clients && fg.clients.length && !formGroup.get('feinStr').value)
                formGroup.get('feinStr').setValue(fg.clients[0].fein);

            fg.clients.forEach(c => clients.push(this.createFeinGroupClientFG(c)));

            this.feinGroupArray().push(formGroup);
        });
    }
}
