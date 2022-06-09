import { AfterViewInit, Component, EventEmitter, HostBinding, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { BILLMURRAYMOVIES } from '@ds/docs/ds-components/shared/billMurray';
import { IMovies } from '@ds/docs/ds-components/shared/models';
import { AbstractControl, FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { YEARS } from '@ds/docs/ds-components/shared/years';
import { BehaviorSubject, Observable, observable, Subject } from 'rxjs';
import { DsTableComponent } from '@ds/core/ui/ds-table/ds-table.component';
import { MatSort, Sort } from '@angular/material/sort';
import { DsTableDataSource } from '@ds/core/ui/ds-table/ds-table-extender/ds-table.source';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { MessageService } from 'apps/ds-company/src/app/services/message.service';

@Component({
  selector: 'ds-table-add-edit',
  templateUrl: './table-add-page.component.html',
  styleUrls: ['./table-add-page.component.scss']
})
export class TableAddPageComponent implements OnInit, AfterViewInit  {

  displayedColumns: string[] = ['character', 'movie', 'year', 'quote', 'action'];
  years = YEARS;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  form: FormGroup = this.createForm(BILLMURRAYMOVIES);
  formData = new Subject();
  dataSource = new DsTableDataSource<AbstractControl>([]);

  get formItems() {
    return this.form.get('items') as FormArray;
  }
 // @ViewChild(DsTableComponent, { static: false }) table: DsTableComponent;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  shouldApplyFocus: string = 'false';
  sortedData = [] = BILLMURRAYMOVIES;

  isEdit: boolean;
  newRow: boolean = false;

  constructor(
    private fb: FormBuilder,
    private confirmDialog: ConfirmDialogService,
    private ngxMsg: MessageService
  ) {
    this.dataSource.next(this.formItems.controls);
  }

  editForm(state: boolean) {
    this.isEdit = state;

    if (this.isEdit)
      this.form.enable();
    else
      this.form.disable();
  }


  ngOnInit() {

    this.sortedData = this.formItems.value;

    // Disabled form to prevent interaction
    this.form.disable();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy() {}

  private createForm(data) {
    return this.fb.group({ items: this.formArray(data) })
  }
  formArray(data) {
    let result = this.fb.array([]);
    data.forEach((element: IMovies) => {
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

  addRow() {
    this.newRow = true;
    this.editForm(true);
    let row = this.fb.group({
      character: this.fb.control(null),
      movie: this.fb.control(null),
      year: this.fb.control(null),
      quote: this.fb.control(null)
    }) as AbstractControl;


    this.formItems.push( row );
    this.dataSource.next( this.formItems.controls );
    this.paginator.lastPage();

  }

  sortFormArray(sort: Sort) {
    let sortArray = this.formItems.value;

    if (!sort.active || sort.direction === '') {
      this.sortedData = sortArray;
      return;
    }

    this.sortedData = sortArray.sort((a,b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'character':
          return compare(a.character, b.character, isAsc);
        case 'movie':
          return compare(a.movie, b.movie, isAsc);
        case 'year':
          return compare(a.year, b.year, isAsc);
        default:
          return 0;
      }
    });
    this.form.controls.items.patchValue(this.sortedData);
  }

  save() {
    this.editForm(false);
    this.ngxMsg.setSuccessMessage("Saved successfully");
  }

  cancel() {
    this.editForm(false);
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

        // Remove the form control then update DataSource with new formControl values
        this.formItems.removeAt(index);
        this.dataSource.next( this.formItems.controls );
      } else {
        return false;
      }
    })

  }
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}

function ngOnDestroy() {
  throw new Error('Function not implemented.');
}
