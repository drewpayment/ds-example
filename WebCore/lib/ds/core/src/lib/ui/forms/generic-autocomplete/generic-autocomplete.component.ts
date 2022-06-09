import { Component, OnInit, forwardRef, Input, AfterViewInit, ViewChild } from "@angular/core";
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  FormControl,
} from "@angular/forms";
import { AutocompleteItem } from "@ds/core/groups/shared/autocomplete-item.model";
import { startWith, map } from "rxjs/operators";
import { Maybe } from "@ds/core/shared/Maybe";
import { MatAutocompleteSelectedEvent, MatAutocompleteTrigger } from "@angular/material/autocomplete";
import { Observable } from "rxjs";

@Component({
  selector: "ds-generic-autocomplete",
  templateUrl: "./generic-autocomplete.component.html",
  styleUrls: ["./generic-autocomplete.component.scss"],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => GenericAutocompleteComponent),
      multi: true,
    },
  ],
})
export class GenericAutocompleteComponent
  implements OnInit, ControlValueAccessor
{
  control = new FormControl();
  selectedItems: AutocompleteItem[] = [];
  availableItems$: Observable<AutocompleteItem[]>;

  private _isDisabled: boolean;

  @Input() value: FormControl = new FormControl();
  @Input() mapper: (item: any) => AutocompleteItem;

  private _items: AutocompleteItem[];
  @Input()
  public get items(): AutocompleteItem[] {
    return this._items;
  }
  public set items(value: AutocompleteItem[]) {
    this._items = value;
  }

  private _customSort: (items: AutocompleteItem[]) => AutocompleteItem[];
  @Input()
  public get customSort(): (items: AutocompleteItem[]) => AutocompleteItem[] {
    return this._customSort;
  }
  public set customSort(
    value: (items: AutocompleteItem[]) => AutocompleteItem[]
  ) {
    this._customSort = value;
  }

  onChange: any = () => {};
  onTouched: any = () => {};

  @ViewChild(MatAutocompleteTrigger, { static: true })
  trigger: MatAutocompleteTrigger;

  writeValue(obj: any): void {
    this.value.setValue(obj);
    const keys = Object.keys(obj == null ? {} : obj);
    keys.forEach((key) => {
      const index = +key;
      if (!isNaN(index)) {
        this.moveList(obj[index], this.items, this.selectedItems);
      }
    });
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;

    let preselectedItems = this.value.value as any[];
    if (preselectedItems && preselectedItems.length && this.mapper) {
      preselectedItems.forEach(item => {
        const acItem = this.mapper(item);
        const existingIndex = this.selectedItems.findIndex(x => x.value == acItem.value);
        if (existingIndex < 0) this.selectedItems.push(acItem);
      });
    }

    this.value.valueChanges.subscribe(fn);
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    isDisabled ? this.value.disable() : this.value.enable();
  }

  constructor() {}

  ngOnInit() {
    this.availableItems$ = this.control.valueChanges.pipe(
      startWith(""),
      map((value) => new Maybe(value).map((x) => x.display).valueOr(value)),
      map((value) => this.defaultFilter(value)),
      map((filtered) =>
        this.customSort
          ? this.customSort(filtered)
          : this.defaultSortFn(filtered)
      )
    );
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.moveList(event.option.value.value, this.items, this.selectedItems);
    this.value.setValue(this.getIds(this.selectedItems));
    this.control.setValue(null);
    this.trigger.openPanel();
  }

  reopenPanel(e): void {
    e.stopPropagation();
    this.trigger.openPanel();
  }

  remove(item: AutocompleteItem): void {
    this.moveList(item.value, this.selectedItems, this.items);
    this.value.setValue(this.getIds(this.selectedItems));
    this.control.setValue(this.control.value); // rerun filter to show the available items
  }

  private getIds(list: AutocompleteItem[]): any[] {
    return list.map((x) => x.value);
  }

  private defaultSortFn(items: AutocompleteItem[]): AutocompleteItem[] {
    return items.sort((a, b) =>
      NormalizeDisplay(a.display).localeCompare(NormalizeDisplay(b.display))
    );
  }

  private defaultFilter(userInput: string): AutocompleteItem[] {
    const normalizedInput = NormalizeDisplay(userInput);
    return this.items
    .filter(item => this.selectedItems.findIndex(x => x.value === item.value) < 0)
    .filter((item) =>
      NormalizeDisplay(item.display).includes(normalizedInput)
    );
  }

  moveList(
    id: number,
    source: AutocompleteItem[],
    target: AutocompleteItem[]
  ): AutocompleteItem[] {
    const index = source.findIndex((value) => value.value === id);
    if (index >= 0) {
      const targetIndex = target.findIndex(x => x.value === id);
      if (targetIndex < 0)
        target.push(source.splice(index, 1)[0]);
      else
        source.splice(index, 1);
    }
    return target;
  }
}

export type DataManagementServiceFactory =
  () => AutocompleteDataManagementServiceBase;

export function NormalizeDisplay(val: string) {
  return new Maybe(val)
    .map((x) => x.toLowerCase().replace(/\s/g, ""))
    .valueOr("");
}

export interface AutocompleteDataManagementServiceBase {
  SortItems: (
    items: AutocompleteItem[]
  ) => AutocompleteDataManagementServiceBase;
  FilterUsingInput: (
    userInput: string,
    items: AutocompleteItem[]
  ) => AutocompleteDataManagementServiceBase;
  SetSelected: (
    autoCompleteValue: any
  ) => AutocompleteDataManagementServiceBase;
  SetRemoved: (autoCompleteValue: any) => AutocompleteDataManagementServiceBase;
  GetItems: () => AutocompleteItem[];
  SetItems: (
    items: AutocompleteItem[]
  ) => AutocompleteDataManagementServiceBase;
}

export interface AutocompleteDataManagementService<T extends AutocompleteItem>
  extends AutocompleteDataManagementServiceBase {
  SetItems: (items: T[]) => AutocompleteDataManagementService<T>;
}

export class DefaultAutoCompleteDataManagementStrategy
  implements AutocompleteDataManagementServiceBase
{
  private _selectedItems: AutocompleteItem[];
  public get selectedItems(): AutocompleteItem[] {
    return this._selectedItems ? this._selectedItems : [];
  }
  public set selectedItems(value: AutocompleteItem[]) {
    this._selectedItems = value;
  }

  private _notSelectedItems: AutocompleteItem[];
  public get notSelectedItems(): AutocompleteItem[] {
    return this._notSelectedItems ? this._notSelectedItems : [];
  }
  public set notSelectedItems(value: AutocompleteItem[]) {
    this._notSelectedItems = value;
  }

  SortItems(): AutocompleteDataManagementServiceBase {
    this.notSelectedItems.sort((a, b) =>
      NormalizeDisplay(a.display).localeCompare(NormalizeDisplay(b.display))
    );
    return this;
  }

  FilterUsingInput(userInput: string): AutocompleteDataManagementServiceBase {
    const normalizedInput = NormalizeDisplay(userInput);

    this.notSelectedItems.filter((item) =>
      NormalizeDisplay(item.display).includes(normalizedInput)
    );
    return this;
  }

  SetSelected(autoCompleteValue: any): AutocompleteDataManagementServiceBase {
    const index = this.notSelectedItems.findIndex(
      (value) => value.value === autoCompleteValue
    );
    if (index >= 0) {
      this.selectedItems.push(this.notSelectedItems.splice(index, 1)[0]);
    }
    return this;
  }

  SetRemoved(autoCompleteValue: any): AutocompleteDataManagementServiceBase {
    const index = this.selectedItems.findIndex(
      (value) => value.value === autoCompleteValue
    );
    if (index >= 0) {
      this.notSelectedItems.push(this.selectedItems.splice(index, 1)[0]);
    }
    return this;
  }
  GetItems(): AutocompleteItem[] {
    return this.selectedItems;
  }

  SetItems(items: AutocompleteItem[]): AutocompleteDataManagementServiceBase {
    this.notSelectedItems = items;
    return this;
  }
}
