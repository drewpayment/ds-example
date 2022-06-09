import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { SearchFilterDialogComponent } from './search-filter-dialog.component';
import { ISearchFilterDialogData } from './search-filter-dialog-data.model';
import { ISearchFilterDialogResult } from './search-filter-dialog-result.model';
import { EmployeeSearchOptions } from '@ds/employees/search/shared/models/employee-search-options';
import { IEmployeeSearchFilter } from '@ds/employees/search/shared/models/employee-search-filter';


@Injectable({
    providedIn: 'root'
})
export class SearchFilterDialogService {
    constructor(private dialog: MatDialog) { }

    open(options: EmployeeSearchOptions, filters: IEmployeeSearchFilter[] = null) {
        let config = new MatDialogConfig<ISearchFilterDialogData>();
        config.data = {
            options: options,
            filters: filters
        };

        config.maxWidth = '80vw';
        config.minWidth = '50vw';

        return this.dialog.open<SearchFilterDialogComponent, ISearchFilterDialogData, ISearchFilterDialogResult>(SearchFilterDialogComponent, config);
    }
}
