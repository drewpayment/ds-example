import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { KeyValue } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';
import { EmployeeTaxesService } from '../../../services/employee-taxes.service';

@Component({
  selector: 'ds-add-tax-dialog',
  templateUrl: './add-tax-dialog.component.html',
  styleUrls: ['./add-tax-dialog.component.css']
})
export class AddTaxDialogComponent implements OnInit {
  isLoading: boolean = true;
  availableClientTaxes: KeyValue[];
  form: FormGroup;
  isSubmitted:boolean = false;

  constructor(
    private fb: FormBuilder,
    private employeeTaxService: EmployeeTaxesService,
    private dialogRef: MatDialogRef<AddTaxDialogComponent>,
    private ngxMsg: NgxMessageService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
    this.employeeTaxService
      .getAvailableClientTaxList(this.data.clientId, this.data.employeeId)
      .subscribe((res) => {
        this.initForm();
        this.availableClientTaxes = res;
        this.isLoading = false;
      });
  }

  initForm() {
    this.form = this.fb.group({
      clientTax: [null, Validators.required],
    });
  }

  save() {
    this.isSubmitted = true;
    this.form.markAllAsTouched();
    if ( this.form.invalid ) return;
    this.employeeTaxService.saveNewEmployeeTax( this.data.clientId, this.data.employeeId, this.form.controls['clientTax'].value ).subscribe((response) => {
      this.ngxMsg.setSuccessMessage("Tax Added Successfully");
      this.dialogRef.close(response);
      this.isSubmitted = false;
    }, (error: HttpErrorResponse) => {
      this.ngxMsg.setErrorResponse(error);
    });
  }

  close() {
    this.dialogRef.close(null);
  }
}
