import { Component, OnInit, Input } from '@angular/core';
import { ICompetency } from '../shared/competency.model';

const fakeData:ICompetency = {
    competencyId: 1,
    clientId: 1182, 
    description: 'this is my fake competency data until I wire upt he list... ',
    name: 'My Fake Competency',
    isArchived: false,
    isCore: true,
    difficultyLevel: 4
}

@Component({
    selector: 'ds-competency',
    templateUrl: './competency.component.html',
    styleUrls: ['./competency.component.scss']
})
export class CompetencyComponent implements OnInit {

    @Input() competency:ICompetency = fakeData;

    constructor() { }

    ngOnInit() {
        
    }

}
