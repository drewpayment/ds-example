import { RecalcPointsDialogComponent } from './recalc-points-dialog.component';
import { Injectable } from '@angular/core';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { IRecalcPointsDialogData, IRecalcPointsDialogResult } from './recalc-points-dialog.model';

@Injectable({
  providedIn: 'root'
})
export class RecalcPointsDialogService {

  constructor(private dialog: MatDialog) { }

  public openRecalcPointsModal(clientId: number){
    let config = new MatDialogConfig<IRecalcPointsDialogData>();

    config.data = {
        clientId: clientId
    };

    return this.dialog.open<RecalcPointsDialogComponent, IRecalcPointsDialogData, IRecalcPointsDialogResult>(RecalcPointsDialogComponent, config);
  }
}
