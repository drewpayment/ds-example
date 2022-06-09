import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
    ClientService,
    ClientOption
} from '../shared';
import {
    AccountOptionInfoWithClientSelectionByControlIdName,
    ClientOptionKeyCapitalize,
    ClientOptionKeyUncapitalize,
    IAccountOptionInfoWithClientSelection,
    isInstanceOfClientOptionKey,
    toUncapitalizedString
} from '../shared/account-option-info-with-client-selection.model';
import * as _ from "lodash";
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { BehaviorSubject, combineLatest, concat, EMPTY, iif, Observable, of } from 'rxjs';
import {
    map,
    debounceTime,
    distinctUntilChanged,
    catchError,
    take,
    tap,
    switchMap,
    shareReplay
} from 'rxjs/operators';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'ds-client-options',
    templateUrl: './client-options.component.html',
    styleUrls: ['./client-options.component.scss']
})
export class ClientOptionsComponent implements OnInit {

    /**
     * Primary "upstream" source, from which the rest of the observables in this component are derived.
     */
    private readonly _clientAccountOptionsByControlIdName$ = new BehaviorSubject<AccountOptionInfoWithClientSelectionByControlIdName>(
        null as AccountOptionInfoWithClientSelectionByControlIdName
    );
    readonly clientAccountOptionsByControlIdName$ = this._clientAccountOptionsByControlIdName$.asObservable();

    /**
     * "Unwraps" key-value structure of clientAccountOptionsByControlIdName$ into an array.
     * Due to the nature of JS allowing objects to have arbitrary keys at runtime,
     * some validation and cleanup is necessary to ensure that only the intended keys are mapped
     * into the downstream array.
     */
    readonly clientAccountOptions$ = this.clientAccountOptionsByControlIdName$.pipe(
        map(x => {
            if (x == null) {
                return null;
            }
            
            return Object.keys(x)
                .filter(key => isInstanceOfClientOptionKey(key))
                .map((controlId: ClientOptionKeyUncapitalize) => x[controlId]);
        }),
        map(x => Array.isArray(x)
            ? x as IAccountOptionInfoWithClientSelection[]
            : null as IAccountOptionInfoWithClientSelection[]
        ),
        shareReplay(1),
    );

    readonly searchText = new FormControl("");

    readonly searchText$ = concat(
        // Want this to emit immediately once subscribed.
        of(''),
        // Then rate-limit subsiquent user input.
        (this.searchText.valueChanges as Observable<string>).pipe(
            debounceTime(250),
            distinctUntilChanged(),
            map(x => x.trim().toLowerCase()),
        )
    );

    /**
     * Results that are used/shown in the template.
     */
    readonly clientAccountOptionsFilteredBySearch$ = combineLatest([
        this.clientAccountOptions$,
        this.searchText$,
    ]).pipe(
        map(([clientAccountOptions, searchText]) => {
            if (clientAccountOptions == null) {
                return null;
            }
            
            return clientAccountOptions
                .filter(x => x.description.trim().toLowerCase().includes(searchText));
        }),
    );

    readonly searchLength$ = this.clientAccountOptionsFilteredBySearch$
        .pipe(map(x => (x || []).length));

    readonly maxLength$ = this.clientAccountOptions$
        .pipe(map(x => (x || []).length));

    readonly isLoaded$ = this.clientAccountOptionsFilteredBySearch$
        .pipe(map(x => Array.isArray(x)));

    @Input("page") page: string;

    constructor(
        private route: ActivatedRoute,
        private clientApi: ClientService,
        private msg:DsMsgService
    ) {
        // Nothing.
    }

    ngOnInit() {
        this.page = this.route.snapshot.paramMap.get("page");
        if (this.page == null || this.page == "" || (this.page !== "payroll" && this.page !== "timeclock")) {
            this.page = "payroll";
        }
        
        this.clientApi.getAccountOptionInfoWithClientSelectionByControlIds(this.page).pipe(
            catchError((err, data) => {
                this.msg.showWebApiException(err);
                return EMPTY;
            }),
        )
        .subscribe(data => {
            this._clientAccountOptionsByControlIdName$.next(data);
        });
    }

    save() {
        this.msg.sending(true);
        this.clientAccountOptionsByControlIdName$.pipe(
            take(1),
            switchMap(dtos => this.clientApi.saveAccountOptionInfoWithClientSelectionByControlIds(dtos)),
            catchError((err, data) => {
                this.msg.sending(false);
                this.msg.showWebApiException(err);
                return EMPTY;
            }),
        )
        .subscribe(data => {
            this.msg.sending(false);
            this.msg.setTemporarySuccessMessage('Successfully saved company options.');
            
            // Update model with any new ids that were inserted
            const value = this._clientAccountOptionsByControlIdName$.value;

            Object.keys(data)
                .filter(key => isInstanceOfClientOptionKey(key))
                .forEach(key => {
                    const keyUncap = toUncapitalizedString(key) as ClientOptionKeyUncapitalize;
                    const current = value[keyUncap];

                    // If the ~clientAccountOptionId~ `ClientOptionControlId` is anything other than an integer greater than zero,
                    // it represents a new record that was just inserted into the table and assigned an actual Id.
                    // Let's update our local model to reflect this new Id, so that if the user saves again before reloading the page,
                    // it sends the correct Id of the existing record to the backend, so it, in turn, knows how to update it.
                    if (!(current.clientSelection.clientAccountOptionId > 0)) {
                        current.clientSelection.clientAccountOptionId = data[keyUncap].clientSelection.clientAccountOptionId;
                    }
                });

            this._clientAccountOptionsByControlIdName$.next(value);
        });
    }
}
