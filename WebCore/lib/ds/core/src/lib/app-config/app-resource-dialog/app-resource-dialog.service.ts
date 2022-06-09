import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { IApplicationResource } from '../shared';
import { IAppResourceDialogData } from './app-resource-dialog-data.model';
import { AppResourceDialogComponent } from './app-resource-dialog.component';
import { IAppResourceDialogResult } from './app-resource-dialog-result.model';

@Injectable({
    providedIn: 'root'
})
export class AppResourceDialogService {

    constructor(private dialog: MatDialog) { }

    open(resource: IApplicationResource) {
        let config = new MatDialogConfig<IAppResourceDialogData>();
        config.data = {
            resource: resource
        };

        config.maxWidth = '80vw';
        config.minWidth = '50vw';

        return this.dialog.open<AppResourceDialogComponent, IAppResourceDialogData, IAppResourceDialogResult>(AppResourceDialogComponent, config);
    }
}
