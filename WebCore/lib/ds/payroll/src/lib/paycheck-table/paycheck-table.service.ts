import { Injectable, OnDestroy } from '@angular/core';
import { IPayrollPayCheckList } from '..';
import { BehaviorSubject, Subject, combineLatest } from 'rxjs';
import { map, tap, takeUntil } from 'rxjs/operators';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortable, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

type ListToValueFunc<A, T> = (payrollPayCheckList: Array<A> | null) => T;

class ListToDerivedAndDefaultValue<A, T> {
    constructor(public listToDerivedValueFunc: ListToValueFunc<A, T>,
                public listToDefaultValueFunc: ListToValueFunc<A, T>) {}
}

/**
 * Singleton Service, intended for the parent context (component wishing to display a PaycheckTableComponent)
 * This service is injected into both of these components.
 *
 * Currently, this setup only allows for one instance of PaycheckTableComponent/PaycheckTableService at a time.
 */
@Injectable()
export class PaycheckTableService implements OnDestroy {

    private _destroy$ = new Subject();

    // Parent Context: the component using the PaycheckTable.
    // This service is injected into both of these components.

    // Generally should be readonly in the parent context.
    emptyStateMessage$ = new BehaviorSubject('There is no paycheck history to display.');
    displaySummaryFooter$ = new BehaviorSubject(false);
    showVoidChecksButton$ = new BehaviorSubject<boolean>(null);
    displayedColumns$ = new BehaviorSubject<string[]>(null);

    // Computed from payrollPayCheckList$
    totalGrossPay$ = new BehaviorSubject(0);
    totalNetPay$ = new BehaviorSubject(0);
    totalCheckAmount$ = new BehaviorSubject(0);
    // areAnyChecksToVoid$ = new BehaviorSubject(false);

    // Computed from theWholeEnchilada$
    totalPaychecks$ = new BehaviorSubject(0);
    resultingPaychecks$ = new BehaviorSubject(0);

    // Parent context will "push" onto these streams.
    filterValue$ = new BehaviorSubject('');
    doDisplayVendors$ = new BehaviorSubject(false);
    payrollPayCheckList$ = new BehaviorSubject<IPayrollPayCheckList[]>(null);

    // DataSource for the Table. Computed from the theWholeEnchilada$.
    private _payrollPayCheckMatList = new MatTableDataSource<IPayrollPayCheckList>(null);
    payrollPayCheckMatList$ = new BehaviorSubject<MatTableDataSource<IPayrollPayCheckList>>(this._payrollPayCheckMatList);

    // That good stuff. Everything from the parent context necessary to rebuild a fresh payrollPayCheckMatList.
    theWholeEnchilada$  = combineLatest(this.filterValue$,
                                        this.doDisplayVendors$,
                                        this.payrollPayCheckList$);

    sort$ = new BehaviorSubject<MatSort>(null);
    paginator$ = new BehaviorSubject<MatPaginator>(null);

    // Default value for most parent contexts: {id: 'checkDate', start: 'desc', disableClear: false} as MatSortable;
    initialSortState$ = new BehaviorSubject<MatSortable>(null);
    private _hasInitialSortBeenSet: boolean;

    sortPaginatorMatList$ = combineLatest(this.payrollPayCheckMatList$,
                                          this.sort$,
                                          this.paginator$,
                                          this.initialSortState$);

    private yieldZero = () => 0;
    private yieldFalse = () => false;

    // Map the Subjects to the functions needed to compute their next value.
    // tslint:disable-next-line: member-ordering
    private readonly _computedSummaryFooterSubjectsMap:
        // Map<Subject<number | boolean>, ListToDerivedAndDefaultValue<IPayrollPayCheckList, number | boolean>> = new Map([
        Map<Subject<number>, ListToDerivedAndDefaultValue<IPayrollPayCheckList, number>> = new Map([
            [this.totalGrossPay$,       new ListToDerivedAndDefaultValue(this.getTotalGrossPay, this.yieldZero)],
            [this.totalNetPay$,         new ListToDerivedAndDefaultValue(this.getTotalNetPay, this.yieldZero)],
            [this.totalCheckAmount$,    new ListToDerivedAndDefaultValue(this.getTotalCheckAmount, this.yieldZero)],

            // Ignore the typescript error here. It is being dumb, this is valid.
            // [this.areAnyChecksToVoid$,  new ListToDerivedAndDefaultValue(this.getAreAnyChecksToVoid, this.yieldFalse)],
    ]);

    constructor() {
        // theWholeEnchilada$ subscription pushes onto payrollPayCheckMatList$
        this.sortPaginatorMatList$.pipe(
            takeUntil(this._destroy$),
        ).subscribe(([payrollPayCheckMatList, sort, paginator, initialSortState]) => {
            if (payrollPayCheckMatList) {

                // We only want to do this once, on "initial load".
                if (sort && paginator && initialSortState && !this._hasInitialSortBeenSet) {
                    this.setSort(payrollPayCheckMatList, initialSortState);
                    this._hasInitialSortBeenSet = true;
                }

                payrollPayCheckMatList.paginator = paginator;
                payrollPayCheckMatList.sort = sort;
            }
        });

        this.theWholeEnchilada$.pipe(
            takeUntil(this._destroy$),
        ).subscribe(([filterValue, doDisplayVendors, payrollPayCheckList]) => {
            let maybePaycheckList: IPayrollPayCheckList[] | null;

            const isArray = Array.isArray(payrollPayCheckList);

            if (isArray) {
                const payrollPayCheckMatList = this._payrollPayCheckMatList;
                // Order of these matters. Set the data (pre-filtered on non-string filtering criteria).
                // Then filter that set further with the MatTableDataSource.filter string.
                const filtered = payrollPayCheckList.filter(item => doDisplayVendors || !item.isVendor);
                payrollPayCheckMatList.data = filtered;

                // const payrollPayCheckMatList = new MatTableDataSource<IPayrollPayCheckList>(payrollPayCheckList);
                // const payrollPayCheckMatList = new MatTableDataSource<IPayrollPayCheckList>(filtered);

                const filter = filterValue.trim().toLowerCase();
                payrollPayCheckMatList.filter = filter;

                // payrollPayCheckMatList.data = filtered;

                this.payrollPayCheckMatList$.next(payrollPayCheckMatList);

                // this.areAnyChecksToVoid$.next(this.getAreAnyChecksToVoid(filtered));

                // I'm guessing that we want to limit the denominator to only include
                // the max number of checks that could match a search result.
                // This will make it so that it matches the result of filtering on doDisplayVendors.
                this.totalPaychecks$.next(filtered.length);

                // The number of paychecks with a name property containing the substring filterValue.
                const doubleFiltered = filtered.filter(x => x.name.trim().toLowerCase().includes(filterValue));
                this.resultingPaychecks$.next(doubleFiltered.length);
                // this.resultingPaychecks$.next(payrollPayCheckMatList.filteredData.length);
                // this.resultingPaychecks$.next(payrollPayCheckMatList._filterData.length);

                // If we wanted to update the summaryFooterSubjects, to reflect only the data currently displayed in the table
                // (data left after the various filtering we just did), uncomment the next line.
                // maybePaycheckList = filtered;
                // maybePaycheckList = doubleFiltered;
                // maybePaycheckList = payrollPayCheckMatList.filteredData;
                // maybePaycheckList = payrollPayCheckMatList._filterData(filtered);

                // If we want summaryFooterSubjects to always reflect the unfiltered data, uncomment the next line.
                maybePaycheckList = payrollPayCheckList;
            } else {
                maybePaycheckList = payrollPayCheckList;

                this.totalPaychecks$.next(0);
                this.resultingPaychecks$.next(0);
            }

            this.setNextOnComputedSummaryFooterSubjects(this._computedSummaryFooterSubjectsMap, maybePaycheckList, isArray);
        });
    }

  ngOnDestroy() {
      this._destroy$.next();
  }

  // A workaround for a mat-table/mat-sort issue.
  // Makes the sort arrow shows as intended, after programmatically changing the sort.
  // https://github.com/angular/components/issues/10242#issuecomment-470726829
  setSort(dataSource: MatTableDataSource<IPayrollPayCheckList>, matSortable: MatSortable) {
      matSortable.start = matSortable.start || 'asc';
      const matSort = dataSource.sort;
      const toState = 'active';
      const disableClear = false;

      // reset state so that start is the first sort direction that you will see
      matSort.sort({ id: null, start: matSortable.start, disableClear });
      matSort.sort(matSortable);

      // ugly hack
      (matSort.sortables.get(matSortable.id) as MatSortHeader)._setAnimationTransitionState({ toState });
  }

  private setNextOnComputedSummaryFooterSubjects<A, T>(
      someMap: Map<Subject<T>, ListToDerivedAndDefaultValue<A, T>>,
      maybePaycheckList: Array<A> | null,
      isArray: boolean
  ) {
      const isArrayFunc = (a: Array<A> | null) => isArray;
      // IterableIterator
      const keysIterator = someMap.keys();
      // Init this for the first pass through the loop.
      let currentSubject = keysIterator.next();

      // Iterate over the subjects until there are none left to process.
      // Get listToValueFunc corresponding to the currentSubject, and use it to compute the next value for the currentSubject.
      // Then set the next value for the currentSubject accordingly.
      while (!currentSubject.done) {
          const subject = currentSubject.value;
          const value = someMap.get(subject);
          const listToDerivedValueFunc = value.listToDerivedValueFunc;
          const listToDefaultValueFunc = value.listToDefaultValueFunc;
          const nextValue = this.isArrayComputedOrDefaultValue(maybePaycheckList, listToDerivedValueFunc,
                                                               listToDefaultValueFunc, isArrayFunc);
          subject.next(nextValue);
          currentSubject = keysIterator.next();
      }
  }

  private isArrayComputedOrDefaultValue<A, T>(
      a: Array<A> | null,
      isArrayFunc: ListToValueFunc<A, T>,
      isNotArrayFunc: ListToValueFunc<A, T>,
      isArray = (aa: Array<A> | null) => Array.isArray(aa)
  ): T {
      return isArray(a) ? isArrayFunc(a) : isNotArrayFunc(a);
  }

  private getTotalGrossPay(payrollPayCheckList: IPayrollPayCheckList[]): number {
      return payrollPayCheckList.map(c => c.grossPay).reduce((acc, value) => acc + value, 0);
  }

  private getTotalNetPay(payrollPayCheckList: IPayrollPayCheckList[]): number {
      return payrollPayCheckList.map(c => c.netPay).reduce((acc, value) => acc + value, 0);
  }

  private getTotalCheckAmount(payrollPayCheckList: IPayrollPayCheckList[]): number {
      return payrollPayCheckList.map(c => c.checkAmount).reduce((acc, value) => acc + value, 0);
  }

  // private getAreAnyChecksToVoid = (data: IPayrollPayCheckList[]) => this.getChecksToVoid(data).length > 0;

  getChecksToVoid(data: IPayrollPayCheckList[]): IPayrollPayCheckList[] {
      return this.isArrayComputedOrDefaultValue(data, this.getChecksToVoidIsArrayFunc, this.getChecksToVoidIsNotArrayFunc);
  }

  private getChecksToVoidIsArrayFunc = (x: IPayrollPayCheckList[]) => x.filter(y => y.voidCheck);
  private getChecksToVoidIsNotArrayFunc = () => [] as Array<IPayrollPayCheckList>;
}
