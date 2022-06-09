import * as angular from "angular";
import { downgradeComponent } from "@angular/upgrade/static";
import { CompetencyModelAssignmentComponent } from "@ds/performance/competencies/competency-model-assignment/competency-model-assignment.component";

export module CompetenciesDowngradeModule {
    export const MODULE_NAME = 'CompetenciesDowngradeModule';
    export const MODULE_DEPENDENCIES = [];

    export const AjsModule = angular.module(MODULE_NAME, MODULE_DEPENDENCIES);

    AjsModule
        .directive(
            'dsCompetencyModelAssignment',
            downgradeComponent({ component: CompetencyModelAssignmentComponent  }) as ng.IDirectiveFactory
        )
}