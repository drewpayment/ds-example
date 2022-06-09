import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { EmployeeService } from '@ds/core/employees/employee.service';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-turbo-tax-modal',
  templateUrl: './turbo-tax-modal.component.html',
  styleUrls: ['./turbo-tax-modal.component.scss']
})
export class TurboTaxModalComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<TurboTaxModalComponent>,
    private employeeService: EmployeeService,
    private msgService: NgxMessageService
    // , @Inject(MAT_DIALOG_DATA) public data: DialogData) {}
  ) { }

  ngOnInit() {
  }

  onNoClick(): void {
    this.employeeService.updateAllowTurboTax(0).subscribe(() => {
      this.dialogRef.close();
    }, err => {
      this.msgService.setErrorMessage(err.message);
      this.dialogRef.close();
    });
  }

  close() {
    this.dialogRef.close();
  }

  onLeanMoreClick(): void {
    window.location.href = 'CompanyTurboTaxConsent.aspx';
  }
}
