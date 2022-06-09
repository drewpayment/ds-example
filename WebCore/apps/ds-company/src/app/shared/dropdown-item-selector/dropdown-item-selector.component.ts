import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Observable, of, combineLatest } from 'rxjs';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { startWith, map, filter, switchMap, tap, withLatestFrom } from 'rxjs/operators';
import { IOption } from '@models';

@Component({
    selector: 'ds-dropdown-item-selector',
    templateUrl: './dropdown-item-selector.component.html',
    styleUrls: ['./dropdown-item-selector.component.scss']
})
export class DropdownItemSelectorComponent implements OnInit {
    filteredOptions$: Observable<IOption[]>;
    @Input() options: IOption[];
    @Input() defaultOption: IOption;
    @Input() dropdownItemSelectorFormGroup: FormGroup;
    @Input() required: boolean;
    @Input() dropdownName: string;

    @Input() isInvalid: boolean;
    selected: IOption = {filter: "", id: null};
    get dropdownItem() { return this.dropdownItemSelectorFormGroup.get(this.dropdownName) as FormControl; }

    constructor(private formBuilder: FormBuilder) { }

    ngOnInit() {
        if(this.defaultOption){
            this.options.unshift(this.defaultOption);
        }

        this.filteredOptions$ = combineLatest(this.dropdownItem.valueChanges.pipe(
            startWith('')
        ), of(this.options))
            .pipe(
                map(value => this._filter(value[0], value[1]))
            );
    }

    displayFn(dropdownItem?: IOption): string | undefined {
        return dropdownItem ? dropdownItem.filter : undefined;
    }

    private _filter(value: string, options: IOption[]): IOption[] {
        if (typeof value === "string" && value && value != "" && options) {
            const filterValue = value.toLowerCase().trim();
            return options.filter(x => x.filter.toLowerCase().trim().includes(filterValue));
        }

        return options;
    }

    focusOnAutocomplete() {
        this.setSelectedDropdownItem();
        if (this.dropdownItem){
            this.dropdownItem.setValue(null);
        }
    }

    private setSelectedDropdownItem() {
        if (this.dropdownItem && this.dropdownItem.value && this.selected !== this.dropdownItem.value) {
            this.selected = this.dropdownItem.value;
        }
    }

    focusOffAutocomplete() {
        this.setSelectedDropdownItem();
        this.dropdownItem.setValue(this.selected);
    }

    private _isObject(target): boolean {
        return typeof target === 'object' && target != null;
    }
}
