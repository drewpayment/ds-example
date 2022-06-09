import { Injectable } from '@angular/core';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import {
    ReviewTemplateDialogComponent
} from './review-template-dialog.component';
import { IContactWithClient } from '@ds/core/contacts';
import { UserInfo } from '@ds/core/shared';
import { IReviewTemplate } from '../../../../../core/src/lib/groups/shared/review-template.model';
import { IReviewTemplateEditDialogData } from '..';
import { ReferenceDate } from '../../../../../core/src/lib/groups/shared/schedule-type.enum';

@Injectable({
    providedIn: 'root'
})
export class ReviewTemplateDialogService {
    constructor(
        private dialog: MatDialog,
    ) {}

    open(reviewPolicys: IReviewTemplate[], employee: IContactWithClient, currentUser: UserInfo) {
        let config = new MatDialogConfig();

        config.data = {
            reviewPolicy: reviewPolicys,
            employee: employee,
            currentUser: currentUser,
            groups: null
        };

        config.width = "60vw";

        return this.dialog.open(ReviewTemplateDialogComponent, config);
    }

    openReview(reviewTemplate: IReviewTemplate, employee: IContactWithClient, currentUser: UserInfo, cycle: any, openRecurringView: boolean, refDateType?: ReferenceDate) {
        let config = new MatDialogConfig();

        config.data = {
            reviewTemplate: reviewTemplate,
            employee: employee,
            currentUser: currentUser,
            cycle: cycle,
            type: refDateType,
            groups: null,
            openRecurringView: openRecurringView
        };

        config.width = '60vw';

        return this.dialog.open<ReviewTemplateDialogComponent, IReviewTemplateEditDialogData, IReviewTemplate>(ReviewTemplateDialogComponent, config);
    }
}
