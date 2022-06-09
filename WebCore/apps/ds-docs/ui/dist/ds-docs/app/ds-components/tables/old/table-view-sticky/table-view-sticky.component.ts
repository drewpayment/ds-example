import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { DsTableDataSource } from '@ds/core/ui/ds-table/ds-table-extender/ds-table.source';
import { BILLMURRAYMOVIES } from '@ds/docs/ds-components/shared/billMurray';
import { IMovies } from '@ds/docs/ds-components/shared/models';
import { YEARS } from '@ds/docs/ds-components/shared/years';

@Component({
  selector: 'ds-table-view-sticky',
  templateUrl: './table-view-sticky.component.html',
  styleUrls: ['./table-view-sticky.component.scss']
})
export class TableViewStickyComponent implements OnInit {

  displayedColumns: string[] = ['character', 'movie', 'year', 'quote', 'action'];

  dataSource = new DsTableDataSource<IMovies>(BILLMURRAYMOVIES);
  years = YEARS;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  form: FormGroup;
  formData = BILLMURRAYMOVIES;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    private confirmDialog: ConfirmDialogService
  ) { }

  ngOnInit() {
      this.form = this.fb.group({
        row: this.createFormArray()
      })

      // Disabled form to prevent interaction
      this.form.disable();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  createFormArray() {
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


  save() {}

  delete(element) {
    const options = {
      title: 'Are you sure you want to delete this movie?',
      message: "It's a classic",
      confirm: 'Delete'
    }

    this.confirmDialog.open(options);
    this.confirmDialog.confirmed().subscribe(confirmed => {
      if ( confirmed ) {
        const index = this.dataSource.data.indexOf(element);
        this.dataSource.data.splice(index, 1);
        this.dataSource = new DsTableDataSource<IMovies>(this.dataSource.data);
      } else {
        return false;
      }
    })

  }

  editForm() {
    this.isEdit = !this.isEdit;

    if (this.isEdit) 
      this.form.enable();
    else 
      this.form.disable();
  }

}
