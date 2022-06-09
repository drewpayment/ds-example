import { Component, OnInit, Input, Type, ChangeDetectionStrategy } from '@angular/core';
import { FieldType } from '@ds/core/shared';
import { BooleanFeedbackResponse } from '../shared/feedback-response-boolean.model';
import { ListItemFeedbackResponse } from '../shared/feedback-response-list.model';
import { MultiSelectFeedbackResponse } from '../shared/feedback-response-multiselect.model';
import { DateFeedbackResponse } from '../shared/feedback-response-date.model';
import { TextItemFeedbackResponse } from '../shared/feedback-response-text.model';
import * as moment from 'moment';
const inputDateFormatString = 'YYYY-MM-DDTHH:mm:ss';
const outputDateFormatString = 'MM/DD/YYYY';

@Component({
    selector: 'ds-feedback-yes-no',
    template: `<div class="print-page-li-label">{{feedback.feedbackBody}} <span *ngIf="!feedback.isRequired" class="ml-1 instruction-text">Optional</span>
    <span *ngIf="!feedback.isRequired && !feedback.isVisibleToEmployee" class="instruction-text">;</span>
    <div *ngIf="feedback.value != null && feedback.value === true">
        <div class="custom-control custom-checkbox" >
            <input type="checkbox" class="custom-control-input" checked />
            <label class="custom-control-label">Yes</label>
            <svg class="print-checkbox" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M19 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.11 0 2-.9 2-2V5c0-1.1-.89-2-2-2zm-9 14l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
        </div>
        <div class="custom-control custom-checkbox">
            <input type="checkbox" class="custom-control-input" />
            <label class="custom-control-label">No</label>
            <svg class="print-checkbox" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M19 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.11 0 2-.9 2-2V5c0-1.1-.89-2-2-2zm-9 14l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
        </div>
    </div>
    <div *ngIf="feedback.value != null && feedback.value === false">
        <div class="custom-control custom-checkbox">
            <input type="checkbox" class="custom-control-input" />
            <label class="custom-control-label">Yes</label>
            <svg class="print-checkbox" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M19 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.11 0 2-.9 2-2V5c0-1.1-.89-2-2-2zm-9 14l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
        </div>
        <div class="custom-control custom-checkbox">
            <input type="checkbox" class="custom-control-input" checked />
            <label class="custom-control-label">No</label>
            <svg class="print-checkbox" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M19 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.11 0 2-.9 2-2V5c0-1.1-.89-2-2-2zm-9 14l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
        </div>
    </div>
    <div *ngIf="!((feedback.value != null && feedback.value === true) || (feedback.value != null && feedback.value === false))">
        <div class="custom-control custom-checkbox">
            <input type="checkbox" class="custom-control-input" />
            <label class="custom-control-label">Yes</label>
            <svg class="print-checkbox" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M19 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.11 0 2-.9 2-2V5c0-1.1-.89-2-2-2zm-9 14l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
        </div>
        <div class="custom-control custom-checkbox">
            <input type="checkbox" class="custom-control-input" />
            <label class="custom-control-label">No</label>
            <svg class="print-checkbox" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M19 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.11 0 2-.9 2-2V5c0-1.1-.89-2-2-2zm-9 14l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
        </div>
    </div>
    `,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class FeedbackYesNoComponent implements OnInit {
    @Input() feedback: BooleanFeedbackResponse;

    constructor() { }

    ngOnInit(): void { }
}

@Component({
    selector: 'ds-feedback-list',
    template: `
    <div class="print-page-li-label">{{feedback.feedbackBody}}<span *ngIf="!feedback.isRequired" class="ml-1 instruction-text">Optional</span>
    <span *ngIf="!feedback.isRequired && !feedback.isVisibleToEmployee" class="instruction-text">;</span>
    <ng-container *ngFor="let item of feedback.feedbackItems">
        <div class="custom-control custom-checkbox">
            <input *ngIf="feedback.value != null && item.feedbackItemId == feedback.value.feedbackItemId; else notSelected" type="checkbox" class="custom-control-input" checked>
            <label class="custom-control-label">{{item.itemText}}</label>
            <svg class="print-checkbox" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M19 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.11 0 2-.9 2-2V5c0-1.1-.89-2-2-2zm-9 14l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
        </div>
    </ng-container>
    <ng-template #notSelected><input type="checkbox" class="custom-control-input"></ng-template>
    `,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ListComponent implements OnInit {
    @Input() feedback: ListItemFeedbackResponse;

    constructor() { }

    ngOnInit(): void { }
}

@Component({
    selector: 'ds-feedback-multiselect',
    template: `
    <div class="print-page-li-label">{{feedback.feedbackBody}}<span *ngIf="!feedback.isRequired" class="ml-1 instruction-text">Optional</span>
    <span *ngIf="!feedback.isRequired && !feedback.isVisibleToEmployee" class="instruction-text">;</span>
    <ng-container *ngFor="let item of feedback.feedbackItems">
        <div class="custom-control custom-checkbox">
            <input *ngIf="feedback.value != null && hasItem(feedback.value,item.feedbackItemId); else notSelected" type="checkbox" class="custom-control-input" checked>
            <label class="custom-control-label">{{item.itemText}}</label>
            <svg class="print-checkbox" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M19 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.11 0 2-.9 2-2V5c0-1.1-.89-2-2-2zm-9 14l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
        </div>
    </ng-container>
    <ng-template #notSelected><input type="checkbox" class="custom-control-input"></ng-template>
    `,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class MultiSelectComponent implements OnInit {
    @Input() feedback: MultiSelectFeedbackResponse;

    constructor() { }

    ngOnInit(): void { }
    hasItem(listStr:string,itemId:number):boolean{
        return ((','+listStr+',').indexOf(','+itemId+',')>-1);
    }
}

@Component({
    selector: 'ds-feedback-date',
    template: `
    <div class="print-page-li-label">{{feedback.feedbackBody}}<span *ngIf="!feedback.isRequired" class="ml-1 instruction-text">Optional</span>
    <span *ngIf="!feedback.isRequired && !feedback.isVisibleToEmployee" class="instruction-text">;</span>
    <div *ngIf="dateString != null; else noDate">{{dateString}}</div>
    <ng-template #noDate>
    <div class="single-line-text-initial-state"></div>
    </ng-template>
    `,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class DateComponent implements OnInit {
    private _feedback: DateFeedbackResponse;
    dateString: string;
    @Input() set feedback(val) {
        this._feedback = val;
        this.dateString = this._feedback.value == null ? null : this.getMoment(this._feedback.value).format(outputDateFormatString);
    }
    get feedback() {
        return this._feedback;
    }

    constructor() { }

    ngOnInit(): void { }

    private getMoment(date: string | Date): moment.Moment {
        return typeof date === 'string' ? moment(date, inputDateFormatString) : moment(date);
    }
}

@Component({
    selector: 'ds-feedback-text',
    template: `
    <div class="break-avoid">
        <div class="print-page-li-label">{{feedback.feedbackBody}}
            <span *ngIf="!feedback.isRequired" class="ml-1 instruction-text">Optional</span>
            <span *ngIf="!feedback.isRequired && !feedback.isVisibleToEmployee" class="instruction-text">;</span>
        <div *ngIf="null != feedback.value && feedback.value.trim() !== \'\'; else noVal"
            class="text-box-response" [innerHTML]="feedback.value | formatComment">
        </div>
    </div>
    <ng-template #noVal><div class="comment-box"></div></ng-template>`,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class TextComponent implements OnInit {
    @Input() feedback: TextItemFeedbackResponse;

    constructor() { }

    ngOnInit(): void { }
}

export const FieldTypeToComponent: (val: FieldType) => Type<{}> =
    (val: FieldType) => {
        if (FieldType.Text === val || FieldType.Numeric === val)
            return TextComponent;
        if (FieldType.Date === val)
            return DateComponent;
        if (FieldType.List === val)
            return ListComponent;
        if (FieldType.MultipleSelection === val)
            return MultiSelectComponent;
        if (FieldType.Boolean === val)
            return FeedbackYesNoComponent;
        throw 'No component found for that FieldType!'
    }
