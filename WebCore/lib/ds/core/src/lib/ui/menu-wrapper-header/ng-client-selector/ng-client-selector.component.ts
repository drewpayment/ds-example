import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
} from "@angular/core";
import { IClientData } from "@ajs/onboarding/shared/models";
import { Observable, of, merge } from "rxjs";
import { FormControl } from "@angular/forms";
import {
  startWith,
  switchMap,
  map,
  debounceTime,
  distinctUntilChanged,
} from "rxjs/operators";
import { Moment } from "moment";
import { ClientSelectorService } from "./client-selector.service";
import { isObject } from "angular";
import { isString } from "@util/coercion";
import { coerceBooleanProperty } from "@angular/cdk/coercion";
import { MatAutocompleteTrigger, MatAutocomplete } from '@angular/material/autocomplete';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

interface DialogData {
  selectedClient$: Observable<IClientData>;
}

interface ClientCache {
  data: IClientData[];
  updatedAt: Moment;
}

@Component({
  selector: "ds-ng-client-selector",
  templateUrl: "./ng-client-selector.component.html",
  styleUrls: ["./ng-client-selector.component.scss"],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NgClientSelectorComponent implements OnInit {

    clientSearchInput = new FormControl();
    searchByIsActive?:boolean = true;
    filterActive = new FormControl(true);
    clients$: Observable<IClientData[]>;
    selectedClient: IClientData;

    @ViewChild(MatAutocompleteTrigger, { static: false }) trigger: MatAutocompleteTrigger;
    @ViewChild(MatAutocomplete, { static: true }) auto: MatAutocomplete;

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private dialogRef: MatDialogRef<NgClientSelectorComponent>,
        private clientService: ClientSelectorService,
        private cd: ChangeDetectorRef,
    ) {}

    ngOnInit() {
        this.clients$ = this.clientService.getCurrentClient()
            .pipe(
                switchMap(client => {
                    if (client && !this.selectedClient) {
                        this.selectedClient = client;
                        this.clientSearchInput.setValue(this.selectedClient);

                        if (this.trigger) {
                            this.trigger.openPanel();
                            this.cd.detectChanges();
                        }
                    }
                    return this.clientSearchInput.valueChanges;
                }),
                startWith(''),
                distinctUntilChanged(),
                debounceTime(250),
                switchMap(value => {
                    return merge(this.filterActive.valueChanges, this.clientSearchInput.valueChanges);
                }),
                map((value) => {
                    if (isString(value)) {
                        return value;
                    } else {
                        this.searchByIsActive = value;
                        if (isObject(value)) {
                            this.searchByIsActive = true;
                        }
                    }

                    return this.clientSearchInput != null ? this.clientSearchInput.value : '';
                }),
                switchMap(value => {
                    if (isObject(value)) {
                        const c = value as IClientData;

                        if (c.clientId !== this.selectedClient.clientId) {
                            if (localStorage != null) localStorage.clear();
                            this.clientService.selectClient(c.clientId);
                            this.dialogRef.close();
                            return of(null);
                        }
                    }

                    return this.clientService.getClients(this.searchByIsActive)
                        .pipe(map(clients => {
                            const search = !isObject(value) ? value.trim().toLowerCase() : '';
                            return clients.filter(c => {
                                const matches = c.clientCode.trim().toLowerCase().includes(search);
                                if (matches) return matches;

                                const matchesFedId = c.federalId.trim().toLowerCase().includes(search);
                                if (matchesFedId) return matchesFedId;

                                return c.clientName.trim().toLowerCase().includes(search);
                            }).slice(0, 100);
                        }));
                })
            );
    }

    displayFn(client: IClientData) {
        return '';
    }
}
