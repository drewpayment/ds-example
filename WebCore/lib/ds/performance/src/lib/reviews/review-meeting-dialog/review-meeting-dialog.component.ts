import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { NgForm, FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IReview } from '../shared/review.model';
import { IReviewMeetingDialogData } from './review-meeting-dialog-data.model';
import { IReviewMeetingDialogResult } from './review-meeting-dialog-result.model';
import { ReviewsService } from '../shared/reviews.service';
import { IReviewMeeting } from '../shared/review-meeting.model';
import { IMeetingAttendee } from '@ds/core/meetings';
import { Observable, throwError } from 'rxjs';
import { startWith, debounceTime, concatMap, map, catchError } from 'rxjs/operators';
import * as moment from "moment";
import { DsApiCommonProvider } from '@ajs/core/api/ds-api-common.provider';
import { HttpErrorResponse } from '@angular/common/http';
import * as _ from "lodash";
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { EvaluationRoleType } from '@ds/performance/evaluations/shared/evaluation-role-type.enum';
import { IContact } from '@ds/core/contacts/shared/contact.model';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';

@Component({
    selector: 'ds-review-meeting-dialog',
    templateUrl: './review-meeting-dialog.component.html',
    styleUrls: ['./review-meeting-dialog.component.scss']
})
export class ReviewMeetingDialogComponent implements OnInit {

    review: IReview;
    reviewMeeting: IReviewMeeting;
    reviewMeetingDates = {
        $startTime: null
    };
    hasReviewMeeting: boolean;
    attendees: Observable<IContact[]>;
    empContactAttendee: IMeetingAttendee;

    @ViewChild('meetingAttendeesInput', { static: true }) meetingAttendeesInput: ElementRef<HTMLInputElement>;
    meetingAttendeesCtrl = new FormControl();

    constructor(
        private dialogRef: MatDialogRef<ReviewMeetingDialogComponent, IReviewMeetingDialogResult>,
        @Inject(MAT_DIALOG_DATA)
        private dialogData: IReviewMeetingDialogData,
        private reviewSvc: ReviewsService,
        private msgSvc: DsMsgService
    ) { }

    ngOnInit() {
        this.review = this.dialogData.review;
        this.review.meetings = this.review.meetings || [];

        let existingMeeting = _.first(this.review.meetings);
        this.reviewMeeting = existingMeeting ? _.cloneDeep(existingMeeting) : <IReviewMeeting>{
            reviewId: this.review.reviewId,
            startDateTime: null,
            endDateTime: null,
            location: null,
            attendees: []
        };

        if (!this.reviewMeeting.meetingId) {
            let employeeContact = this.review.reviewedEmployeeContact;

            this.empContactAttendee = <IMeetingAttendee>{
                employeeId: employeeContact.employeeId,
                userId:     employeeContact.userId,
                firstName:  employeeContact.firstName,
                lastName:   employeeContact.lastName
            };

            this.reviewMeeting.attendees.push(this.empContactAttendee)

            this.review.evaluations.forEach(x => {
                if(x.role !== EvaluationRoleType.Self)
                    this.reviewMeeting.attendees.push(Object.assign(<IMeetingAttendee>{}, x.evaluatedByContact))
            });
        }
        this.setReviewMeetingStartTime();

        this.attendees = this.meetingAttendeesCtrl.valueChanges.pipe(
            startWith(null),
            debounceTime(500),
            concatMap((searchText: string) => this.reviewSvc.getReviewSetupContacts({ page: 1, pageSize: 75, searchText: searchText, excludeTimeClockOnly: true, haveActiveEmployee: true, ifSupervisorGetSubords: true})),
            map(searchResults => this._filterContacts(searchResults.results))
        );
    }

    meetingStartDateChanged() {

        if(!this.reviewMeeting.startDateTime)
            return;

        let mDate = moment(this.reviewMeeting.startDateTime);

        if(this.reviewMeetingDates.$startTime) {
            this.reviewMeetingDates.$startTime = moment(this.reviewMeetingDates.$startTime).set({
                'date':  mDate.get('date'),
                'month': mDate.get('month'),
                'year':  mDate.get('year'),
            });

            this.reviewMeeting.startDateTime = mDate.set({
                'hour':   this.reviewMeetingDates.$startTime.get('hour'),
                'minute': this.reviewMeetingDates.$startTime.get('minute'),
                'second': this.reviewMeetingDates.$startTime.get('second')
            });
        }

        if(this.reviewMeeting.endDateTime) {
            this.reviewMeeting.endDateTime = moment(this.reviewMeeting.endDateTime).set({
                'date':  mDate.get('date'),
                'month': mDate.get('month'),
                'year':  mDate.get('year'),
            });
        }
    }

    addAttendee($event: MatAutocompleteSelectedEvent) {
        let attendee = <IMeetingAttendee>$event.option.value;

        if(!this.reviewMeeting.attendees)
            this.reviewMeeting.attendees = [];

        this.reviewMeeting.attendees.push(attendee);

        this.meetingAttendeesInput.nativeElement.value = '';
        this.meetingAttendeesCtrl.setValue(null);
    }

    removeAttendee(attendee: IMeetingAttendee) {
        _.remove(this.reviewMeeting.attendees, a => a.userId === attendee.userId && a.employeeId === attendee.employeeId);
        this.meetingAttendeesInput.nativeElement.focus();
    }

    save(form: NgForm) {
        if (form.valid) {
            this.review.isReviewMeetingRequired = true;
            this.reviewMeeting.title         = `${this.review.reviewedEmployeeContact.firstName} ${this.review.reviewedEmployeeContact.lastName} - ${this.review.title}`;
            this.reviewMeeting.startDateTime = moment(this.reviewMeetingDates.$startTime).format(DsApiCommonProvider.TimeFormat.DATETIME);
            this.reviewMeeting.endDateTime   = moment(this.reviewMeeting.endDateTime).format(DsApiCommonProvider.TimeFormat.DATETIME);
            this.review.meetings             = [ this.reviewMeeting ];

            this.msgSvc.loading(true);

            this.reviewSvc.saveReview(this.review)
                .pipe(catchError((err:HttpErrorResponse, caught) => {
                    this.msgSvc.showWebApiException(err.error);
                    return throwError("Error saving review");
                }))
                .subscribe(review => {
                    this.dialogRef.close({review: review, reviewMeeting: this.reviewMeeting});
                    this.msgSvc.setTemporarySuccessMessage("Review meeting saved successfully");
                });
        }
    }

    cancel() {
        this.dialogRef.close(null);
    }

    private setReviewMeetingStartTime() {
        this.reviewMeetingDates.$startTime = this.reviewMeeting.startDateTime ? moment(this.reviewMeeting.startDateTime) : null;
    }

    private _filterContacts(contacts: IContact[]) {

        return contacts.filter(c => {
            let existingAttendee = false;
            if(this.reviewMeeting && this.reviewMeeting.attendees)
                existingAttendee = !this.reviewMeeting.attendees.every(a => !(a == this.empContactAttendee && a.employeeId == c.employeeId) && (c.userId !== a.userId || c.employeeId != a.employeeId));
            return !existingAttendee;
        });
    }
}
