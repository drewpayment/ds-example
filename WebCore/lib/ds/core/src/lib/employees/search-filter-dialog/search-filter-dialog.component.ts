import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BreakpointObserver } from '@angular/cdk/layout';
import { FormBuilder, FormGroup } from '@angular/forms';
import { EmployeeApiService } from '../shared/employee-api.service';
import { ISearchFilterDialogData } from './search-filter-dialog-data.model';
import { EmployeeSearchOptions } from '@ds/employees/search/shared/models/employee-search-options';
import { IEmployeeSearchFilterOption } from '@ds/employees/search/shared/models/employee-search-filter-option';
import { IEmployeeSearchFilter } from '@ds/employees/search/shared/models/employee-search-filter';
import { ClientService } from '@ds/core/clients/shared';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { EmployeeSearchFilterType } from '@ds/employees/search/shared/models/employee-search-filter-type';
import { ActionTypes } from '@ds/core/constants/action-types';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'ds-search-filter-dialog',
  templateUrl: './search-filter-dialog.component.html',
  styleUrls: ['./search-filter-dialog.component.scss'],
})
export class SearchFilterDialogComponent implements OnInit, OnDestroy {
  form: FormGroup = this.createForm();
  options: EmployeeSearchOptions = {};

  searchFilters: IEmployeeSearchFilter[];
  employeeStatusOptions: IEmployeeSearchFilterOption[];
  divisionOptions: IEmployeeSearchFilterOption[];
  departmentOptions: IEmployeeSearchFilterOption[];
  jobTitleOptions: IEmployeeSearchFilterOption[];
  costCenterOptions: IEmployeeSearchFilterOption[];
  groupOptions: IEmployeeSearchFilterOption[];
  payTypeOptions: IEmployeeSearchFilterOption[];
  shiftOptions: IEmployeeSearchFilterOption[];
  supervisorOptions: IEmployeeSearchFilterOption[];
  competencyModelOptions: IEmployeeSearchFilterOption[];
  timePolicyOptions: IEmployeeSearchFilterOption[];
  showCompetencyModel: boolean;
  user: UserInfo;
  destroy$ = new Subject();

  constructor(
    public ref: MatDialogRef<SearchFilterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ISearchFilterDialogData,
    private breakpoints: BreakpointObserver,
    private fb: FormBuilder,
    private employeeService: EmployeeApiService,
    private accountService: AccountService,
    private clientService: ClientService,
  ) {}

  ngOnInit() {
    this.breakpoints.observe(['(max-width:576px)'])
      .pipe(takeUntil(this.destroy$))
      .subscribe((result) => {
        if (result.matches) this.resizeSmall();
      });

    this.accountService.canPerformAction(ActionTypes.Features.PerformanceReviews)
      .subscribe((res) => {
        this.showCompetencyModel = res;
      });

    if (this.data) {
      this.options = this.data.options;

      if (this.data.filters && this.data.filters.length) {
        this.options.filters = this.data.filters;
      }
    } else {
      this.options = {} as EmployeeSearchOptions;
    }

    if (this.options.filters) {
      this.setFilterOptionFields(this.options.filters);
    } else {
      this.employeeService
        .getEmployeeSearchFilters(true, true, true)
        .subscribe((filters) => {
          this.setFilterOptionFields(filters);
        });
    }
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onNoClick() {
    this.ref.close();
  }

  clearFilters() {
    this.options.filters.forEach(x => x.$selected = null);
    this.form.reset();
  }

  saveFilters() {
    if (this.form.invalid) return;
    this.prepareModel();
    this.ref.close({ options: this.options });
  }

  private setFilterOptionFields(filters: IEmployeeSearchFilter[]) {
    this.employeeStatusOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.EmployeeStatus
    );
    this.divisionOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.Division
    );
    this.departmentOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.Department
    );
    this.jobTitleOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.JobTitle
    );
    this.costCenterOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.CostCenter
    );
    this.groupOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.Group
    );
    this.payTypeOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.PayType
    );
    this.shiftOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.Shift
    );
    this.supervisorOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.Supervisor
    );
    this.competencyModelOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.CompetencyModel
    );
    this.timePolicyOptions = this.getFilterOptions(
      filters,
      EmployeeSearchFilterType.TimePolicy
    );

    this.updateForm();
  }

  private getFilterOptions(
    filters: IEmployeeSearchFilter[],
    type: EmployeeSearchFilterType
  ) {
    let filter = filters.find((f) => f.filterType == type);
    return filter
      ? filter.filterOptions.sort((o1, o2) => {
          return o1.name > o2.name ? 1 : -1;
        })
      : [];
  }

  private resizeSmall() {
    this.ref.updateSize('100vw');
  }

  private createForm(): FormGroup {
    return this.fb.group({
      employeeStatus: this.fb.control(''),
      activeOnly: this.fb.control(''),
      excludeTemps: this.fb.control(''),
      division: this.fb.control(''),
      department: this.fb.control(''),
      jobTitle: this.fb.control(''),
      costCenter: this.fb.control(''),
      group: this.fb.control(''),
      payType: this.fb.control(''),
      shift: this.fb.control(''),
      supervisor: this.fb.control(''),
      competencyModel: this.fb.control(''),
      timePolicy: this.fb.control(''),
    });
  }

  private updateForm(): void {
    this.form.patchValue({
      employeeStatus: this.getFilterValue(
        EmployeeSearchFilterType.EmployeeStatus
      ),
      activeOnly: this.options.isActiveOnly,
      excludeTemps: this.options.isExcludeTemps,
      division: this.getFilterValue(EmployeeSearchFilterType.Division),
      department: this.getFilterValue(EmployeeSearchFilterType.Department),
      jobTitle: this.getFilterValue(EmployeeSearchFilterType.JobTitle),
      costCenter: this.getFilterValue(EmployeeSearchFilterType.CostCenter),
      group: this.getFilterValue(EmployeeSearchFilterType.Group),
      payType: this.getFilterValue(EmployeeSearchFilterType.PayType),
      shift: this.getFilterValue(EmployeeSearchFilterType.Shift),
      supervisor: this.getFilterValue(
        EmployeeSearchFilterType.Supervisor
      ),
      competencyModel: this.getFilterValue(
        EmployeeSearchFilterType.CompetencyModel
      ),
      timePolicy: this.getFilterValue(
        EmployeeSearchFilterType.TimePolicy
      ),
    });
  }

  private prepareModel(): void {
    const enumLength = Object.keys(EmployeeSearchFilterType).length / 2;
    let filters: number[] = new Array(enumLength);

    for (let i = 0; i < enumLength; i++) {
      const camelKey = EmployeeSearchFilterType[i].toCamelCase();
      if (
        this.form.value[camelKey] &&
        typeof this.form.value[camelKey] === 'string'
      )
        filters[i] = this.form.value[camelKey];
    }

    this.options.filters.forEach(filter => {
      const formFilter = filters[filter.filterType];

      if (formFilter) {
        const filterOption = filter.filterOptions.find(fo => fo.id == formFilter);

        filter.$selected = {
          filterType: filter.filterType,
          id: formFilter,
          name: filterOption != null ? filterOption.name : '',
        } as IEmployeeSearchFilterOption;
      } else {
        delete filter.$selected;
      }
    });

    this.options.isActiveOnly = this.form.value.activeOnly || false;
    this.options.isExcludeTemps = this.form.value.excludeTemps || false;
  }

  private resetForm() {
    this.form.setValue({
      employeeStatus: '',
      activeOnly: '',
      excludeTemps: '',
      division: '',
      department: '',
      jobTitle: '',
      costCenter: '',
      group: '',
      payType: '',
      shift: '',
      supervisor: '',
      competencyModel: '',
      timePolicy:'',
    });
    this.form.reset();
  }

  private getFilterValue(filterType: EmployeeSearchFilterType) {
    const filter =
      this.data.filters != null && this.data.filters.length
        ? this.data.filters.find((x) => x.filterType === filterType)
        : this.options != null &&
          this.options.filters != null &&
          this.options.filters.length
        ? this.options.filters.find((x) => x.filterType === filterType)
        : null;
    if (!filter) return;
    return filter.$selected != null ? filter.$selected.id : null;
  }
}
