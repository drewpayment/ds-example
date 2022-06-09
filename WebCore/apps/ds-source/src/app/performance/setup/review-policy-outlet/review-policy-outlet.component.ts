import { Component, OnInit } from '@angular/core';
import {
    Router,
    NavigationEnd
} from '@angular/router';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { IContactWithClient } from '@ds/core/contacts';
import { UserInfo } from '@ds/core/shared';
import { Observable } from 'rxjs';
import { IReview } from '@ds/performance/reviews/shared/review.model';
import { IReviewTemplate } from '@ds/core/groups/shared/review-template.model';
import { ReviewPolicyApiService } from '@ds/performance/review-policy/review-policy-api.service';
import { ReviewTemplateDialogService } from '@ds/performance/review-policy/review-template-dialog/review-template-dialog.service';
import { AccountService } from '@ds/core/account.service';

@Component({
    selector: 'ds-review-policy-outlet',
    templateUrl: './review-policy-outlet.component.html',
    styleUrls: ['./review-policy-outlet.component.scss']
})
export class ReviewPolicyOutletComponent implements OnInit {
    $reviews: Observable<IReview[]>;
    isListComponent:boolean = false;
    isEditComponent:boolean = false;
    reviewProfiles: IReviewTemplate[];
    reviews: IReview[];
    currentUser: IContactWithClient;
    private _user: UserInfo;
    _id: number;

    hasReviewProfiles: boolean;

    constructor(
            private router: Router,
            private reviewPolicysService: ReviewPolicyApiService,
            public dialog: ReviewTemplateDialogService,
            private messageService: DsMsgService,
            private _accountService: AccountService        ) {
            this.router.events.subscribe((event) => {
                if (event instanceof NavigationEnd) {
                    this.isListComponent = this.router.url.includes('list');
                    this.isEditComponent = this.router.url.includes('edit');
                }
            })
    }

    ngOnInit() {
        this.getReviews();

        this._accountService.getUserInfo().subscribe(user => {
            this._user, this.currentUser = user;
        });

        // this.reviewProfiles = this.reviewPolicysService.reviewPolicyReviews$;

        // this.reviewPolicysService.getReviewCycleReviews(this.review.reviewPolicyId, this.review.reviewId).subscribe(profile => {
        //     this.reviewProfiles = profile;
        // });
    }

    getReviews() {
        this.reviewPolicysService.getReviews().subscribe(reviews => {
            this.reviews = reviews;
            console.log('Outlet Component\n', this.reviews);
        });
    }

    // openDialog() {
    //     this.dialog.open(this.review, this.currentUser, this._user)
    //         .afterClosed()
    //         .subscribe(result => console.log('From Dialog Subscriber...\n', result));
    // }

    recieveMessage($event) {
        console.log('Message Receiver: \r\n', $event);
        this.reviewProfiles = $event;
    }

    saveCycle() {

    }

    cancel() {
        this.messageService.loading(false);
        this.router.navigate(["performance", "setup", "review-policy", "list"]);
    }
}
