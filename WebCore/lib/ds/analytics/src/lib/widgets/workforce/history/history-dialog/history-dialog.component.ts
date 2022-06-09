import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, ValidatorFn } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'ds-history-dialog',
  templateUrl: './history-dialog.component.html',
  styleUrls: ['./history-dialog.component.css']
})
export class HistoryDialogComponent implements OnInit {

  dataForm: FormGroup;
  yearInfo = new Map();
  yearsWithData: MatTableDataSource<number>;
  yearsDisplayed: MatTableDataSource<number>;
  years: number[] = [];
  saveDisabled: boolean = true;
  submitted:boolean = false;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<HistoryDialogComponent>, private fb: FormBuilder) {
    this.dataForm = this.fb.group({

    });
   }

  ngOnInit() {
    this.yearsWithData = new MatTableDataSource<number>(this.data.yearsWithData);
    this.yearsDisplayed = new MatTableDataSource<number>(this.data.yearsDisplayed);
    this.yearsWithData.paginator = this.paginator;
    this.years = this.yearsWithData.data;
    this.years.forEach(element => {
      this.dataForm.addControl("Year_" + String(element), new FormControl(''));
    });

    const yearsValidation = this.yearsValidator( this.yearsWithData.data );
    this.dataForm.setValidators(yearsValidation);

    this.yearsDisplayed.filteredData.forEach(element => {
      this.yearInfo.set(element, element);
    });
    this.saveDisabled = this.checkValidity();
  }

  private yearsValidator( years: Number[] ): ValidatorFn {
    return (form: FormGroup) => {
      var yearsSelected = 0;

      years.forEach(yr => {
        if(form.controls['Year_' + yr ].value) yearsSelected++;
      });

        if (yearsSelected > 5 || yearsSelected < 2) {
          return {
            selectionNotValid: true
          }
        }

        return null;
    }
  }

  yearChange(event) {
    if (this.yearInfo.has(Number(event.target.value)))
      this.yearInfo.delete(Number(event.target.value));
    else
      this.yearInfo.set(Number(event.target.value), Number(event.target.value));
    this.saveDisabled = this.checkValidity();
  }

  yearSelected(year) {
    return (this.yearsDisplayed.filteredData.includes(year));
  }

  checkValidity() {
    return (this.yearInfo.size > 5 || this.yearInfo.size < 2)
  }

  saveYears() {
    let temp = new Array;
    this.yearInfo.forEach((key: number, value: number) => {
      temp.push(key);
    })

    temp.sort();
    this.submitted = true;
    this.dataForm.updateValueAndValidity();
    if(this.dataForm.valid){
      this.dialogRef.close(temp);
    }
  }

  close(): void {
    this.dialogRef.close();
  }
}
