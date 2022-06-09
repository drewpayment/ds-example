import { EventEmitter } from '@angular/core';
import { ICalendarYearForm } from '@ds/performance/shared/forms/calendar-year-form/calendar-year-form.model';
import { IReviewTemplate } from './review-template.model';

export interface IReviewTemplateFormComponent {
    SetForm: ICalendarYearForm;
    Submitted: boolean;
    FormValue: EventEmitter<IReviewTemplate>;
}