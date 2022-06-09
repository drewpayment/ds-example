import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Injectable } from '@angular/core';
import { IUpdateBalanceDialogData, IUpdateBalanceDialogResult } from './update-balance-dialog.model';
import { UpdateBalanceDialogComponent } from './update-balance-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class UpdateBalanceDialogService {

  constructor(private dialog: MatDialog) { }

  public openUpdateBalanceModal(clientId: number, userId: number){
    let config = new MatDialogConfig<IUpdateBalanceDialogData>();

    config.data = {
        clientId: clientId,
        userId: userId
    };

    config.width = '500px';

    return this.dialog.open<UpdateBalanceDialogComponent, IUpdateBalanceDialogData, IUpdateBalanceDialogResult>(UpdateBalanceDialogComponent, config);
  }
}
