import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { DsTableDataSource } from '@ds/core/ui/ds-table/ds-table-extender/ds-table.source';
import { DsTableComponent } from '@ds/core/ui/ds-table/ds-table.component';
import { Subject } from 'rxjs';
import { BILLMURRAYMOVIES } from '../../shared/billMurray';
import { IMovies } from '../../shared/models';
import { YEARS } from '../../shared/years';

@Component({
  selector: 'ds-table-validation',
  templateUrl: './table-validation.component.html',
  styleUrls: ['./table-validation.component.scss']
})
export class TableValidationComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['character', 'movie', 'year'];
  years = YEARS;
  
  form: FormGroup = this.createForm(BILLMURRAYMOVIES);
  dataSource = new DsTableDataSource<AbstractControl>([]);
  
  sortedData = [] = BILLMURRAYMOVIES;

  get formItems() {
    return this.form.get('items') as FormArray;
  }

  @ViewChild(DsTableComponent, { static: false }) table: DsTableComponent;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

  isSubmitted = false;
  newRow = false;

  constructor(
    private fb: FormBuilder
  ) { 
    this.dataSource.next(this.formItems.controls);
  }

  ngOnInit(): void {
    
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  private createForm(data) {
    return this.fb.group({ items: this.formArray(data) })
  }

  formArray(data) {
    let result = this.fb.array([]);
    data.forEach((element: IMovies) => {
      let year = this.years.find(yr => yr.year === element.year)

      let row = this.fb.group({
        character: this.fb.control(
          element.character,
          { validators: 
            [Validators.required, Validators.pattern('^[\.a-zA-Z0-9,!? ]*$')], 
            updateOn: 'change' 
          }
        ),
        movie: this.fb.control(
          element.movie,
          { validators: Validators.required, updateOn: 'change' }
        ),
        year: this.fb.control(
          year.id,
          { validators: Validators.required, updateOn: 'change' }
        )
      });
      result.push(row);
    });
    return result;
  }

  addRow() {
    this.newRow = true;
    let row = this.fb.group({
      character: this.fb.control(
        null,
        { validators: [Validators.required, Validators.pattern('^[\.a-zA-Z0-9,!? ]*$')], updateOn: 'change' }
      ),
      movie: this.fb.control(
        null,
        { validators: Validators.required, updateOn: 'change' }
      ),
      year: this.fb.control(
        null,
        { validators: Validators.required, updateOn: 'change' }
      )
    });

    //this.shouldApplyFocus = 'true'; 

    this.formItems.push( row );
    this.dataSource.next( this.formItems.controls );
    this.paginator.lastPage();
  }

  resetData() {
    
  }

  save() {
    this.isSubmitted = true;
    this.form.markAllAsTouched();

    if (this.form.invalid) return;
  }
}
