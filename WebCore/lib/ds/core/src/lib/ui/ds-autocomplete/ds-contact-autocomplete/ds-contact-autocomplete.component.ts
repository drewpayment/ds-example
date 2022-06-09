import {
  Component,
  OnInit,
  Input,
  ViewEncapsulation,
  ElementRef,
  ViewChildren,
  QueryList,
  forwardRef,
  OnDestroy,
  Optional,
  Self,
  HostBinding,
  AfterViewInit,
  ViewChild,
  Output,
  EventEmitter,
} from "@angular/core";
import { IContact } from "@ds/core/contacts";
import { coerceBooleanProperty } from "@angular/cdk/coercion";
import { FormControl, NgControl, ControlValueAccessor } from "@angular/forms";
import { Observable, Subject } from "rxjs";
import * as _ from "lodash";
import { FocusMonitor } from "@angular/cdk/a11y";
import { checkboxComponent } from "@ajs/applicantTracking/application/inputComponents";
import { MatFormFieldControl } from "@angular/material/form-field";
import { ErrorStateMatcher, MatOptionSelectionChange } from "@angular/material/core";
import { MatInput } from "@angular/material/input";
import {
  MatAutocompleteTrigger,
  MatAutocomplete,
  MatAutocompleteSelectedEvent,
} from "@angular/material/autocomplete";
import { MatChipList, MatChip, MatChipEvent } from "@angular/material/chips";
import { debounceTime, distinctUntilChanged, startWith } from "rxjs/operators";

@Component({
  selector: "ds-contact-autocomplete",
  exportAs: "dsContactAutocomplete",
  templateUrl: "./ds-contact-autocomplete.component.html",
  styleUrls: ["./ds-contact-autocomplete.component.scss"],
  encapsulation: ViewEncapsulation.None,
  host: {
    "[class.single-select]": "!multiple",
    "[class.contact]": "multiple",
  },
  providers: [
    {
      provide: MatFormFieldControl,
      useExisting: forwardRef(() => DsContactAutocompleteComponent),
    },
  ],
})
export class DsContactAutocompleteComponent
  implements
    OnInit,
    MatFormFieldControl<IContact | IContact[]>,
    OnDestroy,
    ControlValueAccessor,
    AfterViewInit
{
  static nextId = 0;

  @Input()
  set contacts(value: IContact[]) {
    this._contacts = value;
    this.filteredContacts$.next(this.filterContacts(this.contactCtrl.value));
    if (this.waitingToOpen) {
      this.trigger.autocompleteDisabled = false;
      this.trigger.openPanel();
    } else if (this.trigger.panelOpen) {
      this.trigger.closePanel();
    }
    this.stateChanges.next();
  }
  get contacts() {
    return this._contacts;
  }

  private _filterLocalContacts = true;
  @Input()
  set filterLocalContacts(value: boolean) {
    this._filterLocalContacts = coerceBooleanProperty(value);
  }
  get filterLocalContacts(): boolean {
    return this._filterLocalContacts;
  }

  waitingToOpen = false;

  private _displayUserType: boolean;
  @Input()
  public get displayUserType(): boolean {
    return this._displayUserType;
  }
  public set displayUserType(value: boolean) {
    this._displayUserType = coerceBooleanProperty(value);
  }
  private _contacts: IContact[];
  filteredContacts$: Subject<IContact[]> = new Subject<IContact[]>();
  filteredContacts: Observable<IContact[]>;
  selectedContacts: IContact[] = [];

  // CHIP LIST
  @Input("aria-orientation")
  ariaOrientation: "horizontal" | "vertical";

  @Input()
  compareWith: (o1: any, o2: any) => boolean;

  @Input()
  errorStateMatcher: ErrorStateMatcher;

  @Input()
  set multiple(value: boolean) {
    this._multiple = coerceBooleanProperty(value);
  }
  get multiple(): boolean {
    return this._multiple;
  }
  private _multiple = false;

  @ViewChild(MatChipList, { static: true })
  chipList: MatChipList;

  @ViewChildren(MatChip)
  chips: QueryList<MatChip>;

  // INPUT
  @Input("inputControl") contactCtrl = new FormControl();

  @ViewChild("contactInput", { static: true })
  contactInput: ElementRef<MatInput>;

  @ViewChild(MatAutocompleteTrigger, { static: true })
  trigger: MatAutocompleteTrigger;

  // AUTOCOMPLETE

  @Input()
  get isOpen(): boolean {
    return this._isOpened;
  }
  set isOpen(value: boolean) {
    this._isOpened = coerceBooleanProperty(value);
  }
  private _isOpened: boolean;

  @Input()
  get autoActiveFirstOption(): boolean {
    return this._autoActiveFirstOption;
  }
  set autoActiveFirstOption(value: boolean) {
    this._autoActiveFirstOption = coerceBooleanProperty(value);
  }
  private _autoActiveFirstOption: boolean = true;

  @Input("class") classList: string;
  @Input("formControlClass") formControlClass = "";

  @Input()
  disableRipple: boolean;

  @Input()
  displayWith: ((value: any) => string) | null;

  @Input()
  panelWidth: string | number;

  @ViewChild(MatAutocomplete, { static: true })
  autocomplete: MatAutocomplete;

  /** MAT FORM FIELD CONTROL IMPLEMENTATIONS */
  stateChanges = new Subject<void>();
  placeholder: string;
  focused = false;
  id = `ds-contact-autocomplete-${DsContactAutocompleteComponent.nextId++}`;
  get empty() {
    return !this.selectedContacts || !this.selectedContacts.length;
  }
  get shouldLabelFloat() {
    return this.focused || !this.empty;
  }

  /** determines if the input is required based on user input */
  @Input()
  get required(): boolean {
    return this._required;
  }
  set required(value: boolean) {
    this._required = coerceBooleanProperty(value);
    this.stateChanges.next();
  }
  protected _required: boolean = false;

  @Input()
  get includeSelectAll() {
    return this._includeSelectAll;
  }
  set includeSelectAll(value: boolean) {
    this._includeSelectAll = coerceBooleanProperty(value);
    this.stateChanges.next();
  }
  private _includeSelectAll = false;

  /** if the input is required, this is the error the will be shown when invalid */
  @Input() errorFeedback: string;

  @Input()
  get formSubmitted(): boolean {
    return this._formSubmitted;
  }
  set formSubmitted(value: boolean) {
    this._formSubmitted = coerceBooleanProperty(value);
  }
  private _formSubmitted: boolean = false;

  @Input()
  set displayEmployeeNumber(value: boolean) {
    this._displayEmployeeNumber = coerceBooleanProperty(value);
  }
  get displayEmployeeNumber(): boolean {
    return this._displayEmployeeNumber;
  }
  private _displayEmployeeNumber = false;
  disabled: boolean;
  errorState: boolean;
  controlType?: string = "ds-contact-autocomplete";
  autofilled?: boolean;

  @HostBinding("attr.aria-describedby") describedBy = "";

  /** Current IContact object that should be selected on the form */

  @Input()
  get value(): any {
    const hasContacts = this.selectedContacts && this.selectedContacts.length;
    if (!hasContacts) return null;

    return this.multiple ? this.selectedContacts : this.selectedContacts[0];
  }
  set value(v: any) {
    const contacts = !v ? [] : _.isArray(v) ? v : [v];
    this.selectedContacts = contacts;
    this.filteredContacts$.next(this.filterContacts(this.contactCtrl.value));
    this.stateChanges.next();
  }

  private _firstFocus = true;

  /** NG_VALUE_ACCESSOR IMPLEMENTATIONS (used for template-drive form functionality) */
  private onTouchedCallback: () => void = () => {};
  private onChangeCallback: (_: any) => void = () => {};
  @Output() change = new EventEmitter();
  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(" ");
  }
  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() != "input") {
      if (this.contactInput && this.contactInput.nativeElement)
        this.contactInput.nativeElement.focus();
    }
  }

  constructor(
    fm: FocusMonitor,
    elRef: ElementRef<HTMLElement>,
    @Optional() @Self() public ngControl: NgControl
  ) {
    // handle focus/touch
    fm.monitor(elRef, true).subscribe((origin) => {
      if (this.focused && !origin && !this.trigger.autocomplete.isOpen) {
        this.onTouchedCallback();
      }
      this.focused = !!origin;

      if (this._firstFocus && this.focused) {
        this._firstFocus = false;
        this._resetInput();
      }

      this.stateChanges.next();
    });

    // map internal component interaction back onto any
    // registered [formControl] or [(ngModel)] on the component itself
    if (this.ngControl != null) {
      this.ngControl.valueAccessor = this;
    }
  }

  ngOnInit() {
    this.filteredContacts = this.filteredContacts$.asObservable();

    /** make sure our function inputs are functions before we try to pass them to their respective components */
    if (_.isFunction(this.compareWith))
      this.chipList.compareWith = this.compareWith;

    if (_.isFunction(this.displayWith))
      this.autocomplete.displayWith = this.displayWith;

    /** we watch the autocomplete's formcontrol for changes and filter the list of contacts on changes */
    this.autocomplete.closed.subscribe((_) => {
      if (!this.focused) {
        this.onTouchedCallback();
      }
    });
  }

  ngAfterViewInit(): void {
    this.contactCtrl.valueChanges
      .pipe(debounceTime(250))
      .subscribe((searchText) => {
        if (this.filterLocalContacts) {
          this.filteredContacts$.next(this.filterContacts(searchText));
        } else if (searchText !== null) {
          this.trigger.closePanel();
          this.trigger.autocompleteDisabled = true;
          this.waitingToOpen = true;
        }
      });
  }

  /** prevent memory leak by completing our subscription to statechanges events */
  ngOnDestroy() {
    this.stateChanges.complete();
  }

  /** used on single mode and has the ability to "remove all" mat-chips from one method. */
  removeChips() {
    this.chips.forEach((chip) => {
      chip.remove();
    });
  }

  /** Filter to handle keyword input on the contact list */
  private filterContacts(value: string): IContact[] {
    if (this._contacts == null) return [];
    const filtered = this._contacts.filter((c) => {
      let existingContact = false;
      if (this.selectedContacts != null)
        existingContact = !this.selectedContacts.every(
          (s) => s.userId != c.userId || c.employeeId != s.employeeId
        );

      let textMatch = true;
      if (typeof value === "string" && value) {
        const lowerValue = value.toLowerCase();
        textMatch = `${c.firstName} ${c.lastName}`.toLowerCase().indexOf(lowerValue) > -1;
        if (!textMatch)
          textMatch = `${c.employeeNumber}`.indexOf(value) > -1
      }
      
      return !existingContact && textMatch;
    });

    return filtered;
  }

  /**
   * Method to handle removing a single mat-chip when the multi-selection template is being used.
   */
  removeChip(chip: any) {
    // const idx = _.findIndex(this.selectedContacts, { 'userId': event.chip.value });
    // this.selectedContacts.splice(idx, 1);
    const idx = this.selectedContacts.indexOf(chip);
    if (idx >= 0) {
      this.selectedContacts.splice(idx, 1);
      this._handleChange();
    }
  }

  autocompleteSelected(event: MatAutocompleteSelectedEvent) {
    let selectedContact: IContact;

    if (Number.isInteger(event.option.value)) {
      selectedContact = <IContact>{
        employeeId: event.option.value,
        firstName: "All",
        lastName: "Selected",
      };
    } else {
      selectedContact = event.option.value as IContact;
    }

    // if control is being used for single contact selection, we make sure we aren't pushing more than
    // one to the array at a time
    if (!this.multiple && this.selectedContacts.length > 0)
      this.selectedContacts = [];

    this.selectedContacts.push(selectedContact);

    // bubble the event to the host, but only send single object if they're using single contact selection
    this._handleChange();
  }

  private _resetInput() {
    // clear the input so the autocomplete keeps working like it should
    this.contactInput.nativeElement.value = "";
    this.contactCtrl.setValue(null);
    this.contactCtrl.reset();
  }

  // From ControlValueAccessor interface
  writeValue(value: IContact | IContact[]) {
    this.value = value;
  }

  // From ControlValueAccessor interface
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  // From ControlValueAccessor interface
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  _handleChange() {
    this.onChangeCallback(
      this.multiple && this.value
        ? (<IContact[]>this.value).map((c) => c)
        : this.value
    );
    this.change.emit(this.value);
    this._resetInput();

    if (!this.filterLocalContacts) {
      setTimeout(() => {
        this.selectedContacts = [];
        (this.contactInput.nativeElement as any).blur();
      }, 1500);
    }

  }

  selectOption(e) {
    if (this.multiple) {
      e.stopPropagation();
      this._handleChange();
      this.trigger.openPanel();
    }

    this.waitingToOpen = false;
  }
}
