import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { IClientAccrual } from '@ds/core/employee-services/models';
import { Subject } from 'rxjs';
import { map, take, takeUntil } from 'rxjs/operators';
import { ClientAccrualsStoreService } from '../../../../../client-management/services/client-accruals-store.service';
import { ClientAccrualsService } from '../../../../../client-management/services/client-accruals.service';

@Component({
    selector: 'ds-client-accruals-general-card',
    templateUrl: './client-accruals-general-card.component.html',
    styleUrls: ['./client-accruals-general-card.component.scss'],
})
export class ClientAccrualsGeneralCardComponent {

    f = this.store.form;

    dropdownLists$ = this._clientAccrualsSvc.dropdownLists$;

    constructor(
        private _fb: FormBuilder,
        private _cd: ChangeDetectorRef,
        private _clientAccrualsSvc: ClientAccrualsService,
        private store: ClientAccrualsStoreService,
    ) { }

}
