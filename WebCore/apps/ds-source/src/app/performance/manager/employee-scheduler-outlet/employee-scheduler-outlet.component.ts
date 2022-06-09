import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Observable, merge, BehaviorSubject, Subscription, of, throwError, forkJoin, iif, empty, Subject, defer } from 'rxjs';
import { startWith, switchMap, map, catchError, shareReplay, tap, concat, concatMap, takeUntil } from 'rxjs/operators';
import * as moment from 'moment';
import { IEmployeeSelection } from '@ds/core/employees/shared/employee-selection.model';
import { SortDirection } from '@ds/core/shared/sort-direction.enum';
import { SelectionModel } from '@angular/cdk/collections';
import { MultiReviewEditDialogComponent } from '../multi-review-edit-dialog/multi-review-edit-dialog.component';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { SupervisorOutletComponent } from '../supervisor-outlet/supervisor-outlet.component';
import { CompetencyDialogComponent } from '../competency-dialog/competency-dialog.component';
import { IContact } from '@ds/core/contacts';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Maybe } from '@ds/core/shared/Maybe';
import { IReviewSearchOptions } from '@ds/performance/performance-manager';
import { PerformanceManagerService } from '@ds/performance/performance-manager/performance-manager.service';
import { ICompetencyModel, ICompetency } from '@ds/performance/competencies';
import { ReviewsService } from '@ds/performance/reviews/shared/reviews.service';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { IEmployeeSearchResult, IEmployeeSearchResultResponseData } from '@ds/employees/search/shared/models/employee-search-result';
import { EmployeeSearchFilterType } from '@ds/employees/search/shared/models/employee-search-filter-type';

@Component({
    selector: 'ds-employee-scheduler-outlet',
    templateUrl: './employee-scheduler-outlet.component.html',
    styleUrls: ['./employee-scheduler-outlet.component.scss']
})
export class EmployeeSchedulerOutletComponent implements OnInit, OnDestroy {

    isLoading: boolean;
    private unsubscriber = new Subject();
    searchResultData: IEmployeeSearchResultResponseData;
    dataSource: IEmployeeSearchResult[] = [];
    displayColumns: string[] = ['avatar', 'selected', 'name', 'number', 'division', 'department', 'jobTitle', 'supervisor',
        'hireDate', 'serviceLength', 'payType', 'competencyModel'];
    employeeSelection: IEmployeeSelection = {} as IEmployeeSelection;
    searchOptions: IReviewSearchOptions;
    sortedData = new BehaviorSubject<IEmployeeSearchResult[]>([]);
    pagingLength: number = 0;
    compModels: ICompetencyModel[];
    selectedCompModel: ICompetencyModel;
    matTableWatcherSubscription: Subscription;
    employeeSearchResponseSubscription: Subscription;
    activeReviewSearchOptionSubscription: Subscription;
    private _totalEmps = 0;

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    allowMultiSelect: boolean = true;
    selectedEmployees: IEmployeeSearchResult[] = [];
    selection = new SelectionModel<IEmployeeSearchResult>(this.allowMultiSelect, this.selectedEmployees);
    get hasSelectedReviewTemplate() {
        return this.searchOptions && this.searchOptions.reviewTemplateId;
    }

    isInvalid = false;

    constructor(
        private manager: PerformanceManagerService,
        private dialog: MatDialog,
        private reviewsService: ReviewsService,
        private perfService: PerformanceReviewsService,
        private message: DsMsgService    ) {

    }

    ngOnInit() {
        this.isLoading = true;

        if (this.hasSelectedReviewTemplate == undefined) this.isLoading = false;

        this.manager.employeeSearchResponse$.pipe(
            map(result => new Maybe(result).map(x => x.totalCount).valueOr(0)),
            tap(x => this._totalEmps = x),
            takeUntil(this.unsubscriber)).subscribe(),

        this.selection.changed.subscribe(row => {

            this.isInvalid = false;
            row.removed.forEach(r => {
                const index = this.selectedEmployees.findIndex(se => se.employeeId == r.employeeId);
                if (index < 0) return;
                this.selectedEmployees.splice(index, 1);
            });

            row.added.forEach(r => this.selectedEmployees.push(r));

        });

        this.matTableWatcher();
    }


    ngOnDestroy(): void {
        if (this.matTableWatcherSubscription) {
            this.matTableWatcherSubscription.unsubscribe();
            this.matTableWatcherSubscription = null;
        }

        if (this.employeeSearchResponseSubscription) {
            this.employeeSearchResponseSubscription.unsubscribe();
            this.employeeSearchResponseSubscription = null;
        }

        if (this.activeReviewSearchOptionSubscription) {
            this.activeReviewSearchOptionSubscription.unsubscribe();
            this.activeReviewSearchOptionSubscription = null;
        }

        this.unsubscriber.next();
    }

    getServiceLength(item: IEmployeeSearchResult): string {
        const today = moment();
        let hireDate = moment(item.hireDate);
        let lengthOfYears: number = today.diff(hireDate, 'years');

        if (lengthOfYears < 1) {
            const lengthOfMonths = today.diff(hireDate, 'months');

            if (lengthOfMonths < 1)
                return today.diff(hireDate, 'days').toString() + ' days';
            else
                return lengthOfMonths.toString() + ' months';
        }

        return lengthOfYears.toString() + ' years';
    }

    matTableWatcher() {

        let optionSubject = this.manager.activeReviewSearchOptions$.pipe(map(options => {
            if (options == null) return;
            this.isLoading = true;
            this.searchOptions = options;
            return options;
        }));


        // this subscription is fired anytime the search options change or the table is interacted with
        this.matTableWatcherSubscription = merge(optionSubject, this.sort.sortChange, this.paginator.page)
            .pipe(
                startWith({}),
                switchMap(() => {
                    if (!this.searchOptions || this.searchOptions.reviewTemplateId == null) {
                        return of(null);
                    }

                    this.searchOptions.sortBy        = this.getSortBy(this.sort.active).toString();
                    this.searchOptions.sortDirection = this.getSortDirection();
                    this.searchOptions.page          = this.paginator.pageIndex + 1;
                    this.searchOptions.pageSize      = this.paginator.pageSize || 10;

                    this.employeeSelection.searchOptions = this.searchOptions;

                    //this will fire an employee search and trigger the employeeSearchResponseSubscription below
                    return this.manager.searchEmployees(this.searchOptions);
                })
            ).subscribe(() => {this.isLoading = false});

        // this is fired anytime an employee search is done via the manager
        // updates the table data and paging info
        this.employeeSearchResponseSubscription = this.manager.employeeSearchResponse$.subscribe(resp => {
            //this.message.loading(false);
            if (resp == null) return;
            this.searchResultData = resp;
            this.dataSource = resp.results.filter(x => x.isActive);
            this.pagingLength = resp.filterCount;
        });
    }

    masterToggle() {
        this.isAllSelected() ?
            this.selection.clear() :
            this.dataSource.forEach(row => this.selection.select(row));
    }

    isAllSelected(): boolean {
        const numSelected = this.selection.selected.length;
        const numRows = this.dataSource.length;
        return numSelected == numRows;
    }

    isSelected(item:IEmployeeSearchResult):boolean {
        return this.selection.selected.find(s => s.employeeId == item.employeeId) != null;
    }

    showAddReviewDialog() {

        if (this.selectedEmployees.length < 1) {
            this.isInvalid = true;
            return
        }
        this.manager.getDirectSupervisors().pipe(
            concatMap(supervisors =>
                this.dialog.open(MultiReviewEditDialogComponent, {
                    width: '1000px',
                    data: {
                        options: this.searchOptions,
                        employees: this.selectedEmployees,
                    supervisors: supervisors,
                    totalEmps: this._totalEmps
                    }
                })
                    .afterClosed()),
            concatMap(result => {
                const DoNothing$ = empty();
                const ReloadData$ = of(null).pipe(
                    //tap(() => this.message.loading(true)),
                    concatMap(() =>this.reviewsService.saveReviewWithEmployeeList(result).pipe(
                    tap(() => {
                        this.message.setTemporarySuccessMessage('Successfully saved!', 5000);

                        this.clearSelectedEmployees();
                        this.searchEmployees();
                    }),
                    catchError(err => {
                        this.message.showWebApiException(err);
                        return throwError(err);
                    })
                )));

                return iif(() => result == null,
                    DoNothing$,
                    ReloadData$);
            })).subscribe();
    }

    private clearSelectedEmployees(): void {
        this.selectedEmployees = [];
        this.selection.clear();
    }

    /**
     * Use context's search options to make API call and update table with
     * results of the search.
     */
    private searchEmployees() {
        this.manager.searchEmployees(this.searchOptions).pipe(map(resp => {
            this.isLoading = true;
            return resp;
        })).subscribe(() => {});
    }

    showSupervisorDialog(item: IEmployeeSearchResult) {
        this.dialog.open(SupervisorOutletComponent, {
            width: '500px',
            data: {
                employeeId: item.employeeId,
                name: item.firstName + " " + item.lastName
            }
        })
            .afterClosed()
            .subscribe((result: IContact) => {
                if (result == null) return;
                item.directSupervisor = `${result.firstName} ${result.lastName}`;
            });
    }

    showCompetencyDialog(item: IEmployeeSearchResult) {
        forkJoin(
            this.perfService.getCompetencyModelsForCurrentClient(),
            this.perfService.getPerformanceCompetenciesForCurrentClient(false).pipe(map(x => x.filter(y => y.isCore)))
        ).pipe(
            map(x => ({ compModels: x[0], coreComps: x[1] })),
            // remove any archived competencies
            map(val => {
                val.coreComps = val.coreComps.sort(this.sortFn)
                const compModels = val.compModels || [];
                compModels.forEach(compModel => {
                    compModel.competencies = compModel.competencies.sort(this.sortFn);
                    // insert core competencies at the beginning of the list like we do in the competency model edit dialog
                    compModel.competencies =
                        val.coreComps.concat((compModel.competencies || []).filter(comp => !comp.isArchived));
                });
                return val;
            }),
            tap(x => {
                this.compModels = x.compModels;
            }),
            //show modal
            concatMap(data => this.dialog.open(CompetencyDialogComponent, {
                width: '500px',
                data: {
                    employeeId: item.employeeId,
                    clientId: item.clientId,
                    models: data.compModels,
                    selectedModel: "0"
                }
            }).afterClosed()),
            tap(result => {
                if (result == null) return;
                item.competencyModel = result;
            })
        )
            .subscribe();

    }

    private getSortBy(sortActive: string) {
        switch (sortActive) {
            case 'name': return EmployeeSearchFilterType.Name;
            case 'number': return EmployeeSearchFilterType.Number;
            case 'division': return EmployeeSearchFilterType.Division;
            case 'department': return EmployeeSearchFilterType.Department;
            case 'jobTitle': return EmployeeSearchFilterType.JobTitle;
            case 'supervisor': return EmployeeSearchFilterType.Supervisor;
            case 'hireDate': return EmployeeSearchFilterType.HireDate;
            case 'serviceLength': return EmployeeSearchFilterType.LengthOfService;
            case 'payType': return EmployeeSearchFilterType.PayType;
            case 'competencyModel': return EmployeeSearchFilterType.CompetencyModel;
            default: return EmployeeSearchFilterType.Name;
        }
    }

    private getSortDirection(sort: Sort | MatSort = null): SortDirection {
        sort = sort == null ? this.sort : sort;
        return sort.direction === 'desc'
            ? SortDirection.descending
            : SortDirection.ascending;
    }

    private readonly sortFn = (a: ICompetency, b: ICompetency) => a.name.toLowerCase().localeCompare(b.name.toLowerCase());


}
