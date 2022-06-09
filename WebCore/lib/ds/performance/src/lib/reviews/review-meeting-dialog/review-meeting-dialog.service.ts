import { Injectable } from '@angular/core';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { IReview } from '../shared/review.model';
import { IReviewMeetingDialogData } from './review-meeting-dialog-data.model';
import { ReviewMeetingDialogComponent } from './review-meeting-dialog.component';
import { IReviewMeetingDialogResult } from './review-meeting-dialog-result.model';

@Injectable({
  providedIn: 'root'
})
export class ReviewMeetingDialogService {

  constructor(private dialog: MatDialog) { }

  open(review: IReview) {
    let config = new MatDialogConfig<IReviewMeetingDialogData>();
    config.data = {
        review: review
    };

    config.width = "550px";

    return this.dialog.open<ReviewMeetingDialogComponent, IReviewMeetingDialogData, IReviewMeetingDialogResult>(ReviewMeetingDialogComponent, config);
  }
}
