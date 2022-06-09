import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FeedbackApiService } from "../feedback-api.service";
import { Subject, ReplaySubject } from "rxjs";

import * as _ from 'lodash';
import { UserInfo } from "@ds/core/shared";
import { IFeedbackSetup } from "../";

export interface DefaultFeedbacksDialogData {
    user: UserInfo,
    selected: IFeedbackSetup[]
}

export interface SelectedPerformanceFeedback extends IFeedbackSetup {
    selected?:boolean
}

@Component({
    selector: 'ds-default-feedbacks-dialog',
    templateUrl: './default-feedbacks-dialog.component.html',
    styleUrls: ['./default-feedbacks-dialog.component.scss']
})
export class DefaultFeedbacksDialogComponent implements OnInit {
    user:UserInfo;
    userSelectedFeedbacks: IFeedbackSetup[];
    feedbacks: SelectedPerformanceFeedback[];
    feedbacks$: Subject<SelectedPerformanceFeedback[]> = new ReplaySubject(1);
    isLoading = true;

    get hasFeedbacks() {
        return this.isLoading || (this.feedbacks && this.feedbacks.length)
    }

    constructor(
        public dialogRef: MatDialogRef<DefaultFeedbacksDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:DefaultFeedbacksDialogData,
        private service: FeedbackApiService
    ) {}

    ngOnInit() {
        this.user = this.data.user;
        this.userSelectedFeedbacks = this.data.selected;

        this.service.getDefaultFeedbacks(this.user.lastClientId || this.user.clientId)
            .subscribe(feedbacks => {
                _.remove(feedbacks, (c: IFeedbackSetup) => {
                    return _.find(this.userSelectedFeedbacks, { 'body': c.body }) != null;
                });
                this.feedbacks = this.sortFeedbacks(feedbacks);
                this.feedbacks$.next(this.feedbacks);
                this.isLoading = false;
            });
    }

    saveSelectedFeedbacks():void {
        let selected = _.filter(this.feedbacks, { 'selected': true }).map(s => s.feedbackId);
        this.service.duplicateDefaultFeedbacks(this.user.selectedClientId(), selected)
            .subscribe(feedbacks => {
                this.dialogRef.close(feedbacks);
            });
    }

    selectAllFeedbacks(event):void {
        for (let i = 0; i < this.feedbacks.length; i++) {
            this.feedbacks[i].selected = event.target.checked;
        }
        this.feedbacks$.next(this.feedbacks);
    }

    onNoClick():void {
        this.dialogRef.close();
    }

    private sortFeedbacks(fbs: IFeedbackSetup[]): IFeedbackSetup[] {
        return _.sortBy(fbs, ['isSupervisor','body']);
    }
}
