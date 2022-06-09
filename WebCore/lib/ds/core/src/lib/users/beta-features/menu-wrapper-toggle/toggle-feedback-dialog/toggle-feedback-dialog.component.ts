import { Component, OnInit, Inject, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ISystemFeedbackData, SystemFeedbackType } from './toggle-feedback-dialog-data.model';
import { IRemark } from '@ds/core/shared';

@Component({
    selector: 'ds-toggle-feedback-dialog',
    templateUrl: './toggle-feedback-dialog.component.html',
    styleUrls: ['./toggle-feedback-dialog.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ToggleFeedbackDialogComponent implements OnInit {
    
    form:FormGroup = this.createForm();
    feedback:ISystemFeedbackData;
    
    
    constructor(
        private ref:MatDialogRef<ToggleFeedbackDialogComponent>, 
        private msgDialog: MatDialog,
        @Inject(MAT_DIALOG_DATA) public data:ISystemFeedbackData,
        private fb:FormBuilder,
    ) { 
        
    }

    ngOnInit() {
        this.defaultValues();
    }

    defaultValues(){
        this.feedback = this.data || <ISystemFeedbackData>{};
        this.feedback.remark = this.feedback.remark || <IRemark>{};
    }
    
    clear() {
        this.form.reset();
        this.ref.close({ feedback: null});
    }
    
    submit() {
        if(this.form.invalid) return;
        
        this.prepareModel();
        
        // Open Thanks
        let thanks = this.msgDialog.open(ToggleThankYouComponent, {
            width: '40vw',
        });

        
        // Close Feedback Dialog
        this.ref.close({ feedback: this.feedback, thankYou: thanks});
    }
    
    private createForm():FormGroup {
        this.defaultValues();

        return this.fb.group({
            activeOption: this.fb.control('1'),
            comments: this.fb.control(''),
        });
    }
    
    private prepareModel():void {
        this.feedback.remark = this.feedback.remark || <IRemark>{};
        this.feedback.systemFeedbackTypeId = SystemFeedbackType.MenuWrapper;
        let desc = this.getDescription( parseInt(this.form.value.activeOption));
        this.feedback.remark.description = desc;
    }

    private getDescription(option:number){
        var txt = 'Other' + (this.form.value.comments ? (' - ' + this.form.value.comments ) : '');

        switch (option) {
            case 1:
                txt = "I don't like the new look/I prefer the old look";
                break;
            case 2:
                txt = "It was difficult to find what I was looking for in the new layout";
                break;
            case 3:
                txt = "I kept getting errors";
                break;
        }
        return txt;
    }
}

@Component({
    selector: 'ds-toggle-thank-you',
    template: `
        <div mat-dialog-header class="pb-0">
            <h2 class="modal-title">
            </h2>
            <button type="button" class="close" (click)="clear()">
                <mat-icon>clear</mat-icon>
            </button>
        </div>
        <div mat-dialog-content>
            <div class="row">
                <div class="col-sm-12 text-center">
                    <div class="mb-4">
                        <b>Thank you for your input!</b>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 text-center">
                    <div class="mb-4">
                        You can continue to use Dominion's old site design now, but our plan is 
                        to switch over to the new site design permanently in the near future.
                    </div>
                </div>
            </div>
        </div>
    `
})
export class ToggleThankYouComponent {
    constructor(
        private ref:MatDialogRef<ToggleThankYouComponent>,
    ) {}

    clear() {
        this.ref.close();
    }
}
