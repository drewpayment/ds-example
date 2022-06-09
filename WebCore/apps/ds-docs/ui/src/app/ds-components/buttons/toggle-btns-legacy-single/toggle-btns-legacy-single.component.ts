import { Component, OnInit } from '@angular/core';
import { LENGTHOFTIME } from '../../shared/lengthOfTime';

@Component({
    selector: 'ds-toggle-btns-legacy-single',
    templateUrl: './toggle-btns-legacy-single.component.html',
    styleUrls: ['./toggle-btns-legacy-single.component.scss']
})
export class ToggleBtnsLegacySingleComponent implements OnInit {

    items = LENGTHOFTIME;
    itemSelected: boolean;

    constructor() { }

    ngOnInit() {
    }

    changeToggle(item) {
        if ( this.itemSelected == item) { 
            this.itemSelected = null; // reset to nothing selected if the user selects this twice
        } else {
            this.itemSelected = item;
        }
    }
}
