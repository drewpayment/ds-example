import { Injectable } from '@angular/core';
import { IReview } from '../shared/review.model';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ReviewEditDialogComponent } from './review-edit-dialog.component';
import { IReviewEditDialogData } from './review-edit-dialog-data.model';
import { IReviewEditDialogResult } from './review-edit-dialog-result.model';
import { UserInfo } from '@ds/core/shared';
import { IContactWithClient } from '@ds/core/contacts';

@Injectable({
  providedIn: 'root'
})
export class ReviewEditDialogService {

  constructor(private dialog: MatDialog) { }

  open(review: IReview, employee: IContactWithClient, currentUser?: UserInfo) {
    let config = new MatDialogConfig<IReviewEditDialogData>();
    config.data = {
        employee: employee,
        review: review,
        currentUser: currentUser
    };

    config.width = "1000px";

    return this.dialog.open<ReviewEditDialogComponent, IReviewEditDialogData, IReviewEditDialogResult>(ReviewEditDialogComponent, config);
  }
}
