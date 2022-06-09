import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { BILLMURRAYMOVIES } from '@ds/docs/ds-components/shared/billMurray';
import { SelectionModel } from '@angular/cdk/collections';
import { IMovies } from '@ds/docs/ds-components/shared/models';
import { MatSort } from '@angular/material/sort';
@Component({
  selector: 'ds-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class TableComponent implements OnInit, AfterViewInit  {

  displayedColumns: string[] = ['selected', 'character', 'movie', 'year'];
  dataSource = new MatTableDataSource<IMovies>(BILLMURRAYMOVIES);

  allowMultiSelect: boolean = true;
  selectedMovies: IMovies[] = [];
  selection = new SelectionModel<IMovies>(this.allowMultiSelect, this.selectedMovies);
  activeYears: number;
  nothingSelected: boolean = false;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  constructor() { }

  ngOnInit() {
    this.totalActiveYears();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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

  totalActiveYears() {
    let earliestYear = 2500;
    let latestYear = 0;
    this.dataSource.data.forEach(item => {
      if ( item.year < earliestYear ) earliestYear = item.year;
      if ( item.year > latestYear ) latestYear = item.year;
    });

    this.activeYears = ( latestYear - earliestYear );
  }

}
