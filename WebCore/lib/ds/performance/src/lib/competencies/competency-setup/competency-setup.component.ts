import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { AccountService } from "@ds/core/account.service";
import { ReplaySubject, Subject } from "rxjs";
import * as _ from 'lodash';
import { UserInfo } from "@ds/core/shared";
import { PerformanceReviewsService } from '../../shared/performance-reviews.service';
import { ICompetency, ICompetencyGroup } from '../';
import { CompetencyEditDialogComponent } from '../competency-edit-dialog/competency-edit-dialog.component';
import { DefaultCompetenciesDialogComponent } from '../default-competencies-dialog/default-competencies-dialog.component';
import { CompetencyDeleteConfirmDialogComponent } from '../competency-delete-confirm-dialog/competency-delete-confirm-dialog.component';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

export interface DialogResultData {
    competency:ICompetency,
    competencyGroups:ICompetencyGroup[]
}

@Component({
  selector: 'ds-competency-setup',
  templateUrl: './competency-setup.component.html',
  styleUrls: ['./competency-setup.component.scss']
})
export class CompetencySetupComponent implements OnInit {

    user:UserInfo;
    private _competencies:ICompetency[];
    competencies:ICompetency[];
    competencies$:Subject<ICompetency[]> = new ReplaySubject(1);
    competencyGroups:ICompetencyGroup[];
    expandCompetencyDescription:boolean[];
    showArchivedCompetencies:boolean = false;
    isLoading = true;
    
    get hasCompetencies() {
        return this.isLoading || (this.competencies && this.competencies.length)
    }

    constructor(
        private dialog:MatDialog, 
        private account:AccountService, 
        private service:PerformanceReviewsService,
        private msg:DsMsgService
    ) {}

    ngOnInit() {
        this.account.getUserInfo().subscribe(user => {
            this.user = user;

            this.service.getCompetencyGroups(this.user.selectedClientId())
                .subscribe(groups => {
                    this.competencyGroups = groups;
                });
            this.service.getPerformanceCompetencies(this.user.selectedClientId())
                .subscribe(competencies => {
                    this._competencies = competencies;
                    this.competencies = this.sortCompetencies(competencies);
                    this.competencies$.next(this.competencies);

                    this.expandCompetencyDescription = [];
                    for(let i = 0; i < competencies.length; i++) {
                        this.expandCompetencyDescription.push(false);
                    }
                    this.isLoading = false;
                });
        });
    }

    showAddCompetencyDialog():void {
        let ref:MatDialogRef<any, any>;
        if(this.user == null) {

            this.account.getUserInfo().subscribe(user => {
                this.user = user;
                ref = this.renderAddCompetencyDialog(this.user);
            });

        } else {
            ref = this.renderAddCompetencyDialog(this.user);
        }

        ref.afterClosed().subscribe((result:DialogResultData) => {
            if(result == null) return;
            this.competencyGroups = result.competencyGroups;
            this.msg.setTemporarySuccessMessage('Successfully saved competency.', 5000);
            this.service.getPerformanceCompetencies(this.user.lastClientId || this.user.clientId)
                .subscribe(competencies => {
                    this.competencies = this.sortCompetencies(competencies);
                    this.competencies$.next(this.competencies);
                });
        });
    }

    openAvailableCompetenciesDialog():void {
        const ref = this.dialog.open(DefaultCompetenciesDialogComponent, {
            width: '800px',
            data: {
                user: this.user,
                selected: this.competencies
            }
        });

        ref.afterClosed().subscribe(result => {
            if(result == null) return;
            
            /** if we don't have a list of competencies yet, let's instantiate an array so we can push some things to it */
            if(this.competencies == null) this.competencies = [];
            
            // append all returned competencies to our list of selected competencies to be 
            // displayed to our user
            for(let i = 0; i < result.length; i++) {
                this.competencies.push(result[i]);
            }
            
            this.competencies = this.sortCompetencies(this.competencies);
            this.competencies$.next(this.competencies);
        });
    }

    private renderAddCompetencyDialog(user:UserInfo):MatDialogRef<CompetencyEditDialogComponent, any> {
        return this.dialog.open(CompetencyEditDialogComponent, {
            width: '600px',
            data: {
                user: this.user,
                groups: this.competencyGroups
            }
        });
    }

    showEditCompetencyDialog(competency:ICompetency):void {
        const ref = this.renderEditCompetencyDialog(competency);

        ref.afterClosed().subscribe((result:DialogResultData) => {
            if(!result) return;
            this.competencyGroups = result.competencyGroups;
            this.msg.setTemporarySuccessMessage('Successfully saved competency.', 5000);
            this.service.getPerformanceCompetencies(this.user.lastClientId || this.user.clientId)
                .subscribe(competencies => {
                    this.competencies = this.sortCompetencies(competencies);
                    this.competencies$.next(this.competencies);
                });
        });
    }

    private renderEditCompetencyDialog(competency:ICompetency = null):MatDialogRef<CompetencyEditDialogComponent, any> {
        const data = competency != null
            ? { user: this.user, competency: competency, groups: this.competencyGroups }
            : { user: this.user };
        
        return this.dialog.open(CompetencyEditDialogComponent, {
            width: '600px', 
            data: data
        });
    }

    getCompetencyGroupName(groupId:number):string {
        if(groupId == null) return;
        const group = _.find(this.competencyGroups, { 'competencyGroupId': groupId }) as ICompetencyGroup;
        if(group == null) return null;
        return group.name;
    }

    checkForExpandedDescription(index:number):boolean {
        return this.expandCompetencyDescription != null
            && this.expandCompetencyDescription[index] != null
                ? !this.expandCompetencyDescription[index]
                : true;
    }
    
    archiveCompetency(comp:ICompetency, isArchived:boolean = true):void {
        this.msg.sending(true);
        comp.isArchived = isArchived;
        this.service.savePerformanceCompetency(this.user.selectedClientId(), comp)
            .subscribe(competency => {
                this.competencies.forEach((c, i) => {
                    if(c.competencyId != competency.competencyId) return;
                    
                    if(isArchived) {
                        this.competencies.splice(i, 1);    
                    } else {
                        this.competencies[i] == competency;
                    }
                });
                
                this.mergeCompetencies([competency]);
                this.competencies = this.sortCompetencies(this.competencies);
                this.competencies$.next(this.competencies);
                this.msg.sending(false);
                
                const archivedCompetencies = this.competencies.filter(c => c.isArchived);
                if(archivedCompetencies != null && archivedCompetencies.length === 0) {
                    this.showArchivedCompetencies = false;
                } else {
                    this.showArchivedCompetencies = true;
                }
            });
    }
    
    toggleArchivedCompetencies():void {
        this.showArchivedCompetencies = !this.showArchivedCompetencies;
        this.service.getPerformanceCompetencies(this.user.selectedClientId(), this.showArchivedCompetencies)
            .subscribe(competencies => {
                
                if(this.showArchivedCompetencies) {
                    this.mergeCompetencies(competencies);    
                } else {
                    this.competencies = competencies;
                }
                
                this.competencies = this.sortCompetencies(this.competencies);
                this.competencies$.next(this.competencies);
            });
    }
    
    /**
     * Merges a new list of ICompetency
     * 
     * @param newArray The new array of ICompetency that you want to merge into the existing list.
     */
    private mergeCompetencies(newArray:ICompetency[]):void {
        this.competencies = this.competencies
            .filter(tc => !newArray.find(c => c.competencyId === tc.competencyId))
            .concat(newArray);
    }

    confirmDeleteCompetency(comp:ICompetency):void {
        const ref = this.dialog.open(CompetencyDeleteConfirmDialogComponent, {
            width: '400px'
        });

        ref.afterClosed().subscribe(deleteConfirmed => {
            if(!deleteConfirmed) return;
            this.deleteCompetency(comp);
        });
    }

    deleteCompetency(comp:ICompetency):void {
        this.msg.sending(true);
        this.service.deletePerformanceCompetency(comp.clientId || this.user.selectedClientId(), comp.competencyId)
            .subscribe(() => {
                this.msg.setTemporarySuccessMessage('Successfully deleted competency.', 5000);
                this.service.getPerformanceCompetencies(this.user.selectedClientId(), this.showArchivedCompetencies)
                    .subscribe(competencies => {
                        this.competencies = this.sortCompetencies(competencies);
                        this.competencies$.next(this.competencies);
                    });
            });
    }

    private sortCompetencies(comps:ICompetency[]):ICompetency[] {
        return _.sortBy(comps, ['name']);
    }

}
