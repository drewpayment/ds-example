import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { BILLMURRAYMOVIES } from '@ds/docs/ds-components/shared/billMurray';
import { IMovies } from '@ds/docs/ds-components/shared/models';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { TableEditDialogComponent } from '@ds/docs/ds-components/tables/old/table-edit-dialog/table-edit-dialog.component';
import { YEARS } from '@ds/docs/ds-components/shared/years';
import { MatSort } from '@angular/material/sort';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';

@Component({
  selector: 'ds-table-edit-with-dialog',
  templateUrl: './table-edit-with-dialog.component.html',
  styleUrls: ['./table-edit-with-dialog.component.scss']
})
export class TableEditWithDialogComponent implements OnInit, AfterViewInit  {

  displayedColumns: string[] = ['lastName', 'movie', 'year', 'edit'];
  dataSource = new MatTableDataSource<IMovies>(BILLMURRAYMOVIES);
  isEditView: boolean = false;
  years = YEARS;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  constructor(
    private confirmDialog: ConfirmDialogService,
    private dialog: MatDialog
  ) { }

  ngOnInit() {

  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  editInDialog(element) {
    var config = new MatDialogConfig<any>();
    config.data = element;
    config.width = '500px';
    const dialogRef = this.dialog.open(TableEditDialogComponent, config);


  }

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
