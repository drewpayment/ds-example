import { Injectable } from '@angular/core';

import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ICheckStockDialogData } from './check-stock-dialog-data.model';
import { CheckStockDialogComponent } from './check-stock-dialog.component';
import { ICheckStockResult } from './check-stock-dialog-result.model';

@Injectable({
    providedIn: 'root'
})
export class CheckStockDialogService {

    constructor(private dialog: MatDialog) { }

    /**
     * Opens a feedback setup modal dialog used to add/edit a feedback object.
     * @param feedback (Optional) If null, a new feedback object will be created.  
     * Otherwise, the dialog will edit the specified feedback.
     */
    open(payrollId:number) {
        let config  = new MatDialogConfig<ICheckStockDialogData>();
        config.data = {
            payrollId: payrollId
        };

        config.width        = "800px";
        config.disableClose = true;

        return this.dialog.open<CheckStockDialogComponent, ICheckStockDialogData, ICheckStockResult>(CheckStockDialogComponent, config);
    }

}
