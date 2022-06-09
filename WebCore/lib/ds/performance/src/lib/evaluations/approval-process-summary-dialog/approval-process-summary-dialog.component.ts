import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IEvaluationSummaryDialogResult } from '../evaluation-summary-dialog/evaluation-summary-dialog-result.model';
import { IEvaluationSummaryDialogData } from '../evaluation-summary-dialog/evaluation-summary-dialog-data.model';
import { IEvaluationWithStatusInfo } from '../shared/evaluation-status-info.model';
import { IContactSearchResult } from '@ds/core/contacts/shared/contact-search-result.model';
import { IContact } from '@ds/core/contacts/shared/contact.model';
import { IUserInfo } from '@ajs/user';
import { IReview } from '@ds/performance/reviews';
import { PerformanceManagerService } from '@ds/performance/performance-manager/performance-manager.service';


@Component({
  selector: 'ds-approval-process-summary-dialog',
  templateUrl: './approval-process-summary-dialog.component.html',
  styleUrls: ['./approval-process-summary-dialog.component.scss']
})
export class ApprovalProcessSummaryDialogComponent implements OnInit {

  evaluation: IEvaluationWithStatusInfo;
  review: IReview;
  approver: IContact;
  user: IUserInfo;
  isLoading = true;

  constructor(
    private dialogRef: MatDialogRef<ApprovalProcessSummaryDialogComponent, IEvaluationSummaryDialogResult>,
    @Inject(MAT_DIALOG_DATA) public data:any,
    private perfManagerSvc: PerformanceManagerService
  ) { }

  ngOnInit() {
    this.evaluation = this.data.evaluation;
    this.review = this.data.review;
    this.user = this.data.user;
    this.perfManagerSvc.getDirectSupervisors(true).subscribe((data: any[]) => {
      this.approver = data.find(x => { return x.userId == this.evaluation.currentAssignedUserId });
      this.isLoading = false;
    });
  }

  close() {
    this.dialogRef.close({
      evaluation: this.evaluation
    });
  }

}
