import { Injectable } from '@angular/core';
import { IEvaluationDetail } from '../shared/evaluation-detail.model';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { IEvaluationSummaryDialogData } from './evaluation-summary-dialog-data.model';
import { EvaluationSummaryDialogComponent } from './evaluation-summary-dialog.component';
import { IEvaluationSummaryDialogResult } from './evaluation-summary-dialog-result.model';
import { IReview } from '@ds/performance/reviews';
import { IChartSettings } from '../shared/chart-settings.model';
import { IEvaluationWithStatusInfo } from '../shared/evaluation-status-info.model';

@Injectable({
    providedIn: 'root'
})
export class EvaluationSummaryDialogService {

    constructor(private dialog: MatDialog) { }

    open(evaluation: IEvaluationWithStatusInfo, review: IReview, score: number) {
        let config = new MatDialogConfig<IEvaluationSummaryDialogData>();
        config.data = {
            evaluation: evaluation,
            review: review,
            score: score
        };

        config.width = "80vw";

        return this.dialog.open<EvaluationSummaryDialogComponent, IEvaluationSummaryDialogData, IEvaluationSummaryDialogResult>(EvaluationSummaryDialogComponent, config);
    }
}
