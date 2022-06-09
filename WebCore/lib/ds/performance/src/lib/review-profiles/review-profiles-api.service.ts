import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IReviewProfileBasic } from './shared/review-profile-basic.model';
import { IReviewProfileSetup } from './shared/review-profile-setup.model';

@Injectable({
    providedIn: 'root'
})
export class ReviewProfilesApiService {

    private BASE_URI = "api/review-profiles";

    constructor(private http: HttpClient) { }

    /**
     * Gets basic review profile info for the current user's client.
     */
    getReviewProfiles(isArchived = false) {
        let params = new HttpParams().append("isArchived", isArchived.toString());

        return this.http.get<IReviewProfileBasic[]>(`${this.BASE_URI}`, { params: params });
    }

    /**
     * Gets full setup info for a review profile.
     */
    getReviewProfileSetup(reviewProfileId: number) {
        return this.http.get<IReviewProfileSetup>(`${this.BASE_URI}/${reviewProfileId}/setup`);
    }

    /**
     * Saves setup changes to a new or existing review profile.
     */
    saveReviewProfileSetup(setup: IReviewProfileSetup) {
        let uri = this.BASE_URI;
        if (setup.reviewProfileId)
            uri += `/${setup.reviewProfileId}`;
        return this.http.post<IReviewProfileSetup>(uri, setup);
    }

    archiveReviewProfile(reviewProfileId: number) {
        return this.http.post(`${this.BASE_URI}/${reviewProfileId}/archive`, null);
    }
    
    restoreReviewProfile(reviewProfileId: number) {
        return this.http.post(`${this.BASE_URI}/${reviewProfileId}/restore`, null);
    }
}
