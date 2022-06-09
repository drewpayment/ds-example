import { COMMA, ENTER } from "@angular/cdk/keycodes";
import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { MatAutocompleteSelectedEvent, MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { ClientEarning } from "@ds/core/employee-services/models/client-earning.model";
import { BehaviorSubject, combineLatest, Subject } from "rxjs";
import {
  debounceTime,
  distinctUntilChanged,
  filter,
  map,
  startWith,
  take,
  takeUntil,
} from "rxjs/operators";
import { ClientAccrualsStoreService } from "../../../../../client-management/services/client-accruals-store.service";
import { ClientAccrualsService } from "../../../../../client-management/services/client-accruals.service";

@Component({
  selector: "ds-client-accruals-earnings-card",
  templateUrl: "./client-accruals-earnings-card.component.html",
  styleUrls: ["./client-accruals-earnings-card.component.scss"],
})
export class ClientAccrualsEarningsCardComponent implements OnInit, OnDestroy {
  private readonly _destroy$ = new Subject();
  form: FormGroup = this.store.form;
  isLoading = true;
  addOnBlur = true;
  private _allEarnings: ClientEarning[] = [];
  filteredEarnings$ = new BehaviorSubject<ClientEarning[]>([]);

  get clientEarningsControl(): FormControl {
    return this.form.get("accrual.clientEarnings") as FormControl;
  }

  private get searchEarningsControl(): FormControl {
    return this.form.get("accrual.searchEarningsCtrl") as FormControl;
  }
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  @ViewChild('searchInput', { static: false })
  searchInput: ElementRef<HTMLInputElement>;

  constructor(
    private _clientAccrualsSvc: ClientAccrualsService,
    private store: ClientAccrualsStoreService
  ) {}

  ngOnInit() {
    // Fetch ClientEarnings from API.
    this._clientAccrualsSvc.getTimeOffPoliciesPageViewDependencies$
      .pipe(take(1))
      .subscribe((data) => {
        this._allEarnings = data.clientEarnings;
        this.filteredEarnings$.next(this._allEarnings);
        this.isLoading = false;
      });

    // Auto-complete ClientEarnings from user input.
    this.searchEarningsControl.valueChanges
      .pipe(
        startWith(""),
        debounceTime(250),
        filter((value) => value != null),
        takeUntil(this._destroy$),
      )
      .subscribe((value) => {
        value = value != null ? value.trim().toLowerCase() : "";

        const filtered = this.filteredEarnings$.getValue();

        const searchResult = filtered.filter((e) =>
          e.description
            .trim()
            .replace(/\s/g, "")
            .toLowerCase()
            .includes(value)
        );
        this.filteredEarnings$.next(searchResult);
      });

    // Using this as a signal that the form reset:
    // - has been triggered (emits true)
    // - and has completed (emits false).
    const emitWhenTrueToFalse_isResetForm$ = this.store.isResetForm$
      .pipe(
        startWith(true),
        distinctUntilChanged(),
        filter(isResetForm => !isResetForm)
      );

    const distinctUntilChanged_clientEarningsValueChanges$ = this.clientEarningsControl
      .valueChanges
      .pipe(
        startWith([] as ClientEarning[]),
        map(earnings => (earnings || []) as ClientEarning[]),
        distinctUntilChanged((a, b) => {
          let result = (a.length === b.length);
          // If they're different lengths, there's changes, so return.
          if (!result) return result;

          // They're the same length.
          // Now, verify each earning in `a` has a matching earning in `b`.
          a.forEach(ae => {
            result = result
              && b.some(be => be.clientEarningId === ae.clientEarningId);
          });

          return result;
        })
      );

    // Reset search input upon whole form reset.
    emitWhenTrueToFalse_isResetForm$.pipe(
      takeUntil(this._destroy$),
    ).subscribe(_ => {
        this.searchEarningsControl.reset();
        if (this.searchInput && this.searchInput.nativeElement) {
          this.searchInput.nativeElement.value = '';
        }
    });

    // Update filtered earnings to "allEarnings except selectedEarnings"
    combineLatest(
      distinctUntilChanged_clientEarningsValueChanges$,
      emitWhenTrueToFalse_isResetForm$
    ).pipe(
      takeUntil(this._destroy$)
    ).subscribe(([selectedEarnings, isResetForm]) => {
      const filtered = this._allEarnings.filter((e) => {
        const index = selectedEarnings.findIndex((se) => {
          return se.clientEarningId == e.clientEarningId;
        });
        return index < 0;
      });
      this.filteredEarnings$.next(filtered);
    });
  }

  ngOnDestroy() {
    this._destroy$.next();
  }

  selected(event: MatAutocompleteSelectedEvent) {
    const selected = this._allEarnings.find(
      (e) => e.clientEarningId == event.option.value
    );

    if (selected == null) return;

    const currentEarnings =
      (this.clientEarningsControl.value || []) as ClientEarning[];
    const selectedEarnings = [...currentEarnings, selected];

    this.clientEarningsControl.setValue(selectedEarnings);
    this.clientEarningsControl.markAsTouched();
    this.clientEarningsControl.markAsDirty();

    // clear out search text
    this.searchEarningsControl.reset();
    this.searchInput.nativeElement.value = '';
  }

  remove(earning: ClientEarning) {
    // Spread into new array, so that the splice doesn't mutate the actual form value.
    // Otherwise, it causes multiple/duplicate clientEarningsControl.valueChanges emissions,
    // which affects distinctUntilChanged_clientEarningsValueChanges$.
    const currentEarnings = [...this.clientEarningsControl.value] as ClientEarning[];

    const index = currentEarnings.findIndex(
      (e) => e.clientEarningId == earning.clientEarningId
    );

    if (index > -1) {
      currentEarnings.splice(index, 1);

      this.clientEarningsControl.setValue(currentEarnings);
      this.clientEarningsControl.markAsTouched();
      this.clientEarningsControl.markAsDirty();
    }
  }
  selectOption(e: Event, trigger: MatAutocompleteTrigger) {
    e.stopPropagation();
    trigger.openPanel();
  }

}
