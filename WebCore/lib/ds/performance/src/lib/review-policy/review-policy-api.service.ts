import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {
    Observable,
    of,
    BehaviorSubject,
    throwError,
    combineLatest
} from "rxjs";
import {
    catchError,
    tap,
    publishReplay,
    refCount,
    take,
    map,
    withLatestFrom,
} from "rxjs/operators";
import {
    IReviewTemplateDetail,
    IReviewTemplate
} from '../../../../core/src/lib/groups/shared/review-template.model';
import { IReviewProfileBasic } from '../review-profiles/shared/review-profile-basic.model';
import { AccountService } from '@ds/core/account.service';
import { IReview } from '../reviews';
import { IReviewProfile } from './shared/review-profile.model';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { IReviewProfileBasicSetup } from '../review-profiles/shared/review-profile-basic-setup.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { GroupService } from '@ds/core/groups/group.service';
import { Maybe } from '@ds/core/shared/Maybe';
import { Group } from '@ds/core/groups/shared/group.model';
import { ReviewPolicyService } from './review-policy-setup-form/review-policy.service';

@Injectable({
    providedIn: 'root'
})
export class ReviewPolicyApiService {
    BASE_URI = {
        TEMPLATES: 'api/review-templates',
        PROFILES: 'api/review-profiles',
        REVIEWS: 'api/performance/reviews'
    };

    private getReviewTemplatesByClientId$: Observable<IReviewTemplate[]>;
    private reviewProfileCache: {[profileId: number]: Observable<IReviewProfile>} = {};
    private getReviewProfileSetups$: Observable<IReviewProfileBasicSetup[]>;


    private _reviewProfiles: IReviewProfile[];
    reviewProfiles$ = new BehaviorSubject<IReviewProfile[]>(null);

    set reviewProfiles(value) {
        this._reviewProfiles = value;
        this.reviewProfiles$.next(value);
    }

    private _reviews: IReview[];

    reviews$ = new BehaviorSubject<IReview[]>(null);

    private _id: number;

    id$ = new BehaviorSubject<number>(null);

    set cycleId(value: number) {
        this._id = value;
        this.id$.next(value);
    }

    get cycleId() {
        return this.id$.value;
    }

    private _reviewTemplates: IReviewTemplate[];

    reviewTemplates$ = new BehaviorSubject<IReviewTemplate[]>(null);

    private _reviewTemplate: IReviewTemplate;
    reviewTemplate$ = new BehaviorSubject<IReviewTemplate>(null);

    set reviewTemplate(value: IReviewTemplate) {
        this._reviewTemplate = value;
        this.reviewTemplate$.next(value);
    }

    set reviewTemplates(value: IReviewTemplate[]) {
        this._reviewTemplates = value;
        this.reviewTemplates$.next(value);
    }

    set reviews(value: IReview[]) {
        this._reviews = value;
        this.reviews$.next(value);
    }

    get reviews() {
        return this.reviews;
    }

    constructor(
        private http: HttpClient,
        private account: AccountService,
        private msg:DsMsgService
        ) { }

    /**
     *
     * @description
     * Gets Review Profiles that aren't archived.
     *
     * @param {boolean} isArchived
     *
     * @returns {IReviewProfile} @see IReviewProfile
     *
     */
    getReviewProfilesFull(isArchived?: boolean) {
        let params = new HttpParams();

        if (!isArchived) {
            params = params.append('isArchived', isArchived.toString());
        }

        return this.http.get<IReviewProfile[]>(`${this.BASE_URI.PROFILES}`, { params: params }).pipe(
            catchError(this.httpError('Get Review Profile Falied.', <IReviewProfile[]>[]))
        );
    }

    /**
     *
     * @description
     * Need to fill in description.
     *
     * @param {number} reviewPolicyId
     * The selected review cycle's id.
     *
     * @param {number} reviewTemplateId
     * The selected review cycle review's id.
     *
     */
    getReviewTemplateDetail(reviewTemplateId: number) {
        return this.http.get<IReviewTemplate>(`${this.BASE_URI.TEMPLATES}/${reviewTemplateId}`).pipe(
            catchError(this.httpError('Get Review Cycle Detail Falied.', <IReviewTemplate>{}))
        );
    }

    /**
     *
     * @description
     * Gets a review cycle review by review cycle id
     *
     * @param {number} clientId
     * The selected client's id.
     *
     * @returns {IReviewTemplate[]}
     *
     */
    readonly getReviewTemplatesByClientId = (clientId: number, includeArchived: boolean = false, reload: boolean = false) => {
        return this.getReviewTemplatesByClientId$ == null || reload == true ? this.getReviewTemplatesByClientId$ = this.http.get<IReviewTemplate[]>(`${this.BASE_URI.TEMPLATES}/client/${clientId}/include-archived/${includeArchived}`).pipe(
            publishReplay(1),
            refCount(),
            tap(response => this.reviewTemplates$.next(response))
        ) : this.getReviewTemplatesByClientId$;
    }

    /**
     *
     * @description
     * Gets all review profiles.
     *
     * @param {boolean} isArchived
     * Includes profiles that are archived
     *
     * @returns {IReviewProfileBasic}
     *
     */
    getReviewProfiles(isArchived = false) {
        let params = new HttpParams().append("isArchived", isArchived.toString());
        return this.http.get<IReviewProfileBasic[]>(`${this.BASE_URI.PROFILES}`, { params: params }).pipe(
            catchError(this.httpError('Get Profiles Falied', <IReviewProfileBasic[]>[]))
        );
    }

               /**
     *
     * @description
     * Need to fill in description.
     *
     * @param {number} reviewTemplateId
     * The selected review template's id.
     *
     */
    getReviewTemplateForEmployee(reviewTemplateId: number, employeeId: number) {
        return this.http.get<IReviewTemplate>(`${this.BASE_URI.TEMPLATES}/reviews/${reviewTemplateId}/employee/${employeeId}`).pipe(
            catchError(error => {
                let errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;

            this.account.log(error, `Get Review Cycle Review Detail For Employee Falied. failed: ${errorMsg}`);

            this.msg.setTemporaryMessage(
                `Sorry, this operation failed: ${errorMsg}`,
                MessageTypes.error,
                6000
            );
            return throwError(error);
            })
        );
    }

    readonly getReviewProfileSetups = (ids: number[]) => {
        if (this.getReviewProfileSetups$ != null) {
            return this.getReviewProfileSetups$;
        }

        return this.getReviewProfileSetups$ = this.account.PassUserInfoToRequest(info => this.http.post<IReviewProfileBasicSetup[]>(`${this.BASE_URI.PROFILES}/client/${info.clientId || info.lastClientId}`, ids))
            .pipe(
                publishReplay(),
                refCount());
    }

    /**
     *
     * @description
     * Stuff
     *
     * @param {number} clientId
     * @param {number} employeeId
     *
     * @returns {IReview}
     *
     */
    getReviews(clientId?: number, employeeId?: number) {
        return this.http.get<IReview[]>(this.BASE_URI.REVIEWS).pipe(
            publishReplay(1),
            refCount(),
            catchError(this.httpError('Get Reviews', <IReview[]>[])),
            tap(response => this.reviews$.next(response),
            take(1))
        );
    }

    /**
     *
     * @description
     * Saves a review profile cycle.
     *
     * @param {any} data
     * Review Cycle Profile data being saved.
     *
     * @returns {IReviewPolicyDetail}
     *
     */
    saveReviewProfileCycle(data: any): Observable<IReviewTemplate> {
        const url = `${this.BASE_URI.TEMPLATES}`;
        return this.http.post<IReviewTemplate>(url, data).pipe(
            catchError(this.httpError('Save Review Profile Failed.', <IReviewTemplate>{}))
        );
    }

    /**
     *
     * @description
     * Saves the review template
     *
     *
     */
    saveReviewTemplate(input: IReviewTemplate, groupSvc: GroupService, policySvc: ReviewPolicyService): Observable<IReviewTemplate> {
        return this.http.post<IReviewTemplate>(`${this.BASE_URI.TEMPLATES}`, input).pipe(
            withLatestFrom(groupSvc.groups$),
            tap(x => {
                const groups = new Maybe(x[1]);
                const template = x[0];
                groups.valueOr([] as Group[]).forEach(group => {
                    const templates = new Maybe(group.reviewTemplates).valueOr([] as number[]);
                    const groupRefToTemplate = templates.find(temp => template.reviewTemplateId == temp);
                    const tempRefToGroup = template.groups.find(tempGroup => group.groupId === tempGroup);
                    if(groupRefToTemplate == null && tempRefToGroup != null){
                        group.reviewTemplates.push(template.reviewTemplateId)
                    }

                    if(groupRefToTemplate != null && tempRefToGroup == null){
                        const locationToRemove = group.reviewTemplates.indexOf(template.reviewTemplateId);
                        group.reviewTemplates.splice(locationToRemove, 1);
                    }
                });
                groupSvc.ReactToTemplateUpdated(groups.value(), policySvc);
            }),
            map(x => x[0]));
    }

    deleteReviewTemplate(id: number): Observable<void> {
        return this.http.delete<void>(`${this.BASE_URI.TEMPLATES}/${id}`);
    }

    /**
     *
     * @description
     * Closes the selected review template if no profiles are attached.
     *
     * @param {number} id
     * Id of the selected review template to close.
     *
     */
    archiveReviewTemplate(id) {
        return this.http.post<IReviewTemplate>(`${this.BASE_URI.TEMPLATES}/archive/${id}`, {}).pipe(
            catchError(this.httpError('Could not archive review template.', <IReviewTemplate>{}))
        )
    }

    /**
     *
     * @description
     * Gets the selected profiles setup details.
     *
     * @param {number} input
     *
     */
    getProfileSetup(input: number, reload: boolean = false) {

        if(!(this.reviewProfileCache[input] == null || reload == true)){
            return this.reviewProfileCache[input];
        }


        return this.reviewProfileCache[input] = this.http.get<IReviewProfile>(`${this.BASE_URI.PROFILES}/${input}/setup`).pipe(
            catchError(this.httpError('Save Review Cycle Review Failed.', <IReviewProfile>{}))
        );
    }

    private httpError<T>(operation = 'operation', result?:T) {
        return (error:any):Observable<T> => {
            let errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;

            this.account.log(error, `${operation} failed: ${errorMsg}`);

            this.msg.setTemporaryMessage(
                `Sorry, this operation failed: ${errorMsg}`,
                MessageTypes.error,
                6000
            );

            return of(result as T);
        }
    }
}
