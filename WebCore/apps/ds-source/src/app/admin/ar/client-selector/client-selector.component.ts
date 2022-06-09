import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Observable, of, combineLatest } from 'rxjs';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { startWith, map, filter, switchMap, tap, withLatestFrom } from 'rxjs/operators';
import { IOption } from '../shared/option.model';

@Component({
    selector: 'ds-client-selector',
    templateUrl: './client-selector.component.html',
    styleUrls: ['./client-selector.component.scss']
})
export class ClientSelectorComponent implements OnInit {
    filteredOptions$: Observable<IOption[]>;
    @Input() options: IOption[];
    @Input() defaultOption: IOption;
    @Input()
    clientSelectorFormGroup: FormGroup;
    @Input() required: boolean;

    @Input() isInvalid: boolean;
    selected: IOption = {filter: "", id: 0};
    get client() { return this.clientSelectorFormGroup.get('client') as FormControl; }

    constructor(private formBuilder: FormBuilder) { }

    ngOnInit() {
        if(this.defaultOption){
            this.options.unshift(this.defaultOption);
        }

        this.filteredOptions$ = combineLatest(this.client.valueChanges.pipe(
            startWith('')
        ), of(this.options))
            .pipe(
                map(value => this._filter(value[0], value[1]))
            );
    }

    displayFn(client?: IOption): string | undefined {
        return client ? client.filter : undefined;
    }

    private _filter(value: string, options: IOption[]): IOption[] {
        if (typeof value === "string" && value && value != "" && options) {
            const filterValue = value.toLowerCase().replace(/\s/g, '');
            return options.filter(x => x.filter.toLowerCase().includes(filterValue));
        }

        return options;
    }

    focusOnAutocomplete() {
        this.setSelectedClient();
        if (this.client){
            this.client.setValue(null);
        }
    }

    private setSelectedClient() {
        if (this.client && this.client.value && this.selected !== this.client.value) {
            this.selected = this.client.value;
        }
    }

    focusOffAutocomplete() {
        this.setSelectedClient();
        this.client.setValue(this.selected);
    }

    private _isObject(target): boolean {
        return typeof target === 'object' && target != null;
    }
}
