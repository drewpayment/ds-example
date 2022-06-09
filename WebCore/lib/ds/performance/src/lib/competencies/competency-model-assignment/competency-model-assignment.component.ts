import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { ICompetencyModelBasic } from '../shared/competency-model.model';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { zip } from 'rxjs';
import { ICompetency } from '..';

@Component({
    selector: 'ds-competency-model-assignment',
    templateUrl: './competency-model-assignment.component.html',
    styleUrls: ['./competency-model-assignment.component.scss']
})
export class CompetencyModelAssignmentComponent implements OnInit {

    private _modelId:number;
    
    @Input()
    set modelId(value:number) {
        this._modelId = value;
        this.setSelectedModel();
    };
    
    @Output()
    modelSelected = new EventEmitter<ICompetencyModelBasic>();
    
    isLoading = true;
    models: ICompetencyModelBasic[];
    coreCompetencies: ICompetency[];
    selectedModel: ICompetencyModelBasic;

    get hasModels() {
        return this.models && this.models.length;
    }
    get hasData() {
        return this.hasModels || (this.coreCompetencies && this.coreCompetencies.length);
    }

    constructor(
        private perfSvc: PerformanceReviewsService
    ) { }

    ngOnInit() {
        zip(this.perfSvc.getCoreCompetencies(),
            this.perfSvc.getBasicCompetencyModelInfoForClient(),
            (coreComps, models) => { return {coreComps, models}})        
        .subscribe(data => {
            this.coreCompetencies = this.sortCompetencies(data.coreComps);
            this.models = data.models;

            if (this.models) {
                this.models.forEach(m => this.sortCompetencies(m.competencies));
            }

            this.sortModels(this.models);
            this.setSelectedModel();

            this.isLoading = false;
        })
    }

    selectedModelChanged(model:ICompetencyModelBasic) {
        this.modelSelected.emit(model);
    }

    private sortModels(models: ICompetencyModelBasic[]) {
        if (!models) return;

        models.sort((m1, m2) => {
            return m1.name > m2.name ? 1 : -1;
        });

        return models;
    }

    private sortCompetencies(comps: ICompetency[]) {
        if (!comps) return;

        comps.sort((comp1, comp2) => {
            const comp1Name = (comp1.name || '').toLowerCase().trim();
            const comp2Name = (comp2.name || '').toLowerCase().trim();
                return comp1Name.localeCompare(comp2Name);
        });

        return comps;
    }

    private setSelectedModel() {
        if (this.models && this._modelId) {
            this.selectedModel = this.models.find(m => m.competencyModelId === this._modelId);
        } 
        else {
            this.selectedModel = null;
        }
    }
}
