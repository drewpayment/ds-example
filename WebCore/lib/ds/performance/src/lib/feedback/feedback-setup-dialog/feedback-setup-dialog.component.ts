import { Component, OnInit, Inject, ViewChildren, QueryList, ElementRef } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IFeedbackSetupDialogData } from './feedback-setup-dialog-data.model';
import { IFeedbackSetupDialogResult } from './feedback-setup-dialog-result.model';
import { IFeedbackSetup } from '../shared/feedback-setup.model';
import { NgForm } from '@angular/forms';
import { FieldType } from '@ds/core/shared';
import { FeedbackApiService } from '../feedback-api.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IFeedbackItem } from '@ds/performance/feedback/shared/feedback-item.model';
import * as _ from "lodash";

@Component({
    selector: 'ds-feedback-setup-dialog',
    templateUrl: './feedback-setup-dialog.component.html',
    styleUrls: ['./feedback-setup-dialog.component.scss']
})
export class FeedbackSetupDialogComponent implements OnInit {

    fieldTypes = [
        { value: FieldType.Date,    text: "Date" },
        { value: FieldType.List,    text: "List" },
        { value: FieldType.Text,    text: "Text" },
        { value: FieldType.Boolean, text: "Yes/No" },
        // { value: FieldType.MultipleSelection, text: "Multiple Selection" },
    ];

    feedback: IFeedbackSetup;
    selectedItem: IFeedbackItem;
    origItemText: string;
    isCopy: boolean;

    @ViewChildren('editItemInput') listInputs: QueryList<ElementRef>;

    get isListType() {
        return this.feedback.fieldType === FieldType.List || this.feedback.fieldType === FieldType.MultipleSelection ;
    }

    constructor(
        private dialogRef: MatDialogRef<FeedbackSetupDialogComponent, IFeedbackSetupDialogResult>,
        @Inject(MAT_DIALOG_DATA)
        private dialogData: IFeedbackSetupDialogData,
        private msgSvc: DsMsgService,
        private feedbackApi: FeedbackApiService
    ) { }

    ngOnInit() {
        this.isCopy = this.dialogData.isCopy;

        if (!this.dialogData || !this.dialogData.feedback){
            this.feedback = <IFeedbackSetup>{
                fieldType: FieldType.Text,
                isSupervisor: true,
                isSelf: true,
                isRequired: true,
                feedbackItems: [],
                isEnabled: true,
                isVisibleToEmployee: false
            };
        }
        else {
            this.feedback = _.cloneDeep(this.dialogData.feedback);
        }
    }

    addItem() {
        if (!this.feedback.feedbackItems)
            this.feedback.feedbackItems = [];

        let newItem: IFeedbackItem = {
            feedbackId:     this.feedback.feedbackId,
            feedbackItemId: 0,
            itemText:       "",
            checked:        false,
        };

        this.feedback.feedbackItems.push(newItem);
        this.selectedItem = newItem;
        this.origItemText = "";

        this.listInputs.changes.subscribe((queryList:QueryList<ElementRef>) => {
            if(queryList == null || queryList.last == null) return;
            queryList.last.nativeElement.focus();
        });
    }

    editItem(item:IFeedbackItem) {
        if (this.selectedItem) {
            this.cancelItem(this.selectedItem);
        }

        this.selectedItem = item;
        this.origItemText = item.itemText;
    }

    saveItem(item: IFeedbackItem) {
        this.selectedItem = null;
        this.origItemText = null;

        if(!item.itemText || !item.itemText.trim())
            _.remove(this.feedback.feedbackItems, i => i === item);
    }

    cancelItem(item: IFeedbackItem) {
        this.selectedItem.itemText = this.origItemText;
        this.saveItem(item);
    }

    saveFeedback(form: NgForm) {
        if(form.invalid)
            return;

        if (this.feedback.fieldType === FieldType.MultipleSelection || this.feedback.fieldType === FieldType.List) {
            if (!this.feedback.feedbackItems || !this.feedback.feedbackItems.length) {
                this.addItem();
                return;
            }
        } else {
            this.feedback.feedbackItems = null;
        }

        this.msgSvc.loading();

        this.feedbackApi.saveFeedbackSetup(this.feedback)
            .subscribe(saved => {
                this.msgSvc.setTemporarySuccessMessage("Feedback successfully saved.");
                this.dialogRef.close({ feedback: saved });
            },
            (error:HttpErrorResponse) => {
                this.msgSvc.showWebApiException(error.error);
            });
    }

    cancel() {
        this.dialogRef.close(null);
    }
}
