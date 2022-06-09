import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { IContactWithClient } from '@ds/core/contacts';
import { DsStyleLoaderService } from '@ajs/ui/ds-styles/ds-styles.service';
import { Router } from '@angular/router';
import { IEmployeeSearchResult } from '@ajs/employee/search/shared/models';
import { IEvaluationSelectedData } from '@ds/performance/reviews/review-list/review-list.component';
import { ActiveEvaluationService } from '@ds/performance/evaluations/shared/active-evaluation.service';

@Component({
    selector: 'ds-reviews',
    templateUrl: './reviews.component.html',
    styleUrls: ['./reviews.component.scss']
})
export class EmployeeReviewsComponent implements OnInit, AfterViewChecked {

    employee: { contact: IContactWithClient, empData: IEmployeeSearchResult };;
    showArchivedReviews = false;

    constructor(
        private accountSvc: AccountService,
        private styles: DsStyleLoaderService,
        private activeEvaluationSvc: ActiveEvaluationService,
        private router: Router) { }

    ngOnInit() {
        this.accountSvc.getUserInfo().subscribe(u => {
            this.employee = {
                contact: {
                clientId:   u.selectedClientId(),
                employeeId: u.employeeId,
                firstName:  u.firstName,
                lastName:   u.lastName,
                userId:     u.userId
                },
                empData: <IEmployeeSearchResult>{

                }
            }
        })
    }

    ngAfterViewChecked() {
        this.styles.useMainStyleSheet();
    }

    selectEvaluation(selection: IEvaluationSelectedData) {
        this.activeEvaluationSvc.setActiveEvaluation(selection.evaluation, selection.review);
        this.activeEvaluationSvc.returnUrl = ['/performance/reviews'];
        this.router.navigate(['performance', 'evaluations', selection.evaluation.evaluationId]);
    }
}
