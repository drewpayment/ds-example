import {
  Component,
  Inject,
  OnInit,
} from "@angular/core";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { AutocompleteItem } from "@ds/core/groups/shared/autocomplete-item.model";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import { IEmployeeTaxCostCenterConfiguration, KeyValue } from '@models';
import { NEVER, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { EmployeeTaxesService } from "../../../services/employee-taxes.service";

@Component({
  selector: "ds-configure-cost-centers-dialog",
  templateUrl: "./configure-cost-centers-dialog.component.html",
  styleUrls: ["./configure-cost-centers-dialog.component.scss"],
})
export class ConfigureCostCentersDialogComponent implements OnInit {
  isLoading: boolean = true;
  form: FormGroup = this.createForm();
  selectedCostCenterList: KeyValue[] = [];
  availableCostCenterList: KeyValue[] = [];

  get availableCostCenters() {
    return this.form.get("availableCostCenters") as FormControl;
  }
  get selectedCostCenters() {
    return this.form.get("selectedCostCenters") as FormControl;
  }

  constructor(
    private employeeTaxService: EmployeeTaxesService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ConfigureCostCentersDialogComponent>,
    private confirmDialog: ConfirmDialogService,
    private ngxMsg: NgxMessageService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit() {
    this.employeeTaxService
      .getCostCenterList(this.data.clientId, this.data.employeeTaxId)
      .subscribe((data) => {
        this.availableCostCenterList = data.availableCostCenters;
        this.selectedCostCenterList = data.selectedCostCenters;
        this.syncAvailableCostCenterList();
        this.patchForm(data);
        this.isLoading = false;
      });
  }

  costCenterMapper = (val) => ({display: val.description, value: val.id} as AutocompleteItem);

  private createForm() {
    return this.fb.group({
      availableCostCenters: this.fb.control([]),
      selectedCostCenters: this.fb.control([]),
    });
  }

  private patchForm(costCenters: IEmployeeTaxCostCenterConfiguration): void {
    this.form.patchValue({availableCostCenters: costCenters.availableCostCenters,
    selectedCostCenters: costCenters.selectedCostCenters})
  }

  save() {
    this.employeeTaxService
      .saveEmployeeTaxCostCenters(
        this.data.clientId,
        this.data.employeeId,
        this.data.employeeTaxId,
        this.availableCostCenters.value.filter(x => this.selectedCostCenters.value.includes(x.id))
      )
      .pipe(
        catchError((error) => {
          const errorMsg =
            error.error.errors != null && error.error.errors.length
              ? error.error.errors[0].msg
              : error.message;
          this.ngxMsg.setErrorMessage(errorMsg);
          return throwError(error);
        })
      )
      .subscribe((result) => {
        this.ngxMsg.setSuccessMessage("Cost Centers saved successfully!");
        this.dialogRef.close(null);
      });
  }

  close() {
    if (this.selectedCostCenters.touched || this.selectedCostCenters.dirty) {
      const options = {
        title: "You have unsaved cost center changes.",
        message: "Are you sure you want to close?",
        confirm: "Close",
      };

      this.confirmDialog.open(options);
      this.confirmDialog
      .confirmed()
      .subscribe(confirmed => {
        if (confirmed) {
          this.dialogRef.close(null);
        }
        else{
          return NEVER;
        }
      });
    }
    else {
      this.dialogRef.close(null);
    }
  }

  private syncAvailableCostCenterList() {
    for (let costCenter of this.selectedCostCenterList) {
      this.removeFromAvailableCostCenterList(costCenter);
    }
  }

  private removeFromAvailableCostCenterList(costCenter: KeyValue){
    let index = this.availableCostCenters.value.findIndex(cc => cc.id === costCenter.id);

    if ( index != null && index >=0 ) {
      this.availableCostCenters.value.splice(index, 1);
    }
  }
}
