import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { DsTableExtender } from '@ds/core/ui/ds-table/ds-table-extender/ds-table-extender.component';
import { DsTableDataSource } from '@ds/core/ui/ds-table/ds-table-extender/ds-table.source';
import { BILLMURRAYMOVIES } from '@ds/docs/ds-components/shared/billMurray';
import { IMovies } from '@ds/docs/ds-components/shared/models';
import { YEARS } from '@ds/docs/ds-components/shared/years';

@Component({
  selector: 'ds-table-add',
  templateUrl: './table-add.component.html',
  styleUrls: ['./table-add.component.scss']
})
export class TableAddComponent implements OnInit {

  displayedColumns: string[] = ['character', 'movie', 'year'];
  years = YEARS;

  @ViewChild(DsTableExtender, { static: false }) table: DsTableExtender<any>;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) set sort(sorter: MatSort) {
    if (sorter) this.dataSource.sort = sorter;
  }
  dataSource = new DsTableDataSource<AbstractControl>([]);
  data: IMovies[] = BILLMURRAYMOVIES;
  rows: FormArray = this.fb.array([]);
  form: FormGroup = this.fb.group({ 
    'rows': this.rows 
  });

  shouldApplyFocus: string;

  constructor( 
    private fb: FormBuilder,
    private changeDetectorRef: ChangeDetectorRef
  ) { 
    this.data = this.data.slice();
  }

  ngOnInit() {
    this.sortData();
    this.createForm();
    
    //this.updateView();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  updateView() {
    this.dataSource.next(this.rows.controls);
  }
  createForm() {
    this.rows.controls = [];
    this.data.forEach((row: IMovies) => this.addRow(row, true));
    this.updateView();
  }

  addRow(r?: IMovies, noRefresh?: Boolean) {
    let year;

    if(r) year = this.years.find(yr => yr.year === r.year);

    let row = this.fb.group({
      character: new FormControl(r && r.character ? r.character : null),
      movie: new FormControl(r && r.movie ? r.movie : null),
      year: new FormControl(r && year ? year.id : null)
    })
    this.rows.push(row);
    
    if (!noRefresh) {
      //this.pager.lastPage();
      this.updateView();
    }

    
  }
  sortData(sort?: Sort) {
    const data = this.data.slice();
    if (sort == null || !sort.active || sort.direction === '') {
      this.data = data;
      return;
    }
    
    this.data = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'character': return this.compare(a.character, b.character, isAsc);
        case 'movie': return this.compare(a.movie, b.movie, isAsc);
        case 'year': return this.compare(a.year, b.year, isAsc);
        default: return 0;
      }
    });
    this.createForm();
  };
  compare(a: number | string, b: number | string, isAsc: boolean) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }

  // save() {}

  // delete(element) {
  //   const options = {
  //     title: 'Are you sure you want to delete this movie?',
  //     message: "It's a classic",
  //     confirm: 'Delete'
  //   }

  //   this.confirmDialog.open(options);
  //   this.confirmDialog.confirmed().subscribe(confirmed => {
  //     if ( confirmed ) {
  //       const index = this.dataSource.data.indexOf(element);
  //       this.dataSource.data.splice(index, 1);
  //       this.dataSource = new DsTableDataSource<IMovies>(this.dataSource.data);
  //     } else {
  //       return false;
  //     }
  //   })
    
  // }

}
