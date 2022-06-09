import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompetenciesModule } from '@ds/performance/competencies/competencies.module';
import { CompetencyModelAssignmentComponent } from '@ds/performance/competencies/competency-model-assignment/competency-model-assignment.component';

@NgModule({
    imports: [
        CommonModule,
        CompetenciesModule
    ],
    declarations: [
    ],
    entryComponents: [
        CompetencyModelAssignmentComponent
    ]
})
export class CompanyJobProfilesModule { }
