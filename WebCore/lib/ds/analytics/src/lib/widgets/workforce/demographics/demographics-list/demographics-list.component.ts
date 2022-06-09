import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { DemographicData } from '@ds/analytics/shared/models/DemographicData.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'ds-demographics-list',
  templateUrl: './demographics-list.component.html'
})
export class DemographicsListComponent implements OnInit {

  dataSource: MatTableDataSource<DemographicData>;
  displayedColumns: string[] = ['Name', 'Status', 'Pay Type', 'Length Of Service', 'Age', 'Gender', 'Ethnicity'];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  @Input() data: any[];

  constructor() { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<DemographicData>(this.data);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.sortingDataAccessor = (data, header) => data[header.toString()];
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

}
