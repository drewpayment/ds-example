import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AccrualBalanceOption } from 'apps/ds-company/src/app/models/leave-management/accrual-balance-option';
import { ClientAccrualConstants } from 'apps/ds-company/src/app/models/leave-management/client-accrual-constants';
import { ServiceBeforeAfter } from 'apps/ds-company/src/app/models/leave-management/service-before-after';
import { ServiceReferencePointFrequency } from 'apps/ds-company/src/app/models/leave-management/service-reference-point-frequency';
import { forkJoin, Subject } from 'rxjs';
import { map, take, takeUntil } from 'rxjs/operators';
import { ClientAccrualsStoreService } from '../../../../../client-management/services/client-accruals-store.service';
import { ClientAccrualsService } from '../../../../../client-management/services/client-accruals.service';
import { LeaveManagementApiService } from '../../../../../client-management/services/leave-management-api.service';

@Component({
    selector: 'ds-client-accruals-computation-card',
    templateUrl: './client-accruals-computation-card.component.html',
    styleUrls: ['./client-accruals-computation-card.component.scss'],
})
export class ClientAccrualsComputationCardComponent implements OnInit, OnDestroy {

    form: FormGroup = this.store.form;
    // showCarryOverToBalanceLimit: boolean = false;
    nestedForm = this.form.get('accrual') as FormGroup;
    get carryOverToId() { return this.nestedForm.controls.carryOverToId as FormControl };
    
    dropdownLists$ = this._clientAccrualsSvc.dropdownLists$;
    availableClientAccruals$ = this.store.clientAccruals$.pipe(
      map(x => Array.isArray(x)
        ? x.filter(y => y.clientAccrualId != ClientAccrualConstants.NEW_ENTITY_ID)
        : []
      ),
    );

    private readonly _destroy$ = new Subject();

    constructor(
        private _fb: FormBuilder,
        private _cd: ChangeDetectorRef,
        private _clientAccrualsSvc: ClientAccrualsService,
        private store: ClientAccrualsStoreService,
    ) { }

    ngOnInit() {
        // this.form = this._clientAccrualsSvc.restoreOrCreateFormGroup('computation') as FormGroup;

        // this.selectedAccrualPolicy$.pipe(takeUntil(this._destroy$)).subscribe(x => {
        //   this._cd.detectChanges();
        // });
    }

    ngOnDestroy() {
        this._destroy$.next();
    }

    // carryOverToIdChange(){
    //     if (this.form.value.accrual.carryOverToId > 0) this.showCarryOverToBalanceLimit = true;
    //     else this.showCarryOverToBalanceLimit = false;
    // }

}
