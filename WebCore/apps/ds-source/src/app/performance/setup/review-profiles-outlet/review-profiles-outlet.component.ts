import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IReviewProfileSetup } from '@ds/performance/review-profiles';

@Component({
    selector: 'ds-review-profiles-outlet',
    templateUrl: './review-profiles-outlet.component.html',
    styleUrls: ['./review-profiles-outlet.component.scss']
})
export class ReviewProfilesOutletComponent implements OnInit {

    constructor(private router: Router) { }

    ngOnInit() {
    }

    onProfileEdit(profile: IReviewProfileSetup) {
        this.router.navigate(['performance','setup','review-profiles','edit']);
    }
}
