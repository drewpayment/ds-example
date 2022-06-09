import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { ClientAccrualsStoreService } from '../../../../../client-management/services/client-accruals-store.service';
import { ClientAccrualsService } from '../../../../../client-management/services/client-accruals.service';
import { LeaveManagementApiService } from '../../../../../client-management/services/leave-management-api.service';

@Component({
    selector: "ds-client-accruals-atm-export-card",
    templateUrl: "./client-accruals-atm-export-card.component.html",
    styleUrls: ["./client-accruals-atm-export-card.component.scss"],
})
export class ClientAccrualsAtmExportCardComponent implements OnInit, OnDestroy {


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
        // this.form = this._clientAccrualsSvc.restoreOrCreateFormGroup('atmExport') as FormGroup;

        // this.selectedAccrualPolicy$.pipe(takeUntil(this._destroy$)).subscribe(x => {
        //   this._cd.detectChanges();
        // });
    }

    ngOnDestroy() {
        this._destroy$.next();
    }

}
