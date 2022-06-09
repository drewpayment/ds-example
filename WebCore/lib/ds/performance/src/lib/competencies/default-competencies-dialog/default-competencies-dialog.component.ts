import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { PerformanceReviewsService } from "../../shared/performance-reviews.service";
import { Subject, ReplaySubject } from "rxjs";

import * as _ from 'lodash';
import { UserInfo } from "@ds/core/shared";
import { ICompetency } from "../";

export interface DefaultCompetenciesDialogData {
    user: UserInfo,
    selected:ICompetency[]
}

export interface SelectedPerformanceCompetency extends ICompetency {
    selected?:boolean
}

@Component({
  selector: 'ds-default-competencies-dialog',
  templateUrl: './default-competencies-dialog.component.html',
  styleUrls: ['./default-competencies-dialog.component.scss']
})
export class DefaultCompetenciesDialogComponent implements OnInit {
    user:UserInfo;
    userSelectedCompetencies:ICompetency[];
    competencies:SelectedPerformanceCompetency[];
    competencies$:Subject<SelectedPerformanceCompetency[]> = new ReplaySubject(1);
    isLoading = true;

    get hasCompetencies() {
        return this.isLoading || (this.competencies && this.competencies.length)
    }

    constructor(
        public dialogRef:MatDialogRef<DefaultCompetenciesDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:DefaultCompetenciesDialogData,
        private service:PerformanceReviewsService
    ) {}

    ngOnInit() {
        this.user = this.data.user;
        this.userSelectedCompetencies = this.data.selected;

        this.service.getDefaultCompetencies(this.user.lastClientId || this.user.clientId)
            .subscribe(competencies => {
                _.remove(competencies, (c:ICompetency) => {
                    return _.find(this.userSelectedCompetencies, {'name': c.name}) != null
                        || _.find(this.userSelectedCompetencies, {'description': c.description}) != null;
                });
                this.competencies = this.sortCompetencies(competencies);
                this.competencies$.next(this.competencies);
                this.isLoading = false;
            });
    }

    saveSelectedCompetencies():void {
        let selected = _.filter(this.competencies, { 'selected':true }).map(s => s.competencyId);
        this.service.duplicateDefaultCompetencies(this.user.selectedClientId(), selected)
            .subscribe(competencies => {
                this.dialogRef.close(competencies);
            });
    }

    selectAllCompetencies(event):void {
        for(let i = 0; i < this.competencies.length; i++) {
            this.competencies[i].selected = event.target.checked;
        }
        this.competencies$.next(this.competencies);
    }

    onNoClick():void {
        this.dialogRef.close();
    }
    
    private sortCompetencies(comps:ICompetency[]):ICompetency[] {
        return _.sortBy(comps, ['name']);
    }
}