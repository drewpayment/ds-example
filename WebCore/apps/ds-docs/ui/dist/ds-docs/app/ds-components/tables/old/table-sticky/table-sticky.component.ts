import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { BILLMURRAYMOVIES } from '@ds/docs/ds-components/shared/billMurray';
import { MatSort } from '@angular/material/sort';
import { IMovies } from '@ds/docs/ds-components/shared/models';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';

@Component({
  selector: 'ds-table-sticky',
  templateUrl: './table-sticky.component.html',
  styleUrls: ['./table-sticky.component.scss']
})
export class TableStickyComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['character', 'movie', 'quote', 'year', 'edit'];
  dataSource = new MatTableDataSource<IMovies>(BILLMURRAYMOVIES);
  isEditView: boolean = false;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  constructor(
    private confirmDialog: ConfirmDialogService
  ) { }

  ngOnInit() {
  }
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  editSingle(element) {
    element.isEditView = true;
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
