import { Component, OnInit } from '@angular/core';
import { IFeedbackSetup } from '../shared/feedback-setup.model';
import { FeedbackApiService } from '../feedback-api.service';
import { FeedbackSetupDialogService } from '../feedback-setup-dialog/feedback-setup-dialog.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { HttpErrorResponse } from '@angular/common/http';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import * as _ from "lodash";
import { DefaultFeedbacksDialogComponent } from '../default-feedbacks-dialog/default-feedbacks-dialog.component';
import { MatDialog } from "@angular/material/dialog";
import { UserInfo } from "@ds/core/shared";
import { IFeedback } from '../';
import { AccountService } from "@ds/core/account.service";
import { tap, catchError } from 'rxjs/operators';
import { Pipe, PipeTransform } from '@angular/core';
import { empty } from 'rxjs';

@Component({
    selector: 'ds-feedback-setup-list',
    templateUrl: './feedback-setup-list.component.html',
    styleUrls: ['./feedback-setup-list.component.scss']
})
export class FeedbackSetupListComponent implements OnInit {

    feedbackList: IFeedbackSetup[];
    user: UserInfo;
    feedbacks: IFeedback[];
    displayArchived: boolean = false;

    constructor(
        private feedbackApi: FeedbackApiService,
        private feedbackDialog: FeedbackSetupDialogService,
        private msgSvc: DsMsgService,
        private confirmSvc: DsConfirmService,
        private dialog: MatDialog,
        private account: AccountService,
    ) { }

    ngOnInit() {
        this.feedbackList = [];
        this.account.getUserInfo().subscribe(user => {
            this.user = user;
            this.refreshFeedback();
        });
    }

    addFeedback() {
        this.openFeedbackDialog();
    }

    editFeedback(feedback:IFeedbackSetup) {
        this.openFeedbackDialog(feedback);
    }

    copyFeedback(feedback:IFeedbackSetup) {
        let copy = <IFeedbackSetup>_.cloneDeep(feedback);
        copy.feedbackId = 0;
        copy.body = `(copy) ${feedback.body}`;
        if(copy.feedbackItems) {
            copy.feedbackItems.forEach(i => {
                i.feedbackId = 0;
                i.feedbackItemId = 0;
            });
        }

        this.openFeedbackDialog(copy);
    }

    deleteFeedback(feedback:IFeedbackSetup) {

        const shouldArchive = shouldArchiveFeedback(feedback);

        const bodyText = shouldArchive ?
        "Do you want to archive this item?  Archiving feedback removes it from all future reviews." :
        "Do you want to remove this item?  This item will be removed from all future reviews and is not recoverable.";

        this.confirmSvc.show(null, {
            bodyText: bodyText,
            swapOkClose: true,
            actionButtonText: shouldArchive ? "Archive" : "Remove",
            closeButtonText: "Cancel"
        }).then(result => {
            this.msgSvc.loading(true);

            this.feedbackApi.removeFeedbackSetup(feedback.feedbackId, shouldArchive)
                .subscribe(() => {
                    this.msgSvc.setTemporaryMessage("Feedback removed successfully");
                    if(shouldArchive){
                        feedback.isArchived = true;
                    } else {
                        _.remove(this.feedbackList, f => f === feedback);
                    }
                },
                (error:HttpErrorResponse) => {
                    this.msgSvc.showWebApiException(error.error);
                });
        });
    }

    restoreFeedback(feedback: IFeedbackSetup) {
        feedback.isArchived = false;
        this.msgSvc.loading(true);
        this.feedbackApi.saveFeedbackSetup(feedback).pipe(
            tap(result => {
                for (var i = 0; i < this.feedbackList.length; i++) {
                    const current = this.feedbackList[i];
                    if (current.feedbackId === result.feedbackId) {
                        this.feedbackList[i] = result;
                        break;
                    }
                }
                this.msgSvc.setTemporarySuccessMessage('Successfully restored feedback.', 5000);
            }),
            catchError((error: HttpErrorResponse) => {
                this.msgSvc.showWebApiException(error.error);
                return empty();
            })).subscribe();
    }

    private openFeedbackDialog(feedback?: IFeedbackSetup) {
        let dialog = this.feedbackDialog.open(feedback, feedback && !feedback.feedbackId);

        dialog.afterClosed().subscribe(result => {
            if (result && result.feedback) {
                this.refreshFeedback();
            }
        });
    }

    private refreshFeedback() {
        this.feedbackApi.getFeedbackSetup()
            .subscribe(data => {
                this.feedbackList = data.sort((a, b) => {
                    return a.body.toLowerCase().localeCompare(b.body.toLowerCase());
                });
            });
    }

    toggleArchivedFeedback() {
        this.displayArchived = !this.displayArchived;
    }

    openAvailableFeedbacksDialog(): void {
        const ref = this.dialog.open(DefaultFeedbacksDialogComponent, {
            width: '800px',
            data: {
                user: this.user,
                selected: this.feedbackList
            }
        });

        ref.afterClosed().subscribe(result => {
            if (result == null) return;

            if (this.feedbackList == null) this.feedbackList = [];

            for (let i = 0; i < result.length; i++) {
                this.feedbackList.push(result[i]);
            }

            this.feedbackList = this.sortFeedbacks(this.feedbackList);
        });
    }

    private sortFeedbacks(fbs: IFeedbackSetup[]): IFeedbackSetup[] {
        return _.sortBy(fbs, ['body']);
    }
}



function shouldArchiveFeedback(feedback: IFeedbackSetup): boolean {
    return !!((feedback.reviewProfileEvaluations && feedback.reviewProfileEvaluations.length) || (feedback.feedbackResponses && feedback.feedbackResponses.length))
}

@Pipe({name: 'shouldArchiveFeedback'})
export class ShouldArchiveFeedbackPipe implements PipeTransform {
    transform = shouldArchiveFeedback;
}
