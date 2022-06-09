import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { IReviewStatus } from '../shared/review-status.model';
import { IReviewSummaryDialogData } from './review-summary-dialog-data.model';
import { ReviewSummaryDialogComponent } from './review-summary-dialog.component';
import { IReviewSummaryDialogResult } from './review-summary-dialog-result.model';

@Injectable({
    providedIn: 'root'
})
export class ReviewSummaryDialogService {

    constructor(private dialog: MatDialog) { }

    open(reviewStatus: IReviewStatus) {
        let config = new MatDialogConfig<IReviewSummaryDialogData>();
        config.data = {
            reviewStatus: reviewStatus
        };

        config.width = "750px";

        return this.dialog.open<ReviewSummaryDialogComponent, IReviewSummaryDialogData, IReviewSummaryDialogResult>(ReviewSummaryDialogComponent, config);
    }
}
