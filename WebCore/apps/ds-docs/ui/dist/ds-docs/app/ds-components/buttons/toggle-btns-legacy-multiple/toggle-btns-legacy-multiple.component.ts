import { Component, OnInit } from '@angular/core';
import { LENGTHOFTIME } from '../../shared/lengthOfTime';

@Component({
    selector: 'ds-toggle-btns-legacy-multiple',
    templateUrl: './toggle-btns-legacy-multiple.component.html',
    styleUrls: ['./toggle-btns-legacy-multiple.component.scss']
})
export class ToggleBtnsLegacyMultipleComponent implements OnInit {
    items = LENGTHOFTIME;
    constructor() { }

    ngOnInit() {
    }

    changeToggle(item) {
        if ( item.selected ) {
            item.selected = false; // reset to nothing selected if the user selects this twice
        } else {
            item.selected = true;
        }
    }
}
