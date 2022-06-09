import { Component, OnInit } from '@angular/core';
import { ICheckDeactivator } from '@ds/core/ui/change-track';

@Component({
    selector: 'ds-change-track-docs',
    templateUrl: './change-track-docs.component.html',
    styleUrls: ['./change-track-docs.component.scss']
})
export class ChangeTrackDocsComponent implements OnInit, ICheckDeactivator {

    toggle1 = false;
    toggle2 = false;
    toggle3 = false;
    toggle4 = false;

    constructor() { }

    ngOnInit() {
    }

}
