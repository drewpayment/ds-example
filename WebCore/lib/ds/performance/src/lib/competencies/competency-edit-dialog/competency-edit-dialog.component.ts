import * as _ from 'lodash';
import { Component, Inject, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { PerformanceReviewsService } from "../../shared/performance-reviews.service";
import { ICompetencyGroup, ICompetency} from "../";
import { UserInfo } from "@ds/core/shared";
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';

export interface CompetencyEditDialogData {
    user:UserInfo,
    competency:ICompetency,
    groups:ICompetencyGroup[]
}

export interface CompetencyEditDialogResult {
    competency:ICompetency,
    competencyGroups:ICompetencyGroup[]
}

@Component({
  selector: 'ds-competency-edit-dialog',
  templateUrl: './competency-edit-dialog.component.html',
  styleUrls: ['./competency-edit-dialog.component.scss']
})
export class CompetencyEditDialogComponent implements OnInit {

    user:UserInfo;
    competency:ICompetency;
    selectedCompetencyGroup:ICompetencyGroup = { competencyGroupId: null, clientId: null, name: null };
    formSubmitted:boolean;
    form:FormGroup;
    editCompetencyTitle:string;
    competencyGroups:ICompetencyGroup[];

    constructor(
        public dialogRef:MatDialogRef<CompetencyEditDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:CompetencyEditDialogData,
        private fb:FormBuilder,
        private service:PerformanceReviewsService,
        private msg:DsMsgService
    ) {}

    ngOnInit():void {
        this.user = this.data.user;
        this.competency = this.data.competency != null
            ? _.cloneDeep(this.data.competency)
            : this.createEmptyCompetency();

        this.competencyGroups = this.data.groups;
        this.prepareCompetencyGroups(this.competencyGroups);
        this.createForm();
    }

    saveCompetency():void {
        this.formSubmitted = true;
        this.form.updateValueAndValidity();
        if(this.form.invalid) return;
        this.msg.sending(true);

        const groupControl = this.form.controls.competencyGroup as FormGroup;
        const groupName = groupControl.value.name;

        if (groupControl.value.name === '') {
            this.removeCompetencyGroup(groupControl.value.competencyGroupId);
        }

        // if the group name coming back from the form isn't an existing competencygroup, we are going to save a new one.. 
        if(groupName != null && groupName.length > 0
            && _.findIndex(this.competencyGroups, { 'name': groupControl.value.name }) < 0
        ) {
            const dto:ICompetencyGroup = {
                competencyGroupId: null,
                clientId: this.user.lastClientId || this.user.clientId,
                name: groupControl.value.name
            };

            this.service.createCompetencyGroup(dto.clientId, dto)
                .subscribe(group => {
                    this.selectedCompetencyGroup = group;
                    this.prepareCompetencyGroups(this.selectedCompetencyGroup);
                    this.competency.competencyGroup = group;
                    this.competency.competencyGroupId = group.competencyGroupId;

                    // update the form value which updates the UI and is used to "prepare the model" for saving below
                    (<FormGroup>this.form.get('competencyGroup')).patchValue({
                        competencyGroupId: group.competencyGroupId,
                        clientId: group.clientId,
                        name: group.name
                    });
                    
                    this.service.savePerformanceCompetency(this.user.lastClientId || this.user.clientId, this.prepareSaveModel())
                        .subscribe(competency => {
                            this.competency = competency;

                            this.closeDialogSuccessfully();
                            this.resetForm(competency);
                        });
                });
        } else {
            const dto = this.prepareSaveModel();
            this.service.savePerformanceCompetency(this.user.lastClientId || this.user.clientId, dto)
                .subscribe(competency => {
                    this.competency = competency;

                    this.closeDialogSuccessfully();
                    this.resetForm(competency);
                });
        }
            
    }

    onNoClick():void {
        this.dialogRef.close();
    }

    addGroup(group:ICompetencyGroup):void {
        console.dir(group);
    }

    removeGroup(group:ICompetencyGroup):void {
        this.selectedCompetencyGroup = null;
        this.competency.competencyGroupId = null;

    }

    inputUpdate(event):void {
        console.dir(event);
    }

    removeCompetencyGroup(competencyGroupId:number):void {
        const compGroup = this.form.get('competencyGroup') as FormGroup;
        compGroup.patchValue(this.getNewCompetencyGroup());
        this.selectedCompetencyGroup = null;
        this.competency.competencyGroupId = null;
        compGroup.markAsDirty();
        compGroup.markAsTouched();
    }

    addGroupFromAutocomplete(event:MatAutocompleteSelectedEvent) {
        const group = event.option.value as ICompetencyGroup;
        this.selectedCompetencyGroup = group;
        this.competency.competencyGroupId = group.competencyGroupId;
        (<FormGroup>this.form.controls.competencyGroup).controls.competencyGroupId.setValue(group.competencyGroupId);
        (<FormGroup>this.form.controls.competencyGroup).controls.clientId.setValue(group.clientId);
        (<FormGroup>this.form.controls.competencyGroup).controls.name.setValue(group.name);
    }

    groupsMapper(group:ICompetencyGroup):ICompetencyGroup { 
        this.selectedCompetencyGroup = group;
        return group;
    }

    //temporarily hiding this TPR-147
    // updateDifficultyLevel(event):void {
    //     this.competency.difficultyLevel = event.value;
    //     this.form.patchValue({
    //         difficultyLevel: event.value
    //     });
    //     this.form.markAsDirty();
    //     this.form.markAsTouched();
    // }

    private createEmptyCompetency():ICompetency {
        return {
            competencyId: null,
            clientId: null,
            competencyGroupId: null,
            name: null,
            description: null,
            difficultyLevel: null,
            isCore: null,
            isArchived: null
        }
    }

    private createForm():void {
        this.form = this.fb.group({
            name: this.fb.control(this.competency.name || '', [Validators.required]),
            description: this.fb.control(this.competency.description || '', [Validators.required]),
            competencyGroup: this.fb.group({
                competencyGroupId: this.fb.control(this.competency.competencyGroupId || ''),
                clientId: this.fb.control(this.selectedCompetencyGroup.clientId || this.user.lastClientId || this.user.clientId),
                name: this.fb.control(this.selectedCompetencyGroup.name || '')
            }),
            isCore: this.fb.control(this.competency.isCore || ''),
            //difficultyLevel: this.fb.control(this.competency.difficultyLevel || '')
        });
    }

    private resetForm(c:ICompetency):void {
        this.formSubmitted = false;
        this.form.reset({
            name: c.name,
            description: c.description,
            isCore: c.isCore,
            //difficultyLevel: c.difficultyLevel
        });

        const grp = _.find(this.competencyGroups, { 'competencyGroupId': c.competencyGroupId }) as ICompetencyGroup;
        if(grp == null) return;
        (<FormGroup>this.form.controls.competencyGroup).reset({
            competencyGroupId: grp.competencyGroupId,
            clientId: grp.clientId,
            name: grp.name
        });
    }

    private prepareSaveModel():ICompetency {
        return {
            competencyId: this.competency != null ? this.competency.competencyId : null,
            clientId: this.user.lastClientId || this.user.clientId,
            competencyGroupId: this.form.controls.competencyGroup.value.competencyGroupId,
            name: this.form.value.name,
            description: this.form.value.description,
            isCore: this.form.value.isCore,
            isArchived: this.competency.isArchived || false,
            //difficultyLevel: this.form.value.difficultyLevel
        };
    }

    private prepareCompetencyGroups(g:ICompetencyGroup|ICompetencyGroup[] = null):void {
        if(g != null && !Array.isArray(g)) {
            this.competencyGroups.push(g);
            this.competencyGroups = this.sortCompetencyGroups(this.competencyGroups);
        } else if (g != null && Array.isArray(g)) {
            this.competencyGroups = this.sortCompetencyGroups(g);
            this.selectedCompetencyGroup = _.find(this.competencyGroups, { 'competencyGroupId': this.competency.competencyGroupId })
                || this.getNewCompetencyGroup();
        }
    }

    private sortCompetencyGroups(groups:ICompetencyGroup[]):ICompetencyGroup[] {
        return _.sortBy(groups, ['name']);
    }

    private closeDialogSuccessfully():void {
        this.dialogRef.close(<CompetencyEditDialogResult>{
            competency: this.competency,
            competencyGroups: this.competencyGroups
        });
    }

    private getNewCompetencyGroup(): ICompetencyGroup {
        return {
            competencyGroupId: null,
            clientId: this.user.lastClientId || this.user.clientId,
            name: null
        } as ICompetencyGroup;
    }
}
