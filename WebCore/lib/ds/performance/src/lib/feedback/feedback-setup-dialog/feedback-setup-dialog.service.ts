import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { IFeedbackSetup } from '../shared/feedback-setup.model';
import { IFeedbackSetupDialogData } from './feedback-setup-dialog-data.model';
import { FeedbackSetupDialogComponent } from './feedback-setup-dialog.component';
import { IFeedbackSetupDialogResult } from './feedback-setup-dialog-result.model';

@Injectable({
  providedIn: 'root'
})
export class FeedbackSetupDialogService {

    constructor(private dialog: MatDialog) { }

    /**
     * Opens a feedback setup modal dialog used to add/edit a feedback object.
     * @param feedback (Optional) If null, a new feedback object will be created.  
     * Otherwise, the dialog will edit the specified feedback.
     */
    open(feedback?:IFeedbackSetup, isCopy = false) {
        let config = new MatDialogConfig<IFeedbackSetupDialogData>();
        config.data = {
            feedback: feedback,
            isCopy: isCopy
        };

        config.width = "600px";

        return this.dialog.open<FeedbackSetupDialogComponent, IFeedbackSetupDialogData, IFeedbackSetupDialogResult>(FeedbackSetupDialogComponent, config);
    }
}
