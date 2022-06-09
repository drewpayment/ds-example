import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { IContactWithClient, IProfileImageDetail } from '@ds/core/contacts';
import { EmployeeSearchService } from '@ajs/employee/search/shared/employee-search.service';
import { EmployeeSearchContext } from '@ajs/employee/search/shared/employee-search-context';
import { IEmployeeSearchResult } from '@ajs/employee/search/shared/models';
import { ActiveEvaluationService } from '@ds/performance/evaluations/shared/active-evaluation.service';
import { IEvaluationSelectedData } from '@ds/performance/reviews/review-list/review-list.component';

@Component({
    selector: 'ds-employee-reviews-outlet',
    templateUrl: './employee-reviews-outlet.component.html',
    styleUrls: ['./employee-reviews-outlet.component.scss']
})
export class EmployeeReviewsOutletComponent implements OnInit, OnDestroy {

    activeEmployee: { contact: IContactWithClient, empData: IEmployeeSearchResult };
    private _searchContext: EmployeeSearchContext;
    constructor(
        private activeEvaluationSvc: ActiveEvaluationService,
        private router: Router,
        private employeeSearch: EmployeeSearchService
    ) { }

    ngOnInit() {
        this._searchContext = this.employeeSearch.getCurrentSearchContext();
        this._searchContext.registerActiveEmployeeSetChangeHandler(this.setEmployeeFromContext);
        this.setEmployeeFromContext();
    }
    ngOnDestroy(): void {
        this._searchContext.unregisterActiveEmployeeSetChangeHandler(this.setEmployeeFromContext);
    }
    viewEvaluation(event: IEvaluationSelectedData) {
        this.activeEvaluationSvc.setActiveEvaluation(event.evaluation, event.review);
        //this.activeEvaluationSvc.returnUrl = ['/performance/employees/reviews']; -- set by default
        this.router.navigate(['performance', 'evaluations', event.evaluation.evaluationId]);
    }

    private setEmployeeFromContext = () => {

        if (this._searchContext.activeEmployee) {
            let ee = this._searchContext.activeEmployee;
            let img = ee.profileImage;

            this.activeEmployee = {
                contact: {
                    clientId: ee.clientId,
                    employeeId: ee.employeeId,
                    firstName: ee.firstName,
                    lastName: ee.lastName,

                    profileImage: img ? {
                        clientGuid: img.clientGuid,
                        clientId: img.clientId,
                        employeeGuid: img.employeeGuid,
                        employeeId: img.employeeId,
                        sasToken: img.sasToken,
                        profileImageInfo: <any>img.profileImageInfo,
                    } : null
                },
                empData: ee
            }
        }
        else {
            this.activeEmployee = null;
        }
    }
}
