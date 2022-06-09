import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { result } from 'lodash';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { DsConfirmDialogContentComponent } from './ds-confirm-dialog.component';

interface ConfirmDialogOptions {
  title?: string;
  message?: string;
  confirm?: string;
  allowAction?: boolean;
}

@Injectable()
export class ConfirmDialogService {
  constructor(private dialog: MatDialog) {}
  dialogRef: MatDialogRef<DsConfirmDialogContentComponent>;
  public open(
    options: ConfirmDialogOptions
  ): MatDialogRef<DsConfirmDialogContentComponent> {
    this.dialogRef = this.dialog.open(DsConfirmDialogContentComponent, {
      data: {
        title: options.title,
        message: options.message,
        confirm: options.confirm,
      },
      width: '300px',
    });

    return this.dialogRef;
  }
  public confirmed(): Observable<any> {
    return this.dialogRef.afterClosed().pipe(
      take(1),
      map((result) => {
        return result;
      })
    );
  }
}
