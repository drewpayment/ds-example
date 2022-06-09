import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Moment } from 'moment';
import * as moment  from 'moment/moment';
import { findIndex } from 'lodash';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { PlansService } from '../../shared/plans.service';
import { IPlanAdminSummary, ICopyPlansDialogData, IPlan, ICopyPlanConfig } from '../../shared/plans.model';



@Component({
  selector: 'ds-copy-plans-dialog',
  templateUrl: './copy-plans-dialog.component.html',
  styleUrls: ['./copy-plans-dialog.component.scss']
})
export class CopyPlansDialogComponent implements OnInit {
    private plansAdminSummary: IPlanAdminSummary[];
    private plansAdminSummaryDatesAndPlan: IPlanAdminSummary[];
    planYears: number[];
    yearPlans: IPlan[];

    config: ICopyPlanConfig;

    @ViewChild("copyPlansForm", { static: false} )
    form: NgForm;

    constructor(
        private msgSvc: DsMsgService,
        private dialogRef: MatDialogRef<CopyPlansDialogComponent, ICopyPlansDialogData>,
        private plansService: PlansService,
        @Inject(MAT_DIALOG_DATA)
        private dialogData: ICopyPlansDialogData,) { }

    ngOnInit() {
        this.config = {
            startDate: null,
            endDate: null,
            planYear: null,
            plans: null
        };

        this.plansService.getPlanAdminSummary(this.dialogData.clientId).subscribe(s => {
            this.plansAdminSummary = s;

            this.planYears = [];
            this.plansAdminSummary.forEach(x => {
                if (x.startDate) {
                    let yearStart = moment(x.startDate).year();
                    if (findIndex(this.planYears, y => y == yearStart) == -1) {
                        this.planYears.push(yearStart);
                    }
                }
                if (x.endDate) {
                    let yearEnd = moment(x.endDate).year();
                    if (findIndex(this.planYears, y => y == yearEnd) == -1) {
                        this.planYears.push(yearEnd);
                    }
                }
            });
            this.planYears = this.planYears.sort();
            this.planYears = this.planYears.reverse();
            if (this.planYears.length > 0) {
                this.config.planYear = this.planYears[0];
            }

            this.plansService.getPlanAdminSummaryDatesAndPlan(this.dialogData.clientId).subscribe(s => {
                this.plansAdminSummaryDatesAndPlan = s;
                this.loadPlans();
            });
        });


    }

    loadPlans() {
        this.yearPlans = [];

        if (this.config.planYear) {
            this.plansAdminSummaryDatesAndPlan.forEach(x => {
                let startYear = moment(x.startDate).year();
                let endYear = moment(x.endDate).year();

                if (this.config.planYear == startYear || this.config.planYear == endYear) {
                    this.yearPlans.push({ planId: x.planId, planName: x.planName, selected: false });
                }

                if (!startYear && !endYear && this.config.planYear == 0) {
                    this.yearPlans.push({ planId: x.planId, planName: x.planName, selected: false });
                }
            });
        }
    }

    selectAllPlans(e) {
        if (this.config.planYear && this.yearPlans.length>0) {
            this.yearPlans.forEach(x => {
                x.selected = e.target.checked;
            });
        }
    }

    noPlansChosen() {
        if(!this.yearPlans || this.yearPlans.length == 0 ) return true;
        return this.yearPlans.filter(x=>x.selected).length == 0;
    }

    copyPlans() {
        if (this.form.invalid) return;

        this.config.plans = this.yearPlans.filter(x => x.selected).map(x => x.planId);
        if (!this.config.startDate || !this.config.endDate || !this.config.plans) {
            return;
        }
        this.plansService.copyPlans(this.config).subscribe(data => {
            this.msgSvc.setTemporarySuccessMessage("Plans copied succesfully", 1000);
            //var redirectToUrl = this.companyRootUrl + "BenefitPlans.aspx" + result.employeeId.toString();
            var redirectToUrl = "BenefitPlans.aspx";
            window.location.href = redirectToUrl;
        },
        (error: HttpErrorResponse) =>
                this.msgSvc.showWebApiException(error.error));
    }

    cancel() {
        this.dialogRef.close();
    }
}
