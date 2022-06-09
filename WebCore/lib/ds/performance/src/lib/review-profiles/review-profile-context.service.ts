import { Injectable } from '@angular/core';
import { ReviewProfilesApiService } from './review-profiles-api.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { IReviewProfileBasic } from './shared/review-profile-basic.model';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { map } from 'rxjs/operators';
import { IReviewProfileSetup } from '.';

@Injectable({
    providedIn: 'root'
})
export class ReviewProfileContextService {
    private _user: UserInfo;
    private _activeReviewProfileSetupSource = new BehaviorSubject<IReviewProfileSetup>(null);
    activeReviewProfileSetup$ = this._activeReviewProfileSetupSource.asObservable();

    constructor(private reviewProfileApi: ReviewProfilesApiService, private accountSvc: AccountService) {
        this.accountSvc.getUserInfo().subscribe(user => this._user = user);
    }

    addReviewProfile(): Observable<IReviewProfileSetup> {
        return new Observable((observer) => {
            let newProfile = <IReviewProfileSetup>{ clientId: this._user.selectedClientId() };
            this._activeReviewProfileSetupSource.next(newProfile);
            return observer.next(newProfile);
        })
    }

    loadActiveReviewProfileSetup(reviewProfileId: number) {
        return this.reviewProfileApi.getReviewProfileSetup(reviewProfileId).pipe(map(profile => {
            this._activeReviewProfileSetupSource.next(profile);
            return profile;
        }));
    }
}
