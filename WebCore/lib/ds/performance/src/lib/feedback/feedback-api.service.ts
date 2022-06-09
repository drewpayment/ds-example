import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IFeedbackSetup} from './shared/feedback-setup.model';

@Injectable({
    providedIn: 'root'
})
export class FeedbackApiService {
    private BASE_API = 'api/performance/feedback';

    constructor(private http: HttpClient) { }

    /**
     * Gets the list of feedback questions with setup info for a client. 
     * @param clientId (Optional) If not specified the current user's client will be assumed.
     */
    getFeedbackSetup(clientId?: number, isArchived?: boolean) {
        let options = {
            params: null
        };

        if (clientId) {
            options.params = new HttpParams().set("clientId", clientId.toString());
        }

        if(isArchived != null){
            options.params = new HttpParams().set("isArchived", isArchived.toString());
        }

        return this.http.get<IFeedbackSetup[]>(`${this.BASE_API}`, options);
    }

    /** 
     * Saves changes to a particular feedback question.
     */
    saveFeedbackSetup(feedback: IFeedbackSetup) {
        let url = `${this.BASE_API}`;
        if (feedback.feedbackId)
            url += `/${feedback.feedbackId}`;

        return this.http.post<IFeedbackSetup>(url, feedback);
    }

    /**
     * Attempts to remove a feedback question.
     */
    removeFeedbackSetup(feedbackId: number, isLogicalDelete?: boolean) {
        const options = {
            params: new HttpParams().set('isLogicalDelete', (!!isLogicalDelete).toString())
        }

        return this.http.delete(`${this.BASE_API}/${feedbackId}`, options);
    }

    getDefaultFeedbacks(clientId: number){
        const url = this.BASE_API + '/clients/' + clientId + '/default-feedbacks';
        return this.http.get<IFeedbackSetup[]>(url);
    }

    duplicateDefaultFeedbacks(clientId: number,feedbackIdList: number[]){
        const url = `${this.BASE_API}/clients/${clientId}/feedbacks/duplicates`;
        return this.http.post<IFeedbackSetup[]>(url, feedbackIdList);
    }
}
