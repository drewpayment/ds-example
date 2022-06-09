import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { IMovies } from '../../shared/models';
import { MatSort } from '@angular/material/sort';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { BILLMURRAYMOVIES } from '../../shared/billMurray';
import { DsTableDataSource } from '@ds/core/ui/ds-table/ds-table-extender/ds-table.source';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'ds-table-view',
  templateUrl: './table-view.component.html',
  styleUrls: ['./table-view.component.scss']
})
export class TableViewComponent implements OnInit {

  displayedColumns: string[] = ['select', 'character', 'movie', 'year', 'edit'];
  dataSource = new DsTableDataSource<IMovies>(BILLMURRAYMOVIES);;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  isEdit: boolean;

  allowMultiSelect: boolean = true;
  filteredMovies: IMovies[] = [];
  selection = new SelectionModel<IMovies>(this.allowMultiSelect, this.filteredMovies);
  nothingSelected: boolean = false;

  constructor(
    private confirmDialog: ConfirmDialogService
  ) { }

  ngOnInit() {}

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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
        this.dataSource.next( this.dataSource.data );
      } else {
        return false;
      }
    })

  }

  masterToggle() {
    this.isAllSelected() ? this.selection.clear() : this.dataSource.data.forEach(item => this.selection.select(item));
  }

  isAllSelected(): boolean {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected == numRows;
  }
  isSelected(item: IMovies):boolean {
    return this.selection.selected.find(i => i.id == item.id) != null;
  }

  rowsSelected() {
    // Check if any rows are selected. If there are none, set an invalid flag
    return this.nothingSelected = this.selection.selected.length == 0;
  }

}


