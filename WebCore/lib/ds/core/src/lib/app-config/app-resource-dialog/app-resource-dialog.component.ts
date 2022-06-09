import * as _ from "lodash";
import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { IAppResourceDialogResult } from './app-resource-dialog-result.model';
import { IAppResourceDialogData } from './app-resource-dialog-data.model';
import { AppConfigApiService, IApplicationResource, ApplicationResourceType, ApplicationSourceType } from '../shared';
import { NgForm } from '@angular/forms';
import { zip } from "rxjs";

@Component({
    selector: 'ds-app-resource-dialog',
    templateUrl: './app-resource-dialog.component.html',
    styleUrls: ['./app-resource-dialog.component.scss']
})
export class AppResourceDialogComponent implements OnInit {

    resourceTypes: { resourceType: ApplicationResourceType, name: string }[];
    applicationSources: { sourceType: ApplicationSourceType, name: string }[];
    resource: IApplicationResource;

    constructor(
        private dialogRef: MatDialogRef<AppResourceDialogComponent, IAppResourceDialogResult>,
        @Inject(MAT_DIALOG_DATA)
        private dialogData: IAppResourceDialogData,
        private msgSvc: DsMsgService,
        private apiSvc: AppConfigApiService
    ) { }

    ngOnInit() {
        this.resource = this.dialogData.resource || <IApplicationResource>{
            applicationSourceType: ApplicationSourceType.SourceWeb,
            resourceType: ApplicationResourceType.WebPage
        };

        const rt$ = this.apiSvc.getApplicationResourceTypes();
        const st$ = this.apiSvc.getApplicationSourceTypes();

        zip(rt$, st$, (rt, st) => { return { resourceTypes: rt, sourceTypes: st } })
        .subscribe(result => {
            this.resourceTypes = result.resourceTypes;
            this.applicationSources = result.sourceTypes;
        });

    }

    save(form: NgForm) {
        if (!form.valid)
            return;

        this.apiSvc.saveApplicationResource(this.resource)
            .subscribe(data => {
                this.resource.resourceId = data.resourceId;
                this.dialogRef.close({
                    resource: data
                });
                this.msgSvc.setTemporarySuccessMessage("Application resource saved successfully.")
            });
    }

    cancel() {
        this.dialogRef.close(null);
    }
}
