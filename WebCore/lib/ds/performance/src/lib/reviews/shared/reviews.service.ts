import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { IReview, IReviewWithEmployees } from './review.model';
import { map, take, concatMap } from 'rxjs/operators';
import { AccountService } from '@ds/core/account.service';
import { ContactProfileImageLoader, ContactsProfileImageLoader, IContactSearchOptions, ContactSearchOptions, IContactSearchResult, ContactSearchResultProfileImageLoader, IContact } from '@ds/core/contacts';
import { IReviewTemplate } from '@ds/core/groups/shared/review-template.model';

@Injectable({
    providedIn: 'root'
})
export class ReviewsService {

    static readonly REVIEWS_API_BASE = "api/performance/reviews";

    constructor(private http: HttpClient,
        private accountSvc: AccountService) { }

    getReviewsByReviewId(reviewId: number) {
        return this.http.get<IReview>(`${ReviewsService.REVIEWS_API_BASE}/${reviewId}`);
    }
    getReviewsByEvaluationId(evaluationId: number) {
        return this.http.get<IReview>(`${ReviewsService.REVIEWS_API_BASE}/evaluations/${evaluationId}`);
    }

    /**
     * Gets performance reviews for an employee.
     */
    getEmployeeReviews(employeeId: number) {

        if(!employeeId)
            return throwError("Employee must be specified.");

        let options = { 
            params: new HttpParams().set("employeeId", employeeId.toString())
        };

        return this.http.get<IReview[]>(`${ReviewsService.REVIEWS_API_BASE}`, options)
            .pipe(map(reviews => {
                reviews.forEach(review => {
                    ContactProfileImageLoader(review.reviewedEmployeeContact);
                    ContactProfileImageLoader(review.reviewOwnerContact);

                    if(review.evaluations) {
                        review.evaluations.forEach(evl => ContactProfileImageLoader(evl.evaluatedByContact));
                    }

                    if(review.meetings) {
                        /** the typescript error was bugging me... Drew */
                        review.meetings.forEach(meeting => ContactsProfileImageLoader(meeting.attendees as IContact[]));
                    }
                });

                return reviews;
            }));
    }

    getReviewScore(reviewId: number): Observable<{data: number}> {
return this.accountSvc.getUserInfo().pipe(
    take(1), 
    concatMap(x => this.http.get<{data: number}>('api/performance/clients/' + (x.lastClientId || x.clientId) + '/review/' + reviewId + '/score')));
    }

    /**
     * Creates a new review or saves changes to an existing review (per ReviewId).
     * @param review Review to add and/or update.
     */
    saveReview(review: IReview) {
        if (!review)
            return throwError("Review must be specified.");
        if (!review.clientId)
            return throwError("Client must be specified.");
        return review.reviewId ? 
            this.http.post<IReview>(`${ReviewsService.REVIEWS_API_BASE}/${review.reviewId}`, review) :
            this.http.put<IReview>(`${ReviewsService.REVIEWS_API_BASE}`, review);

    // public class ReviewProfileSetupDto : ReviewProfileBasicDto
    // {
    //     public string DefaultInstructions    { get; set; }
    //     public bool   IncludeReviewMeeting   { get; set; }
    //     public bool   IncludeScoring         { get; set; }
    //     public bool   IncludePayrollRequests { get; set; }
    //
    //     public IEnumerable<ReviewProfileEvaluationDto> Evaluations { get; set; }
    // }
    
    }
    
    saveReviewWithEmployeeList(review:{ calendarYear: IReviewWithEmployees, dateOfHire: IReviewTemplate, owner: number, supervisor: number}):Observable<IReview[]> {
        const url = `${ReviewsService.REVIEWS_API_BASE}/group-scheduler`;
        const params = new HttpParams().set('reviewId', review.calendarYear.reviewId != null ? review.calendarYear.reviewId.toString() : '');
        return this.http.post<IReview[]>(url, review, { params: params });
    }

    /**
     * Gets contacts available to use when setting up a review.
     */
    getReviewSetupContacts(options: IContactSearchOptions = null) {
        let params = new HttpParams();

        if (options) {
            params = new ContactSearchOptions(options).toHttpParams();             
        }       

        return this.http.get<IContactSearchResult>(`${ReviewsService.REVIEWS_API_BASE}/contacts`, { params: params })
            .pipe(map(ContactSearchResultProfileImageLoader));
    }
    
}
