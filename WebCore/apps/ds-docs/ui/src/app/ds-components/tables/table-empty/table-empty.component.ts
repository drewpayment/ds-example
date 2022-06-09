import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { DsTableDataSource } from '@ds/core/ui/ds-table/ds-table-extender/ds-table.source';
import { DsTableComponent } from '@ds/core/ui/ds-table/ds-table.component';
import { Subject } from 'rxjs';
import { BILLMURRAYMOVIES } from '../../shared/billMurray';
import { IMovies } from '../../shared/models';
import { YEARS } from '../../shared/years';

@Component({
  selector: 'ds-table-empty',
  templateUrl: './table-empty.component.html',
  styleUrls: ['./table-empty.component.scss']
})
export class TableEmptyComponent implements OnInit {

  displayedColumns: string[] = ['character', 'movie', 'year', 'quote', 'action'];

  // Data Source is blank on load in this example
  dataSource = new DsTableDataSource<IMovies>();
  years = YEARS;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  form: FormGroup;
  formData = BILLMURRAYMOVIES;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      row: this.createFormArray()
    })
  }

  ngAfterViewInit() { }

  createFormArray() {
    if (this.dataSource) {
      let result = this.fb.array([]);
      this.dataSource.data.forEach((element: IMovies) => {
        let year = this.years.find(yr => yr.year === element.year)

        let row = this.fb.group({
          character: this.fb.control(element.character),
          movie: this.fb.control(element.movie),
          year: this.fb.control(year.id),
          quote: this.fb.control(element.quote)
        });
        result.push(row);
      });
      return result;
    }

  }

}
