import { Validators } from '@angular/forms';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IRecalcPointsDialogResult, IRecalcPointsDialogData } from './recalc-points-dialog.model';
import { Component, OnInit, EventEmitter, Output, Inject } from '@angular/core';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AutomatedPointsApiService } from '../shared/automated-points-api.service';
import { IRecalcPointsRequest } from '../shared/recalc-points-request.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

@Component({
  selector: 'ds-recalc-points-dialog',
  templateUrl: './recalc-points-dialog.component.html',
  styleUrls: ['./recalc-points-dialog.component.scss']
})
export class RecalcPointsDialogComponent implements OnInit {
    form: FormGroup;
    startInvalid: boolean = false;
    endInvalid: boolean = false;

    start: Date;

    constructor(private dialogRef: MatDialogRef<RecalcPointsDialogComponent, IRecalcPointsDialogResult>,
        private automatedPointsAPi: AutomatedPointsApiService,
        @Inject(MAT_DIALOG_DATA) private data: IRecalcPointsDialogData,
        private formBuilder: FormBuilder,
        private msg: DsMsgService) {
            this.loadData();
        }

    ngOnInit() {
        this.form = this.formBuilder.group({
            startDate: [null, Validators.required],
            endDate: [null, Validators.required]
        });
    }

    public loadData(){
        this.msg.loading(true);
        this.msg.loading(false);
    }

    public closeModal(){
      this.dialogRef.close();
    }

    public recalcPoints(){
        if (this.form.invalid){
            if (this.form.value.startDate == null){
                this.startInvalid = true;
            }else{
                this.startInvalid = false;
            }

            if (this.form.value.endDate == null){
                this.endInvalid = true;
            }else{
                this.endInvalid = false;
            }

            return;
        }

        let request: IRecalcPointsRequest = {
        startDate: this.form.value.startDate,
        endDate: this.form.value.endDate,
        clientId: this.data.clientId
        };

        this.msg.sending(true);

        this.automatedPointsAPi.recalculateAutomatedPoints(request).subscribe(data => {
            this.dialogRef.close();
            this.msg.setTemporarySuccessMessage("Recalculated automatic points successfully.");
        },
            (data => {
                this.msg.showWebApiException;
                this.dialogRef.close();
            })
        );
    }
}
