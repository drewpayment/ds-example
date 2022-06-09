import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { BILLMURRAYMOVIES } from '@ds/docs/ds-components/shared/billMurray';
import { IMovies } from '@ds/docs/ds-components/shared/models';
import { MatSort } from '@angular/material/sort';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { YEARS } from '@ds/docs/ds-components/shared/years';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { DataSource } from '@angular/cdk/table';

@Component({
  selector: 'ds-table-edit',
  templateUrl: './table-edit.component.html',
  styleUrls: ['./table-edit.component.scss']
})
export class TableEditComponent implements OnInit, AfterViewInit  {

  displayedColumns: string[] = ['character', 'movie', 'year', 'edit'];
  dataSource = new MatTableDataSource<IMovies>(BILLMURRAYMOVIES);
  isEditView: boolean = false;
  isGridEditView: boolean = false;
  years = YEARS;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  form: FormGroup;
  formData = BILLMURRAYMOVIES;

  constructor(
    private fb: FormBuilder,
    private confirmDialog: ConfirmDialogService
  ) { }

  ngOnInit() {
      this.form = this.fb.group({
        items: this.createFormArray()
      })
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
        year: this.fb.control(year.id)
      });
      result.push(row);
    });
    return result;
  }

  editSingle(element): void {
    element.isEditView = !element.isEditView
  }

  editAll(): void {
    if ( !this.isGridEditView ) {
      this.dataSource.data.forEach(element => {
        element.isEditView = true;
      });
      this.isGridEditView = true;
    } else {
      this.dataSource.data.forEach(element => {
        element.isEditView = false;
      });
      this.isGridEditView = false;
    }
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
        this.dataSource = new MatTableDataSource<IMovies>(this.dataSource.data);
      } else {
        return false;
      }
    })

  }
}
