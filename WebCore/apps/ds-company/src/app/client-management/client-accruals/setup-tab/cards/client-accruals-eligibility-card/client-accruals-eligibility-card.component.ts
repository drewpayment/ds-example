import { coerceNumberProperty } from '@angular/cdk/coercion';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { ClientAccrualsStoreService } from '../../../../../client-management/services/client-accruals-store.service';
import { ClientAccrualsService } from '../../../../../client-management/services/client-accruals.service';
import { LeaveManagementApiService } from '../../../../../client-management/services/leave-management-api.service';

@Component({
    selector: 'ds-client-accruals-eligibility-card',
    templateUrl: './client-accruals-eligibility-card.component.html',
    styleUrls: ['./client-accruals-eligibility-card.component.scss'],
})
export class ClientAccrualsEligibilityCardComponent implements OnInit, OnDestroy {

    form: FormGroup = this.store.form;

    dropdownLists$ = this._clientAccrualsSvc.dropdownLists$;
    // selectedAccrualPolicy$ = this._clientAccrualsSvc.selectedAccrualPolicy$;

    private readonly _destroy$ = new Subject();

    constructor(
        private _fb: FormBuilder,
        private _cd: ChangeDetectorRef,
        private _clientAccrualsSvc: ClientAccrualsService,
        private store: ClientAccrualsStoreService,
    ) { }

    ngOnInit() {
        // this.form = this._clientAccrualsSvc.restoreOrCreateFormGroup('eligibility') as FormGroup;

        // this.selectedAccrualPolicy$.pipe(takeUntil(this._destroy$)).subscribe(x => {
        //   this._cd.detectChanges();
        // });
    }

    ngOnDestroy() {
        this._destroy$.next();
    }

}
