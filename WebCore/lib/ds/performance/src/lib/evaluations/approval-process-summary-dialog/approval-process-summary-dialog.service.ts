import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { IReview } from '@ds/performance/reviews/shared/review.model';
import { IEvaluationWithStatusInfo } from '../shared/evaluation-status-info.model';
import { ApprovalProcessSummaryDialogComponent } from './approval-process-summary-dialog.component';
import { IEvaluationSummaryDialogData } from '../evaluation-summary-dialog/evaluation-summary-dialog-data.model';
import { IEvaluationSummaryDialogResult } from '../evaluation-summary-dialog/evaluation-summary-dialog-result.model';
import { UserInfo } from '@ds/core/shared';

@Injectable({
  providedIn: 'root'
})
export class ApprovalProcessSummaryDialogService {

  constructor(private dialog: MatDialog) { }

  open(evaluation: IEvaluationWithStatusInfo, review: IReview, user: UserInfo) {
      let config = new MatDialogConfig<any>();
      config.data = {
          evaluation: evaluation,
          review: review,
          user: user
      };
      config.width = "500px";
      return this.dialog.open<ApprovalProcessSummaryDialogComponent, any, IEvaluationSummaryDialogResult>(ApprovalProcessSummaryDialogComponent, config);
  }
}
