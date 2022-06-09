import { Component, OnInit, ElementRef, Inject, ViewChild, ChangeDetectionStrategy }  from '@angular/core';
import { INameVal } from '@ajs/labor/models';
import { IReviewRating } from '@ds/performance/ratings/shared/review-rating.model';
import { GraphItemWithRange } from '../review-scoring-graph.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

interface DialogData {
    employeeId : number,
    employeeName : string,
    competencyId : number,
    competencyName: string
}

@Component({
    selector    : 'ds-employees-rating-list-dialog',
    templateUrl : './employees-rating-list-dialog.component.html',
    styleUrls   : ['./employees-rating-list-dialog.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})

export class EmployeesRatingListDialogComponent implements OnInit {
    employeeList: DialogData[];MAT_RADIO_GROUP_CONTROL_VALUE_ACCESSOR
    rating: GraphItemWithRange;
    allRatings: IReviewRating[];
    competency: INameVal;
    dialogTitle: string;
    ratedList: number[];
    @ViewChild('dialogContent', { static: false }) dialogContent:ElementRef<HTMLElement>;

    constructor(
        public dialogRef    : MatDialogRef<EmployeesRatingListDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:any
    ) {
    }

    ngOnInit() {
        if(this.data.isCompetencyView){
            this.competency = this.data.competency;
            this.allRatings = this.data.allRatings;
            this.employeeList = this.data.employeeList;
            this.allRatings = this.data.allRatings;
            this.ratedList    = [...this.allRatings.filter(x=>x.rating>0).map(y=>y.rating)];
            this.ratedList.sort((x,y)=> y - x);
            if(this.rating){
                this.dialogTitle      = this.rating.name;
            }
        } else {
            this.rating = this.data.rating;
            this.allRatings = this.data.allRatings;
            this.ratedList    = [...this.allRatings.filter(x=>x.rating>0).map(y=>y.rating)];
            this.ratedList.sort((x,y)=> y - x);
            if(this.rating){
                this.dialogTitle      = this.rating.name;
            }
        }

    }


    save() {

    }

    onNoClick():void {
        this.dialogRef.close();
    }

}   // end of class
