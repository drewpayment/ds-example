import { FocusMonitor } from '@angular/cdk/a11y';
import { Component, ElementRef, forwardRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { MatChipRemove } from '@angular/material/chips';
import { MatOptionSelectionChange } from '@angular/material/core';
import { MatInput } from '@angular/material/input';
import { AutocompleteItem } from '@ds/core/groups/shared/autocomplete-item.model';
import { debug } from 'lib/utilties';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { debounceTime, filter, startWith, takeUntil } from 'rxjs/operators';



@Component({
  selector: 'ds-type-hint',
  exportAs: 'dsTypeHint',
  templateUrl: './ds-type-hint.component.html',
  styleUrls: ['./ds-type-hint.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DsTypeHintComponent),
      multi: true,
    },
  ],
})
export class DsTypeHintComponent implements OnInit, OnDestroy, ControlValueAccessor {

  //#region Private Vars

  private destroy$ = new Subject();
  private _multiple = true;
  private _allOptions: AutocompleteItem[] = [];
  private _filteredItems$ = new BehaviorSubject<AutocompleteItem[]>([]);

  //#endregion

  selected: AutocompleteItem[] = [];
  filteredItems$: Observable<AutocompleteItem[]> = this._filteredItems$.asObservable();

  @Input('options')
  set providedOptions(value: AutocompleteItem[]) {
    this._allOptions = Array.isArray(value) ? value : [value];
    this._filteredItems$.next(this._allOptions);
  }
  @Input() autoActiveFirstOption: boolean = false;
  @Input() disableRipple: boolean = true;
  @Input() panelWidth: string | number;
  @Input()
  get multiple(): boolean {
    return this._multiple;
  }
  set multiple(value: boolean) {
    this._multiple = !!value;
  }

  onChange = (selected: AutocompleteItem[]) => {};
  onTouched = () => {};
  touched = false;
  disabled = false;


  //#region UI Controls

  @ViewChild('acInput', { static: true }) acInput: ElementRef<MatInput>;
  @ViewChild('acTrigger', { static: true }) trigger: MatAutocompleteTrigger;
  inputControl = new FormControl();

  //#endregion

  constructor(fm: FocusMonitor, elRef: ElementRef<HTMLElement>,) {
    // origin: the type of thing creating focus
    // when clicking the field origin = 'mouse'
    // tabbing away causes origin = null
    // tabbing into causes origin = 'keyboard'
    fm.monitor(elRef, true).subscribe(origin => {
      // user is unfocusing
      if (!origin) {
        this.resetInput();
      } else if (origin === 'program') {
        setTimeout(() => this.trigger.openPanel());
      } else {
        this.onTouched();
      }
    });
  }

  ngOnInit(): void {
    this.trigger.optionSelections
      .pipe(
        takeUntil(this.destroy$),
        filter(options => options.isUserInput),
      )
      .subscribe(options => this.onSelect(options));

    this.inputControl.valueChanges
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(250),
        startWith(''),
        filter(value => typeof value === 'string'),
        debug('Search Value'),
      )
      .subscribe((value: string) => {
        const search = this.prepareSearch(value);

        if (!!search) {
          const filteredItems = this.filterSelections().filter(x => this.prepareSearch(x.display).includes(search));
          this._filteredItems$.next(filteredItems);
        } else {
          this.resetFilteredItems();
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
  }
  writeValue(value: AutocompleteItem[]): void {
    this.selected = value;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onSelect(event: MatOptionSelectionChange) {
    this.markAsTouched();
    if (!this.disabled && event.isUserInput) {
      const item = event.source.value as AutocompleteItem;
      this.selected.push(item);



      if (!this.trigger.panelOpen) this.trigger.openPanel();

      this.resetFilteredItems();
    }
  }

  onRemove(removed: AutocompleteItem) {
    this.markAsTouched();
    if (!this.disabled) {
      this.selected = this.selected.filter(x => x.value !== removed.value);
      this.onChange(this.selected);
      this.resetFilteredItems();
    }
  }

  markAsTouched() {
    if (!this.touched) {
      this.onTouched();
      this.touched = true;
    }
  }

  /**
   * Filters all options by selected options and updates the UI.
   */
  private resetFilteredItems() {
    const filtered = this.filterSelections();
    this._filteredItems$.next(filtered);
  }

  /**
   * Filters all initially available options and removes the currently selected items,
   * and returns a new array with the result.
   *
   * @returns AutcompleteItem[]
   */
  private filterSelections(): AutocompleteItem[] {
    return this._allOptions.filter(o => !this.selected.includes(o));
  }

  private prepareSearch(value: string): string {
    return value.trim().toLowerCase().split(' ').join('');
  }

  private resetInput() {
    this.acInput.nativeElement.value = '';
    this.inputControl.reset();
  }

}
