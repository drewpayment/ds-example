import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { NPSSurveyDialogComponent } from './nps-survey-dialog.component';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

@Component({
    selector: 'ds-nps-survey-trigger',
    templateUrl: './nps-survey-trigger.component.html',
})
export class NPSSurveyTriggerComponent implements OnInit {
    constructor(
        private dialog: MatDialog,
        private msg: DsMsgService,
    ) { }

    ngOnInit() { }

    openNPSSurvey() {
        const config = new MatDialogConfig<any>();

        config.width = '560px';
        config.panelClass = 'nps-survey';

        const dialogRef = this.dialog.open(NPSSurveyDialogComponent, config);

        dialogRef.afterClosed().subscribe(result => {
            if (result == null) {

            } else {
                if (result) {
                    this.msg
                        .setTemporarySuccessMessage('Thank you for your feedback. We value your input.'
                            , 5000);
                } else {
                    this.msg
                        .setTemporaryMessage('There was an error that occurred while saving your input.'
                            , this.msg.messageTypes.error);
                }
            }
        });
    }

}
