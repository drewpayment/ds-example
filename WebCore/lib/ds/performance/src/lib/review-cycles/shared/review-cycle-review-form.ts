import { IReviewCycleReview } from './review-cycle-review.model';
import { IReviewProfileSetup } from '@ds/performance/review-profiles';
import { EventEmitter } from '@angular/core';
import { ICalendarYearForm } from '@ds/performance/shared/forms/calendar-year-form/calendar-year-form.model';

export interface IReviewCycleReviewFormComponent {
    SetForm: ICalendarYearForm;
    Submitted: boolean;
    FormValue: EventEmitter<IReviewCycleReview>;
}