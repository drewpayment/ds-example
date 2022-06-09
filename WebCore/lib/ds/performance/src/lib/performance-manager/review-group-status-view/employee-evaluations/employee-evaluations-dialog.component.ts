import { Component, OnInit, ElementRef, Input, Inject, ViewChild }  from '@angular/core';
import { IFeedbackSetup, IFeedbackResponseData } from '@ds/performance/feedback/';
import { switchMap, map, catchError, tap } from 'rxjs/operators';
import { INameVal } from '@ajs/labor/models';
import { EvaluationRoleType } from '@ds/performance/evaluations/shared/evaluation-role-type.enum';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector    : 'ds-employee-evaluations-dialog',
    templateUrl : './employee-evaluations-dialog.component.html',
    styleUrls   : ['./employee-evaluations-dialog.component.scss']
})
export class EmployeeEvaluationsDialogComponent implements OnInit {
    competency: INameVal;
    displayedColumns: Array<string>;
    pagingLength: number;
    dialogTitle: string;
    responseText: string;

    feedback: IFeedbackSetup;
    selectedFeedbackItemId: number;
    selectedFeedbackResponse: string;
    responseList: Array<Record>;

    constructor(
        public dialogRef    : MatDialogRef<EmployeeEvaluationsDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:any
    ) {
    }

    ngOnInit() {
        this.responseList = [];

        this.data.responseList
            .sort((a, b) => a.responseByContact.employeeId - b.responseByContact.employeeId)
            .forEach(item => {
                let k:Record = this.responseList.find(x=>x.id == item.responseByContact.employeeId);
                if(k) k.count++;
                else {
                    this.responseList.push({id:item.responseByContact.employeeId,
                        name:item.responseByContact.firstName+' '+item.responseByContact.lastName,
                        count:1,
                        role: item.evaluationRoleType,
                    });
                }
            });

        this.responseList.sort((a, b) => a.name.toLowerCase().trim().localeCompare(b.name.toLowerCase().trim()));

        this.feedback                  = this.data.feedback;
        this.selectedFeedbackItemId    = this.data.feedbackItemId;
        this.selectedFeedbackResponse  = this.data.feedbackResponse;
        this.pagingLength              = this.responseList.length;

        this.displayedColumns   = ['e_or_s','name'];
        this.dialogTitle        = this.feedback.body;
        if(this.selectedFeedbackItemId){
            this.responseText   = "Listed below are the employees who chose \"" + this.feedback.feedbackItems
                .find(x=>x.feedbackItemId==this.selectedFeedbackItemId).itemText + "\"";
        } else {
            this.responseText   = "Listed below are the employees with the response \"" + this.selectedFeedbackResponse + "\"";
        }
    }


    save() {

    }

    onNoClick():void {
        this.dialogRef.close();
    }

}   // end of class

class Record{
    id: number;
    name: string;
    count: number;
    role: EvaluationRoleType;
}
