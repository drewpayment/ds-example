import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CopyPlansDialogComponent } from './copy-plans-dialog.component';
import { ICopyPlansDialogData } from '../../shared/plans.model'

export interface ICopyPlansDialogResult {
    someData: any;
}

@Injectable({
    providedIn: 'root'
})
export class CopyPlansDialogService {

    constructor(private dialog: MatDialog) { }

    open(clientId: number) {
        let config = new MatDialogConfig<ICopyPlansDialogData>();
        config.width = "600px";
        config.data = {
            clientId: clientId
        };

        return this.dialog.open<CopyPlansDialogComponent, ICopyPlansDialogData, ICopyPlansDialogResult>(CopyPlansDialogComponent, config);
    }
}
