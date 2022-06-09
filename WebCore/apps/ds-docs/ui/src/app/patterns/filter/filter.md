# Filters

## Employee Filter
** Alignment **
If the filter is going to align to the right, the filter fielsds will need to be on teh left of the filter button. Place the filter fields to the left of the button if the filter is aligned to the right.
<div class="button-bar">
    <button class="btn btn-icon" ng-click="toggleEmployeeFilter = !toggleEmployeeFilter">
        <i class="material-icons">code</i>
        View Code
    </button>
</div>
<div class="example-block" ng-class="{'open': toggleEmployeeFilter}">
    <div class="ui mb-4">
        <div class="row">
            <div class="col-md-6">
                <div class="d-flex align-items-center mb-3 mb-md-0">
                    <button
                        class="btn p-1"
                        ng-class="{'btn-outline-primary':!$ctrl.context.hasEnabledFilters(), 'btn-primary':$ctrl.context.hasEnabledFilters()}"
                        type="button"
                        ng-click="$ctrl.context.showFilterOptionModal()">
                        <i class="material-icons md-24">filter_list</i>
                    </button>
                    <div><div class="d-inline-block text-muted clickable hoverable ml-3 font-xs"
                            ng-repeat="filter in $ctrl.context.getEnabledFilters()"
                            ng-click="$ctrl.context.clearFilterSelection(filter)">
                            {{ filter.$selected.name }} <i class="material-icons md-12 text-muted hover-show">clear</i>
                            {{ !$last || $ctrl.context.$state.$filter.isActiveOnly || $ctrl.context.$state.$filter.isExcludeTemps ? " /" : ""}}
                        </div>
                        <div class="d-inline-block text-muted clickable hoverable ml-3 font-xs"
                            ng-if="$ctrl.context.$state.$filter.isActiveOnly"
                            ng-click="$ctrl.context.toggleActiveOnly()">
                            Active Only <i class="material-icons md-12 text-muted hover-show">clear</i>
                            {{ $ctrl.context.$state.$filter.isExcludeTemps ? " /" : ""}}
                        </div>
                        <div class="d-inline-block text-muted clickable hoverable ml-3 font-xs"
                            ng-if="$ctrl.context.$state.$filter.isExcludeTemps"
                            ng-click="$ctrl.context.toggleExcludeTemps()">
                            Temps Excluded <i class="material-icons md-12 text-muted hover-show">clear</i>
                        </div>
                        <div class="d-inline-block text-muted ml-3 font-xs"
                            ng-if="!$ctrl.context.hasEnabledFilters()">
                            Filters Off
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="d-flex align-items-center mb-3 mb-md-0 justify-content-end">
                    <div><div class="d-inline-block text-muted clickable hoverable mr-3 font-xs"
                            ng-repeat="filter in $ctrl.context.getEnabledFilters()"
                            ng-click="$ctrl.context.clearFilterSelection(filter)">
                            {{ filter.$selected.name }} <i class="material-icons md-12 text-muted hover-show">clear</i>
                            {{ !$last || $ctrl.context.$state.$filter.isActiveOnly || $ctrl.context.$state.$filter.isExcludeTemps ? " /" : ""}}
                        </div>
                        <div class="d-inline-block text-muted clickable hoverable mr-3 font-xs"
                            ng-if="$ctrl.context.$state.$filter.isActiveOnly"
                            ng-click="$ctrl.context.toggleActiveOnly()">
                            Active Only <i class="material-icons md-12 text-muted hover-show">clear</i>
                            {{ $ctrl.context.$state.$filter.isExcludeTemps ? " /" : ""}}
                        </div>
                        <div class="d-inline-block text-muted clickable hoverable mr-3 font-xs"
                            ng-if="$ctrl.context.$state.$filter.isExcludeTemps"
                            ng-click="$ctrl.context.toggleExcludeTemps()">
                            Temps Excluded <i class="material-icons md-12 text-muted hover-show">clear</i>
                        </div>
                        <div class="d-inline-block text-muted mr-3 font-xs"
                            ng-if="!$ctrl.context.hasEnabledFilters()">
                            Filters Off
                        </div>
                    </div>
                    <button
                        class="btn p-1"
                        ng-class="{'btn-outline-primary':!$ctrl.context.hasEnabledFilters(), 'btn-primary':$ctrl.context.hasEnabledFilters()}"
                        type="button"
                        ng-click="$ctrl.context.showFilterOptionModal()">
                        <i class="material-icons md-24">filter_list</i>
                    </button>
                </div>
            </div>
        </div>
    </div>

```html
// Angular 
<div class="d-flex align-items-center">
    <!-- Repeat this for each filter -->
    <ng-container *ngIf="employeeFilters && employeeFilters.length; else noEmployeeFilters" >
        <div *ngFor="let f of employeeFilters; let isLast = last;" 
            class="d-inline-block text-muted clickable hoverable mr-3 font-xs"
            (click)="clearFilterSelection(f)"
        >
            {{f.filterName}} <i class="material-icons md-12 text-muted hover-show">clear</i> {{ !isLast ? '/': ''}}
        </div>    
    </ng-container>
    <ng-template #noEmployeeFilters>
        <!-- If not enabled -->
        <div class="d-inline-block text-muted mr-3 font-xs">
            Filters Off
        </div>
    </ng-template>
    <button
        class="btn p-1"
        [class.btn-outline-primary]="!filtersEnabled"
        [class.btn-primary]="filtersEnabled"
        type="button"
        (click)="showFilterDialog()"
    >
        <i class="material-icons">filter_list</i>
    </button>
</div>

// AngularJS component
<employee-search-filter ></employee-search-filter>
```

</div>