import { Component, OnInit, Inject } from '@angular/core';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ICompetencyModel, IJobProfileDisplayable } from '@ds/performance/competencies';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


interface DialogData {
    employeeId : number,
    clientId   : number,
    models : ICompetencyModel[],
    selectedModel: ICompetencyModel
}

@Component({
    selector    : 'ds-competency-dialog',
    templateUrl : './competency-dialog.component.html',
    styleUrls   : ['./competency-dialog.component.scss']
})

export class CompetencyDialogComponent implements OnInit {


    coreCompCount: number;
    // models: ICompetencyModel[];
    selectedModel: ICompetencyModel;
    allJobProfiles: IJobProfileDisplayable[];


    constructor(
        public dialogRef    : MatDialogRef<CompetencyDialogComponent>,
        private perfService : PerformanceReviewsService,
        private msg         : DsMsgService,
        @Inject(MAT_DIALOG_DATA) public data:DialogData
    ) { }

    ngOnInit() {
        this.selectedModel = this.data.selectedModel;
    }   // end of ngOnInit()

    // private assignModelObservable(): void {
    //     var gettingModels$ = this.pefService.getCompetencyModelsForCurrentClient().pipe(shareReplay(1),
    //     // remove any archived competencies
    //     map(val => {
    //         (val || []).forEach(compModel => {
    //         compModel.competencies =
    //         (compModel.competencies || []).filter(comp => !comp.isArchived);
    //         });
    //         return val;
    //     }));
    //     gettingModels$.subscribe(data => {
    //        this.models        = data;
    //        this.selectedModel = this.models[0];
    //     });
    // }

    save() {
        this.perfService.assignCompetencyModelToEmployeeByEmployeeId(this.data.employeeId, this.data.selectedModel.competencyModelId)
            .subscribe(() => {
                this.msg.setTemporarySuccessMessage("Successfully assigned the competency model to the employee.");
                this.dialogRef.close(this.data.selectedModel);
            },(error: HttpErrorResponse) => {
                this.msg.showWebApiException(error.error);
            });
    }

    onNoClick():void {
        this.dialogRef.close();
    }

}   // end of class
