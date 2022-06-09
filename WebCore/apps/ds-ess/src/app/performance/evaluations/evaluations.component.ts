import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { DsStyleLoaderService } from '@ajs/ui/ds-styles/ds-styles.service';

@Component({
    selector: 'ds-evaluations',
    templateUrl: './evaluations.component.html',
    styleUrls: ['./evaluations.component.scss']
})
export class EmployeeEvaluationViewComponent implements OnInit, AfterViewChecked {

    constructor(
        private styles: DsStyleLoaderService) {
    }

    ngOnInit() {
    }

    ngAfterViewChecked() {
        this.styles.useMainStyleSheet();
    }
}

